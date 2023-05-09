using RonWeb.API.Enum;
using RonWeb.API.Interface.RefreshToken;
using RonWeb.API.Models.RefreshToken;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.Database.Mongo;
using RonWeb.Database.Service;
using MongoDB.Driver.Linq;
using RonWeb.API.Models.CustomizeException;
using RonWeb.Database.Models;
using MongoDB.Bson;

namespace RonWeb.API.Helper.RefreshToken
{
    public class RefreshTokenHelper : IRefreshTokenHelper
    {
        public async Task<Token> Refresh(RefreshTokenRequest data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var userMain = srv.Query<Database.Models.UserMain>();
            ObjectId userId = new ObjectId();
            if (ObjectId.TryParse(data.UserId, out userId))
            {
                // 沒過期才能換新token
                var log = await srv.Query<Database.Models.RefreshTokenLog>()
                    .Where(a => a.UserId == ObjectId.Parse(data.UserId))
                    .Where(a => a.RefreshToken == data.RefreshToken)
                    .Join(userMain, a => a.UserId, b => b._id, (a, b) => new
                    {
                        UserId = b._id,
                        Email = b.Email,
                        ExpirationDate = a.ExpirationDate,
                    })
                    .SingleOrDefaultAsync();
                if (log == null)
                {
                    throw new NotFoundException();
                }
                else if (log.ExpirationDate < DateTime.Now)
                {
                    throw new AuthExpiredException();
                }
                else
                {
                    string refreshToken = JwtTool.CreateRefreshToken();
                    var claims = JwtTool.CreateClaims(log.Email ?? "", log.UserId.ToString(), RoleEnum.ManagerUser.Description());
                    string issuer = Environment.GetEnvironmentVariable(EnvVarEnum.ISSUER.Description())!;
                    string audience = Environment.GetEnvironmentVariable(EnvVarEnum.AUDIENCE.Description())!;
                    string jwtKey = Environment.GetEnvironmentVariable(EnvVarEnum.JWTKEY.Description())!;
                    var toeknExpTime = DateTime.Now.AddHours(1);
                    var refreshTokenExpTime = DateTime.Now.AddDays(3);
                    var model = new JwtModel()
                    {
                        Issuer = issuer,
                        Audience = audience,
                        Key = jwtKey,
                        Claims = claims,
                        ExpirationTime = toeknExpTime
                    };
                    var token = JwtTool.GenerateToken(model);
                    var tokenLog = new RefreshTokenLog()
                    {
                        RefreshToken = refreshToken,
                        UserId = log.UserId,
                        ExpirationDate = refreshTokenExpTime,
                        CreateDate = DateTime.Now
                    };
                    await srv.CreateAsync(log);
                    return new Token(token, refreshToken);
                }
            }
            else 
            {
                throw new NotFoundException();
            }
        }
    }
}

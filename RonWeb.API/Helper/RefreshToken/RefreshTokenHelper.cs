using RonWeb.API.Enum;
using RonWeb.API.Interface.RefreshToken;
using RonWeb.API.Models.RefreshToken;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using MongoDB.Driver.Linq;
using RonWeb.API.Models.CustomizeException;
using RonWeb.Database.MySql.RonWeb.DataBase;
using Microsoft.EntityFrameworkCore;

namespace RonWeb.API.Helper.RefreshToken
{
    public class RefreshTokenHelper : IRefreshTokenHelper
    {
        private readonly RonWebDbContext _db;

        public RefreshTokenHelper(RonWebDbContext dbContext)
        {
            this._db = dbContext;
        }

        public async Task<Token> Refresh(RefreshTokenRequest data)
        {
            var log = await _db.RefreshTokenLog
                    .Where(a => a.UserId == data.UserId)
                    .Where(a => a.RefreshToken == data.RefreshToken)
                    .Include(a => a.UserMain)
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
                var claims = JwtTool.CreateClaims(log.UserMain.Email ?? "", log.UserId.ToString(), RoleEnum.ManagerUser.Description());
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
                var tokenLog = new RonWeb.Database.MySql.RonWeb.Table.RefreshTokenLog()
                {
                    RefreshToken = refreshToken,
                    UserId = log.UserId,
                    ExpirationDate = refreshTokenExpTime,
                    CreateDate = DateTime.Now
                };
                await _db.AddAsync(tokenLog);
                await _db.SaveChangesAsync();
                return new Token(token, refreshToken);
            }
        }
    }
}

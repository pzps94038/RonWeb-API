using System;
using RonWeb.API.Enum;
using RonWeb.API.Interface.Login;
using RonWeb.API.Models.Login;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.Database.Models;
using RonWeb.Database.Mongo;
using RonWeb.Database.Service;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RonWeb.API.Models.CustomizeException;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RonWeb.API.Helper.Login
{
	public class LoginHelper : ILoginHelper
    {
        public async Task<Token> Login(LoginRequest data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            string iv = Environment.GetEnvironmentVariable(EnvVarEnum.AESIV.Description())!;
            string key = Environment.GetEnvironmentVariable(EnvVarEnum.AESKEY.Description())!;
            var encrypt = EncryptTool.AESEncrypt(data.Password, iv, key);
            var encryptPassword = EncryptTool.SHA256Encrypt(encrypt);
            var user = await srv.Query<UserMain>()
                .Where(a => a.Account == data.Account)
                .Where(a => a.Password == encryptPassword)
                .SingleOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException();
            }
            else
            {
                string refreshToken = JwtTool.CreateRefreshToken();
                var claims = JwtTool.CreateClaims(user.Email ?? "", user._id.ToString(), RoleEnum.ManagerUser.Description());
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
                var log = new RefreshTokenLog()
                {
                    RefreshToken = refreshToken,
                    UserId = user._id,
                    ExpirationDate = refreshTokenExpTime,
                    CreateDate = DateTime.Now
                };
                await srv.CreateAsync(log);
                return new Token(token, refreshToken);
            }
        }
    }
}


using RonWeb.API.Enum;
using RonWeb.API.Interface.Login;
using RonWeb.API.Models.Login;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RonWeb.API.Models.CustomizeException;
using Microsoft.EntityFrameworkCore;
using RonWeb.Database.Entities;

namespace RonWeb.API.Helper.Login
{
    public class LoginHelper : ILoginHelper
    {
        private readonly RonWebDbContext _db;

        public LoginHelper(RonWebDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<LoginResponse> Login(LoginRequest data)
        {
            string iv = Environment.GetEnvironmentVariable(EnvVarEnum.AESIV.Description())!;
            string key = Environment.GetEnvironmentVariable(EnvVarEnum.AESKEY.Description())!;
            var encrypt = EncryptTool.AESEncrypt(data.Password, iv, key);
            var encryptPassword = EncryptTool.SHA256Encrypt(encrypt);
            var user = await _db.UserMain
                .Where(a => a.Account == data.Account)
                .Where(a => a.Password == encryptPassword)
                .SingleOrDefaultAsync();
            if (user == null)
            {
                throw new NotFoundException();
            }
            string refreshToken = JwtTool.CreateRefreshToken();
            var claims = JwtTool.CreateClaims(user.Email ?? "", user.UserId.ToString(), RoleEnum.ManagerUser.Description());
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
                UserId = user.UserId,
                ExpirationDate = refreshTokenExpTime,
                CreateDate = DateTime.Now
            };
            await _db.AddAsync(log);
            await _db.SaveChangesAsync();
            return new LoginResponse()
            {
                Token = new Token(token, refreshToken),
                UserId = user.UserId
            };
        }
    }
}


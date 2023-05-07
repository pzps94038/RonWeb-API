using System;
using MongoDB.Driver;
using RonWeb.API.Enum;
using RonWeb.API.Interface.Register;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Register;
using RonWeb.Core;
using RonWeb.Database.Mongo;
using RonWeb.Database.Service;
using MongoDB.Driver.Linq;
namespace RonWeb.API.Helper.Register
{
    public class RegisterHelper : IRegisterHelper
    {
        public async Task RegisterUser(RegisterRequest data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var hasData = await srv.Query<Database.Models.UserMain>()
                .FirstOrDefaultAsync(a => a.Account == data.Account);
            if (hasData != null)
            {
                throw new UniqueException();
            }
            var indexModel = new CreateIndexModel<Database.Models.UserMain>(
                Builders<Database.Models.UserMain>.IndexKeys.Ascending("Account"),
                new CreateIndexOptions { Unique = true }
            );
            string iv = Environment.GetEnvironmentVariable(EnvVarEnum.AESIV.Description())!;
            string key = Environment.GetEnvironmentVariable(EnvVarEnum.AESKEY.Description())!;
            var encrypt = EncryptTool.AESEncrypt(data.Password, iv, key);
            var encryptPassword = EncryptTool.SHA256Encrypt(encrypt);
            var user = new RonWeb.Database.Models.UserMain()
            {
                Account = data.Account,
                Password = encryptPassword,
                UserName = data.UserName,
                Email = data.Email,
                CreateDate = DateTime.Now
            };
            var collection = srv.GetCollection<Database.Models.UserMain>();
            await collection.Indexes.CreateOneAsync(indexModel);
            await srv.CreateAsync(user);
        }
    }
}


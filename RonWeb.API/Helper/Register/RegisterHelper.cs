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
using RonWeb.Database.MySql.RonWeb.DataBase;
using Microsoft.EntityFrameworkCore;

namespace RonWeb.API.Helper.Register
{
    public class RegisterHelper : IRegisterHelper
    {
        public async Task RegisterUser(RegisterRequest data)
        {
            using (var db = new RonWebDbContext())
            {
                var user = await db.UserMain.SingleOrDefaultAsync(a => a.Account == data.Account);
                if (user != null)
                {
                    throw new UniqueException();
                }
                else
                {
                    string iv = Environment.GetEnvironmentVariable(EnvVarEnum.AESIV.Description())!;
                    string key = Environment.GetEnvironmentVariable(EnvVarEnum.AESKEY.Description())!;
                    var encrypt = EncryptTool.AESEncrypt(data.Password, iv, key);
                    var encryptPassword = EncryptTool.SHA256Encrypt(encrypt);
                    user = new RonWeb.Database.MySql.RonWeb.Table.UserMain
                    {
                        Account = data.Account,
                        Password = encryptPassword,
                        UserName = data.UserName,
                        Email = data.Email,
                        CreateDate = DateTime.Now
                    };
                    await db.UserMain.AddAsync(user);
                    await db.SaveChangesAsync();
                }
            }
        }
    }
}


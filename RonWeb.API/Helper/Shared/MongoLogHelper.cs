﻿using System;
using RonWeb.API.Enum;
using RonWeb.API.Interface.Shared;
using RonWeb.Core;
using RonWeb.Database.Models;
using RonWeb.Database.Mongo;
using RonWeb.Database.Service;

namespace RonWeb.API.Helper.Shared
{
	public enum Level
	{
		Info,
		Error,
	}

	public static class MongoLogHelper
    {
		public static async void Info(string msg)
		{
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var data = new ExceptionLog()
            {
                Message = msg,
				Level = Level.Info.Description(),
				CreateDate = DateTime.Now
			};
			await srv.CreateAsync<ExceptionLog>(MongoTableEnum.ExceptionLog.Description(), data);
        }

        public static async void Error(Exception ex)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var data = new ExceptionLog()
            {
                StackTrace = ex.StackTrace,
                Message = ex.Message,
                Level = Level.Error.Description(),
                CreateDate = DateTime.Now
            };
            await srv.CreateAsync<ExceptionLog>(MongoTableEnum.ExceptionLog.Description(), data);
        }
    }
}


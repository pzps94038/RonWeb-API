﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RonWeb.Database.Mongo.MongoAttribute;

namespace RonWeb.Database.Models
{
    [MongoAttribute("ExceptionLog")]
    public class ExceptionLog
    {
        /// <summary>
        /// Mongo id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        /// <summary>
        /// 訊息
        /// </summary>
        [BsonElement("Message")]
        public string? Message { get; set; }

        /// <summary>
        /// 堆疊
        /// </summary>
        [BsonElement("StackTrace")]
        public string? StackTrace { get; set; }

        /// <summary>
        /// 信息級別
        /// </summary>
        [BsonElement("Level")]
        public string Level { get; set; } = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}

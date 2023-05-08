using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RonWeb.Database.Mongo.MongoAttribute;

namespace RonWeb.Database.Models
{
    [MongoAttribute("RefreshTokenLog")]
    public class RefreshTokenLog
    {
        /// <summary>
        /// Mongo id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        /// <summary>
        /// RefreshToken
        /// </summary>
        [BsonElement("RefreshToken")]
        public string RefreshToken = string.Empty;

        /// <summary>
        /// 帳號
        /// </summary>
        [BsonElement("UserId")]
        public ObjectId UserId { get; set; }

        /// <summary>
        /// 過期時間
        /// </summary>
        [BsonElement("ExpirationDate")]
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}

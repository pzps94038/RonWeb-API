using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RonWeb.Database.Models
{
    public class RefreshTokenLog
    {
        /// <summary>
        /// Mongo id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id = string.Empty;

        /// <summary>
        /// RefreshToken
        /// </summary>
        [BsonElement("RefreshToken")]
        public string RefreshToken = string.Empty;

        /// <summary>
        /// 帳號
        /// </summary>
        [BsonElement("Account")]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 過期日期
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

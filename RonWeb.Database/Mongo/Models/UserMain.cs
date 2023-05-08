using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using RonWeb.Database.Mongo.MongoAttribute;

namespace RonWeb.Database.Models
{
    [MongoAttribute("UserMain")]
    public class UserMain
    {
        /// <summary>
        /// Mongo id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        [BsonElement("Account")]
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// 密碼
        /// </summary>
        [BsonElement("Password")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// 姓名
        /// </summary>
        [BsonElement("UserName")]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 電子郵件
        /// </summary>
        [BsonElement("Email")]
        public string? Email { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set;}

        /// <summary>
        /// 更新日期
        /// </summary>
        [BsonElement("UpdateDate")]
        public DateTime? UpdateDate { get; set; }
    }
}
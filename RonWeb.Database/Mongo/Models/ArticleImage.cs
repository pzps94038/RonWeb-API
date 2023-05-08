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
    [MongoAttribute("ArticleImage")]
    public class ArticleImage
    {
        /// <summary>
        /// Mongo id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        /// <summary>
        /// 文章id
        /// </summary>
        [BsonElement("ArticleId")]
        public ObjectId ArticleId { get; set; }

        /// <summary>
        /// 圖片id
        /// </summary>
        [BsonElement("ImageId")]
        public ObjectId ImageId { get; set; }

        /// <summary>
        /// 圖片名稱
        /// </summary>
        [BsonElement("ImageName")]
        public string ImageName { get; set; } = string.Empty;

        /// <summary>
        /// 圖片路徑
        /// </summary>
        [BsonElement("ImagePath")]
        public string ImagePath { get; set; } = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}

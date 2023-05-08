using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using RonWeb.Database.Mongo.MongoAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RonWeb.Database.Models
{
    [MongoAttribute("ArticleCategory")]
    public class ArticleCategory
    {
        /// <summary>
        /// Mongo id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        /// <summary>
        /// 類別名稱
        /// </summary>
        [BsonElement("CategoryName")]
        public string CategoryName = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}

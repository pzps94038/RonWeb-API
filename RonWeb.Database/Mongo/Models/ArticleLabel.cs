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
    [MongoAttribute("ArticleLabel")]
    public class ArticleLabel
    {
        /// <summary>
        /// Mongo id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        /// <summary>
        /// 標籤名稱
        /// </summary>
        [BsonElement("LabelName")]
        public string LabelName { get; set; } = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}

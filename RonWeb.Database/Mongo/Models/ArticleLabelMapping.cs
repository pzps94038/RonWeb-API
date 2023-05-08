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
    [MongoAttribute("ArticleLabelMapping")]
    public class ArticleLabelMapping
    {
        /// <summary>
        /// Mongo Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId _id { get; set; }

        /// <summary>
        /// 標籤 Id
        /// </summary>
        [BsonElement("LabelId")]
        public ObjectId LabelId { get; set; }

        /// <summary>
        /// 文章 Id
        /// </summary>
        [BsonElement("ArticleId")]
        public ObjectId ArticleId { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}

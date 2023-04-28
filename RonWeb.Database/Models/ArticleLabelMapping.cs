using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RonWeb.Database.Models
{
    public class ArticleLabelMapping
    {
        /// <summary>
        /// Mongo Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id = string.Empty;

        /// <summary>
        /// 標籤 Id
        /// </summary>
        [BsonElement("LabelId")]
        public string LabelId = string.Empty;

        /// <summary>
        /// 文章 Id
        /// </summary>
        [BsonElement("ArticleId")]
        public string ArticleId = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}

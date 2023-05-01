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
    [MongoAttribute("Article")]
    public class Article
    {
        /// <summary>
        /// Mongo id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 文章標題
        /// </summary>
        [BsonElement("ArticleTitle")]
        public string ArticleTitle { get; set; } = string.Empty;

        /// <summary>
        /// 文章內容
        /// </summary>
        [BsonElement("Content")]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 類別id
        /// </summary>
        [BsonElement("CategoryId")]
        public string CategoryId { get; set; } = string.Empty;

        /// <summary>
        /// 瀏覽次數
        /// </summary>
        [BsonElement("ViewCount")]
        public int ViewCount { get; set; } = 0;

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        [BsonElement("CreateBy")]
        public string CreateBy { get; set; } = string.Empty;

        /// <summary>
        /// 更新日期
        /// </summary>
        [BsonElement("UpdateDate")]
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [BsonElement("UpdateBy")]
        public string? UpdateBy { get; set; }
    }
}

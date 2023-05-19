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
        /// 文章Id
        /// </summary>
        [BsonElement("ArticleId")]
        public ObjectId ArticleId { get; set; }

        /// <summary>
        /// 檔名
        /// </summary>
        [BsonElement("FileName")]
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 檔案路徑
        /// </summary>
        [BsonElement("Path")]
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Network Path
        /// </summary>
        [BsonElement("Url")]
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        [BsonElement("CreateBy")]
        public ObjectId CreateBy { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        [BsonElement("UpdateDate")]
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 更新者
        /// </summary>
        [BsonElement("UpdateBy")]
        public ObjectId? UpdateBy { get; set; }
    }
}

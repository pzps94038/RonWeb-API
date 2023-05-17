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

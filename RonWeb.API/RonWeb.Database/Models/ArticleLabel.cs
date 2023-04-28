using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RonWeb.Database.Models
{
    public class ArticleLabel
    {
        /// <summary>
        /// Mongo id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string LabelId = string.Empty;

        /// <summary>
        /// 標籤名稱
        /// </summary>
        [BsonElement("LabelName")]
        public string LabelName = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}

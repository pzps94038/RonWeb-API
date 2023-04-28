using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RonWeb.Database.Models
{
    public class SearchLog
    {
        /// <summary>
        /// Mongo Id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id = string.Empty;

        /// <summary>
        /// 搜尋關鍵字
        /// </summary>
        [BsonElement("Keyword")]
        public string Keyword = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}

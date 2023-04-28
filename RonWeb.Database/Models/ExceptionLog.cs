using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RonWeb.Database.Models
{
    public class ExceptionLog
    {
        /// <summary>
        /// Mongo id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id = string.Empty;

        /// <summary>
        /// 例外信息
        /// </summary>
        [BsonElement("ExceptionDetail")]
        public string ExceptionDetail = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}

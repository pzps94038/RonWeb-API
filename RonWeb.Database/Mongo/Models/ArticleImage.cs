using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RonWeb.Database.Models
{
    public class ArticleImage
    {
        /// <summary>
        /// Mongo id
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 文章id
        /// </summary>
        [BsonElement("ArticleId")]
        public string ArticleId = string.Empty;

        /// <summary>
        /// 圖片id
        /// </summary>
        [BsonElement("ImageId")]
        public string ImageId = string.Empty;

        /// <summary>
        /// 圖片名稱
        /// </summary>
        [BsonElement("ImageName")]
        public string ImageName = string.Empty;

        /// <summary>
        /// 圖片路徑
        /// </summary>
        [BsonElement("ImagePath")]
        public string ImagePath = string.Empty;

        /// <summary>
        /// 建立日期
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }
    }
}

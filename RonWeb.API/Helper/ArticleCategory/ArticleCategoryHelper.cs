using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using RonWeb.API.Enum;
using RonWeb.API.Interface.ArticleCategory;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.ArticleCategory;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using RonWeb.Database.Models;
using RonWeb.Database.Mongo;
using RonWeb.Database.Service;

namespace RonWeb.API.Helper.ArticleCategory
{
    public class ArticleCategoryHelper : IArticleCategoryHelper
    {
        public async Task CreateAsync(CreateArticleCategoryRequest data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var hasData = await srv.Query<Database.Models.ArticleCategory>()
                .FirstOrDefaultAsync(a => a.CategoryName == data.CategoryName);
            if (hasData != null)
            {
                throw new UniqueException();
            }
            ObjectId userId = new ObjectId();
            if ((ObjectId.TryParse(data.UserId, out userId)))
            {
                var category = new Database.Models.ArticleCategory()
                {
                    CategoryName = data.CategoryName,
                    CreateDate = DateTime.Now,
                    CreateBy = userId
                };
                var indexModel = new CreateIndexModel<Database.Models.ArticleCategory>(
                    Builders<Database.Models.ArticleCategory>.IndexKeys.Ascending("CategoryName"),
                    new CreateIndexOptions { Unique = true }
                );
                var collection = srv.GetCollection<Database.Models.ArticleCategory>();
                await collection.Indexes.CreateOneAsync(indexModel);
                await srv.CreateAsync(category);
            }
            else 
            {
                throw new NotFoundException();
            }
        }

        public async Task DeleteAsync(string data)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            ObjectId categoryId = new ObjectId();
            if ((ObjectId.TryParse(data, out categoryId)))
            {
                var filter = Builders<Database.Models.ArticleCategory>.Filter.Eq(a => a._id, ObjectId.Parse(data));
                await srv.DeleteAsync(filter);
            }
            else 
            {
                throw new NotFoundException();
            }
        }

        public async Task<Category> GetAsync(string id)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var category = srv.Query<Database.Models.ArticleCategory>();
            ObjectId objId = new ObjectId();
            if (ObjectId.TryParse(id, out objId))
            {
                var data = await srv.Query<Database.Models.ArticleCategory>()
                    .SingleOrDefaultAsync(a => a._id == objId);
                if (data == null)
                {
                    throw new NotFoundException();
                }
                var result = new Category()
                {
                    CategoryId = data._id.ToString(),
                    CategoryName = data.CategoryName,
                    CreateDate = data.CreateDate
                };
                return result;
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public async Task<GetArticleCategoryResponse> GetListAsync(int? page)
        {
            string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
            var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
            var query = srv.Query<Database.Models.ArticleCategory>()
                .Select(a => new
                {
                    CategoryId = a._id,
                    CategoryName = a.CategoryName,
                    CreateDate = a.CreateDate
                });

            var total = query.Count();

            if (page != null) 
            {
                var pageSize = 10;
                int skip = (int)((page - 1) * pageSize);
                if (skip == 0)
                {
                    query = query.Take(pageSize);
                }
                else 
                {
                    query = query.Skip(skip).Take(pageSize);
                }
            }

            var list = await query.ToListAsync();
            var result = list.Select(a => new Category() { 
                CategoryId = a.CategoryId.ToString(), 
                CategoryName = a.CategoryName,
                CreateDate = a.CreateDate
            }).ToList();

            return new GetArticleCategoryResponse() 
            {
                Total = total,
                Categorys = result
            };
        }

        public async Task UpdateAsync(string id, UpdateArticleCategoryRequest data)
        {
            ObjectId categoryId = new ObjectId();
            ObjectId userId = new ObjectId();
            if ((ObjectId.TryParse(data.CategoryId, out categoryId) && (ObjectId.TryParse(data.UserId, out userId))))
            {
                string conStr = Environment.GetEnvironmentVariable(EnvVarEnum.RON_WEB_MONGO_DB_CONSTR.Description())!;
                var srv = new MongoDbService(conStr, MongoDbEnum.RonWeb.Description());
                var filter = Builders<Database.Models.ArticleCategory>.Filter.Eq(a => a._id, categoryId);
                var update = Builders<Database.Models.ArticleCategory>.Update.Set(a => a.CategoryName, data.CategoryName)
                    .Set(a => a.UpdateDate, DateTime.Now)
                    .Set(a => a.UpdateBy, userId);
                await srv.UpdateAsync(filter, update);
            }
            else 
            {
                throw new NotFoundException();
            }
        }
    }
}

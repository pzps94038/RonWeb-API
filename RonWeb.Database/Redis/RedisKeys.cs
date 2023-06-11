using System;
namespace RonWeb.Database.Redis
{
	public static class RedisKeys
	{
        public const string Site = "ron_web_api";
        public const string ArticleByIdPrefix = Site + ":articleById:";
        public const string ArticlePagePrefix = Site + ":articlePage:";
        public const string SearchCategoryPrefix = Site + ":searchCategory:";
        public const string SearchLabelPrefix = Site + ":searchLabel:";
        public const string ArticleCategoryPrefix = Site + ":articleCategory";
        public const string ArticleLabelPrefix = Site + ":articleLabel";
       
    }
}


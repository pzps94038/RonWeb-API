using System;
using RonWeb.API.Models.Search;

namespace RonWeb.API.Interface.Search
{
	public interface ISearchHelper
	{
		public Task<KeywordeResponse> Keyword(string keyword, int? page);
        public Task<KeywordeResponse> Label(string id, int? page);
        public Task<KeywordeResponse> Category(string id, int? page);
    }
}


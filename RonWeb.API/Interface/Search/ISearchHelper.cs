using System;
using RonWeb.API.Models.Search;

namespace RonWeb.API.Interface.Search
{
	public interface ISearchHelper
	{
        public Task<KeywordeResponse> Category(long id, int? page);
        public Task<KeywordeResponse> Label(long id, int? page);
    }
}


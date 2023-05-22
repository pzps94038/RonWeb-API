using System;
using RonWeb.API.Models.Search;
using RonWeb.API.Models.SiteMap;

namespace RonWeb.API.Interface.SiteMap
{
	public interface ISiteMapHelper
	{
        public Task<List<SiteMapResponse<long>>> Article();
        public Task<List<SiteMapResponse<long>>> Category();
        public Task<List<SiteMapResponse<long>>> Label();
    }
}


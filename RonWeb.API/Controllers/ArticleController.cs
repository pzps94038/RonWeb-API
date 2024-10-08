﻿using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Interface.Article;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    /// <summary>
    /// 文章
    /// </summary>
    [Route("api/[controller]")]
    public class ArticleController : Controller
    {
        private readonly IArticleHelper _helper;
        public ArticleController(IArticleHelper helper)
        {
            _helper = helper;
        }

        /// <summary>
        /// 取得文章列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetArticleResponse>> GetArticle(int? page, string? keyword)
        {
            var result = new BaseResponse<GetArticleResponse>();
            var data = await _helper.GetListAsync(page, keyword);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }

        /// <summary>
        /// 取得文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<BaseResponse<GetByIdArticleResponse>> GetArticleById(long id)
        {
            var result = new BaseResponse<GetByIdArticleResponse>();
            var data = await _helper.GetAsync(id);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.Success.Description();
            result.Data = data;
            return result;
        }

        /// <summary>
        /// 更新文章瀏覽次數
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("[action]/{id}")]
        [HttpPatch]

        public async Task<BaseResponse> UpdateArticleViews(long id)
        {
            var result = new BaseResponse();
            await _helper.UpdateArticleViews(id);
            result.ReturnCode = ReturnCode.Success.Description();
            result.ReturnMessage = ReturnMessage.ModifySuccess.Description();
            return result;
        }
    }
}


﻿using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using RonWeb.API.Enum;
using RonWeb.API.Helper.Shared;
using RonWeb.API.Interface.Article;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.CustomizeException;
using RonWeb.API.Models.Shared;
using RonWeb.Core;

namespace RonWeb.API.Controllers
{
    [Route("api/[controller]")]
    public class ArticleController : Controller
    {
        private readonly IArticleHelper _helper;
        public ArticleController(IArticleHelper helper)
        {
            this._helper = helper;
        }

        /// <summary>
        /// 取得文章列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BaseResponse<GetArticleResponse>> Get(int? page, string? keyword)
        {
            var result = new BaseResponse<GetArticleResponse> ();
            try
            {
                var data = await this._helper.GetListAsync(page, keyword);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.Success.Description();
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.Fail.Description();
                LogHelper.Error(ex);
            }

            return result;
        }

        /// <summary>
        /// 取得文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<BaseResponse<GetByIdArticleResponse>> Get(long id)
        {
            var result = new BaseResponse<GetByIdArticleResponse>();
            try
            {
                var data = await this._helper.GetAsync(id);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.Success.Description();
                result.Data = data;
            }
            catch (NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.NotFound.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.Fail.Description();
                LogHelper.Error(ex);
            }
            return result;
        }

        /// <summary>
        /// 新增文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public async Task<BaseResponse> Post([FromBody] CreateArticleRequest data)
        {
            var result = new BaseResponse();

            try
            {
                await this._helper.CreateAsync(data);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.CreateSuccess.Description();
            }
            catch (NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.NotFound.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.CreateFail.Description();
                LogHelper.Error(ex);
            }

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
            try
            {
                await this._helper.UpdateArticleViews(id);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.ModifySuccess.Description();
            }
            catch (NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.NotFound.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.ModifyFail.Description();
                LogHelper.Error(ex);
            }

            return result;
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [Authorize]
        public async Task<BaseResponse> Patch(long id, [FromBody] UpdateArticleRequest data)
        {
            var result = new BaseResponse();

            try
            {
                await this._helper.UpdateAsync(id, data);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.ModifySuccess.Description();
            }
            catch (NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.NotFound.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
                LogHelper.Error(ex);
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.ModifyFail.Description();
                LogHelper.Error(ex);
            }

            return result;
        }

        /// <summary>
        /// 刪除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<BaseResponse> Delete(long id)
        {
            var result = new BaseResponse();

            try
            {
                await this._helper.DeleteAsync(id);
                result.ReturnCode = ReturnCode.Success.Description();
                result.ReturnMessage = ReturnMessage.DeleteSuccess.Description();
            }
            catch (NotFoundException ex)
            {
                result.ReturnCode = ReturnCode.NotFound.Description();
                result.ReturnMessage = ReturnMessage.NotFound.Description();
                LogHelper.Error(ex);
            }
            catch (Exception ex)
            {
                result.ReturnCode = ReturnCode.Fail.Description();
                result.ReturnMessage = ReturnMessage.DeleteFail.Description();
                LogHelper.Error(ex);
            }

            return result;
        }
    }
}


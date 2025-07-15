using Microsoft.AspNetCore.Mvc;
using Moq;
using RonWeb.API.Controllers;
using RonWeb.API.Interface.Article;
using RonWeb.API.Models.Article;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using Xunit;

namespace RonWeb.API.Tests.Controllers
{
    public class ArticleControllerTests
    {
        private readonly Mock<IArticleHelper> _mockArticleHelper;
        private readonly ArticleController _controller;

        public ArticleControllerTests()
        {
            _mockArticleHelper = new Mock<IArticleHelper>();
            _controller = new ArticleController(_mockArticleHelper.Object);
        }

        [Fact]
        public async Task GetArticle_WithValidParameters_ShouldReturnSuccessResponse()
        {
            // Arrange
            var page = 1;
            var keyword = "test";
            var articleResponse = new GetArticleResponse();

            _mockArticleHelper.Setup(x => x.GetListAsync(page, keyword))
                             .ReturnsAsync(articleResponse);

            // Act
            var result = await _controller.GetArticle(page, keyword);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ReturnCode.Success.Description(), result.ReturnCode);
            Assert.Equal(ReturnMessage.Success.Description(), result.ReturnMessage);
            Assert.Equal(articleResponse, result.Data);
        }

        [Fact]
        public async Task GetArticle_WithNullParameters_ShouldReturnSuccessResponse()
        {
            // Arrange
            var articleResponse = new GetArticleResponse();

            _mockArticleHelper.Setup(x => x.GetListAsync(null, null))
                             .ReturnsAsync(articleResponse);

            // Act
            var result = await _controller.GetArticle(null, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ReturnCode.Success.Description(), result.ReturnCode);
            Assert.Equal(ReturnMessage.Success.Description(), result.ReturnMessage);
            Assert.Equal(articleResponse, result.Data);
        }

        [Fact]
        public async Task GetArticle_ShouldCallHelperOnce()
        {
            // Arrange
            var page = 1;
            var keyword = "test";
            var articleResponse = new GetArticleResponse();

            _mockArticleHelper.Setup(x => x.GetListAsync(page, keyword))
                             .ReturnsAsync(articleResponse);

            // Act
            await _controller.GetArticle(page, keyword);

            // Assert
            _mockArticleHelper.Verify(x => x.GetListAsync(page, keyword), Times.Once);
        }

        [Fact]
        public async Task GetArticleById_WithValidId_ShouldReturnSuccessResponse()
        {
            // Arrange
            var id = 123L;
            var articleResponse = new GetByIdArticleResponse();

            _mockArticleHelper.Setup(x => x.GetAsync(id))
                             .ReturnsAsync(articleResponse);

            // Act
            var result = await _controller.GetArticleById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ReturnCode.Success.Description(), result.ReturnCode);
            Assert.Equal(ReturnMessage.Success.Description(), result.ReturnMessage);
            Assert.Equal(articleResponse, result.Data);
        }

        [Fact]
        public async Task GetArticleById_ShouldCallHelperOnce()
        {
            // Arrange
            var id = 123L;
            var articleResponse = new GetByIdArticleResponse();

            _mockArticleHelper.Setup(x => x.GetAsync(id))
                             .ReturnsAsync(articleResponse);

            // Act
            await _controller.GetArticleById(id);

            // Assert
            _mockArticleHelper.Verify(x => x.GetAsync(id), Times.Once);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(999)]
        [InlineData(long.MaxValue)]
        public async Task GetArticleById_WithVariousIds_ShouldReturnSuccessResponse(long id)
        {
            // Arrange
            var articleResponse = new GetByIdArticleResponse();

            _mockArticleHelper.Setup(x => x.GetAsync(id))
                             .ReturnsAsync(articleResponse);

            // Act
            var result = await _controller.GetArticleById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ReturnCode.Success.Description(), result.ReturnCode);
            Assert.Equal(ReturnMessage.Success.Description(), result.ReturnMessage);
        }

        [Fact]
        public async Task UpdateArticleViews_WithValidId_ShouldReturnSuccessResponse()
        {
            // Arrange
            var id = 123L;

            _mockArticleHelper.Setup(x => x.UpdateArticleViews(id))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateArticleViews(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ReturnCode.Success.Description(), result.ReturnCode);
            Assert.Equal(ReturnMessage.ModifySuccess.Description(), result.ReturnMessage);
        }

        [Fact]
        public async Task UpdateArticleViews_ShouldCallHelperOnce()
        {
            // Arrange
            var id = 123L;

            _mockArticleHelper.Setup(x => x.UpdateArticleViews(id))
                             .Returns(Task.CompletedTask);

            // Act
            await _controller.UpdateArticleViews(id);

            // Assert
            _mockArticleHelper.Verify(x => x.UpdateArticleViews(id), Times.Once);
        }

        [Fact]
        public async Task UpdateArticleViews_ShouldReturnBaseResponse()
        {
            // Arrange
            var id = 123L;

            _mockArticleHelper.Setup(x => x.UpdateArticleViews(id))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateArticleViews(id);

            // Assert
            Assert.IsType<BaseResponse>(result);
        }

        [Fact]
        public void Controller_ShouldHaveCorrectRouteAttribute()
        {
            // Act
            var routeAttribute = _controller.GetType().GetCustomAttributes(typeof(RouteAttribute), false).FirstOrDefault() as RouteAttribute;

            // Assert
            Assert.NotNull(routeAttribute);
            Assert.Equal("api/[controller]", routeAttribute.Template);
        }

        [Fact]
        public void GetArticle_Method_ShouldHaveHttpGetAttribute()
        {
            // Act
            var method = _controller.GetType().GetMethod("GetArticle");
            var httpGetAttribute = method.GetCustomAttributes(typeof(HttpGetAttribute), false).FirstOrDefault() as HttpGetAttribute;

            // Assert
            Assert.NotNull(httpGetAttribute);
        }

        [Fact]
        public void GetArticleById_Method_ShouldHaveHttpGetAttributeWithIdRoute()
        {
            // Act
            var method = _controller.GetType().GetMethod("GetArticleById");
            var httpGetAttribute = method.GetCustomAttributes(typeof(HttpGetAttribute), false).FirstOrDefault() as HttpGetAttribute;

            // Assert
            Assert.NotNull(httpGetAttribute);
            Assert.Equal("{id}", httpGetAttribute.Template);
        }

        [Fact]
        public void UpdateArticleViews_Method_ShouldHaveHttpPatchAttribute()
        {
            // Act
            var method = _controller.GetType().GetMethod("UpdateArticleViews");
            var httpPatchAttribute = method.GetCustomAttributes(typeof(HttpPatchAttribute), false).FirstOrDefault() as HttpPatchAttribute;

            // Assert
            Assert.NotNull(httpPatchAttribute);
        }

        [Fact]
        public async Task UpdateArticleViews_ShouldReturnCorrectReturnCodeAndMessage()
        {
            // Arrange
            var id = 123L;

            _mockArticleHelper.Setup(x => x.UpdateArticleViews(id))
                             .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateArticleViews(id);

            // Assert
            Assert.Equal("00", result.ReturnCode);
            Assert.Equal("修改資料成功", result.ReturnMessage);
        }

        [Fact]
        public async Task GetArticle_ShouldReturnCorrectReturnCodeAndMessage()
        {
            // Arrange
            var articleResponse = new GetArticleResponse();

            _mockArticleHelper.Setup(x => x.GetListAsync(null, null))
                             .ReturnsAsync(articleResponse);

            // Act
            var result = await _controller.GetArticle(null, null);

            // Assert
            Assert.Equal("00", result.ReturnCode);
            Assert.Equal("取得資料成功", result.ReturnMessage);
        }

        [Fact]
        public async Task GetArticleById_ShouldReturnCorrectReturnCodeAndMessage()
        {
            // Arrange
            var id = 123L;
            var articleResponse = new GetByIdArticleResponse();

            _mockArticleHelper.Setup(x => x.GetAsync(id))
                             .ReturnsAsync(articleResponse);

            // Act
            var result = await _controller.GetArticleById(id);

            // Assert
            Assert.Equal("00", result.ReturnCode);
            Assert.Equal("取得資料成功", result.ReturnMessage);
        }
    }
}
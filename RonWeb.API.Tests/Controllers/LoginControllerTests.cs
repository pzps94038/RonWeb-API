using Microsoft.AspNetCore.Mvc;
using Moq;
using RonWeb.API.Controllers;
using RonWeb.API.Interface.Login;
using RonWeb.API.Models.Login;
using RonWeb.API.Models.Shared;
using RonWeb.Core;
using Xunit;

namespace RonWeb.API.Tests.Controllers
{
    public class LoginControllerTests
    {
        private readonly Mock<ILoginHelper> _mockLoginHelper;
        private readonly LoginController _controller;

        public LoginControllerTests()
        {
            _mockLoginHelper = new Mock<ILoginHelper>();
            _controller = new LoginController(_mockLoginHelper.Object);
        }

        [Fact]
        public async Task Login_WithValidRequest_ShouldReturnSuccessResponse()
        {
            // Arrange
            var request = new LoginRequest
            {
                Account = "testuser",
                Password = "testpassword"
            };

            var loginResponse = new LoginResponse
            {
                Token = new Token("access_token", "refresh_token"),
                UserId = 123
            };

            _mockLoginHelper.Setup(x => x.Login(request))
                           .ReturnsAsync(loginResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ReturnCode.Success.Description(), result.ReturnCode);
            Assert.Equal(ReturnMessage.LoginSuccess.Description(), result.ReturnMessage);
            Assert.Equal(loginResponse, result.Data);
        }

        [Fact]
        public async Task Login_WithNullRequest_ShouldStillCallHelper()
        {
            // Arrange
            LoginRequest request = null;
            var loginResponse = new LoginResponse();

            _mockLoginHelper.Setup(x => x.Login(request))
                           .ReturnsAsync(loginResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.NotNull(result);
            _mockLoginHelper.Verify(x => x.Login(request), Times.Once);
        }

        [Fact]
        public async Task Login_ShouldCallLoginHelperOnce()
        {
            // Arrange
            var request = new LoginRequest
            {
                Account = "testuser",
                Password = "testpassword"
            };

            var loginResponse = new LoginResponse();

            _mockLoginHelper.Setup(x => x.Login(request))
                           .ReturnsAsync(loginResponse);

            // Act
            await _controller.Login(request);

            // Assert
            _mockLoginHelper.Verify(x => x.Login(request), Times.Once);
        }

        [Fact]
        public async Task Login_WithEmptyRequest_ShouldReturnSuccessResponse()
        {
            // Arrange
            var request = new LoginRequest();
            var loginResponse = new LoginResponse();

            _mockLoginHelper.Setup(x => x.Login(request))
                           .ReturnsAsync(loginResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ReturnCode.Success.Description(), result.ReturnCode);
            Assert.Equal(ReturnMessage.LoginSuccess.Description(), result.ReturnMessage);
            Assert.Equal(loginResponse, result.Data);
        }

        [Fact]
        public async Task Login_ShouldReturnBaseResponseWithCorrectType()
        {
            // Arrange
            var request = new LoginRequest();
            var loginResponse = new LoginResponse();

            _mockLoginHelper.Setup(x => x.Login(request))
                           .ReturnsAsync(loginResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.IsType<BaseResponse<LoginResponse>>(result);
        }

        [Fact]
        public async Task Login_WhenHelperThrowsException_ShouldPropagateException()
        {
            // Arrange
            var request = new LoginRequest();
            var exception = new Exception("Test exception");

            _mockLoginHelper.Setup(x => x.Login(request))
                           .ThrowsAsync(exception);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Login(request));
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
        public void Login_Method_ShouldHaveHttpPostAttribute()
        {
            // Act
            var method = _controller.GetType().GetMethod("Login");
            var httpPostAttribute = method.GetCustomAttributes(typeof(HttpPostAttribute), false).FirstOrDefault() as HttpPostAttribute;

            // Assert
            Assert.NotNull(httpPostAttribute);
        }

        [Fact]
        public async Task Login_ShouldReturnCorrectReturnCodeAndMessage()
        {
            // Arrange
            var request = new LoginRequest();
            var loginResponse = new LoginResponse();

            _mockLoginHelper.Setup(x => x.Login(request))
                           .ReturnsAsync(loginResponse);

            // Act
            var result = await _controller.Login(request);

            // Assert
            Assert.Equal("00", result.ReturnCode);
            Assert.Equal("登入成功", result.ReturnMessage);
        }
    }
}
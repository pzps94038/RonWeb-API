using RonWeb.API.Models.Shared;
using RonWeb.Core;
using Xunit;

namespace RonWeb.API.Tests.Models.Shared
{
    public class BaseResponseTests
    {
        [Fact]
        public void BaseResponse_DefaultConstructor_ShouldInitializeWithEmptyValues()
        {
            // Act
            var response = new BaseResponse();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(string.Empty, response.ReturnCode);
            Assert.Equal(string.Empty, response.ReturnMessage);
        }

        [Fact]
        public void BaseResponse_ParameterizedConstructor_ShouldInitializeWithGivenValues()
        {
            // Arrange
            var returnCode = "00";
            var returnMessage = "Success";

            // Act
            var response = new BaseResponse(returnCode, returnMessage);

            // Assert
            Assert.Equal(returnCode, response.ReturnCode);
            Assert.Equal(returnMessage, response.ReturnMessage);
        }

        [Fact]
        public void BaseResponse_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var returnCode = "99";
            var returnMessage = "Error occurred";

            // Act
            var response = new BaseResponse
            {
                ReturnCode = returnCode,
                ReturnMessage = returnMessage
            };

            // Assert
            Assert.Equal(returnCode, response.ReturnCode);
            Assert.Equal(returnMessage, response.ReturnMessage);
        }

        [Fact]
        public void BaseResponseGeneric_DefaultConstructor_ShouldInitializeWithEmptyValues()
        {
            // Act
            var response = new BaseResponse<string>();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(string.Empty, response.ReturnCode);
            Assert.Equal(string.Empty, response.ReturnMessage);
            Assert.Equal(default(string), response.Data);
        }

        [Fact]
        public void BaseResponseGeneric_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var returnCode = "00";
            var returnMessage = "Success";
            var data = "Test data";

            // Act
            var response = new BaseResponse<string>
            {
                ReturnCode = returnCode,
                ReturnMessage = returnMessage,
                Data = data
            };

            // Assert
            Assert.Equal(returnCode, response.ReturnCode);
            Assert.Equal(returnMessage, response.ReturnMessage);
            Assert.Equal(data, response.Data);
        }

        [Fact]
        public void BaseResponseGeneric_WithComplexType_ShouldWorkCorrectly()
        {
            // Arrange
            var data = new { Name = "Test", Value = 123 };

            // Act
            var response = new BaseResponse<object>
            {
                Data = data
            };

            // Assert
            Assert.Equal(data, response.Data);
        }

        [Fact]
        public void ReturnCode_EnumValues_ShouldHaveCorrectDescriptions()
        {
            // Assert
            Assert.Equal("00", ReturnCode.Success.Description());
            Assert.Equal("96", ReturnCode.AuthExpired.Description());
            Assert.Equal("97", ReturnCode.Unique.Description());
            Assert.Equal("98", ReturnCode.NotFound.Description());
            Assert.Equal("99", ReturnCode.Fail.Description());
        }

        [Fact]
        public void ReturnMessage_EnumValues_ShouldHaveCorrectDescriptions()
        {
            // Assert
            Assert.Equal("取得資料成功", ReturnMessage.Success.Description());
            Assert.Equal("取得資料失敗", ReturnMessage.Fail.Description());
            Assert.Equal("登入成功", ReturnMessage.LoginSuccess.Description());
            Assert.Equal("帳號或密碼錯誤", ReturnMessage.LoginFail.Description());
            Assert.Equal("身分驗證過期", ReturnMessage.AuthExpired.Description());
            Assert.Equal("系統發生錯誤", ReturnMessage.SystemFail.Description());
        }

        [Theory]
        [InlineData(ReturnCode.Success, "00")]
        [InlineData(ReturnCode.AuthExpired, "96")]
        [InlineData(ReturnCode.Unique, "97")]
        [InlineData(ReturnCode.NotFound, "98")]
        [InlineData(ReturnCode.Fail, "99")]
        public void ReturnCode_EnumValues_ShouldMatchExpectedDescriptions(ReturnCode enumValue, string expectedDescription)
        {
            // Act
            var description = enumValue.Description();

            // Assert
            Assert.Equal(expectedDescription, description);
        }

        [Theory]
        [InlineData(ReturnMessage.Success, "取得資料成功")]
        [InlineData(ReturnMessage.LoginSuccess, "登入成功")]
        [InlineData(ReturnMessage.LoginFail, "帳號或密碼錯誤")]
        [InlineData(ReturnMessage.NotFound, "找不到資料")]
        [InlineData(ReturnMessage.SystemFail, "系統發生錯誤")]
        public void ReturnMessage_EnumValues_ShouldMatchExpectedDescriptions(ReturnMessage enumValue, string expectedDescription)
        {
            // Act
            var description = enumValue.Description();

            // Assert
            Assert.Equal(expectedDescription, description);
        }
    }
}
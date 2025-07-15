using RonWeb.Core;
using Xunit;

namespace RonWeb.Core.Tests
{
    public class FireBaseStorageTests
    {
        [Fact]
        public void FireBaseStorageUrl_DefaultConstructor_ShouldInitializeWithEmptyValues()
        {
            // Act
            var storageUrl = new FireBaseStorageUrl();

            // Assert
            Assert.NotNull(storageUrl);
            Assert.Equal(string.Empty, storageUrl.Path);
            Assert.Equal(string.Empty, storageUrl.Url);
        }

        [Fact]
        public void FireBaseStorageUrl_Properties_ShouldBeSetCorrectly()
        {
            // Arrange
            var path = "images/test.jpg";
            var url = "https://firebasestorage.googleapis.com/v0/b/test-bucket/o/images%2Ftest.jpg?alt=media";

            // Act
            var storageUrl = new FireBaseStorageUrl
            {
                Path = path,
                Url = url
            };

            // Assert
            Assert.Equal(path, storageUrl.Path);
            Assert.Equal(url, storageUrl.Url);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("test.jpg", "")]
        [InlineData("", "https://example.com/file.jpg")]
        [InlineData("folder/subfolder/file.png", "https://firebase.com/file.png")]
        public void FireBaseStorageUrl_WithVariousValues_ShouldSetPropertiesCorrectly(string path, string url)
        {
            // Act
            var storageUrl = new FireBaseStorageUrl
            {
                Path = path,
                Url = url
            };

            // Assert
            Assert.Equal(path, storageUrl.Path);
            Assert.Equal(url, storageUrl.Url);
        }

        [Fact]
        public void FireBaseStorageUrl_Properties_ShouldBeMutable()
        {
            // Arrange
            var storageUrl = new FireBaseStorageUrl
            {
                Path = "initial/path.jpg",
                Url = "https://initial.com/path.jpg"
            };

            // Act
            storageUrl.Path = "new/path.png";
            storageUrl.Url = "https://new.com/path.png";

            // Assert
            Assert.Equal("new/path.png", storageUrl.Path);
            Assert.Equal("https://new.com/path.png", storageUrl.Url);
        }

        [Fact]
        public void FireBaseStorageTool_Constructor_ShouldInitializeWithStorageBucket()
        {
            // Arrange
            var storageBucket = "test-bucket.appspot.com";

            // Act
            var tool = new FireBaseStorageTool(storageBucket);

            // Assert
            Assert.NotNull(tool);
            // Note: We can't test the internal _storage property directly as it's private
            // but we can verify the object was created successfully
        }

        [Fact]
        public void FireBaseStorageTool_Constructor_WithEmptyBucket_ShouldStillInitialize()
        {
            // Arrange
            var storageBucket = "";

            // Act
            var tool = new FireBaseStorageTool(storageBucket);

            // Assert
            Assert.NotNull(tool);
        }

        [Fact]
        public void FireBaseStorageTool_Constructor_WithNullBucket_ShouldStillInitialize()
        {
            // Arrange
            string storageBucket = null;

            // Act
            var tool = new FireBaseStorageTool(storageBucket);

            // Assert
            Assert.NotNull(tool);
        }

        // Note: Testing the actual Upload and Delete methods would require Firebase integration
        // which is beyond the scope of unit tests and would require integration tests instead.
        // The methods interact with external services, so they're not suitable for unit testing
        // without mocking the Firebase SDK, which is complex and not necessary for this basic test coverage.
    }
}
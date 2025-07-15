using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using RonWeb.Core;
using Xunit;

namespace RonWeb.Core.Tests
{
    public class EncryptToolTests
    {
        [Fact]
        public void GenerateRsaKey_ShouldReturnValidRsaKey()
        {
            // Act
            var rsaKey = EncryptTool.GenerateRsaKey();

            // Assert
            Assert.NotNull(rsaKey);
            Assert.NotEmpty(rsaKey.PublicKey);
            Assert.NotEmpty(rsaKey.PrivateKey);
            
            // Verify they are valid base64 strings
            var publicKeyBytes = Convert.FromBase64String(rsaKey.PublicKey);
            var privateKeyBytes = Convert.FromBase64String(rsaKey.PrivateKey);
            
            Assert.True(publicKeyBytes.Length > 0);
            Assert.True(privateKeyBytes.Length > 0);
        }

        [Fact]
        public void GenerateRsaKey_ShouldReturnDifferentKeysOnMultipleCalls()
        {
            // Act
            var rsaKey1 = EncryptTool.GenerateRsaKey();
            var rsaKey2 = EncryptTool.GenerateRsaKey();

            // Assert
            Assert.NotEqual(rsaKey1.PublicKey, rsaKey2.PublicKey);
            Assert.NotEqual(rsaKey1.PrivateKey, rsaKey2.PrivateKey);
        }

        [Fact]
        public void SHA256Encrypt_WithValidText_ShouldReturnBase64Hash()
        {
            // Arrange
            var plainText = "Hello, World!";

            // Act
            var hash = EncryptTool.SHA256Encrypt(plainText);

            // Assert
            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
            
            // Verify it's a valid base64 string
            var bytes = Convert.FromBase64String(hash);
            Assert.Equal(32, bytes.Length); // SHA256 produces 32 bytes
        }

        [Fact]
        public void SHA256Encrypt_WithSameInput_ShouldReturnSameHash()
        {
            // Arrange
            var plainText = "Hello, World!";

            // Act
            var hash1 = EncryptTool.SHA256Encrypt(plainText);
            var hash2 = EncryptTool.SHA256Encrypt(plainText);

            // Assert
            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void SHA256Encrypt_WithDifferentInput_ShouldReturnDifferentHash()
        {
            // Arrange
            var plainText1 = "Hello, World!";
            var plainText2 = "Hello, World!!";

            // Act
            var hash1 = EncryptTool.SHA256Encrypt(plainText1);
            var hash2 = EncryptTool.SHA256Encrypt(plainText2);

            // Assert
            Assert.NotEqual(hash1, hash2);
        }

        [Fact]
        public void AESEncrypt_And_AESDecrypt_WithValidString_ShouldWorkCorrectly()
        {
            // Arrange
            var plainText = "Hello, World!";
            var key = "12345678901234567890123456789012"; // 32 characters for AES-256
            var iv = "1234567890123456"; // 16 characters for AES

            // Act
            var encrypted = EncryptTool.AESEncrypt(plainText, key, iv);
            var decrypted = EncryptTool.AESDecrypt(encrypted, key, iv);

            // Assert
            Assert.NotNull(encrypted);
            Assert.NotEmpty(encrypted);
            Assert.Equal(plainText, decrypted);
        }

        [Fact]
        public void AESEncrypt_And_AESDecrypt_WithValidObject_ShouldWorkCorrectly()
        {
            // Arrange
            var testObject = new { Name = "Test", Value = 123 };
            var key = "12345678901234567890123456789012"; // 32 characters for AES-256
            var iv = "1234567890123456"; // 16 characters for AES

            // Act
            var encrypted = EncryptTool.AESEncrypt(testObject, key, iv);
            var decrypted = EncryptTool.AESDecrypt<object>(encrypted, key, iv);

            // Assert
            Assert.NotNull(encrypted);
            Assert.NotEmpty(encrypted);
            Assert.NotNull(decrypted);
            
            // Convert back to check values
            var decryptedJson = JsonConvert.SerializeObject(decrypted);
            var originalJson = JsonConvert.SerializeObject(testObject);
            Assert.Equal(originalJson, decryptedJson);
        }

        [Fact]
        public void AESDecrypt_WithInvalidCipherText_ShouldThrowException()
        {
            // Arrange
            var invalidCipherText = "InvalidBase64String";
            var key = "12345678901234567890123456789012";
            var iv = "1234567890123456";

            // Act & Assert
            Assert.Throws<FormatException>(() => EncryptTool.AESDecrypt(invalidCipherText, key, iv));
        }

        [Fact]
        public void AESDecrypt_WithInvalidJsonForObject_ShouldThrowAESConvertDataException()
        {
            // Arrange
            var plainText = "InvalidJson{";
            var key = "12345678901234567890123456789012";
            var iv = "1234567890123456";
            
            var encrypted = EncryptTool.AESEncrypt(plainText, key, iv);

            // Act & Assert
            Assert.Throws<JsonReaderException>(() => EncryptTool.AESDecrypt<object>(encrypted, key, iv));
        }

        [Fact]
        public void AESEncrypt_WithDifferentKeys_ShouldProduceDifferentResults()
        {
            // Arrange
            var plainText = "Hello, World!";
            var key1 = "12345678901234567890123456789012";
            var key2 = "12345678901234567890123456789013";
            var iv = "1234567890123456";

            // Act
            var encrypted1 = EncryptTool.AESEncrypt(plainText, key1, iv);
            var encrypted2 = EncryptTool.AESEncrypt(plainText, key2, iv);

            // Assert
            Assert.NotEqual(encrypted1, encrypted2);
        }

        [Fact]
        public void RsaKey_Constructor_ShouldSetProperties()
        {
            // Arrange
            var publicKey = "TestPublicKey";
            var privateKey = "TestPrivateKey";

            // Act
            var rsaKey = new RsaKey(publicKey, privateKey);

            // Assert
            Assert.Equal(publicKey, rsaKey.PublicKey);
            Assert.Equal(privateKey, rsaKey.PrivateKey);
        }

        [Fact]
        public void AESConvertDataException_ShouldHaveCorrectMessage()
        {
            // Act
            var exception = new AESConvertDataException();

            // Assert
            Assert.Equal("AES轉換資料失敗", exception.Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData("Hello")]
        [InlineData("This is a longer test string with numbers 123 and symbols !@#$%")]
        public void SHA256Encrypt_WithVariousInputs_ShouldReturnValidHash(string input)
        {
            // Act
            var hash = EncryptTool.SHA256Encrypt(input);

            // Assert
            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
            
            // Verify it's a valid base64 string representing 32 bytes
            var bytes = Convert.FromBase64String(hash);
            Assert.Equal(32, bytes.Length);
        }

        [Fact]
        public void AESEncrypt_WithEmptyString_ShouldWorkCorrectly()
        {
            // Arrange
            var plainText = "";
            var key = "12345678901234567890123456789012";
            var iv = "1234567890123456";

            // Act
            var encrypted = EncryptTool.AESEncrypt(plainText, key, iv);
            var decrypted = EncryptTool.AESDecrypt(encrypted, key, iv);

            // Assert
            Assert.NotNull(encrypted);
            Assert.NotEmpty(encrypted);
            Assert.Equal(plainText, decrypted);
        }
    }
}
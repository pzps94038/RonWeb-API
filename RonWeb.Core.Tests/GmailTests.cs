using System.Net.Mail;
using RonWeb.Core;
using Xunit;

namespace RonWeb.Core.Tests
{
    public class GmailTests
    {
        [Fact]
        public void GMail_Constructor_ShouldInitializeWithCorrectValues()
        {
            // Arrange
            var address = "test@example.com";
            var displayName = "Test User";
            var senderEmail = "sender@example.com";
            var password = "testpassword";

            // Act
            var gmail = new GMail(address, displayName, senderEmail, password);

            // Assert
            Assert.Equal(address, gmail.From.Address);
            Assert.Equal(displayName, gmail.From.DisplayName);
            Assert.Equal(senderEmail, gmail.SenderEmail);
            Assert.Equal(password, gmail.GmailSmtPwd);
        }

        [Fact]
        public void GMail_DefaultProperties_ShouldBeInitializedCorrectly()
        {
            // Arrange
            var address = "test@example.com";
            var displayName = "Test User";
            var senderEmail = "sender@example.com";
            var password = "testpassword";

            // Act
            var gmail = new GMail(address, displayName, senderEmail, password);

            // Assert
            Assert.NotNull(gmail.Emails);
            Assert.Empty(gmail.Emails);
            Assert.Equal(string.Empty, gmail.Subject);
            Assert.Equal(string.Empty, gmail.Body);
            Assert.True(gmail.IsBodyHtml);
            Assert.Equal(MailPriority.Normal, gmail.Priority);
            Assert.NotNull(gmail.AttachmentPaths);
            Assert.Empty(gmail.AttachmentPaths);
        }

        [Fact]
        public void GMail_Properties_ShouldBeSettable()
        {
            // Arrange
            var gmail = new GMail("test@example.com", "Test User", "sender@example.com", "password");
            var emails = new List<string> { "recipient1@example.com", "recipient2@example.com" };
            var subject = "Test Subject";
            var body = "Test Body";
            var attachments = new List<string> { "file1.txt", "file2.txt" };

            // Act
            gmail.Emails = emails;
            gmail.Subject = subject;
            gmail.Body = body;
            gmail.IsBodyHtml = false;
            gmail.Priority = MailPriority.High;
            gmail.AttachmentPaths = attachments;

            // Assert
            Assert.Equal(emails, gmail.Emails);
            Assert.Equal(subject, gmail.Subject);
            Assert.Equal(body, gmail.Body);
            Assert.False(gmail.IsBodyHtml);
            Assert.Equal(MailPriority.High, gmail.Priority);
            Assert.Equal(attachments, gmail.AttachmentPaths);
        }

        [Fact]
        public void GMail_EmailsList_ShouldAllowAddingEmails()
        {
            // Arrange
            var gmail = new GMail("test@example.com", "Test User", "sender@example.com", "password");

            // Act
            gmail.Emails.Add("recipient1@example.com");
            gmail.Emails.Add("recipient2@example.com");

            // Assert
            Assert.Equal(2, gmail.Emails.Count);
            Assert.Contains("recipient1@example.com", gmail.Emails);
            Assert.Contains("recipient2@example.com", gmail.Emails);
        }

        [Fact]
        public void GMail_AttachmentPaths_ShouldAllowAddingPaths()
        {
            // Arrange
            var gmail = new GMail("test@example.com", "Test User", "sender@example.com", "password");

            // Act
            gmail.AttachmentPaths.Add("file1.txt");
            gmail.AttachmentPaths.Add("file2.pdf");

            // Assert
            Assert.Equal(2, gmail.AttachmentPaths.Count);
            Assert.Contains("file1.txt", gmail.AttachmentPaths);
            Assert.Contains("file2.pdf", gmail.AttachmentPaths);
        }

        [Theory]
        [InlineData(MailPriority.Low)]
        [InlineData(MailPriority.Normal)]
        [InlineData(MailPriority.High)]
        public void GMail_Priority_ShouldAcceptValidMailPriorities(MailPriority priority)
        {
            // Arrange
            var gmail = new GMail("test@example.com", "Test User", "sender@example.com", "password");

            // Act
            gmail.Priority = priority;

            // Assert
            Assert.Equal(priority, gmail.Priority);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GMail_IsBodyHtml_ShouldAcceptBooleanValues(bool isBodyHtml)
        {
            // Arrange
            var gmail = new GMail("test@example.com", "Test User", "sender@example.com", "password");

            // Act
            gmail.IsBodyHtml = isBodyHtml;

            // Assert
            Assert.Equal(isBodyHtml, gmail.IsBodyHtml);
        }

        [Fact]
        public void GMail_FromProperty_ShouldBeCorrectlyInitialized()
        {
            // Arrange
            var address = "test@example.com";
            var displayName = "Test User";

            // Act
            var gmail = new GMail(address, displayName, "sender@example.com", "password");

            // Assert
            Assert.NotNull(gmail.From);
            Assert.Equal(address, gmail.From.Address);
            Assert.Equal(displayName, gmail.From.DisplayName);
        }
    }
}
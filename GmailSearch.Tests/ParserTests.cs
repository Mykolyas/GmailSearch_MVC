using FluentAssertions;
using Google.Apis.Gmail.v1.Data;
using System.Text;
using WebApplication1.Models;
using WebApplication1.Services;
using WebApplication1.Interfaces;
using Xunit;

namespace WebApplication1.Tests.Services
{
    public class EmailParserTests
    {
        private readonly IEmailParser _parser = new EmailParser();

        [Fact]
        public void ParseMetadata_ShouldExtractCorrectValues()
        {
            var message = new Message
            {
                Id = "test123",
                Payload = new MessagePart
                {
                    Headers = new List<MessagePartHeader>
                    {
                        new() { Name = "Subject", Value = "Test Subject" },
                        new() { Name = "From", Value = "sender@example.com" },
                        new() { Name = "Date", Value = "Fri, 12 Jul 2024 10:00:00 +0000" }
                    }
                }
            };

            var result = _parser.ParseMetadata(message);

            result.Should().NotBeNull();
            result.Id.Should().Be("test123");
            result.Subject.Should().Be("Test Subject");
            result.From.Should().Be("sender@example.com");
            result.Date.Should().Be("Fri, 12 Jul 2024 10:00:00 +0000");
        }

        [Fact]
        public void ParseHtmlBody_ShouldReturnDecodedHtml_WhenValidHtmlProvided()
        {
            // Arrange
            var htmlContent = "<h1>Hello World</h1>";
            var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(htmlContent))
                .Replace("+", "-").Replace("/", "_"); // Gmail-safe base64

            var message = new Message
            {
                Payload = new MessagePart
                {
                    MimeType = "text/html",
                    Body = new MessagePartBody
                    {
                        Data = encoded
                    }
                }
            };

            // Act
            var result = _parser.ParseHtmlBody(message);

            // Assert
            result.Should().Be(htmlContent);
        }

        [Fact]
        public void ParseHtmlBody_ShouldReturnFallback_WhenNoHtmlExists()
        {
            // Arrange
            var message = new Message
            {
                Payload = new MessagePart
                {
                    MimeType = "text/plain",
                    Body = new MessagePartBody { Data = null }
                }
            };

            // Act
            var result = _parser.ParseHtmlBody(message);

            // Assert
            result.Should().Contain("No HTML content");
        }
    }
}

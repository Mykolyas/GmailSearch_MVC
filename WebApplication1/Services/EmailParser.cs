using Google.Apis.Gmail.v1.Data;
using System.Text;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class EmailParser : IEmailParser
    {
        public EmailViewModel ParseMetadata(Message message)
        {
            var headers = message.Payload?.Headers ?? new List<MessagePartHeader>();

            return new EmailViewModel
            {
                Id = message.Id,
                Subject = headers.FirstOrDefault(h => h.Name == "Subject")?.Value ?? "(No Subject)",
                From = headers.FirstOrDefault(h => h.Name == "From")?.Value ?? "(Unknown Sender)",
                Date = headers.FirstOrDefault(h => h.Name == "Date")?.Value ?? "(No Date)"
            };
        }

        public string ParseHtmlBody(Message message)
        {
            if (message.Payload == null)
                return "<p><em>No content found in this message.</em></p>";

            string? htmlBody = null;

            // Priority on Part with MimeType text/html
            var htmlPart = message.Payload.Parts?.FirstOrDefault(p => p.MimeType == "text/html");
            htmlBody = htmlPart?.Body?.Data;

            // If MimeType = text/html without Part
            if (htmlBody == null && message.Payload.MimeType == "text/html")
                htmlBody = message.Payload.Body?.Data;

            if (string.IsNullOrEmpty(htmlBody))
                return "<p><em><h1>No HTML content found in this email.</h1></em></p>";

            try
            {
                // Gmail encodes in web-safe base64
                var fixedBase64 = htmlBody.Replace("-", "+").Replace("_", "/");
                var bytes = Convert.FromBase64String(fixedBase64);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (FormatException)
            {
                return "<p><em>Failed to decode email content.</em></p>";
            }
        }
    }
}

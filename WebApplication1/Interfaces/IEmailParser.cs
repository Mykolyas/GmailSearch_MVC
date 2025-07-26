using Google.Apis.Gmail.v1.Data;
using WebApplication1.Models;

namespace WebApplication1.Interfaces
{
    public interface IEmailParser
    {
        EmailViewModel ParseMetadata(Message message);
        string ParseHtmlBody(Message message);
    }
} 
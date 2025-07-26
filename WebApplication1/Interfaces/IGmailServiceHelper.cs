using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;

namespace WebApplication1.Interfaces
{
    public interface IGmailServiceHelper
    {
        Task<IList<Message>> SearchMessages(GmailService service, string query);
        Task<Message?> GetMessage(GmailService service, string id);
    }
} 
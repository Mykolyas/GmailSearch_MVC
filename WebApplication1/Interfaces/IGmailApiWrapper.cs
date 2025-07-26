using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;

namespace WebApplication1.Interfaces
{
    public interface IGmailApiWrapper
    {
        Task<IList<Message>> SearchMessages(UsersResource.MessagesResource.ListRequest request);
        Task<Message> GetMessage(UsersResource.MessagesResource.GetRequest request);
        GmailService CreateService(string accessToken);
    }
} 
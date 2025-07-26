using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;
using WebApplication1.Interfaces;

namespace WebApplication1.Wrappers
{
    public class GmailApiWrapper : IGmailApiWrapper
    {
        public async Task<IList<Message>> SearchMessages(UsersResource.MessagesResource.ListRequest request)
        {
            var response = await request.ExecuteAsync();
            return response.Messages ?? new List<Message>();
        }

        public async Task<Message> GetMessage(UsersResource.MessagesResource.GetRequest request)
        {
            return await request.ExecuteAsync();
        }

        public GmailService CreateService(string accessToken)
        {
            var credential = Google.Apis.Auth.OAuth2.GoogleCredential.FromAccessToken(accessToken);
            return new GmailService(new Google.Apis.Services.BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "GmailSearchApp"
            });
        }
    }
} 
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Gmail.v1;
using Google;
using WebApplication1.Interfaces;
using WebApplication1.Wrappers;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;

namespace WebApplication1.Services
{
    public class GmailServiceHelper : IGmailServiceHelper
    {
        private readonly IGmailApiWrapper _apiWrapper;

        public GmailServiceHelper(IGmailApiWrapper apiWrapper)
        {
            _apiWrapper = apiWrapper;
        }

        public async Task<IList<Message>> SearchMessages(GmailService service, string query)
        {
            try
            {
                var request = service.Users.Messages.List("me");
                request.Q = query;
                return await _apiWrapper.SearchMessages(request);
            }
            catch (GoogleApiException ex)
            {
                Console.WriteLine($"[Gmail API Error] Failed to fetch messages: {ex.Message}");
                return new List<Message>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Unknown Error in SearchMessages] {ex.Message}");
                return new List<Message>();
            }
        }

        public async Task<Message?> GetMessage(GmailService service, string id)
        {
            try
            {
                var request = service.Users.Messages.Get("me", id);
                return await _apiWrapper.GetMessage(request);
            }
            catch (GoogleApiException ex)
            {
                Console.WriteLine($"[Gmail API Error] Failed to fetch message: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Unknown Error in GetMessage] {ex.Message}");
                return null;
            }
        }
    }
}
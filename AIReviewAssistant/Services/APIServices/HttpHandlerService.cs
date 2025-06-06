using AIReviewAssistant.Interfaces.ApiService;
using System.Text;

namespace AIReviewAssistant.Services.APIServices
{
    public class HttpHandlerService : IHttpHandler
    {
        public async Task<HttpResponseMessage> SendPostRequest(string url, string jsonBody, string? token)
        {
            using var httpClient = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            if (token != null)
                requestMessage.Headers.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.SendAsync(requestMessage);
            return response;
        }
    }
}

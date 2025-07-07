using AIReviewAssistant.Interfaces.ApiService;
using System.Net.Http.Headers;
using System.Text;

namespace AIReviewAssistant.Services.APIServices
{
    public class HttpHandlerService : IHttpHandler
    {
        public async Task<HttpResponseMessage> SendGetRequest(string url, string? token)
        {
            using var httpClient = new HttpClient();
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);

            requestMessage.Headers.UserAgent.ParseAdd("AIReviewAssistant/1.0");

            if (!string.IsNullOrEmpty(token))
            {
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await httpClient.SendAsync(requestMessage);
        }

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

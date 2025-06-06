namespace AIReviewAssistant.Interfaces.ApiService
{
    public interface IHttpHandler
    {
        Task<HttpResponseMessage> SendPostRequest(string url, string jsonBody, string token);
    }
}

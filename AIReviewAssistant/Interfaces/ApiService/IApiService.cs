using Newtonsoft.Json.Linq;

namespace AIReviewAssistant.Interfaces.ApiService
{
    public interface IApiService
    {
        Task<JObject> PostRequestAPI(string url, string jsonBody, string? token);

    }
}

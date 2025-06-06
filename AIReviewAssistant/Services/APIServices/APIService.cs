using AIReviewAssistant.Interfaces.ApiService;
using AIReviewAssistant.Interfaces.ErrorLogging;
using AIReviewAssistant.Models;
using AIReviewAssistant.Services.DatabaseServices;
using Newtonsoft.Json.Linq;

namespace AIReviewAssistant.Services.APIServices
{
    public class APIService : IApiService
    {
        private readonly IHttpHandler _handler;
        private readonly IErrorLoggingMongoService _errorLoggingMongoService;
        public APIService(IHttpHandler handler, IErrorLoggingMongoService errorLoggingMongo)
        {
            _handler = handler;
            _errorLoggingMongoService = errorLoggingMongo;
        }

        public async Task<JObject> PostRequestAPI(string url, string jsonBody, string? token)
        {
            try
            {
                var httpResponse = await _handler.SendPostRequest(url, jsonBody, token);
                var responseObject = new JObject
                {
                    { "isSuccess", false }
                };

                if (httpResponse != null)
                {
                    responseObject["isSuccess"] = httpResponse.IsSuccessStatusCode;
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    responseObject.Add("Content", content);
                }

                return responseObject;

            }
            catch(Exception ex)
            {
                var error = new ErrorLogs
                {
                    Service = nameof(APIService),
                    Operation = nameof(PostRequestAPI),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace ?? "No Trace",
                    InnerException = ex.InnerException?.Message,
                    RequestContent = $"Failed in API Service with exception message = {ex.Message}"
                };

                return new JObject
                {
                    {"isSuccess" , false }
                };

            }
        }
    }
}

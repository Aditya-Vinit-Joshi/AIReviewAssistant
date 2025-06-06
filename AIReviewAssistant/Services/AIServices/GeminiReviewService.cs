using AIReviewAssistant.Interfaces.AI;
using AIReviewAssistant.Interfaces.ApiService;
using AIReviewAssistant.Interfaces.ErrorLogging;
using AIReviewAssistant.Models;
using AIReviewAssistant.Services.DatabaseServices;
using AIReviewAssistant.Constants;
using System.Text;
using DotNetEnv;
using System.Diagnostics;
using Newtonsoft.Json;
using AIReviewAssistant.Dtos;

namespace AIReviewAssistant.Services.AIServices
{
    public class GeminiReviewService : IAIReviewService
    {

        private readonly IErrorLoggingMongoService _errorLoggingMongoService;
        private readonly IApiService _apiService;

        public GeminiReviewService(IErrorLoggingMongoService errorLoggingMongoService, IApiService apiService)
        {
            _errorLoggingMongoService = errorLoggingMongoService;
            _apiService = apiService;
        }

        private string GeminiPromptBuilder(List<CodeFile> codeFiles, string language)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"You are an expert software engineer. Review the following {language} code.");
            sb.AppendLine($"For each issue found, return a JSON object with: file name, line number, comment, suggested fix");
            sb.AppendLine("Respond with JSON objects like this:");
            sb.AppendLine("[{\"FileName\": \"Program.py\", \"LineNumber\":12, \"Comment\": \"Some comment based on the code review\", \"SuggestedFix\":\"Some suggested fix based on what is suggested\"}]");
            sb.AppendLine("Else if the code is absolutely perfect then respond with []");
            foreach(var file in codeFiles)
            {
                sb.AppendLine($"\nFile Name: {file.FileName}");
                sb.AppendLine(file.Content);
            }

            return sb.ToString();
        }

        public async Task<List<InlineComments>> GenerateAIReview(ReviewRequestDto dto)
        {
            try
            {
                Env.Load();
                string geminiAPIKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY") ?? "";
                if (string.IsNullOrEmpty(geminiAPIKey))
                    throw new Exception("Gemini API key not found");
                string promptToFeed = GeminiPromptBuilder(dto.codeFiles, dto.Language);
                var geminiPrompt = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new {text = promptToFeed}
                            }
                        }
                    }
                };

                string stringifiedPromptToFeed = JsonConvert.SerializeObject(geminiPrompt);
                var response = await _apiService.PostRequestAPI(Constants.Constants.GeminiAPIUrl + $"{geminiAPIKey}", stringifiedPromptToFeed, null);
                if (!Convert.ToBoolean(response["isSuccess"]))
                {
                    var error = new ErrorLogs
                    {
                        Service = nameof(GeminiReviewService),
                        Operation = nameof(GenerateAIReview),
                        ErrorMessage = "Could Not Get Content from Gemini Model",
                        StackTrace = "No Trace",
                        InnerException = null,
                        RequestContent = "Gemini Issue"
                    };

                    await _errorLoggingMongoService.LogErrorAsync(error);
                    return new List<InlineComments>();
                }

                var json = response["Content"]?.ToString();
                return JsonConvert.DeserializeObject<List<InlineComments>>(json ?? "[]") ?? new();
            }
            catch (Exception ex)
            {
                var error = new ErrorLogs
                {
                    Service = nameof(GeminiReviewService),
                    Operation = nameof(GenerateAIReview),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace ?? "No Trace",
                    InnerException = ex.InnerException?.Message,
                    RequestContent = "GenerateAIReview Error"
                };
                await _errorLoggingMongoService.LogErrorAsync(error);
                return new List<InlineComments>();
            }
            
        }
    }
}

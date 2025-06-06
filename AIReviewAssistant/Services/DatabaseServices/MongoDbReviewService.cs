using AIReviewAssistant.Interfaces.DatabaseServices;
using AIReviewAssistant.Interfaces.ErrorLogging;
using AIReviewAssistant.Models;
using AIReviewAssistant.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AIReviewAssistant.Services.DatabaseServices
{
    public class MongoDbReviewService : IMongoDbReviewService
    {
        private readonly IMongoCollection<ReviewRecord> _collection;
        private readonly IErrorLoggingMongoService _errorLoggingMongoService;
        
        public MongoDbReviewService(IOptions<MongoDbSettings> settings, 
            IMongoClient client,
            IErrorLoggingMongoService errorLoggingMongoService) { 
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _collection = database.GetCollection<ReviewRecord>(settings.Value.CollectionName);
            _errorLoggingMongoService = errorLoggingMongoService;
        }

        public async Task<bool> InsertReviewAsync(ReviewRecord record)
        {
            try
            {
               await _collection.InsertOneAsync(record);
                return true;
            }
            catch (Exception ex)
            {
                var error = new ErrorLogs
                {
                    Service = nameof(MongoDbReviewService),
                    Operation = nameof(InsertReviewAsync),
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace ?? "No Trace",
                    InnerException = ex.InnerException?.Message,
                    RequestContent = $"Repo : {record.RepoName}, PR : {record.RepoName}, User : {record.UserId}"
                };
                await _errorLoggingMongoService.LogErrorAsync(error);
                return false;
            }
            
        }
    }
}

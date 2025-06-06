using AIReviewAssistant.Interfaces.ErrorLogging;
using AIReviewAssistant.Models;
using AIReviewAssistant.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AIReviewAssistant.Services.LoggingServices
{
    public class ErrorLoggingMongoService : IErrorLoggingMongoService
    {
        private readonly IMongoCollection<ErrorLogs> _collection;
        public ErrorLoggingMongoService(IOptions<MongoDbSettings> settings, IMongoClient client)
        {
              var database = client.GetDatabase(settings.Value.DatabaseName);
              _collection = database.GetCollection<ErrorLogs>(settings.Value.ErrorLoggingCollectionName);
        }

        public async Task LogErrorAsync(ErrorLogs log)
        {
            try
            {
                await _collection.InsertOneAsync(log);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to log error" + ex.ToString());
            }
            
        }
    }
}

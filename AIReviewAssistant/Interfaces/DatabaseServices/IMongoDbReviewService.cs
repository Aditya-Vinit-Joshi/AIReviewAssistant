using AIReviewAssistant.Models;

namespace AIReviewAssistant.Interfaces.DatabaseServices
{
    public interface IMongoDbReviewService
    {
        Task<bool> InsertReviewAsync(ReviewRecord record);
    }
}

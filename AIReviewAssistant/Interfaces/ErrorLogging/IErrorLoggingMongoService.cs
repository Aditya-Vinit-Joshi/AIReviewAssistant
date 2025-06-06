using AIReviewAssistant.Models;

namespace AIReviewAssistant.Interfaces.ErrorLogging
{
    public interface IErrorLoggingMongoService
    {
        Task LogErrorAsync(ErrorLogs log);
    }
}

using AIReviewAssistant.Dtos;
using AIReviewAssistant.Models;

namespace AIReviewAssistant.Interfaces.AI
{
    public interface IAIReviewService
    {
        Task<List<InlineComments>> GenerateAIReview(ReviewRequestDto dto);
    }
}

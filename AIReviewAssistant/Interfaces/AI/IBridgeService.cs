using AIReviewAssistant.Dtos;
using AIReviewAssistant.Models;

namespace AIReviewAssistant.Interfaces.AI
{
    public interface IBridgeService
    {
        Task<List<InlineComments>> ReviewAndStore(ReviewRequestDto dto);
    }
}

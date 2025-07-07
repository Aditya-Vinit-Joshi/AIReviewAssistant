using AIReviewAssistant.Dtos;
using AIReviewAssistant.Interfaces.AI;
using AIReviewAssistant.Interfaces.DatabaseServices;
using AIReviewAssistant.Models;
using AutoMapper;

namespace AIReviewAssistant.Services.AIServices
{
    public class BridgeService : IBridgeService
    {
        private readonly IMongoDbReviewService _dbReviewService;
        private readonly IMapper _mapper;
        private readonly IAIReviewService _aiReviewService;
        public BridgeService(IMongoDbReviewService dbReviewService, IMapper mapper, IAIReviewService aiReviewService) 
        { 
            _dbReviewService = dbReviewService;
            _mapper = mapper;
            _aiReviewService = aiReviewService;
        }


        public async Task<List<InlineComments>> ReviewAndStore(ReviewRequestDto dto)
        {
            var reviewRecord = _mapper.Map<ReviewRecord>(dto);
            //reviewRecord.Review = "Using for Testing";

            var inlineComments = await _aiReviewService.GenerateAIReview(dto);

            reviewRecord.InlineComments = inlineComments;
            reviewRecord.Review = !inlineComments.Any() ? "No issues found. Your code looks great!" :
                "Your review has the following suggestions";

            bool result = await _dbReviewService.InsertReviewAsync(reviewRecord);

            return inlineComments;
        }
    }
}

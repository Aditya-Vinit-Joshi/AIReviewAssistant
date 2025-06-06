using AIReviewAssistant.Dtos;
using AIReviewAssistant.Interfaces.AI;
using AIReviewAssistant.Interfaces.DatabaseServices;
using AIReviewAssistant.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AIReviewAssistant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IMongoDbReviewService _dbReviewService;
        private readonly IMapper _mapper;
        private readonly IAIReviewService _aiReviewService;
        public ReviewController(IMongoDbReviewService dbReviewService, IMapper mapper, IAIReviewService aIReviewService)
        {
            _dbReviewService = dbReviewService;
            _mapper = mapper;
            _aiReviewService = aIReviewService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForReview([FromBody]ReviewRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewRecord = _mapper.Map<ReviewRecord>(dto);
            reviewRecord.Review = "Using for Testing";

            var inlineComments = await _aiReviewService.GenerateAIReview(dto);
            
            reviewRecord.InlineComments = inlineComments;
            reviewRecord.Review = !inlineComments.Any() ? "No issues found. Your code looks great!" :
                "Your review has the following suggestions";

            bool result = await _dbReviewService.InsertReviewAsync(reviewRecord);

            if (result)
                return Ok(new { message = "Review submitted successfully." });
            else
                return StatusCode(500, new { message = "Failed to submit review. Please try again later." });
        }
    }
}

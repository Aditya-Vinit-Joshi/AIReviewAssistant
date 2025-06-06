using AIReviewAssistant.Dtos;
using AIReviewAssistant.Models;
using AutoMapper;

namespace AIReviewAssistant.Mappings
{
    public class ReviewMappingProfile : Profile
    {
        public ReviewMappingProfile()
        {
            CreateMap<ReviewRequestDto, ReviewRecord>()
                .ForMember(dest => dest.Review, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Review, opt => opt.Ignore());
        }

    }
}

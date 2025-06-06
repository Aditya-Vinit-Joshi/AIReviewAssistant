using AIReviewAssistant.Models;
using System.ComponentModel.DataAnnotations;

namespace AIReviewAssistant.Dtos
{
    public class ReviewRequestDto
    {
        [Required]
        public string RepoName { get; set; } = null!;
        [Range(1, int.MaxValue)]
        public int PullRequestNumber { get; set; }
        [Required]
        public string BranchName { get; set; } = null!;
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public string Language { get; set; } = null!;
        [MinLength(1)]
        public List<CodeFile> codeFiles { get; set; } = new();
        [Required]
        public string CommitHash { get; set; } = null!;

    }
}

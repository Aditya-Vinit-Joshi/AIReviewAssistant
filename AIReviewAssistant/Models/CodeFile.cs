using System.ComponentModel.DataAnnotations;

namespace AIReviewAssistant.Models
{
    public class CodeFile
    {
        [Required]
        public string FileName { get; set; } = null!;
        [Required]
        public string Content { get; set; } = null!;
        public string? Diff { get; set; }

    }
}

namespace AIReviewAssistant.Models
{
    public class InlineComments
    {
        public string FileName { get; set; } = null!;
        public int? LineNumber { get; set; }
        public string Comment { get; set; } = null!;
        public string? SuggestedFix { get; set; }
    }
}

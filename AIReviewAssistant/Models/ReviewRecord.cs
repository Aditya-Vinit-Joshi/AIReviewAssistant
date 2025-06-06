using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AIReviewAssistant.Models
{
    public class ReviewRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string RepoName { get; set; } = null!;
        public int PullRequestNumber { get; set; }
        public string BranchName { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Language { get; set; } = null!;
        public List<CodeFile> CodeFiles { get; set; } = new();
        public string Review { get; set; } = null!;
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string CommitHash { get; set; } = null!;
        public List<InlineComments> InlineComments { get; set; } = new();
    }
}

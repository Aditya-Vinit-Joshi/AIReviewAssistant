using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AIReviewAssistant.Models
{
    public class ErrorLogs
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        public string Service { get; set; } = null!;
        public string Operation { get; set; } = null!;
        public string ErrorMessage { get; set; } = null!;
        public string StackTrace { get; set; } = null!;
        public string? InnerException { get; set; } = null;
        public string? RequestContent { get; set; } = null!;
    }
}

namespace AIReviewAssistant.Settings
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CollectionName { get; set; } = null!;
        public string ErrorLoggingCollectionName { get; set; } = null!;
    }
}

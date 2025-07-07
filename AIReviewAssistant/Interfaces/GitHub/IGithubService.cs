namespace AIReviewAssistant.Interfaces.GitHub
{
    public interface IGithubService
    {
        Task<Dictionary<string, string>> FetchALlFilesAsync(string repoOwner, string repoName, string baseSha);
    }
}

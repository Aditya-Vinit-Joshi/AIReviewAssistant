using AIReviewAssistant.Interfaces.ApiService;
using AIReviewAssistant.Interfaces.GitHub;
using System.Text.Json;
using System.Text;

namespace AIReviewAssistant.Services.GitHub
{
    public class GitHubService : IGithubService
    {
        private readonly IHttpHandler _handler;
        public GitHubService(IHttpHandler handler) 
        { 
            _handler = handler;
        }

        public async Task<Dictionary<string, string>> FetchALlFilesAsync(string repoOwner, string repoName, string baseSha)
        {
            var fileContents = new Dictionary<string, string>();

            // Step 1: Get full tree recursively
            var treeUrl = $"https://api.github.com/repos/{repoOwner}/{repoName}/git/trees/{baseSha}?recursive=1";
            var treeResponse = await _handler.SendGetRequest(treeUrl, null);
            var treeJson = JsonDocument.Parse(await treeResponse.Content.ReadAsStringAsync());
            var tree = treeJson.RootElement.GetProperty("tree");

            foreach (var item in tree.EnumerateArray())
            {
                if (item.GetProperty("type").GetString() == "blob")
                {
                    var path = item.GetProperty("path").GetString();
                    var sha = item.GetProperty("sha").GetString();

                    var blobUrl = $"https://api.github.com/repos/{repoOwner}/{repoName}/git/blobs/{sha}";
                    var blobResponse = await _handler.SendGetRequest(blobUrl, null);
                    var blobJson = JsonDocument.Parse(await blobResponse.Content.ReadAsStringAsync());

                    var encoded = blobJson.RootElement.GetProperty("content").GetString();
                    var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(encoded.Replace("\n", "")));

                    fileContents[path] = decoded;
                }
            }

            return fileContents;
        }
    }
}

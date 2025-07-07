using AIReviewAssistant.Dtos;
using AIReviewAssistant.Interfaces.AI;
using AIReviewAssistant.Interfaces.ApiService;
using AIReviewAssistant.Interfaces.GitHub;
using AIReviewAssistant.Models;
using AIReviewAssistant.Services.GitHub;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace AIReviewAssistant.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly IGithubService _githubservice;
        private readonly IBridgeService _bridgeservice;
        public GitHubController(IGithubService githubService, IMapper mapper, IBridgeService bridgeservice)
        {
            _githubservice = githubService;
            _bridgeservice = bridgeservice;
        }


        [HttpPost]
        public async Task<List<InlineComments>> Post()
        {
            using var reader = new StreamReader(Request.Body);
            var payload = await reader.ReadToEndAsync();
            var root = JsonDocument.Parse(payload).RootElement;

            var repoOwner = root.GetProperty("repository").GetProperty("owner").GetProperty("login").GetString();
            var repoName = root.GetProperty("repository").GetProperty("name").GetString();
            var fullRepoName = $"{repoOwner}/{repoName}";
            var baseSha = root.GetProperty("pull_request").GetProperty("base").GetProperty("sha").GetString();
            var headSha = root.GetProperty("pull_request").GetProperty("head").GetProperty("sha").GetString();
            var prNumber = root.GetProperty("pull_request").GetProperty("number").GetInt32();
            var branchName = root.GetProperty("pull_request").GetProperty("head").GetProperty("ref").GetString();
            var userLogin = root.GetProperty("sender").GetProperty("login").GetString();
            var files = await _githubservice.FetchALlFilesAsync(repoOwner, repoName, baseSha);

            Console.WriteLine($"Retrieved {files.Count} files.");

            var formattedFiles = files.Select(kv => new CodeFile
            {
                FileName = kv.Key,
                Content = kv.Value,
                Diff = ""
            }).ToList();

            var reviewRequest = new ReviewRequestDto
            {
                RepoName = fullRepoName,
                PullRequestNumber = prNumber,
                BranchName = branchName!,
                UserId = userLogin!,
                CommitHash = headSha!,
                codeFiles = formattedFiles
            };

            List<InlineComments> resultComments = await _bridgeservice.ReviewAndStore(reviewRequest);

            return resultComments;


            
        }
    }
}

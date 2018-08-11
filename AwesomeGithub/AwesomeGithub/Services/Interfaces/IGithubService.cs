using AwesomeGithub.Model;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGithub.Services.Interfaces
{
    public interface IGithubService
    {
        [Headers("User-Agent: Awesome Octocat App")]
        [Get("/search/repositories?q=language:{language}&sort=stars&page=1")]
        Task<GithubRepositoryResult> SearchRepositories(string language);

        [Headers("User-Agent: Awesome Octocat App")]
        [Get("/repos/{name}/{repo}/pulls")]
        Task<List<GithubPullRequest>> RequestPullRequest(string name, string repo);

    }
}

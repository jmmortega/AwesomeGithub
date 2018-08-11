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
        [Get("repos/search/repositories?q=language:{language}&sort=stars&page=1")]
        Task<GithubRepositoryResult> SearchRepositories(string language);

        [Get("repos/{pullRequestSuffix}/pulls")]
        Task<List<GithubPullRequest>> RequestPullRequest(string pullRequestSuffix);

    }
}

using AwesomeGithub.Model;
using System.Collections.Generic;

namespace AwesomeGithub.Services.Interfaces
{
    public interface ICacheService
    {
        List<GithubRepository> GetRepositories();

        List<GithubPullRequest> GetPullRequests(long repoId);

        void AddRepositories(List<GithubRepository> repositories);

        void AddPullRequests(List<GithubPullRequest> pullRequests, long repoId);        
    }
}

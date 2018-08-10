using AwesomeGithub.Model;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeGithub.Services.Interfaces
{
    public interface IGithubService
    {
        [Get("repos/search/repositories?q=language:{language}&sort=stars&page=1")]
        GithubRepositoryResult SearchRepositories(string language);


    }
}

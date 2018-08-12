using AwesomeGithub.Common;
using AwesomeGithub.Model;
using AwesomeGithub.Services;
using AwesomeGithub.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Reactive;
using MonkeyCache.FileStore;

[assembly:Xamarin.Forms.Dependency(typeof(CacheService))]
namespace AwesomeGithub.Services
{
    public class CacheService : ICacheService
    {        
        public CacheService()
        {
            Barrel.ApplicationId = KeyValues.AppName;            
        }

        public List<GithubRepository> GetRepositories()
        {            
            var repos = Barrel.Current.Get<List<GithubRepository>>(nameof(GithubRepository));

            if(repos == null)
            {
                return new List<GithubRepository>();
            }
            return repos;
        }

        public List<GithubPullRequest> GetPullRequests(long repoId)
        {
            var pullRequests = Barrel.Current.Get<List<GithubPullRequest>>(
                                 $"{nameof(GithubPullRequest)}.{repoId.ToString()}" );

            if(pullRequests == null)
            {
                return new List<GithubPullRequest>();
            }

            return pullRequests;
        }

        public void AddRepositories(List<GithubRepository> repositories)
        {                    
            Barrel.Current.Add<List<GithubRepository>>(nameof(GithubRepository), repositories, TimeSpan.FromDays(1));                        
        }

        public void AddPullRequests(List<GithubPullRequest> pullRequests, long repoId)
        {
            Barrel.Current.Add<List<GithubPullRequest>>($"{nameof(GithubPullRequest)}.{repoId.ToString()}", pullRequests, TimeSpan.FromHours(6));
        }                                
    }
}

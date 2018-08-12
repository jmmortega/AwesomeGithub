using AwesomeGithub.Base;
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using AwesomeGithub.Services.Interfaces;
using Refit;
using AwesomeGithub.Common;
using AwesomeGithub.Model;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AwesomeGithub.Extension;
using Xamarin.Forms;
using System.Reactive.Linq;

namespace AwesomeGithub.Features.PullRequest
{
    public class PullRequestViewModel : BaseReactiveViewModel
    {
        private readonly IGithubService githubService;

        private int currentPage = 1;
        private bool completed = false;
        private bool newPullRequestsIncoming = false;

        public string RepositoryName { get; set; }

        public string UserName { get; set; }

        public long RepositoryId { get; set; }

        private int openedPullRequests;

        public int OpenedPullRequests
        {
            get => openedPullRequests;
            set => this.RaiseAndSetIfChanged(ref openedPullRequests, value);
        }

        private int closedPullRequests;

        public int ClosedPullRequests
        {
            get => closedPullRequests;
            set => this.RaiseAndSetIfChanged(ref closedPullRequests, value);
        }

        private Dictionary<long, GithubPullRequest> pullRequests = new Dictionary<long, GithubPullRequest>();

        public Dictionary<long, GithubPullRequest> PullRequests
        {
            get => pullRequests;
            set => this.RaiseAndSetIfChanged(ref pullRequests, value);
        }

        public PullRequestViewModel()
        {
            githubService = RestService.For<IGithubService>(KeyValues.HostApiCall);            
        }
        
        public override async void OnAppearing()
        {
            base.OnAppearing();

            PullRequests = cacheService.GetPullRequests(RepositoryId).ToDictionary(x => x.Id);

            if(!string.IsNullOrWhiteSpace(RepositoryName))
            {
                var prs = (await RequestPullRequests()).Select(x => new Tuple<long, GithubPullRequest>(x.Id, x));
                PullRequests.Clear();
                PullRequests.AddRange(prs);
                CountPullRequestState();
            }                        
        }
        
        public async void RequestNewPage(GithubPullRequest item)
        {                        
            var indexElement = PullRequests.Values.ToList().IndexOf(item);

            if (indexElement == PullRequests.Count - 5 && newPullRequestsIncoming == false && completed == false)
            {
                newPullRequestsIncoming = true;
                currentPage++;
                var newPRs = await RequestPullRequests(currentPage);

                //Maybe there not more PR's for this Repo.
                if(newPRs.Count == 0)
                {
                    completed = true;
                }

                PullRequests.AddRange(newPRs.Select(x => new Tuple<long, GithubPullRequest>(x.Id, x)));

                CountPullRequestState();
                newPullRequestsIncoming = false;
            }
        }
        
        private void CountPullRequestState()
        {
            OpenedPullRequests = PullRequests.Where(x => x.Value.State == "open").Count();
            ClosedPullRequests = PullRequests.Where(x => x.Value.State == "closed").Count();
        }

        private async Task<List<GithubPullRequest>> RequestPullRequests(int page = 1)
        {
            IsBusy = true;
            var pullRequests = await githubService.RequestPullRequest(UserName, RepositoryName, page);                                                
            IsBusy = false;

            return pullRequests;
        }
    }
}

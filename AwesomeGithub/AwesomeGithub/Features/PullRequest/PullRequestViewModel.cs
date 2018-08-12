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

        private ObservableCollection<GithubPullRequest> pullRequests = new ObservableCollection<GithubPullRequest>();

        public ObservableCollection<GithubPullRequest> PullRequests
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

            PullRequests.AddRange(cacheService.GetPullRequests(RepositoryId));
            CountPullRequestState();

            if(!string.IsNullOrWhiteSpace(RepositoryName))
            {
                IsBusy = true && PullRequests.Count == 0;
                var prs = (await RequestPullRequests());
                PullRequests.Clear();
                PullRequests.AddRange(prs.Where(x => !PullRequests.Contains(x)));                
                CountPullRequestState();
                IsBusy = false;
            }                        
        }
        
        public async void RequestNewPage(GithubPullRequest item)
        {                        
            var indexElement = PullRequests.IndexOf(item);

            if (indexElement == PullRequests.Count - 5 && newPullRequestsIncoming == false && completed == false)
            {
                newPullRequestsIncoming = true;
                currentPage++;
                IsBusy = true;
                var newPRs = await RequestPullRequests(currentPage);
                IsBusy = false;

                //Maybe there not more PR's for this Repo.
                if (newPRs.Count == 0)
                {
                    completed = true;
                }

                PullRequests.AddRange(newPRs.Where(x => !pullRequests.Contains(x)));                
                CountPullRequestState();
                newPullRequestsIncoming = false;
            }
        }
        
        private void CountPullRequestState()
        {
            OpenedPullRequests = PullRequests.Where(x => x.State == "open").Count();
            ClosedPullRequests = PullRequests.Where(x => x.State == "closed").Count();
            cacheService.AddPullRequests(PullRequests.ToList(), RepositoryId);
        }

        private async Task<List<GithubPullRequest>> RequestPullRequests(int page = 1)
        {            
            var pullRequests = await githubService.RequestPullRequest(UserName, RepositoryName, page);                                                
            
            return pullRequests;
        }
    }
}

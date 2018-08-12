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

namespace AwesomeGithub.Features.PullRequest
{
    public class PullRequestViewModel : BaseReactiveViewModel
    {
        private readonly IGithubService githubService;

        public string RepositoryName { get; set; }

        public string UserName { get; set; }

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

        private List<GithubPullRequest> pullRequests;

        public List<GithubPullRequest> PullRequests
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

            if(!string.IsNullOrWhiteSpace(RepositoryName))
            {
                IsBusy = true;
                PullRequests = await githubService.RequestPullRequest(UserName, RepositoryName);
                OpenedPullRequests = PullRequests.Where(x => x.State == "open").Count();
                ClosedPullRequests = PullRequests.Where(x => x.State == "closed").Count();
                IsBusy = false;
            }                        
        }
    }
}

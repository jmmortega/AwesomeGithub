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

        private string pullRequestSufix;

        public string PullRequestSufix
        {
            get => pullRequestSufix;
            set => this.RaiseAndSetIfChanged(ref pullRequestSufix, value);
        }

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

            if(!string.IsNullOrWhiteSpace(PullRequestSufix))
            {
                PullRequests = await githubService.RequestPullRequest(PullRequestSufix);
                OpenedPullRequests = PullRequests.Where(x => x.State == "open").Count();
                ClosedPullRequests = PullRequests.Where(x => x.State == "closed").Count(0);
            }                        
        }
    }
}

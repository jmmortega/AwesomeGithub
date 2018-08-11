using AwesomeGithub.Base;
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using AwesomeGithub.Services.Interfaces;
using Refit;
using AwesomeGithub.Common;

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

        public PullRequestViewModel()
        {
            githubService = RestService.For<IGithubService>(KeyValues.HostApiCall);
        }


        public override void OnAppearing()
        {
            base.OnAppearing();

            githubService.
        }
    }
}

using AwesomeGithub.Base;
using AwesomeGithub.Common;
using AwesomeGithub.Messages;
using AwesomeGithub.Model;
using AwesomeGithub.Services.Interfaces;
using ReactiveUI;
using Refit;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Reactive.Linq;

namespace AwesomeGithub.Features.Main
{
    public class MainViewModel : BaseReactiveViewModel
    {
        private readonly IGithubService githubService;

        private GithubRepositoryResult repositoryResult;

        private string languageCode;
        public string LanguageCode
        {
            get => languageCode;
            set => this.RaiseAndSetIfChanged(ref languageCode, value);
        }

        private string searchTerm;

        public string SearchTerm
        {
            get => searchTerm;
            set => this.RaiseAndSetIfChanged(ref searchTerm, value);
        }

        private List<GithubRepository> repositories;

        public List<GithubRepository> Repositories
        {
            get => repositories;
            set => this.RaiseAndSetIfChanged(ref repositories, value);
        }
        
        public MainViewModel()
        {
            githubService = RestService.For<IGithubService>(KeyValues.HostApiCall);

            Subscribe();
        }
        
        public override void OnAppearing()
        {
            MessagingCenter.Subscribe<MessageLanguageCode>(this, nameof(MessageLanguageCode), ChangeLanguageCode);
                
            base.OnAppearing();
        }

        private void Subscribe()
        {
            this.WhenAnyValue(v => v.SearchTerm)
                .Throttle(TimeSpan.FromSeconds(3))
                .Subscribe(ShowRepositories);
        }

        private async void ChangeLanguageCode(MessageLanguageCode language)
        {
            languageCode = language.LanguageCode;

            repositoryResult = await ExecuteInternetCallAsync<GithubRepositoryResult>(() => githubService.SearchRepositories(LanguageCode));


            ShowRepositories(searchTerm);
        }

        private void ShowRepositories(string searchTerm)
        {
            Repositories = repositoryResult.Items.Where(x => x.RepositoryName.ToLower().Contains(searchTerm.ToLower()) ||
                                                        x.Username.ToLower().Contains(searchTerm.ToLower()))                                                        
                                                        .Take(KeyValues.MaxRepositoriesShowed)
                                                        .ToList();
        }

        public override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<MessageLanguageCode>(this, nameof(MessageLanguageCode));

            base.OnDisappearing();
        }
    }
}

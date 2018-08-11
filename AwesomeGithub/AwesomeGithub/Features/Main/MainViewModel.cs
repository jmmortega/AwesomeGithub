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
using System.Threading.Tasks;

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
        
        public override async void OnAppearing()
        {
            MessagingCenter.Subscribe<MessageLanguageCode>(this, nameof(MessageLanguageCode), ChangeLanguageCode);

            await SearchRepositories();
            ShowRepositories(SearchTerm);

            base.OnAppearing();
        }

        private void Subscribe()
        {
            this.WhenAnyValue(v => v.SearchTerm, v => v.repositoryResult)
                .Where(x => x.Item2 != null)
                .Throttle(TimeSpan.FromSeconds(3))
                .Select(x => x.Item1)
                .Subscribe(ShowRepositories);
        }

        private async void ChangeLanguageCode(MessageLanguageCode language)
        {
            languageCode = language.LanguageCode;
            await SearchRepositories(languageCode);
            ShowRepositories(searchTerm);
        }

        private async Task SearchRepositories(string languageCode = "")
        {            
            repositoryResult = await ExecuteInternetCallAsync<GithubRepositoryResult>(() => githubService.SearchRepositories(LanguageCode));
        }

        private void ShowRepositories(string searchTerm)
        {
            if(!string.IsNullOrEmpty(searchTerm))
            {
                Repositories = repositoryResult.Items.Where(x => x.RepositoryName.ToLower().Contains(searchTerm.ToLower()) ||
                                                        x.Owner.Login.ToLower().Contains(searchTerm.ToLower()))
                                                        .Take(KeyValues.MaxRepositoriesShowed)
                                                        .ToList();
            }
            else
            {
                Repositories = repositoryResult.Items.Take(KeyValues.MaxRepositoriesShowed).ToList();
            }                                                
        }

        public override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<MessageLanguageCode>(this, nameof(MessageLanguageCode));

            base.OnDisappearing();
        }
    }
}

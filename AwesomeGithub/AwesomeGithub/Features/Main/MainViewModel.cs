using AwesomeGithub.Base;
using AwesomeGithub.Common;
using AwesomeGithub.Messages;
using AwesomeGithub.Model;
using AwesomeGithub.Services.Interfaces;
using ReactiveUI;
using Refit;
using System;
using System.Linq;
using Xamarin.Forms;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using AwesomeGithub.Extension;
using System.Collections.Generic;

namespace AwesomeGithub.Features.Main
{
    public class MainViewModel : BaseReactiveViewModel
    {
        private readonly IGithubService githubService;

        private GithubRepositoryResult repositoryResult;
        private int currentPage = 1;
        private bool newRepositoriesIncoming;

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

        private Dictionary<long, GithubRepository> allRepositories = new Dictionary<long, GithubRepository>();

        private ObservableCollection<GithubRepository> repositories = new ObservableCollection<GithubRepository>();

        public ObservableCollection<GithubRepository> Repositories
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

            allRepositories = cacheService.GetRepositories().ToDictionary(x => x.Id);
            ShowRepositories(SearchTerm);
            await SearchRepositories();
            ShowRepositories(SearchTerm);


            base.OnAppearing();
        }

        public override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<MessageLanguageCode>(this, nameof(MessageLanguageCode));

            base.OnDisappearing();
        }

        public async void RequestNewPage(GithubRepository item)
        {
            var indexElement = Repositories.IndexOf(item);

            if(indexElement == Repositories.Count -5 && newRepositoriesIncoming == false)
            {
                newRepositoriesIncoming = true;
                currentPage++;
                await SearchRepositories(languageCode, currentPage);                
                ShowRepositories(searchTerm);
                newRepositoriesIncoming = false;
            }
        }

        private void Subscribe()
        {
            this.WhenAnyValue(v => v.SearchTerm)                
                .Throttle(TimeSpan.FromSeconds(1))                
                .Subscribe((searchTerm) =>
                {
                    Repositories.Clear();
                    ShowRepositories(searchTerm);
                });
        }

        private async void ChangeLanguageCode(MessageLanguageCode language)
        {
            languageCode = language.LanguageCode;
            await SearchRepositories(languageCode);
            ShowRepositories(searchTerm);
        }

        private async Task SearchRepositories(string languageCode = "", int page = 1)
        {
            Device.BeginInvokeOnMainThread(() => IsBusy = true);
            repositoryResult = await ExecuteInternetCallAsync<GithubRepositoryResult>(() => githubService.SearchRepositories(LanguageCode, page));

            allRepositories.AddRange(repositoryResult.Items.Select(x => new Tuple<long, GithubRepository>(x.Id, x)));

            cacheService.AddRepositories(allRepositories.Select(x => x.Value).ToList());

            Device.BeginInvokeOnMainThread(() => IsBusy = false);
        }

        private void ShowRepositories(string searchTerm)
        {            
            Device.BeginInvokeOnMainThread(() =>
            {
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    Repositories.AddRange(allRepositories.Select(x => x.Value).Where(x => x.RepositoryName.ToLower().Contains(searchTerm.ToLower()) ||
                                                            x.Owner.Login.ToLower().Contains(searchTerm.ToLower()))
                                                            .Where(x => !Repositories.Contains(x))
                                                            .Take(KeyValues.MaxRepositoriesShowed)                                                            
                                                            .ToList());
                }
                else
                {
                    Repositories.AddRange(allRepositories.Values.Where(x => !Repositories.Contains(x))
                                                                .Take(KeyValues.MaxRepositoriesShowed).ToList());
                }
            });
                        
        }        
    }
}

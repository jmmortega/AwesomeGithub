using AwesomeGithub.Base;
using AwesomeGithub.Common;
using AwesomeGithub.Messages;
using AwesomeGithub.Services.Interfaces;
using ReactiveUI;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AwesomeGithub.Features.Main
{
    public class MainViewModel : BaseReactiveViewModel
    {
        private string languageCode;
        public string LanguageCode
        {
            get => languageCode;
            set => this.RaiseAndSetIfChanged(ref languageCode, value);
        }

        private readonly IGithubService githubService;

        public MainViewModel()
        {
            githubService = RestService.For<IGithubService>(KeyValues.HostApiCall);
        }

        public override void OnAppearing()
        {
            MessagingCenter.Subscribe<MessageLanguageCode>(this, nameof(MessageLanguageCode), ChangeLanguageCode);
                
            base.OnAppearing();
        }

        private void ChangeLanguageCode(MessageLanguageCode language) => languageCode = language.LanguageCode;

        public override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<MessageLanguageCode>(this, nameof(MessageLanguageCode));

            base.OnDisappearing();
        }
    }
}

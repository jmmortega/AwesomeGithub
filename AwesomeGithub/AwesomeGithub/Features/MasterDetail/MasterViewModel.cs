using AwesomeGithub.Base;
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

using Xamarin.Forms;
using AwesomeGithub.Messages;
using System.Reactive.Linq;

namespace AwesomeGithub.Features.MasterDetail
{
    public class MasterViewModel : BaseReactiveViewModel
    {
        private string languageCode;
        public string LanguageCode
        {
            get => languageCode;
            set => this.RaiseAndSetIfChanged(ref languageCode, value);
        }

        public MasterViewModel()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            this.WhenAnyValue(v => v.LanguageCode)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Throttle(TimeSpan.FromSeconds(3))
                .Subscribe(language =>
                {
                    MessagingCenter.Send<MessageLanguageCode>(new MessageLanguageCode() { LanguageCode = language }, nameof(MessageLanguageCode));
                });               
        }
    }
}

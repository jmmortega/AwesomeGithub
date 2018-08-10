using AwesomeGithub.Base;
using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

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


    }
}

using ReactiveUI;
using System;
using Xamarin.Forms.Xaml;

namespace AwesomeGithub.Features.MasterDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterView
	{
		public MasterView ()
		{
			InitializeComponent ();
            
		}

        protected override void CreateBindings(Action<IDisposable> d)
        {
            base.CreateBindings(d);
                        
            d(this.Bind(ViewModel, vm => vm.LanguageCode, v => v.EntryLanguageCode.Text));
        }

        
    }
}
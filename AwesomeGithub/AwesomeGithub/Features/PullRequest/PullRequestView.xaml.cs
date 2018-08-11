using ReactiveUI;
using System;
using Xamarin.Forms.Xaml;

namespace AwesomeGithub.Features.PullRequest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PullRequestView
	{        
        public PullRequestView(string pullSufix) : this()
        {
            ViewModel.PullRequestSufix = pullSufix;
        }

		public PullRequestView ()
		{
			InitializeComponent ();            
		}

        protected override void CreateBindings(Action<IDisposable> d)
        {
            base.CreateBindings(d);

            d(this.OneWayBind(ViewModel, vm => vm.OpenedPullRequests, v => v.LabelPullRequestOpened.Text));
            d(this.OneWayBind(ViewModel, vm => vm.ClosedPullRequests, v => v.LabelPullRequestClosed.Text));

            d(this.OneWayBind(ViewModel, vm => vm.PullRequests, v => v.ListViewPullRequests.ItemsSource));
        }
    }
}
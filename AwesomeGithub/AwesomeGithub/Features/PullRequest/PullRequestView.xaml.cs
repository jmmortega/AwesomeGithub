using AwesomeGithub.Model;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using Xamarin.Forms;
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

            Subscribe(d);
        }

        private void Subscribe(Action<IDisposable> d)
        {
            var listViewSelectedObservable = Observable.FromEventPattern<EventHandler<SelectedItemChangedEventArgs>, SelectedItemChangedEventArgs>(
                h => ListViewPullRequests.ItemSelected += h,
                h => ListViewPullRequests.ItemSelected -= h)
                .Where(x => x != null);

            d(listViewSelectedObservable.Subscribe(args =>
            {
                var repository = (GithubPullRequest)args.EventArgs.SelectedItem;

                Device.OpenUri(new Uri(repository.Url));

                (args.Sender as ListView).SelectedItem = null;
            }));
        }
    }
}
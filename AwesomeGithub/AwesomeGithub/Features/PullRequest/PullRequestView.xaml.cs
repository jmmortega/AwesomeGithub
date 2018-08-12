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
        public PullRequestView(Tuple<string,string> pullRequestParams) : this()
        {
            Title = pullRequestParams.Item2;
            ViewModel.UserName = pullRequestParams.Item1;
            ViewModel.RepositoryName = pullRequestParams.Item2;
        }

		public PullRequestView ()
		{
			InitializeComponent ();            
		}

        protected override void CreateBindings(Action<IDisposable> d)
        {
            base.CreateBindings(d);

            d(this.OneWayBind(ViewModel, vm => vm.OpenedPullRequests, v => v.LabelPullRequestOpened.Text, this.FormatOpened));

            d(this.OneWayBind(ViewModel, vm => vm.ClosedPullRequests, v => v.LabelPullRequestClosed.Text, this.FormatClosed));                              
                
            d(this.OneWayBind(ViewModel, vm => vm.PullRequests, v => v.ListViewPullRequests.ItemsSource));

            d(this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.GridWaiting.IsVisible));

            ListSelectedObservable(d);
        }

        private string FormatOpened(int value) => $"{value} opened";

        private string FormatClosed(int value) => $"{value} closed";
        

        private void ListSelectedObservable(Action<IDisposable> d)
        {
            var listViewSelectedObservable = Observable.FromEventPattern<EventHandler<SelectedItemChangedEventArgs>, SelectedItemChangedEventArgs>(
                h => ListViewPullRequests.ItemSelected += h,
                h => ListViewPullRequests.ItemSelected -= h)
                .Where(x => x != null);

            d(listViewSelectedObservable.Subscribe(args =>
            {
                if(args.EventArgs.SelectedItem != null)
                {
                    var repository = (GithubPullRequest)args.EventArgs.SelectedItem;

                    Device.OpenUri(new Uri(repository.Url));

                    (args.Sender as ListView).SelectedItem = null;
                }
                
            }));
        }

        /*
        private void ListViewPaginationObservable(Action<IDisposable> d)
        {
            var paginationObservable = Observable.FromEventPattern<EventHandler<ItemVisibilityEventArgs>, ItemVisibilityEventArgs>(
                h => ListViewRepositories.ItemAppearing += h,
                h => ListViewRepositories.ItemAppearing -= h)
                .Where(x => x != null)
                .Select(x => (GithubRepository)x.EventArgs.Item);

            d(paginationObservable.Subscribe(args =>
            {
                ViewModel.RequestNewPage(args);
            }));

        }
        */
    }
}
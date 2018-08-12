using AwesomeGithub.Model;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AwesomeGithub.Features.PullRequest
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PullRequestView
	{        
        public PullRequestView(Tuple<long, string,string> pullRequestParams) : this()
        {
            Title = pullRequestParams.Item2;
            ViewModel.RepositoryId = pullRequestParams.Item1;
            ViewModel.UserName = pullRequestParams.Item2;
            ViewModel.RepositoryName = pullRequestParams.Item3;
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
            d(this.OneWayBind(ViewModel, vm => vm.PullRequests, v => v.ListViewPullRequests.ItemsSource, this.ConvertToCollection));
            d(this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.GridWaiting.IsVisible));
            
            ListSelectedObservable(d);
            ListViewPaginationObservable(d);
        }

        private IEnumerable ConvertToCollection(Dictionary<long, GithubPullRequest> arg) => arg.Values;
        
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

        
        private void ListViewPaginationObservable(Action<IDisposable> d)
        {
            var paginationObservable = Observable.FromEventPattern<EventHandler<ItemVisibilityEventArgs>, ItemVisibilityEventArgs>(
                h => ListViewPullRequests.ItemAppearing += h,
                h => ListViewPullRequests.ItemAppearing -= h)
                .Where(x => x != null)
                .Select(x => (GithubPullRequest)x.EventArgs.Item);

            d(paginationObservable.Subscribe(args =>
            {
                ViewModel.RequestNewPage(args);
            }));

        }
        
    }
}
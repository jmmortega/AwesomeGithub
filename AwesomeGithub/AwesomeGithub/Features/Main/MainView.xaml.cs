using AwesomeGithub.Features.PullRequest;
using AwesomeGithub.Model;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AwesomeGithub.Extension;

namespace AwesomeGithub.Features.Main
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainView
	{
		public MainView ()
		{
			InitializeComponent ();
            
		}

        protected override void CreateBindings(Action<IDisposable> d)
        {
            base.CreateBindings(d);

            d(this.Bind(ViewModel, vm => vm.SearchTerm, v => v.EntrySearchBox.Text));
            d(this.OneWayBind(ViewModel, vm => vm.Repositories, v => v.ListViewRepositories.ItemsSource));
            d(this.OneWayBind(ViewModel, vm => vm.IsBusy, v => v.GridWaiting.IsVisible));

            ListViewSelectedObservable(d);
            ListViewPaginationObservable(d);
        }

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
        
        private void ListViewSelectedObservable(Action<IDisposable> d)
        {
            var listViewSelectedObservable = Observable.FromEventPattern<EventHandler<SelectedItemChangedEventArgs>, SelectedItemChangedEventArgs>(
                h => ListViewRepositories.ItemSelected += h,
                h => ListViewRepositories.ItemSelected -= h)
                .Where(x => x != null);
                

            d(listViewSelectedObservable.Subscribe(args =>
            {
                var repository = (GithubRepository)args.EventArgs.SelectedItem;

                Navigation.PushAsync(new PullRequestView(repository.PullRequestParams()));

                (args.Sender as ListView).SelectedItem = null;
            }));                               
        }        
    }
}
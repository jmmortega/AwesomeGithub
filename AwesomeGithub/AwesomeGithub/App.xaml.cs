using AwesomeGithub.Features.Main;
using AwesomeGithub.Features.MasterDetail;
using AwesomeGithub.Styles;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace AwesomeGithub
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

            LoadStyles();

            var detail = new NavigationPage(new MainView()) { BarBackgroundColor = (Color)App.Current.Resources["ApplicationColor"] };
            MainPage = new MasterDetailPage() { Master = new MasterView(), Detail = detail, IsGestureEnabled = true };
		}

        private void LoadStyles()
        {
            var mainStyles = new MainStyles();
            foreach(var resource in mainStyles.Resources)
            {
                App.Current.Resources.Add(resource.Key, resource.Value);
            }
        }

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

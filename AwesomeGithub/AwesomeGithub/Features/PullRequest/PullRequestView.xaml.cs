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
	}
}
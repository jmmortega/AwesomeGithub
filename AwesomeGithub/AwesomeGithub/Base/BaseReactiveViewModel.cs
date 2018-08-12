using AwesomeGithub.Services.Interfaces;
using ReactiveUI;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AwesomeGithub.Base
{
    public abstract class BaseReactiveViewModel : ReactiveObject
    {
        protected readonly ICacheService cacheService;

        private bool isBusy = false;

        public bool IsBusy
        {
            get => isBusy;
            set => this.RaiseAndSetIfChanged(ref isBusy, value);
        }

        public BaseReactiveViewModel()
        {
            cacheService = DependencyService.Get<ICacheService>();
        }

        protected virtual void InitializeCommand() { }

        public virtual void OnAppearing()
        { }

        public virtual void OnDisappearing()
        { }

        
                    

        protected async Task<T> ExecuteInternetCallAsync<T>(Func<Task<T>> operation) where T : new()
        {
            try
            {
                return await operation();
            }
            catch(Exception e)
            {                                 
                System.Diagnostics.Debug.WriteLine(e);
                return new T();
            }
        }
    }
}

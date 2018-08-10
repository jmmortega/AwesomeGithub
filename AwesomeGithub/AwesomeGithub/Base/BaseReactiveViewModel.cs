using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeGithub.Base
{
    public abstract class BaseReactiveViewModel : ReactiveObject
    {


        protected virtual void InitializeCommand() { }

        public virtual void OnAppearing()
        { }

        public virtual void OnDisappearing()
        { }
                    

        protected async Task<T> ExecuteInternetCallAsync<T>(Func<Task<T>> operation)
        {
            try
            {
                return await operation();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return default(T);
            }
        }
    }
}

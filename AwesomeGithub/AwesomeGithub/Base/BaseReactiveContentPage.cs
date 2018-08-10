using ReactiveUI;
using ReactiveUI.XamForms;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeGithub.Base
{
    public abstract class BaseReactiveContentPage<T> : ReactiveContentPage<T>, IViewFor<T> where T : BaseReactiveViewModel
    {
        public BaseReactiveContentPage()
        {
            var viewModel = Activator.CreateInstance<T>();
            BindingContext = viewModel;
            this.WhenActivated(d => CreateBindings(d));
        }
        protected virtual void CreateBindings(Action<IDisposable> d)
        {}
        
    }
}

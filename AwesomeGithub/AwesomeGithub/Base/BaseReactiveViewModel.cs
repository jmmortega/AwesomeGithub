using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeGithub.Base
{
    public abstract class BaseReactiveViewModel : ReactiveObject
    {


        protected virtual void InitializeCommand() { }
    }
}

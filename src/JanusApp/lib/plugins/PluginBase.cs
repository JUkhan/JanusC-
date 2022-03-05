using System;
namespace JanusApp.lib.plugins
{
    public abstract class PluginBase
    {
        public event Action<Cause> Error;
        public event Action Success;

        protected virtual void OnError(string reason)
        {
            if (Error != null) Error(new Cause(reason));
        }
        protected virtual void OnSuccess()
        {
            if (Success != null) Success();
        }
    }
}

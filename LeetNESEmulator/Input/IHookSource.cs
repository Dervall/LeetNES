using System;

namespace NESInput.Hooks
{
    public interface IHookSource<out T> : IDisposable
    {
        bool IsEnabled { get; set; }
        void Register(int hookType, IHookSink<T> sink);
        void Unregister();
    }
}

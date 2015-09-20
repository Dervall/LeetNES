using System;

namespace NESInput.Hooks
{
    public interface IHookSink<in T>
    {
        void OnDataReceived(int message, T data);
        void OnError(Exception exception);
    }
}

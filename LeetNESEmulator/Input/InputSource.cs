using System;
using NESInput.Hooks;

namespace LeetNESEmulator.Input
{
    public sealed class InputSource : IHookSink<Win32KeyboardHookStruct>, IInputSource, IDisposable
    {
        private readonly IHookSource<Win32KeyboardHookStruct> _source;
        private volatile int _state;

        public InputSource()
        {
            _source = new Win32HookSource<Win32KeyboardHookStruct>();
            _source.Register((int)Win32HookType.KeyboardLowLevel, this);
            _source.IsEnabled = true;
        }

        public byte GetState()
        {
            return (byte)(_state & 0xff);
        }

        void IHookSink<Win32KeyboardHookStruct>.OnDataReceived(int message, Win32KeyboardHookStruct data)
        {
            const int vkCodeLeft = 37;
            const int vkCodeRight = 39;
            const int vkCodeUp = 38;
            const int vkCodeDown = 40;
            const int vkCodeA = 65;
            const int vkCodeB = 66;
            const int vkCodeS = 83;
            const int vkCodeR = 82;
            var vkCode = data.VkCode;
            var isPressed = (data.Flags & 0x80) == 0;
            switch (vkCode)
            {
                case vkCodeLeft:
                    if (isPressed)
                    {
                        _state |= 0x01;
                    }
                    else
                    {
                        _state &= ~0x01;
                    }
                    break;
               case vkCodeRight:
                    if (isPressed)
                    {
                        _state |= 0x02;
                    }
                    else
                    {
                        _state &= ~0x02;
                    }
                    break;
               case vkCodeUp:
                    if (isPressed)
                    {
                        _state |= 0x04;
                    }
                    else
                    {
                        _state &= ~0x04;
                    }
                    break; 
               case vkCodeDown:
                    if (isPressed)
                    {
                        _state |= 0x08;
                    }
                    else
                    {
                        _state &= ~0x08;
                    }
                    break; 
               case vkCodeA:
                    if (isPressed)
                    {
                        _state |= 0x10;
                    }
                    else
                    {
                        _state &= ~0x10;
                    }
                    break;
               case vkCodeB:
                    if (isPressed)
                    {
                        _state |= 0x20;
                    }
                    else
                    {
                        _state &= ~0x20;
                    }
                    break;
               case vkCodeS:
                    if (isPressed)
                    {
                        _state |= 0x40;
                    }
                    else
                    {
                        _state &= ~0x40;
                    }
                    break;
               case vkCodeR:
                    if (isPressed)
                    {
                        _state |= 0x80;
                    }
                    else
                    {
                        _state &= ~0x80;
                    }
                    break;
            }
        }

        void IHookSink<Win32KeyboardHookStruct>.OnError(Exception exception)
        {
        }

        public void Dispose()
        {
            _source.Dispose();
        }
    }
}

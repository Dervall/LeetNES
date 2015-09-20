using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using LeetNESEmulator.Input;

namespace NESInput.Hooks
{
    public sealed class Win32HookSource<T> : IHookSource<T>
    {
        private readonly User32Facade _user32;
        private readonly ReaderWriterLockSlim _isEnabledLock = new ReaderWriterLockSlim();
        private IntPtr _hookHandle;
        private User32Facade.HookProc _hookProc;        
        private IHookSink<T> _sink;
        private bool _isEnabled;

        public Win32HookSource()
            : this(new User32())
        {
        }

        public Win32HookSource(User32Facade user32)
        {
            _user32 = user32;
        }

        ~Win32HookSource()
        {
            Dispose(false);
        }

        public bool IsEnabled
        {
            get
            {
                try
                {
                    _isEnabledLock.EnterReadLock();
                    return _isEnabled;
                }
                finally
                {
                    _isEnabledLock.ExitReadLock();
                }
            }

            set
            {
                try
                {
                    _isEnabledLock.EnterWriteLock();
                    _isEnabled = value;
                }
                finally
                {
                    _isEnabledLock.ExitWriteLock();
                }
            }
        }

        public void Register(int hookType, IHookSink<T> sink)
        {
            if (_hookHandle != IntPtr.Zero)
            {
                throw new InvalidOperationException("HookSource has already registered.");
            }
            _sink = sink;
            _hookProc = HookProc;
            var moduleHandle = Environment.OSVersion.Version.Major > 5 ? IntPtr.Zero : Process.GetCurrentProcess().MainModule.BaseAddress;
            _hookHandle = _user32.SetWindowsHookEx(hookType, _hookProc, moduleHandle, 0);
            if (_hookHandle == IntPtr.Zero)
            {
                throw new Win32Exception(Marshal.GetLastWin32Error());
            }
        }

        public void Unregister()
        {
            if (_hookHandle == IntPtr.Zero)
            {
                return;
            }
            try
            {
                if (!_user32.UnhookWindowsHookEx(_hookHandle))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
            }
            finally
            {
                _hookHandle = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    _isEnabledLock.Dispose();
                }

                Unregister();
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {
            }            
        }
        
        private IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (_sink == null)
            {
                return _user32.CallNextHookEx(_hookHandle, nCode, wParam, lParam);
            }
            try
            {
                if (nCode >= 0 && IsEnabled)
                {
                    var message = wParam.ToInt32();
                    var data = (T)Marshal.PtrToStructure(lParam, typeof(T));
                    _sink.OnDataReceived(message, data);
                }
            }
            catch (Exception exception)
            {
                try
                {
                    _sink.OnError(exception);
                }
// ReSharper disable EmptyGeneralCatchClause
                catch
// ReSharper restore EmptyGeneralCatchClause
                {
                    // Eat exception =)
                }                    
            }
            return _user32.CallNextHookEx(_hookHandle, nCode, wParam, lParam);
        }
    }
}

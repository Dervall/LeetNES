using System;

namespace LeetNESEmulator.Input
{
    public abstract class User32Facade
    {  
        public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        public abstract IntPtr SetWindowsHookEx(int hookId, HookProc hookProcedure, IntPtr moduleHandle, uint threadId);
        public abstract bool UnhookWindowsHookEx(IntPtr hookId);
        public abstract IntPtr CallNextHookEx(IntPtr hookHandle, int nCode, IntPtr wParam, IntPtr lParam);
    }
}

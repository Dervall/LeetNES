using System;
using System.Runtime.InteropServices;

namespace LeetNESEmulator.Input
{
    public sealed class User32 : User32Facade
    {
        private const string DllImportPath = "user32.dll";

        public override IntPtr SetWindowsHookEx(int hookId, HookProc hookProcedure, IntPtr moduleHandle, uint threadId)
        {
            return DoSetWindowsHookEx(hookId, hookProcedure, moduleHandle, threadId);
        }

        public override bool UnhookWindowsHookEx(IntPtr hookId)
        {
            return DoUnhookWindowsHookEx(hookId);
        }

        public override IntPtr CallNextHookEx(IntPtr hookHandle, int nCode, IntPtr wParam, IntPtr lParam)
        {
            return DoCallNextHookEx(hookHandle, nCode, wParam, lParam);
        }

        [DllImport(DllImportPath, SetLastError = true, EntryPoint = "SetWindowsHookEx")]
        private static extern IntPtr DoSetWindowsHookEx(int hookId, HookProc hookProcedure, IntPtr moduleHandle, uint threadId);

        [DllImport(DllImportPath, SetLastError = true, EntryPoint = "UnhookWindowsHookEx")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DoUnhookWindowsHookEx(IntPtr hookId);

        [DllImport(DllImportPath, EntryPoint = "CallNextHookEx")]
        private static extern IntPtr DoCallNextHookEx(IntPtr hookHandle, int nCode, IntPtr wParam, IntPtr lParam);
    }
}

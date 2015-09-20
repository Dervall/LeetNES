using System;

namespace NESInput.Hooks
{
    public struct Win32KeyboardHookStruct
    {
        public int VkCode;
        public int ScanCode;
        public int Flags;
        public int Time;        
        public IntPtr DwExtraInfo;
    }
}

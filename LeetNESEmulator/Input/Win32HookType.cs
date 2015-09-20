using System.Diagnostics.CodeAnalysis;

namespace NESInput.Hooks
{
    public enum Win32HookType
    {
        MessageFilter = -1,
        JournalRecord,
        JournalPlayback,
        Keyboard,
        GetMessage,
        CallWndProc,
        Cbt,
        SysMsgFilter,
        Mouse,
        Hardware,
        Debug,
        Shell,
        ForegroundIdle,
        CallWndProcRet,
        KeyboardLowLevel,
        MouseLowLevel
    }
}

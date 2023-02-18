using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Selenium.Heroes.Common;

public static class Windows
{
    // http://msdn.microsoft.com/en-us/library/ms686033(VS.85).aspx
    [DllImport("kernel32.dll")]
    private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

    public static void SetLongRunningConsoleMode()
    {
        IntPtr handle = Process.GetCurrentProcess().MainWindowHandle;
        SetConsoleMode(handle, ENABLE_EXTENDED_FLAGS);
    }

    private const uint ENABLE_EXTENDED_FLAGS = 0x0080;
}

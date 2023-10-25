using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace System
{

    public static class NativeMethods
    {
        [DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr,
    ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        public static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            if (IsWindows10OrGreater(17763))
            {
                var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (IsWindows10OrGreater(18985))
                {
                    attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                }

                int useImmersiveDarkMode = enabled ? 1 : 0;
                return DwmSetWindowAttribute(handle, (int)attribute,
                    ref useImmersiveDarkMode, sizeof(int)) == 0;
            }

            return false;
        }

        const string UxTHEME = "UxTheme.dll";

        [DllImport(UxTHEME, SetLastError = true, CharSet = CharSet.Unicode,
            ExactSpelling = true)]
        public static extern int SetWindowTheme(
             IntPtr hWnd,
             string pszSubAppName,
             string pszSubIdList
            );

        [DllImport("uxtheme.dll", EntryPoint = "#135", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern int SetPreferredAppMode(int preferredAppMode);

        [DllImport("uxtheme.dll", EntryPoint = "#136", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern void FlushMenuThemes();

        public static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 &&
                Environment.OSVersion.Version.Build >= build;
        }
    }
}

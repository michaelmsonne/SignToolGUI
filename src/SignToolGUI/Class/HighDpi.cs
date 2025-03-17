using System;
using System.Runtime.InteropServices;

namespace SignToolGUI.Class
{
    internal class HighDpi
    {
        // Importing the SetProcessDPIAware function from user32.dll to make the process DPI aware
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        // Public method to enable high DPI awareness for the application
        public static bool Enable()
        {
            try
            {
                // Check if the OS version is Windows 7 (version 6.1) or higher
                if (Environment.OSVersion.Version.Major > 6 ||
                    (Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 1))
                {
                    // If the condition is met, call the SetProcessDPIAware function
                    return SetProcessDPIAware();
                }
                // If the OS version is lower than Windows 7, return false as DPI awareness cannot be enabled
                return false;
            }
            catch (Exception)
            {
                // If an exception occurs, return false indicating that DPI awareness could not be enabled
                return false;
            }
        }
    }
}
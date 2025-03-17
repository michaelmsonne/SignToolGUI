using System;
using System.Windows.Forms;
using SignToolGUI.Class;
using SignToolGUI.Forms;

namespace SignToolGUI
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            HighDpi.Enable();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
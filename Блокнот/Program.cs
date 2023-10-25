using System;
using System.Windows.Forms;

namespace Блокнот
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 1)
            {
                // run as windows app
                Application.EnableVisualStyles();
                Application.Run(new forma());
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new forma());
            }
        }
    }
}

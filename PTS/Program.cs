//#define LANGUAGE_IS_ENGLISH

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PTS
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if LANGUAGE_IS_ENGLISH
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
#endif

            Application.Run(new frmMain());
        }
    }
}

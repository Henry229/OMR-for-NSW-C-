using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CSedu.OMR
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            frmLogin login = new frmLogin();

            if (login.ShowDialog() != DialogResult.OK)
            {
                Application.Exit();
                return;
            }

                Application.Run(new frmReader());

        }
    }
}

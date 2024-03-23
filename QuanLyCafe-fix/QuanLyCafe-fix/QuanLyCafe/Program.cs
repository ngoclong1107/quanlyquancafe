using QuanLyCafe;
using QuanLyCafe.DangNhap;
using QuanLyCafe.FormChinh;
using QuanLyCafe.Forms;
using System;
using System.Windows.Forms;

namespace Menu
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormDangNhap());
        }
    }
}

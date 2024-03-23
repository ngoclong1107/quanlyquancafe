using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCafe.Forms
{
    public partial class FormDoiThongTin : Form
    {
        public FormDoiThongTin()
        {
            InitializeComponent();
            LoadTheme();
        }
        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    btn.BackColor = ThemeColor.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
            }
            gbChucNang.BorderColor = ThemeColor.SecondaryColor;
            gbDanhSach.BorderColor = ThemeColor.SecondaryColor;
            gbThongTin.BorderColor = ThemeColor.SecondaryColor;
            btnSua.BackColor = ThemeColor.PrimaryColor;
            btnThem.BackColor = ThemeColor.PrimaryColor;
            btnXoa.BackColor = ThemeColor.PrimaryColor;
            btnResetPassword.BackColor = ThemeColor.PrimaryColor;

        }
    }
}

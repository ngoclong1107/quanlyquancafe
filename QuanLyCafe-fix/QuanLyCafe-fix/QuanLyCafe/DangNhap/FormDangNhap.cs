using QuanLyCafe.Data;
using QuanLyCafe.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Tulpep.NotificationWindow;

namespace QuanLyCafe.DangNhap
{
    public partial class FormDangNhap : Form
    {
        public FormDangNhap()
        {
            InitializeComponent();
        }

        private void txbTenDangNhap_Click(object sender, EventArgs e)
        {
            txbTenDangNhap.Clear(); 
            flowLayoutPanel1.ForeColor = Color.FromArgb(78, 184, 206);
            txbTenDangNhap.ForeColor = Color.FromArgb(78, 184, 206);
        }

        private void txbMatKhau_Click(object sender, EventArgs e)
        {
            txbMatKhau.Clear();
            flowLayoutPanel1.ForeColor = Color.FromArgb(78, 184, 206);
            txbTenDangNhap.ForeColor = Color.FromArgb(78, 184, 206);
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            NhanVienModel nv;
            string user = txbTenDangNhap.Text;
            string pw = txbMatKhau.Text;
            DataContextDataContext db = new DataContextDataContext();
            NhanVien tk = db.NhanViens.SingleOrDefault(n => n.TenDangNhap == user && n.MatKhau == pw);
            if (pw == null || pw.Equals(""))
            {
                MessageBox.Show("Mật khẩu không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (tk == null)
            {
                MessageBox.Show("Tài khoản có thể chưa tồn tại hoặc sai tài khoản, mật khẩu", "Không tìm được tài khoản", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                nv = new NhanVienModel(tk.TenDangNhap, tk.Ten, tk.MaNhanVien, tk.MatKhau);
                //MessageBox.Show("Đăng nhập thành công!\nXin chào " + tk.TenDangNhap, "Đăng nhập thành công!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // Hiển thị thông báo thành công
                PopupNotifier popup = new PopupNotifier();
                popup.Image = Properties.Resources.icons8_done_80px;
                popup.Size = new Size(300, 100);
                popup.TitleText = "Thông báo";
                popup.ContentText = "Đăng nhập thành công\nXin chào " + tk.Ten;
                popup.ShowCloseButton = true;
                popup.Delay = 3000;
                popup.Popup();

                FormMenu mv = new FormMenu(nv);
                mv.Show();
                this.Hide();
            }
        }
    public static string MD5Hash(string text)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
        byte[] result = md5.Hash;
        StringBuilder strBuilder = new StringBuilder();
        for (int i = 0; i < result.Length; i++)
        {
            strBuilder.Append(result[i].ToString("x2"));
        }
        return strBuilder.ToString();
    }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bạn muốn thoát?", "Thoát chương trình?", MessageBoxButtons.YesNo);
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked==true)
            {
                txbMatKhau.UseSystemPasswordChar = false;

            }

            else
            {
                txbMatKhau.UseSystemPasswordChar=true;
            }
        }
    }
}

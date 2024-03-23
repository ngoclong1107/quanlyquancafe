using QuanLyCafe.Data;
using QuanLyCafe.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace QuanLyCafe.Forms
{
    public partial class FromInfoUserStaff : Form
    {
        private NhanVienModel nhanVienBanHang;
        string tendangnhap;
        public FromInfoUserStaff(NhanVienModel nv)
        {

            InitializeComponent();
            nhanVienBanHang = nv;
            ShowInfo();
            LoadTheme();
        }
        void ShowInfo()
        {
            txbTenNguoiDung.Text = nhanVienBanHang.TenDangNhap;
            txbTenDangNhap.Text = nhanVienBanHang.Ten;
            tendangnhap = txbTenDangNhap.Text;
        }

        private bool CheckInfor()
        {
            if (txbMatKhau.Text == "")
            {
                return false;
            }
            return true;
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
            gbThongTin.BorderColor = ThemeColor.SecondaryColor;
            btnCapNhat.BackColor = ThemeColor.PrimaryColor;

        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            if (CheckInfor() == true)
            {
                if (txbTenDangNhap.Text != nhanVienBanHang.Ten && txbMatKhauMoi.Text == "" && txbXacNhanMatKhau.Text == "")
                {
                    if (string.Compare(txbMatKhau.Text, nhanVienBanHang.Matkhau) != 0)
                    {
                        MessageBox.Show("Mật khẩu không đúng");
                        return;
                    }
                    //MessageBox.Show("Tài khoản đã khác ban đầu");
                    DataContextDataContext db = new DataContextDataContext();
                    var query = from h in db.NhanViens
                                where h.MaNhanVien == nhanVienBanHang.Ma && h.Ten == nhanVienBanHang.TenDangNhap
                                select new { h.MaNhanVien, h.Ten };
                    int? MaNV = query.SingleOrDefault()?.MaNhanVien;
                    //string? TenTK = query.SingleOrDefault()?.Ten;
                    NhanVien upNv = db.NhanViens.SingleOrDefault(p => p.MaNhanVien == nhanVienBanHang.Ma && p.TenDangNhap == nhanVienBanHang.TenDangNhap);
                    {
                        //upNv.MaNhanVien = upNv.MaNhanVien;
                        upNv.Ten = txbTenDangNhap.Text;
                        upNv.MatKhau = upNv.MatKhau;
                        upNv.TenDangNhap = upNv.TenDangNhap;
                        upNv.Gioitinh = upNv.Gioitinh;
                        upNv.sdt = upNv.sdt;
                        upNv.Role = upNv.Role;
                    }
                    db.SubmitChanges();
                    nhanVienBanHang.Ten = txbTenDangNhap.Text;
                    ShowInfo();
                    MessageBox.Show("Đổi tên thành công");
                }
                else
                {

                }

                if (txbMatKhauMoi.Text != "" && txbXacNhanMatKhau.Text != "")
                {

                    if (string.Compare(txbMatKhau.Text, nhanVienBanHang.Matkhau) != 0)
                    {
                        MessageBox.Show("Mật khẩu không đúng");
                        return;
                    }

                    //Tiến hành đổi mật khẩu
                    if (string.Compare(txbMatKhauMoi.Text, txbXacNhanMatKhau.Text) != 0)
                    {
                        MessageBox.Show("Mật khẩu mới và mật khẩu xác nhận trùng nhau");
                        return;
                    }

                    DataContextDataContext db = new DataContextDataContext();
                    var query = from h in db.NhanViens
                                where h.MaNhanVien == nhanVienBanHang.Ma && h.TenDangNhap == nhanVienBanHang.TenDangNhap
                                select new { h.MaNhanVien, h.Ten };
                    int? MaNV = query.SingleOrDefault()?.MaNhanVien;

                    if (MaNV.HasValue)
                    {
                        NhanVien upNv = db.NhanViens.SingleOrDefault(p => p.MaNhanVien == MaNV.Value);
                        {
                            upNv.Ten = nhanVienBanHang.Ten;
                            upNv.MatKhau = txbMatKhauMoi.Text;
                        }
                        db.SubmitChanges();
                        nhanVienBanHang.Ten = txbTenDangNhap.Text;
                        ShowInfo();
                        // Hiển thị thông báo thành công
                        PopupNotifier popup = new PopupNotifier();
                        popup.Image = Properties.Resources.icons8_done_80px;
                        popup.Size = new Size(300, 100);
                        popup.TitleText = "Thông báo";
                        popup.ContentText = "Thao tác đổi tên/mật khẩu thành công!";
                        popup.ShowCloseButton = true;
                        popup.Delay = 3000;
                        popup.Popup();
                    }

                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng nhập thêm ít nhất là 1 trường mật khẩu hiện tại");
            }
        }
    }
}

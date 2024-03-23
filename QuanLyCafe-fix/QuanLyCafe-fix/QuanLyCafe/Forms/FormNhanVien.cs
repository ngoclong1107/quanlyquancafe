using QuanLyCafe.Data;
using QuanLyCafe.ThongBao;
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
    public partial class FormNhanVien : Form
    {
        int DD;
        public FormNhanVien()
        {
            InitializeComponent();
            LoadTheme();
        }


        public void hienthi()
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {
                var query = from nv in db.NhanViens
                            join pq in db.Roles
                            on nv.Role equals pq.Marole
                            select new { nv.MaNhanVien, nv.Ten, nv.MatKhau, nv.TenDangNhap, nv.Gioitinh, nv.sdt, nv.Role, nv.Role1, pq.TenRole };
                dataGridView1.DataSource = query.ToList();
                DD = query.Count();
            }


            dataGridView1.Columns["TenRole"].DisplayIndex = 3;
            dataGridView1.Columns["MaNhanVien"].Visible = false;
            dataGridView1.Columns["Role1"].Visible = false;
            dataGridView1.Columns["Role"].Visible = false;
            dataGridView1.Columns["Matkhau"].Visible = false;
            dataGridView1.Columns["Ten"].HeaderText = "Họ và tên";
            dataGridView1.Columns["Gioitinh"].HeaderText = "Giới tính";
            dataGridView1.Columns["TenDangNhap"].HeaderText = "Tên đăng nhập";
            dataGridView1.Columns["sdt"].HeaderText = "Số điện thoại";
            dataGridView1.Columns["TenRole"].HeaderText = "Chức vụ";
        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            hienthi();
            DataContextDataContext gt = new DataContextDataContext();
            comboBox2.DataSource = gt.Roles.ToList();
            comboBox2.DisplayMember = "TenRole";
            comboBox2.ValueMember = "Marole";
            //Hiện combobox giới tính
            //addnv.Role = (int?)comboBox2.SelectedValue;

            comboBox1.Items.Add("Nam");
            comboBox1.Items.Add("Nữ");
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
            btnTaiLai.BackColor = ThemeColor.PrimaryColor;
            btnThem.BackColor = ThemeColor.PrimaryColor;
            btnTimKiem.BackColor = ThemeColor.PrimaryColor;
            btnXoa.BackColor = ThemeColor.PrimaryColor;

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Gioitinh")
            {
                var data = dataGridView1.Rows[e.RowIndex].DataBoundItem as dynamic;
                if (data != null)
                {
                    bool gioiTinh = (bool)data.Gioitinh;
                    string gioiTinhString = gioiTinh ? "Nữ" : "Nam";
                    e.Value = gioiTinhString;
                    e.FormattingApplied = true;
                }
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {
                txbTen.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["Ten"].Value.ToString();
                txbTenDangNhap.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["TenDangNhap"].Value.ToString();
                bool sex = (bool)dataGridView1.SelectedCells[0].OwningRow.Cells["Gioitinh"].Value;
                if (sex == false)
                {
                    comboBox1.Text = "Nam";
                }
                else
                {
                    comboBox1.Text = "Nữ";
                }
                txbSDT.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["sdt"].Value.ToString();
                comboBox2.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["TenRole"].Value.ToString();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            string id = dataGridView1.SelectedCells[0].OwningRow.Cells["MaNhanVien"].Value.ToString();

            if (DD > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có muốn xóa không?", "Xóa nhân viên", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DataContextDataContext db = new DataContextDataContext();

                    NhanVien delete = db.NhanViens.Where(p => p.MaNhanVien.Equals(id)).SingleOrDefault();
                    db.NhanViens.DeleteOnSubmit(delete);
                    db.SubmitChanges();
                    hienthi();
                    PopupNotifier popup = new PopupNotifier();
                    popup.Image = Properties.Resources.icons8_done_80px;
                    popup.Size = new Size(300, 100);
                    popup.TitleText = "Thông báo";
                    popup.ContentText = "Thao tác xóa thành công!";
                    popup.ShowCloseButton = true;
                    popup.Delay = 3000;
                    popup.Popup();

                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nv muốn xóa nếu dữ liệu không trống");
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            DataContextDataContext db = new DataContextDataContext();

            NhanVien addnv = new NhanVien();
            if (addnv != null)
            {
                if (string.IsNullOrEmpty(txbTen.Text))
                {
                    MessageBox.Show("Tên Nhân Viên Không Được Để Trống.");
                    return;
                }
                else
                {
                    addnv.Ten = txbTen.Text;
                }
                if (string.IsNullOrEmpty(txbTenDangNhap.Text))
                {
                    MessageBox.Show("Tên Đăng Nhập Không Được Để Trống.");
                    return;
                }
                else
                {
                    addnv.TenDangNhap = txbTenDangNhap.Text;
                }
                if (string.IsNullOrEmpty(txbMatKhau.Text))
                {
                    MessageBox.Show("Mật Khẩu Không Được Để Trống.");
                    return;
                }
                else
                {
                    addnv.MatKhau = txbMatKhau.Text;
                }
                if (string.IsNullOrEmpty(txbSDT.Text))
                {
                    MessageBox.Show("Số điện thoại không được để trống.");
                    return;
                }
                else if (txbSDT.Text.Length != 10) // Check if phone number has 10 digits
                {
                    MessageBox.Show("Số điện thoại phải có đúng 10 chữ số.");
                    return;
                }
                else
                {
                    addnv.sdt = txbSDT.Text;
                }

                addnv.MatKhau = txbMatKhau.Text;

                bool sex;
                if (dataGridView1.DataSource.ToString() == null)
                {
                    sex = (bool)dataGridView1.SelectedCells[0].OwningRow.Cells["Gioitinh"].Value;
                    if (sex == false)
                    {
                        comboBox1.Text = "Nam";
                    }
                    else
                    {
                        comboBox1.Text = "Nữ";
                    }
                }
                else
                {
                    if (comboBox1.Text == "Nữ")
                    {
                        sex = true;
                    }
                    else if (comboBox1.Text == "Nam")
                    {
                        sex = false;
                    }
                    else
                    {
                        sex = true;
                    }

                }

                addnv.Gioitinh = sex;

                addnv.Role = (int?)comboBox2.SelectedValue;

                db.NhanViens.InsertOnSubmit(addnv);

                db.SubmitChanges();

                hienthi();
                PopupNotifier popup = new PopupNotifier();
                popup.Image = Properties.Resources.icons8_done_80px;
                popup.Size = new Size(300, 100);
                popup.TitleText = "Thông báo";
                popup.ContentText = "Thao tác thêm thành công!";
                popup.ShowCloseButton = true;
                popup.Delay = 3000;
                popup.Popup();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {

            DataContextDataContext db = new DataContextDataContext();
            int id = Convert.ToInt32(dataGridView1.SelectedCells[0].OwningRow.Cells["MaNhanVien"].Value);
            NhanVien addnv = db.NhanViens.SingleOrDefault(p => p.MaNhanVien == id);

            if (addnv != null)
            {
                addnv.Ten = txbTen.Text;
                addnv.MatKhau = txbMatKhau.Text;
                addnv.TenDangNhap = txbTenDangNhap.Text;
                bool gioiTinh = false;
                if (comboBox1.SelectedIndex == 1)
                {
                    gioiTinh = true;
                }
                addnv.Gioitinh = gioiTinh;
                addnv.sdt = txbSDT.Text;
                addnv.Role = (int?)comboBox2.SelectedValue;
            }
            db.SubmitChanges();
            hienthi();
            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.icons8_done_80px;
            popup.Size = new Size(300, 100);
            popup.TitleText = "Thông báo";
            popup.ContentText = "Thao tác sửa thành công!";
            popup.ShowCloseButton = true;
            popup.Delay = 3000;
            popup.Popup();
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            hienthi();
            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.icons8_done_80px;
            popup.Size = new Size(300, 100);
            popup.TitleText = "Thông báo";
            popup.ContentText = "Thao tác tải lại thành công!";
            popup.ShowCloseButton = true;
            popup.Delay = 3000;
            popup.Popup();
        }

        public void Alert(string message)
        {
            UserAlert frm = new UserAlert();
            frm.showAlert(message);
        }
        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txbTimKiemNhanVien.Text.Trim();

            using (DataContextDataContext db = new DataContextDataContext())
            {
                var query = from nv in db.NhanViens
                            join pq in db.Roles
                           on nv.Role equals pq.Marole
                            where nv.Ten.Contains(keyword)
                            select new { nv.MaNhanVien, nv.Ten, nv.MatKhau, nv.TenDangNhap, nv.Gioitinh, nv.sdt, nv.Role, nv.Role1, pq.TenRole };
                dataGridView1.DataSource = query.ToList();
            }
        }
    }

}

using Bunifu.UI.WinForms;
using QuanLyCafe.Data;
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
    public partial class FormKhuyenMai : Form
    {
        public FormKhuyenMai()
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
                    btn.FlatAppearance.BorderColor = ThemeColor.PrimaryColor;

                }
                bunifuGroupBox1.BorderColor = ThemeColor.SecondaryColor;
                bunifuGroupBox2.BorderColor = ThemeColor.SecondaryColor;
                bunifuGroupBox3.BorderColor = ThemeColor.SecondaryColor;
                btnChiTietKM.BackColor = ThemeColor.PrimaryColor;
                btnTaiLai.BackColor = ThemeColor.PrimaryColor;
                btnThem.BackColor = ThemeColor.PrimaryColor;
                btnSua.BackColor = ThemeColor.PrimaryColor;
                btnXoa.BackColor = ThemeColor.PrimaryColor;
                btnTimKiem.BackColor = ThemeColor.PrimaryColor;
            }
        }


       
        public void hienthi()
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {
                var query = from Khuyenmai in db.Khuyenmais
                            join mlkm in db.Loaikms on Khuyenmai.maloaikm equals mlkm.maloaikm
                            select new { Khuyenmai.makm, mlkm.maloaikm, Khuyenmai.tenkm, Khuyenmai.GiaKhuyenMai, Khuyenmai.tgbatdau, Khuyenmai.tgketthuc,   };
                dataGridView1.DataSource = query.ToList();
            }
                dataGridView1.Columns["makm"].HeaderText = "Mã Khuyến mãi";
                dataGridView1.Columns["tenkm"].HeaderText = "Tên Khuyến mãi";
                dataGridView1.Columns["tgbatdau"].HeaderText = "Thời gian bắt đầu";
                dataGridView1.Columns["tgketthuc"].HeaderText = "Thời gian kết thúc";
                dataGridView1.Columns["maloaikm"].HeaderText = "Mã loại Khuyến mãi";
                dataGridView1.Columns["GiaKhuyenMai"].HeaderText = "Giảm giá";
            dataGridView1.Columns["makm"].Width = 150;
            dataGridView1.Columns["tenkm"].Width = 200;
            dataGridView1.Columns["tgbatdau"].Width = 150;
            dataGridView1.Columns["tgketthuc"].Width = 150;
            dataGridView1.Columns["maloaikm"].Width = 150;
            dataGridView1.Columns["GiaKhuyenMai"].Width = 150;

            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView1.Refresh();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowTemplate.Height = 50;
            dataGridView1.ColumnHeadersHeight = 150;
        }

        private void FormKhuyenMai_Load(object sender, EventArgs e)
        {
           
            using (DataContextDataContext db = new DataContextDataContext())
            {
                hienthi();
                cbMaLoaiKM.DataSource = db.Loaikms.ToList();
                cbMaLoaiKM.DisplayMember = "maloaikm";
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            int makm = int.Parse(txbMaKM.Text);
            string tenkm = TxbTenKhuyenMai.Text;
            string giamGia = TxbGiamGia.Text;

            // Kiểm tra các trường không được để trống
            if (!int.TryParse(txbMaKM.Text, out makm) || string.IsNullOrEmpty(tenkm) || string.IsNullOrEmpty(giamGia))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int giaKhuyenMai;
            if (!int.TryParse(giamGia, out giaKhuyenMai))
            {
                MessageBox.Show("Giá khuyến mãi phải là số nguyên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataContextDataContext db = new DataContextDataContext();
            Khuyenmai addnv = new Khuyenmai();
            addnv.makm = makm;
            addnv.tenkm = tenkm;
            addnv.tgbatdau = DateTime.Parse(DateNgayBatDau.Text);
            addnv.tgketthuc = DateTime.Parse(DateNgayKetThuc.Text);
            addnv.maloaikm = ((Loaikm)cbMaLoaiKM.SelectedValue).maloaikm;
            addnv.GiaKhuyenMai = giaKhuyenMai;
            db.Khuyenmais.InsertOnSubmit(addnv);
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

        private void btnSua_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ các điều khiển
            int makm = int.Parse(txbMaKM.Text);
            string tenkm = TxbTenKhuyenMai.Text;
            string giamGia = TxbGiamGia.Text;

            // Kiểm tra các trường không được để trống
            if (!int.TryParse(txbMaKM.Text, out makm) || string.IsNullOrEmpty(tenkm) || string.IsNullOrEmpty(giamGia))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int giaKhuyenMai;
            if (!int.TryParse(giamGia, out giaKhuyenMai))
            {
                MessageBox.Show("Giá khuyến mãi phải là số nguyên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo đối tượng DataContext
            using (DataContextDataContext db = new DataContextDataContext())
            {
                // Tìm khuyến mãi cần sửa theo mã
                Khuyenmai khuyenmai = db.Khuyenmais.FirstOrDefault(km => km.makm == makm);
                if (khuyenmai == null)
                {
                    MessageBox.Show("Khuyến mãi không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Cập nhật thông tin khuyến mãi
                khuyenmai.tenkm = tenkm;
                khuyenmai.tgbatdau = DateTime.Parse(DateNgayBatDau.Text);
                khuyenmai.tgketthuc = DateTime.Parse(DateNgayKetThuc.Text);
                khuyenmai.maloaikm = ((Loaikm)cbMaLoaiKM.SelectedValue).maloaikm;
                khuyenmai.GiaKhuyenMai = giaKhuyenMai;

                // Lưu thay đổi vào cơ sở dữ liệu
                db.SubmitChanges();
            }

            // Hiển thị lại danh sách
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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            // Lấy mã khuyến mãi cần xóa từ TextBox txbMaKM
            int maKM = int.Parse(txbMaKM.Text);

            // Kiểm tra mã khuyến mãi không được để trống
            if (!int.TryParse(txbMaKM.Text, out maKM) == null)
            {
                MessageBox.Show("Vui lòng nhập mã khuyến mãi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Hiển thị hộp thoại xác nhận xóa
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa khuyến mãi này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                using (DataContextDataContext db = new DataContextDataContext())
                {
                    // Tìm khuyến mãi cần xóa trong CSDL
                    Khuyenmai khuyenmai = db.Khuyenmais.FirstOrDefault(km => km.makm == maKM);
                    if (khuyenmai != null)
                    {
                        // Xóa khuyến mãi
                        db.Khuyenmais.DeleteOnSubmit(khuyenmai);
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
                    else
                    {
                        MessageBox.Show("Khuyến mãi không tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbTimkiem.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khuyến mãi muốn tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (DataContextDataContext db = new DataContextDataContext())
            {
                var query = from Khuyenmai in db.Khuyenmais
                            join mlkm in db.Loaikms on Khuyenmai.maloaikm equals mlkm.maloaikm
                            where Khuyenmai.tenkm.ToLower().Contains(txbTimkiem.Text.ToLower())
                            select new { Khuyenmai.makm, Khuyenmai.tenkm, Khuyenmai.tgbatdau, Khuyenmai.tgketthuc, mlkm.maloaikm, Khuyenmai.GiaKhuyenMai };

                dataGridView1.DataSource = query.ToList();
            }

            dataGridView1.Columns["makm"].HeaderText = "Mã Khuyến mãi";
            dataGridView1.Columns["tenkm"].HeaderText = "Tên Khuyến mãi";
            dataGridView1.Columns["tgbatdau"].HeaderText = "Thời gian bắt đầu";
            dataGridView1.Columns["tgketthuc"].HeaderText = "Thời gian kết thúc";
            dataGridView1.Columns["maloaikm"].HeaderText = "Mã loại Khuyến mãi";
            dataGridView1.Columns["GiaKhuyenMai"].HeaderText = "Giảm giá";
            dataGridView1.Columns["makm"].Width = 150;
            dataGridView1.Columns["tenkm"].Width = 200;
            dataGridView1.Columns["tgbatdau"].Width = 150;
            dataGridView1.Columns["tgketthuc"].Width = 150;
            dataGridView1.Columns["maloaikm"].Width = 150;
            dataGridView1.Columns["GiaKhuyenMai"].Width = 150;

            dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            dataGridView1.Refresh();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowTemplate.Height = 50;
            dataGridView1.ColumnHeadersHeight = 150;
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

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {
                txbMaKM.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["makm"].Value.ToString();
                TxbTenKhuyenMai.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["tenkm"].Value.ToString();
                DateNgayBatDau.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["tgbatdau"].Value.ToString();
                DateNgayKetThuc.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["tgketthuc"].Value.ToString();
                cbMaLoaiKM.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["maloaikm"].Value.ToString();
                TxbGiamGia.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["GiaKhuyenMai"].Value.ToString();
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "GiaKhuyenMai" && e.Value != null)
            {
                int giaKhuyenMai;
                if (int.TryParse(e.Value.ToString(), out giaKhuyenMai))
                {
                    e.Value = giaKhuyenMai.ToString("N0") + " VND"; // Định dạng giá trị và thêm chữ "VND"
                    e.FormattingApplied = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
               FormChiTietKM formChiTietKM = new FormChiTietKM();
            formChiTietKM.ShowDialog();
        }
    }
}

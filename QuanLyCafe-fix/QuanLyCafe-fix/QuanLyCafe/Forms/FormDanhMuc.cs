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
using Tulpep.NotificationWindow;

namespace QuanLyCafe.Forms
{

    public partial class FormDanhMuc : Form
    {

        public FormDanhMuc()
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
                btnSua.BackColor = ThemeColor.PrimaryColor;
                btnThem.BackColor = ThemeColor.PrimaryColor;
                btnTimKiem.BackColor = ThemeColor.PrimaryColor;
                btnXem.BackColor = ThemeColor.PrimaryColor;
                btnXoa.BackColor = ThemeColor.PrimaryColor;
            }
        }

        private void FormDanhMuc_Load(object sender, EventArgs e)
        {
            hienthi();
            DataContextDataContext gt = new DataContextDataContext();
        }
        private void hienthi()
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {
                var query = from dm in db.DanhMucs
                            select new { dm.MaDanhMuc, dm.Ten };

                dataGridView1.DataSource = query.ToList();
            }
            dataGridView1.Columns["MaDanhMuc"].HeaderText = "Mã danh mục";
            dataGridView1.Columns["Ten"].HeaderText = "Tên danh mục";
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowTemplate.Height = 50;
            dataGridView1.ColumnHeadersHeight = 150;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {
                txbTimKiem.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["Ten"].Value.ToString();

            }
        }
 
        private void btnThem_Click(object sender, EventArgs e)
        {
            DataContextDataContext db = new DataContextDataContext();
            if (string.IsNullOrWhiteSpace(txbTimKiem.Text))
            {
                MessageBox.Show("Bạn chưa nhập tên danh mục");
                return;
            }
            //int maDanhMuc;
            DanhMuc newDanhMuc = new DanhMuc
            {
                Ten = txbTimKiem.Text,

            };
            db.DanhMucs.InsertOnSubmit(newDanhMuc);
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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn xóa?", "Xác nhận Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                DataContextDataContext db = new DataContextDataContext();
                string id = dataGridView1.SelectedCells[0].OwningRow.Cells["MaDanhMuc"].Value.ToString();
                DanhMuc delete = db.DanhMucs.Where(p => p.MaDanhMuc.Equals(id)).SingleOrDefault();
                db.DanhMucs.DeleteOnSubmit(delete);
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
        }

      

        private void btnSua_Click(object sender, EventArgs e)
        {
            DataContextDataContext db = new DataContextDataContext();
            int id = Convert.ToInt32(dataGridView1.SelectedCells[0].OwningRow.Cells["MaDanhMuc"].Value);
            DanhMuc danhMuc = db.DanhMucs.SingleOrDefault(p => p.MaDanhMuc == id);
            if (danhMuc != null)
            {
                danhMuc.Ten = txbTimKiem.Text;
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
        }

        private void btnXem_Click(object sender, EventArgs e)
        {
            List<DanhMuc> danhSachDanhMuc;
            using (DataContextDataContext db = new DataContextDataContext())
            {
                danhSachDanhMuc = db.DanhMucs.ToList();
            }
            dataGridView1.DataSource = danhSachDanhMuc;
            dataGridView1.Columns["MaDanhMuc"].HeaderText = "Mã danh mục";
            dataGridView1.Columns["Ten"].HeaderText = "Tên danh mục";
            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.icons8_done_80px;
            popup.Size = new Size(300, 100);
            popup.TitleText = "Thông báo";
            popup.ContentText = "Thao tác tải lại thành công!";
            popup.ShowCloseButton = true;
            popup.Delay = 3000;
            popup.Popup();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbTimKiem.Text))
            {
                MessageBox.Show("Bạn chưa nhập từ khóa tìm kiếm");
                return;
            }
            using (DataContextDataContext db = new DataContextDataContext())
            {
                var query = from dm in db.DanhMucs
                            where dm.Ten.ToLower().Contains(txbTimKiem.Text.ToLower())
                            select new { dm.MaDanhMuc, dm.Ten };

                dataGridView1.DataSource = query.ToList();
            }
            dataGridView1.Columns["MaDanhMuc"].HeaderText = "Mã danh mục";
            dataGridView1.Columns["Ten"].HeaderText = "Tên danh mục";
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowTemplate.Height = 50;
            dataGridView1.ColumnHeadersHeight = 150;
        }
    }
}

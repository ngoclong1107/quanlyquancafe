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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using static MetroFramework.Drawing.MetroPaint.BorderColor;
using Button = System.Windows.Forms.Button;
using Tulpep.NotificationWindow;

namespace QuanLyCafe.Forms
{
    public partial class FormMon : Form
    {
        public FormMon()
        {
            InitializeComponent();
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 7);
            hienthi();
            dataGridView1.Columns.Clear();
            LoadTheme();
        }
        public void hienthi()
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {
                var query = from Mon in db.Mons
                            join dm in db.DanhMucs on Mon.MaDanhMuc equals dm.MaDanhMuc
                            join vt in db.Khos on Mon.mavt equals vt.mavt
                            join ncc in db.Nhacungcaps on Mon.mancc equals ncc.mancc
                            select new { Mon.MaMon, Mon.Ten, Mon.GiaTien, /*vt.tenvt,*/ Mon.MaDanhMuc, DanhMuc = dm.Ten, ncc.tenncc, Mon.Hinh };
                dataGridView1.DataSource = query.ToList();
                dataGridView1.DefaultCellStyle.Font = new Font("Arial", 7);

                DataGridViewImageColumn imageCol = new DataGridViewImageColumn();
                imageCol.Name = "HinhAnh";
                imageCol.HeaderText = "Hình Ảnh";
                imageCol.ImageLayout = DataGridViewImageCellLayout.Zoom;

                dataGridView1.Columns.Add(imageCol);

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    row.Height = 100;

                    var cellValue = row.Cells["Hinh"].Value;
                    if (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
                    {
                        string imagePath = @"../.." + cellValue.ToString();
                        if (File.Exists(imagePath))
                        {
                            try
                            {
                                Image img = Image.FromFile(imagePath);
                                row.Cells["HinhAnh"].Value = img;
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {

                            Image img = Image.FromFile(@"../../Hinh/default.jpg");
                            row.Cells["HinhAnh"].Value = img;
                        }
                    }
                    else
                    {
                        Image img = Image.FromFile(@"../../Hinh/default.jpg");
                        row.Cells["HinhAnh"].Value = img;
                    }
                }

            }
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 12);
            dataGridView1.AutoResizeColumnHeadersHeight();

            dataGridView1.Columns["MaMon"].Visible = false;
            dataGridView1.Columns["GiaTien"].HeaderText = "Giá tiền"; ;
            dataGridView1.Columns["MaDanhMuc"].Visible = false;
            dataGridView1.Columns["DanhMuc"].HeaderText = "Tên danh mục";
            dataGridView1.Columns["Ten"].HeaderText = "Tên món";
            dataGridView1.Columns["tenncc"].HeaderText = "Tên NCC";
            dataGridView1.Columns["GiaTien"].HeaderText = "Giá tiền";
            //dataGridView1.Columns["tenvt"].HeaderText = "Tên vật tư";
            dataGridView1.Columns["Hinh"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowTemplate.Height = 100;
            dataGridView1.ColumnHeadersHeight = 150;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                if (column is DataGridViewImageColumn)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                    column.Width = 100;
                }
            }
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
                btnTaiLai.BackColor = ThemeColor.PrimaryColor;
                btnThem.BackColor = ThemeColor.PrimaryColor;
                btnThemHinh.BackColor = ThemeColor.PrimaryColor;
                btnTimKiem.BackColor = ThemeColor.PrimaryColor;
                btnXoa.BackColor = ThemeColor.PrimaryColor;
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                var row = dataGridView1.SelectedRows[0];
                var tenValue = row.Cells["Ten"].Value;
                var danhMucValue = row.Cells["DanhMuc"].Value;
                var hinhValue = row.Cells["Hinh"].Value;
                var giaTienValue = row.Cells["GiaTien"].Value;
                //var tenvtValue = row.Cells["tenvt"].Value;
                var tennccValue = row.Cells["tenncc"].Value;
                var hinhAnhValue = row.Cells["HinhAnh"].Value;
                TxbTenmon.Text = tenValue?.ToString();
                cbTendanhmuc.Text = danhMucValue?.ToString();
                txbHinhminhhoa.Text = hinhValue?.ToString();
                TxbGiatien.Text = giaTienValue?.ToString();
                //cbTenvattu.Text = tenvtValue?.ToString();
                cbTenNcc.Text = tennccValue?.ToString();
                if (hinhAnhValue != null)
                {
                    pictureBox1.Image = (Image)hinhAnhValue;
                }
            }
        }
        private bool LuuHinhAnh(string duongDanHinh, string tenHinh)
        {

            try
            {
                Bitmap b = new Bitmap(duongDanHinh);
                if (File.Exists(@"../../Hinh/" + tenHinh))
                {
                    var randIndex = new Random();

                    tenHinh = randIndex.Next(0, 1000) + tenHinh;
                }


                b.Save(@"../../Hinh/" + tenHinh);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            {
                DataContextDataContext db = new DataContextDataContext();
                Mon addnv = new Mon();
                if (string.IsNullOrEmpty(TxbTenmon.Text))
                {
                    MessageBox.Show("Tên Món Không Được Để Trống.");
                    return;
                }
                else
                {
                    addnv.Ten = TxbTenmon.Text;
                }
                int giaTien;
                if (!int.TryParse(TxbGiatien.Text, out giaTien))
                {
                    MessageBox.Show("Giá Tiền Phải Là Số Nguyên.");
                    return;
                }
                else
                {
                    addnv.GiaTien = giaTien;
                }
                addnv.MaDanhMuc = Convert.ToInt32(cbTendanhmuc.SelectedValue);
                addnv.mavt = Convert.ToInt32(cbTenvattu.SelectedValue);
                addnv.mancc = Convert.ToInt32(cbTenNcc.SelectedValue);
                int pos = txbHinhminhhoa.Text.LastIndexOf("\\") + 1;
                string tenHinh = txbHinhminhhoa.Text.Substring(pos, txbHinhminhhoa.Text.Length - pos);
                try
                {
                    LuuHinhAnh(txbHinhminhhoa.Text, tenHinh);
                    addnv.Hinh = "//Hinh//" + tenHinh;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo lỗi lưu hình");
                }
                db.Mons.InsertOnSubmit(addnv);
                db.SubmitChanges();
                dataGridView1.Columns.Clear();
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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            int selectedRowIndex = dataGridView1.SelectedCells[0].OwningRow.Index;
            DialogResult confirmResult = MessageBox.Show("Bạn có muốn xóa món đang chọn?", "Xóa món", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                DataContextDataContext db = new DataContextDataContext();
                string id = dataGridView1.SelectedCells[0].OwningRow.Cells["MaMon"].Value.ToString();
                Mon delete = db.Mons.Where(p => p.MaMon.Equals(id)).SingleOrDefault();
                db.Mons.DeleteOnSubmit(delete);
                db.SubmitChanges();
                dataGridView1.Columns.Clear();
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
            int id = Convert.ToInt32(dataGridView1.SelectedCells[0].OwningRow.Cells["MaMon"].Value);

            Mon addnv = db.Mons.SingleOrDefault(p => p.MaMon == id);
            if (addnv != null)
            {
                if (string.IsNullOrEmpty(TxbTenmon.Text))
                {
                    MessageBox.Show("Tên không được để trống.");
                    return;
                }

                if (!int.TryParse(TxbGiatien.Text, out int giaTien))
                {
                    MessageBox.Show("Giá tiền phải là số nguyên.");
                    return;
                }

                addnv.GiaTien = giaTien;
                addnv.Ten = TxbTenmon.Text;
                addnv.MaDanhMuc = Convert.ToInt32(cbTendanhmuc.SelectedValue);
                addnv.mavt = Convert.ToInt32(cbTenvattu.SelectedValue);
                addnv.mancc = Convert.ToInt32(cbTenNcc.SelectedValue);
                int pos = txbHinhminhhoa.Text.LastIndexOf("\\") + 1;
                string tenHinh = txbHinhminhhoa.Text.Substring(pos, txbHinhminhhoa.Text.Length - pos);
                try
                {
                    LuuHinhAnh(txbHinhminhhoa.Text, tenHinh);
                    addnv.Hinh = "//Hinh//" + tenHinh;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Thông báo lỗi lưu hình");
                }
                db.SubmitChanges();
                dataGridView1.Columns.Clear();
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

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            {
                string keyword = txbTimkiemmon.Text;

                using (DataContextDataContext db = new DataContextDataContext())
                {
                    var query = from Mon in db.Mons
                                join dm in db.DanhMucs on Mon.MaDanhMuc equals dm.MaDanhMuc
                                join vt in db.Khos on Mon.mavt equals vt.mavt
                                join ncc in db.Nhacungcaps on Mon.mancc equals ncc.mancc
                                where Mon.Ten.Contains(keyword)
                                select new { Mon.MaMon, Mon.Ten, Mon.GiaTien, vt.tenvt, Mon.MaDanhMuc, DanhMuc = dm.Ten, ncc.tenncc, Mon.Hinh };

                    dataGridView1.DataSource = query.ToList();
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        row.Height = 100;

                        var cellValue = row.Cells["Hinh"].Value;
                        if (cellValue != null && !string.IsNullOrEmpty(cellValue.ToString()))
                        {
                            string imagePath = @"../.." + cellValue.ToString();
                            if (File.Exists(imagePath))
                            {
                                try
                                {
                                    Image img = Image.FromFile(imagePath);
                                    row.Cells["HinhAnh"].Value = img;
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                            else
                            {

                                Image img = Image.FromFile(@"../../Hinh/default.jpg");
                                row.Cells["HinhAnh"].Value = img;
                            }
                        }
                        else
                        {
                            Image img = Image.FromFile(@"../../Hinh/default.jpg");
                            row.Cells["HinhAnh"].Value = img;
                        }
                    }

                }

                dataGridView1.Columns["MaMon"].Visible = false;
                dataGridView1.Columns["GiaTien"].HeaderText = "Giá Tiền"; ;
                dataGridView1.Columns["MaDanhMuc"].Visible = false;
                dataGridView1.Columns["DanhMuc"].HeaderText = "Tên Danh Mục";
                dataGridView1.Columns["Ten"].HeaderText = "Tên món";
                dataGridView1.Columns["tenncc"].HeaderText = "Mã NCC";
                dataGridView1.Columns["GiaTien"].HeaderText = "Giá Tiền";
                dataGridView1.Columns["tenvt"].HeaderText = "Mã VT";
                dataGridView1.Columns["Hinh"].Visible = false;
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            dataGridView1.Columns.Clear();
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

        private void btnThemHinh_Click(object sender, EventArgs e)
        {
            {
                {
                    OpenFileDialog openFileDialog1 = new OpenFileDialog
                    {
                        InitialDirectory = @"C:\",
                        Title = "Browse Text Files",

                        CheckFileExists = true,
                        CheckPathExists = true,

                        DefaultExt = "JPG",
                        Filter = "Image files (*.jpg)|*.jpg",
                        FilterIndex = 2,
                        RestoreDirectory = true,

                        ReadOnlyChecked = true,
                        ShowReadOnly = true
                    };

                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
                        txbHinhminhhoa.Text = openFileDialog1.FileName;
                    }
                }
            }
        }

        private void FormMon_Load(object sender, EventArgs e)
        {
            dataGridView1.DefaultCellStyle.Font = new Font("Arial", 7);

            {
                using (DataContextDataContext db = new DataContextDataContext())
                {
                    hienthi();

                    cbTenvattu.DataSource = db.Khos.ToList();
                    cbTenvattu.DisplayMember = "tenvt";
                    cbTenvattu.ValueMember = "mavt";

                    cbTendanhmuc.DataSource = db.DanhMucs.ToList();
                    cbTendanhmuc.DisplayMember = "Ten";
                    cbTendanhmuc.ValueMember = "MaDanhMuc";

                    cbTenNcc.DataSource = db.Nhacungcaps.ToList();
                    cbTenNcc.DisplayMember = "tenncc";
                    cbTenNcc.ValueMember = "mancc";
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "GiaTien" && e.Value != null)
            {
                int GiaTien;
                if (int.TryParse(e.Value.ToString(), out GiaTien))
                {
                    e.Value = GiaTien.ToString("N0") + " VND"; // Định dạng giá trị và thêm chữ "VND"
                    e.FormattingApplied = true;
                }
            }
        }
    }
}

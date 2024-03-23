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
    public partial class FormNhaCungCap : Form
    {
        public FormNhaCungCap()
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
                gbDanhSach.BorderColor = ThemeColor.SecondaryColor;
                gbThongTin.BorderColor = ThemeColor.SecondaryColor;
                gbChucNang.BorderColor = ThemeColor.SecondaryColor;
                btnSua.BackColor = ThemeColor.PrimaryColor;
                btnThem.BackColor = ThemeColor.PrimaryColor;
                btnXoa.BackColor = ThemeColor.PrimaryColor;
            }
        }

        private void FormNhaCungCap_Load(object sender, EventArgs e)
        {
            hienthi();
        }
        public void hienthi()
        {
            {
                using (DataContextDataContext db = new DataContextDataContext())
                {
                    var query = from Nhacungcap in db.Nhacungcaps
                                select new { Nhacungcap.mancc, Nhacungcap.tenncc, Nhacungcap.dischi, Nhacungcap.sdt };
                    dataGridView1.DataSource = query.ToList();
                }

                dataGridView1.Columns["mancc"].HeaderText = "Mã nhà cung cấp";
                dataGridView1.Columns["tenncc"].HeaderText = "Tên nhà cung cấp";
                dataGridView1.Columns["dischi"].HeaderText = "Địa chỉ";
                dataGridView1.Columns["sdt"].HeaderText = "Số điện thoại";
                dataGridView1.Refresh();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.RowTemplate.Height = 50;
                dataGridView1.ColumnHeadersHeight = 150;


            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            DataContextDataContext db = new DataContextDataContext();

            Nhacungcap addnv = new Nhacungcap();

            if (!string.IsNullOrWhiteSpace(txbTenNCC.Text) && !string.IsNullOrWhiteSpace(txbDiaChi.Text))
            {
                addnv.tenncc = txbTenNCC.Text;
                addnv.dischi = txbDiaChi.Text;
                int sdt;
                if (int.TryParse(txbSDT.Text, out sdt))
                {

                    if (txbSDT.Text.Length == 10)
                    {
                        addnv.sdt = txbSDT.Text;
                        db.Nhacungcaps.InsertOnSubmit(addnv);
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
                    else
                    {
                        MessageBox.Show("Số điện thoại phải có 10 chữ số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng nhập chỉ số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Tên nhà cung cấp và địa chỉ không được để trống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {

            DataContextDataContext db = new DataContextDataContext();

            int id = Convert.ToInt32(dataGridView1.SelectedCells[0].OwningRow.Cells["mancc"].Value);
            Nhacungcap addnv = db.Nhacungcaps.SingleOrDefault(p => p.mancc == id);

            if (addnv != null)
            {
                addnv.tenncc = txbTenNCC.Text;
                addnv.dischi = txbDiaChi.Text;

                string sdt = txbSDT.Text.Trim(); // remove leading/trailing whitespaces
                bool isValidSdt = sdt.Length == 10 && int.TryParse(sdt, out int result);
                if (isValidSdt)
                {
                    addnv.sdt = sdt;
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
                else
                {
                    MessageBox.Show("Số điện thoại không hợp lệ. Vui lòng nhập 10 chữ số.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DataContextDataContext db = new DataContextDataContext();


            if (dataGridView1.SelectedCells.Count > 0)
            {
                string id = dataGridView1.SelectedCells[0].OwningRow.Cells["mancc"].Value.ToString();
                Nhacungcap delete = db.Nhacungcaps.Where(p => p.mancc.Equals(id)).SingleOrDefault();
                DialogResult result = MessageBox.Show($"Bạn có chắc chắn muốn xóa phiếu nhập có mã {id}?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    db.Nhacungcaps.DeleteOnSubmit(delete);
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
            else
            {
                MessageBox.Show("Vui lòng chọn một phiếu nhập để xóa.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {

                txbTenNCC.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["tenncc"].Value.ToString();
                txbDiaChi.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["dischi"].Value.ToString();
                txbSDT.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["sdt"].Value.ToString();
            }
        }
    }
}

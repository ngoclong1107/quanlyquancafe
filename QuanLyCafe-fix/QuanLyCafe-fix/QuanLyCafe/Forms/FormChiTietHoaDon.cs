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
    public partial class FormChiTietHoaDon : Form
    {
        int DD;
        private int mahoadon;
        public int MaHoaDon
        {
            get { return mahoadon; }
            set { mahoadon = value; txbMaHoaDon.Text = value.ToString(); }
        }
        public FormChiTietHoaDon()
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
                bunifuGroupBox4.BorderColor = ThemeColor.SecondaryColor;
                btnHienThi.BackColor = ThemeColor.PrimaryColor;
                btnSua.BackColor = ThemeColor.PrimaryColor;
                //btnThem.BackColor = ThemeColor.PrimaryColor;
                btnTimKiem.BackColor = ThemeColor.PrimaryColor;
                btnXoa.BackColor = ThemeColor.PrimaryColor;
            }
        }

        private void FormChiTietHoaDon_Load(object sender, EventArgs e)
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {

                hienthi();

                cbMaMon.DataSource = db.Mons.ToList();
                cbMaMon.DisplayMember = "Ten";
                cbMaMon.ValueMember = "MaMon";


                cbMaHoaDon.DataSource = db.HoaDons.ToList();
                cbMaHoaDon.DisplayMember = "MaHD";
                cbMaHoaDon.ValueMember = "MaHD";


            }

            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.DataSource = null;
        }
        public void hienthi()
        {
            {
                using (DataContextDataContext db = new DataContextDataContext())
                {
                    var query = from ChiTietHoaDon in db.ChiTietHoaDons
                                join mm in db.Mons on ChiTietHoaDon.MaMon equals mm.MaMon
                                join mhd in db.HoaDons on ChiTietHoaDon.MaHD equals mhd.MaHD
                                select new { ChiTietHoaDon.MaChiTietHD, mm.Ten ,ChiTietHoaDon.MaMon, ChiTietHoaDon.SoLuong, ChiTietHoaDon.MaHD,   ChiTietHoaDon.HoaDon, ChiTietHoaDon.Mon};
                    dataGridView1.DataSource = query.ToList();
                }

                dataGridView1.Columns["MaChiTietHD"].HeaderText = "Mã chi tiết HĐ";
                dataGridView1.Columns["MaHD"].HeaderText = "Mã hóa đơn";
                dataGridView1.Columns["Ten"].HeaderText = "Tên món";
                dataGridView1.Columns["MaMon"].Visible = false;
                dataGridView1.Columns["SoLuong"].HeaderText = "Số lượng";
                dataGridView1.Refresh();
                dataGridView1.Columns["HoaDon"].Visible = false;
                dataGridView1.Columns["Mon"].Visible = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.RowTemplate.Height = 50;
                dataGridView1.ColumnHeadersHeight = 150;
                if (dataGridView1.Rows.Count > 0)
                {
                    int lastRow = dataGridView1.Rows.Count - 1;
                    dataGridView1.CurrentCell = dataGridView1.Rows[lastRow].Cells[0];
                }


            }
        }

        //private void btnThem_Click(object sender, EventArgs e)
        //{
        //    DataContextDataContext db = new DataContextDataContext();
        //    ChiTietHoaDon addnv = new ChiTietHoaDon();
        //    if (addnv != null)
        //    {
        //        addnv.MaHD = Convert.ToInt32(cbMaHoaDon.SelectedValue);
        //        addnv.MaMon = Convert.ToInt32(cbMaMon.SelectedValue);

        //        int soLuong;
        //        if (int.TryParse(txbSoLuong.Text, out soLuong))
        //        {
        //            addnv.SoLuong = soLuong;
        //            db.ChiTietHoaDons.InsertOnSubmit(addnv);
        //            db.SubmitChanges();
        //            hienthi();
        //            int lastRow = dataGridView1.Rows.Count - 1;
        //            dataGridView1.CurrentCell = dataGridView1.Rows[lastRow].Cells[0];
        //            PopupNotifier popup = new PopupNotifier();
        //            popup.Image = Properties.Resources.icons8_done_80px;
        //            popup.Size = new Size(300, 100);
        //            popup.TitleText = "Thông báo";
        //            popup.ContentText = "Thao tác thêm thành công!";
        //            popup.ShowCloseButton = true;
        //            popup.Delay = 3000;
        //            popup.Popup();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Số lượng phải là số nguyên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //        db.SubmitChanges();
        //        dataGridView1.DataSource = db.ChiTietHoaDons.Where(cthd => cthd.MaChiTietHD == addnv.MaChiTietHD).ToList();
        //    }
        //}

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DataContextDataContext db = new DataContextDataContext();

            int maChiTietHD = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaChiTietHD"].Value);

            ChiTietHoaDon existingChiTietHD = db.ChiTietHoaDons.SingleOrDefault(cthd => cthd.MaChiTietHD == maChiTietHD);

            if (existingChiTietHD != null)
            {
                DialogResult dialogResult = MessageBox.Show("Bạn có chắc muốn xóa chi tiết hóa đơn này?", "Xác nhận xóa", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    db.ChiTietHoaDons.DeleteOnSubmit(existingChiTietHD);

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
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            DataContextDataContext db = new DataContextDataContext();

            if (dataGridView1.SelectedRows.Count > 0)
            {
                int maChiTietHD = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaChiTietHD"].Value);
                ChiTietHoaDon existingChiTietHD = db.ChiTietHoaDons.SingleOrDefault(cthd => cthd.MaChiTietHD == maChiTietHD);

                if (existingChiTietHD != null)
                {
                    existingChiTietHD.MaHD = Convert.ToInt32(cbMaHoaDon.SelectedValue);
                    existingChiTietHD.MaMon = Convert.ToInt32(cbMaMon.SelectedValue);

                    int soLuong;
                    if (int.TryParse(txbSoLuong.Text, out soLuong))
                    {
                        existingChiTietHD.SoLuong = soLuong;
                        db.SubmitChanges();
                        hienthi();
                        int rowIndex = dataGridView1.Rows.Count - 1;
                        dataGridView1.Rows[rowIndex].Cells["MaHD"].Value = existingChiTietHD.MaHD;
                        dataGridView1.Rows[rowIndex].Cells["MaMon"].Value = existingChiTietHD.MaMon;
                        dataGridView1.Rows[rowIndex].Cells["SoLuong"].Value = existingChiTietHD.SoLuong;
                        dataGridView1.DataSource = db.ChiTietHoaDons.Where(cthd => cthd.MaChiTietHD == maChiTietHD).ToList();
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
                        MessageBox.Show("Số lượng phải là số nguyên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn một dòng trên DataGridView để sửa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {

                TxbMaChiTietHoaDon.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["MaChiTietHD"].Value.ToString();
                cbMaHoaDon.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["MaHD"].Value.ToString();
                cbMaMon.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["MaMon"].Value.ToString();
                txbSoLuong.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["SoLuong"].Value.ToString();


            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {


                using (DataContextDataContext db = new DataContextDataContext())
                {
                    TxbMaChiTietHoaDon.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["MaChiTietHD"].Value.ToString();
                    cbMaHoaDon.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["MaHD"].Value.ToString();
                    cbMaMon.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["MaMon"].Value.ToString();
                    txbSoLuong.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["SoLuong"].Value.ToString();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {


                using (DataContextDataContext db = new DataContextDataContext())
                {
                    TxbMaChiTietHoaDon.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["MaChiTietHD"].Value.ToString();
                    cbMaHoaDon.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["MaHD"].Value.ToString();
                    cbMaMon.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["Ten"].Value.ToString();
                    txbSoLuong.Text = dataGridView1.SelectedCells[0].OwningRow.Cells["SoLuong"].Value.ToString();
                }
            }
            else
            {
                bunifuGroupBox4.Visible = false;
            }
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {
                int MaHD;
                if (!int.TryParse(txbMaHoaDon.Text, out MaHD))
                {
                    MessageBox.Show("Mã hóa đơn phải là một số nguyên.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var query = from ChiTietHoaDon in db.ChiTietHoaDons
                            join mm in db.Mons on ChiTietHoaDon.MaMon equals mm.MaMon
                            join mhd in db.HoaDons on ChiTietHoaDon.MaHD equals mhd.MaHD
                            where ChiTietHoaDon.MaHD == MaHD
                            select new { ChiTietHoaDon.MaChiTietHD, mm.Ten, ChiTietHoaDon.MaMon, ChiTietHoaDon.SoLuong, ChiTietHoaDon.MaHD, ChiTietHoaDon.HoaDon, ChiTietHoaDon.Mon };

                dataGridView1.DataSource = query.ToList();
            }

            // Rest of the code for formatting the DataGridView
            dataGridView1.Columns["MaChiTietHD"].HeaderText = "Mã chi tiết HĐ";
            dataGridView1.Columns["MaHD"].HeaderText = "Mã hóa đơn";
            dataGridView1.Columns["Ten"].HeaderText = "Tên món";
            dataGridView1.Columns["MaMon"].Visible = false;
            dataGridView1.Columns["SoLuong"].HeaderText = "Số lượng";
            dataGridView1.Refresh();
            dataGridView1.Columns["HoaDon"].Visible = false;
            dataGridView1.Columns["Mon"].Visible = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.RowTemplate.Height = 50;
            dataGridView1.ColumnHeadersHeight = 150;

            if (dataGridView1.Rows.Count > 0)
            {
                int lastRow = dataGridView1.Rows.Count - 1;
                dataGridView1.CurrentCell = dataGridView1.Rows[lastRow].Cells[0];
            }
        }

        private void btnHienThi_Click(object sender, EventArgs e)
        {
            hienthi();
        }
    }
}

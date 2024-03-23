using QuanLyCafe.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OfficeOpenXml;
using QuanLyCafe.Model;
using Tulpep.NotificationWindow;

namespace QuanLyCafe.Forms
{
    public partial class FormThongKe : Form
    {
        private int mahoadon;
        public FormThongKe()
        {
            InitializeComponent();
            LoadTheme();
            LoadData();


        }
        public void hienthi()
        {
            {

                //using (DataContextDataContext db = new DataContextDataContext())
                //{
                //    var query = from HoaDon in db.HoaDons
                //                join nv in db.NhanViens on HoaDon.MaNhanVien equals nv.MaNhanVien
                //                join msb in db.BanCafes on HoaDon.MaSoBan equals msb.MaSoBan
                //                select new { HoaDon.MaHD, HoaDon.Ngay, nv.Ten, msb.TenBan, HoaDon.TongTien, HoaDon.GiamGia, HoaDon.NhanVien, HoaDon.BanCafe };
                //    dataGridView1.DataSource = query.ToList();
                //}


                using (DataContextDataContext db = new DataContextDataContext())
                {
                    var query = from HoaDon in db.HoaDons
                                join nv in db.NhanViens on HoaDon.MaNhanVien equals nv.MaNhanVien
                                join msb in db.BanCafes on HoaDon.MaSoBan equals msb.MaSoBan
                                where HoaDon.status == 1
                                select new { HoaDon.MaHD, HoaDon.Ngay, nv.Ten, msb.TenBan, HoaDon.TongTien, HoaDon.GiamGia, HoaDon.NhanVien, HoaDon.BanCafe };
                    dataGridView1.DataSource = query.ToList();
                }

                dataGridView1.Columns["MaHD"].HeaderText = "Mã hóa đơn";
                dataGridView1.Columns["Ngay"].HeaderText = "Ngày";
                dataGridView1.Columns["Ten"].HeaderText = "Tên nhân viên";
                dataGridView1.Columns["TenBan"].HeaderText = "Tên bàn";
                dataGridView1.Columns["GiamGia"].HeaderText = "Giảm giá";
                dataGridView1.Columns["TongTien"].HeaderText = "Tổng tiền";
                dataGridView1.Columns["NhanVien"].Visible = false;
                dataGridView1.Columns["BanCafe"].Visible = false;
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.RowTemplate.Height = 50;
                dataGridView1.ColumnHeadersHeight = 150;



            }
        }
        private void LoadData()
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {
                var query = from HoaDon in db.HoaDons
                            join nv in db.NhanViens on HoaDon.MaNhanVien equals nv.MaNhanVien
                            join msb in db.BanCafes on HoaDon.MaSoBan equals msb.MaSoBan
                            where HoaDon.status == 1
                            select new
                            {
                                HoaDon.MaHD,
                                HoaDon.Ngay,
                                Ten = nv.Ten,
                                TenBan = msb.TenBan,
                                HoaDon.TongTien,
                                HoaDon.GiamGia,
                            };

                ListHoaDon = query.ToList().Select(q => new PageListNv
                {
                    MaHD = q.MaHD,
                    Ngay = q.Ngay,
                    Ten = q.Ten,
                    TenBan = q.TenBan,
                    TongTien = q.TongTien,
                    GiamGia = q.GiamGia
                }).ToList();
            }


            totalPages = (int)Math.Ceiling((double)ListHoaDon.Count / pageSize);
            currentPage = 1;

            LoadPage();
        }

        private List<PageListNv> ListHoaDon = new List<PageListNv>();
        private int pageSize = 5;
        private int totalPages;
        private int currentPage;

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
                btnTaiLai.BackColor = ThemeColor.PrimaryColor;
                btnThongKe.BackColor = ThemeColor.PrimaryColor;
                btnTrangSau.BackColor = ThemeColor.PrimaryColor;
                btnTrangTruoc.BackColor = ThemeColor.PrimaryColor;

            }
        }
        private void LoadPage()
        {
            int skipRows = (currentPage - 1) * pageSize;

            var dataSource = ListHoaDon.Skip(skipRows).Take(pageSize).ToList();

            dataGridView1.DataSource = dataSource;
            Text = "of " + totalPages.ToString();
            Text = currentPage.ToString();

            btnTrangTruoc.Enabled = currentPage > 1;
            btnTrangSau.Enabled = currentPage < totalPages;
        }

        private void FormThongKe_Load(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = new List<HoaDon>();
            hienthi();
            LoadData();
            //DataContextDataContext gt = new DataContextDataContext();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            string keyword = txbTracuuthongke.Text;
            DateTime startDate = dateTimePicker1.Value;
            DateTime endDate = dateTimePicker2.Value;

            DataContextDataContext db = new DataContextDataContext();
            {
                var result = from HD in db.HoaDons
                             where HD.MaHD.ToString().Contains(keyword) &&
                             HD.Ngay >= startDate && HD.Ngay <= endDate
                             select HD;

                dataGridView1.DataSource = result.ToList();
            }
        }


        private void btnThongKe_Click(object sender, EventArgs e)
        {

            DateTime firstDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime lastDayOfMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            using (DataContextDataContext db = new DataContextDataContext())
            {

                var query = from HoaDon in db.HoaDons
                            where HoaDon.Ngay >= firstDayOfMonth && HoaDon.Ngay <= lastDayOfMonth
                            select HoaDon;


                double sum = (double)(query.Sum(x => x.TongTien - x.GiamGia) ?? 0.0);


                MessageBox.Show("Tổng doanh thu tháng này là: " + sum.ToString("N0") + " VNĐ");
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData();
            currentPage = 1;
            LoadPage();
            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.icons8_done_80px;
            popup.Size = new Size(300, 100);
            popup.TitleText = "Thông báo";
            popup.ContentText = "Thao tác tải lại thành công!";
            popup.ShowCloseButton = true;
            popup.Delay = 3000;
            popup.Popup();
        }


        private void btnTrangTruoc_Click_1(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPage();
            }
        }

        private void btnTrangSau_Click_1(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPage();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xuất", "Lỗi");
                return;
            }

            int orderId = (int)dataGridView1.SelectedRows[0].Cells["MaHD"].Value;

            using (DataContextDataContext db = new DataContextDataContext())
            {
                var order = (from hd in db.HoaDons
                             join nv in db.NhanViens on hd.MaNhanVien equals nv.MaNhanVien
                             join bc in db.BanCafes on hd.MaSoBan equals bc.MaSoBan
                             where hd.MaHD == orderId
                             select new
                             {
                                 hd.MaHD,
                                 hd.Ngay,
                                 nv.Ten,
                                 bc.TenBan,
                                 hd.TongTien,
                                 hd.GiamGia,
                                 hd.NhanVien,
                                 hd.BanCafe,
                                 hd.ChiTietHoaDons
                             }).FirstOrDefault();

                if (order == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn này trong database", "Lỗi");
                    return;
                }

                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                ExcelPackage excel = new ExcelPackage();
                var worksheet = excel.Workbook.Worksheets.Add("Order Details");

                worksheet.Cells[1, 1].Value = "Mã hóa đơn";
                worksheet.Cells[1, 2].Value = order.MaHD;

                worksheet.Cells[2, 1].Value = "Ngày";
                worksheet.Cells[2, 2].Value = order.Ngay?.ToShortDateString() ?? "";

                worksheet.Cells[3, 1].Value = "Tên nhân viên";
                worksheet.Cells[3, 2].Value = order.Ten;

                worksheet.Cells[4, 1].Value = "Tên bàn";
                worksheet.Cells[4, 2].Value = order.TenBan;

                worksheet.Cells[5, 1].Value = "Tổng tiền";
                worksheet.Cells[5, 2].Value = $"{order.TongTien} VND";

                worksheet.Cells[6, 1].Value = "Giảm giá";
                worksheet.Cells[6, 2].Value = $"{order.GiamGia} VND";

                worksheet.Cells[7, 1].Value = "Thành tiền";
                worksheet.Cells[7, 2].Formula = $"=B5-B6" ;

                worksheet.Cells["A1:E10"].AutoFitColumns();
                worksheet.View.ShowGridLines = false;

                // Lưu file excel
                string fileName = "HoaDon-" + orderId + ".xlsx";
                string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), fileName);
                FileInfo file = new FileInfo(filePath);
                excel.SaveAs(file);

                // Mở file excel sau khi lưu
                if (File.Exists(filePath))
                {
                    DialogResult result = MessageBox.Show("Xuất hóa đơn thành công. Bạn có muốn mở file excel không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(filePath);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Lỗi khi mở file Excel", "Lỗi");
                        }
                    }
                }
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "TongTien" && e.Value != null)
            {
                int TongTien;
                if (int.TryParse(e.Value.ToString(), out TongTien))
                {
                    e.Value = TongTien.ToString("N0") + " VND"; // Định dạng giá trị và thêm chữ "VND"
                    e.FormattingApplied = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int maHoaDon = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["MaHD"].Value);

                FormChiTietHoaDon formChiTietHoaDon = new FormChiTietHoaDon();
                formChiTietHoaDon.MaHoaDon = maHoaDon;
                formChiTietHoaDon.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn để xem chi tiết.", "Lỗi");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                string maHoaDon = row.Cells["MaHD"].Value.ToString();
                txbTracuuthongke.Text = maHoaDon;
            }
        }
    }
}

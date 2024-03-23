using Bunifu.UI.WinForms;
using QuanLyCafe.Data;
using QuanLyCafe.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tulpep.NotificationWindow;

namespace QuanLyCafe.Forms
{
    public partial class FormBanHang : Form
    {
        private NhanVienModel nhanVienBanHang;

        double Total;
        int tableId;
        string nameTable;
        double? tongGiamGia = 0; // Tổng giá trị giảm giá các sản phẩm
        public FormBanHang(NhanVienModel nv)
        {
            nhanVienBanHang = nv;
            InitializeComponent();
            LoadTheme();
            LoadTables();
            loadMaMon();
          
        }

        private void metroComboBox3_Click(object sender, EventArgs e)
        {
            DataContextDataContext dc = new DataContextDataContext();
            metroComboBox3.DataSource = dc.BanCafes.ToList();
            metroComboBox3.DisplayMember = "TenBan";
            metroComboBox3.ValueMember = "MaSoBan";
        }

        private void loadMaMon()
        {
            metroComboBox1.DataSource = null;
            metroComboBox2.DataSource = null;

            DataContextDataContext dc = new DataContextDataContext();
            metroComboBox1.DataSource = dc.DanhMucs.ToList();
            metroComboBox1.DisplayMember = "Ten";
            metroComboBox1.ValueMember = "MaDanhMuc";
        }
        private void LoadMon()
        {
            DataContextDataContext dc = new DataContextDataContext();
            int maDanhMuc = 0;
            if (metroComboBox1.SelectedValue != null)
            {
                maDanhMuc = int.Parse(metroComboBox1.SelectedValue.ToString());
            }

            var query = dc.Mons.Where(m => m.MaDanhMuc == maDanhMuc).ToList();
            metroComboBox2.DataSource = query; // Gán giá trị mới cho metroComboBox2
            metroComboBox2.ValueMember = "MaMon";
            metroComboBox2.DisplayMember = "Ten";
        }

        private void LoadTables()
        {
            flowLayoutPanel1.Controls.Clear();
            // Khởi tạo đối tượng DataContext
            using (DataContextDataContext dc = new DataContextDataContext())
            {
                // Lấy danh sách các bàn ăn và sắp xếp theo tên bàn
                var query = from tbl in dc.BanCafes
                            orderby tbl.MaSoBan
                            select tbl;
                // Tính số lượng cột và dòng
                int columnCount = (int)Math.Floor((double)flowLayoutPanel1.Width / (80 + 10)); // Chiều rộng của button là 80, khoảng cách giữa các button là 10
                int tableCount = query.Count();
                int rowCount = (int)Math.Ceiling((double)tableCount / columnCount);
                // Điều chỉnh kích thước và vị trí của các button
                int buttonIndex = 0;
                for (int row = 0; row < rowCount; row++)
                {
                    for (int col = 0; col < columnCount; col++)
                    {
                        var tables = query.Skip(buttonIndex).Take(1).ToList();
                        if (tables.Count > 0)
                        {
                            BanCafe table = tables[0];
                            Button btnTable = new Button();
                            btnTable.Text = table.TenBan + Environment.NewLine + table.Trangthai;
                            btnTable.Tag = table.MaSoBan; // Sử dụng Tag để lưu trữ mã bàn
                            switch (table.Trangthai)
                            {
                                case "Trống     ":
                                    btnTable.BackColor = Color.Aqua;
                                    break;
                                default:
                                    btnTable.BackColor = Color.SandyBrown;
                                    break;
                            }
                            btnTable.Click += btnTable_Click;
                            btnTable.Size = new Size(90, 90);
                            btnTable.Location = new Point(col * (80 + 10), row * (80 + 10));
                            btnTable.Margin = new Padding(12);

                            // Thiết lập Font mới cho Button
                            FontFamily fontFamily = new FontFamily("Segoe UI");
                            float fontSize = 12.0f;
                            FontStyle fontStyle = FontStyle.Regular;
                            GraphicsUnit graphicsUnit = GraphicsUnit.Pixel;
                            btnTable.Font = new Font(fontFamily, fontSize, fontStyle, graphicsUnit);

                            flowLayoutPanel1.Controls.Add(btnTable);
                            buttonIndex++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        private void btnTable_Click(object sender, EventArgs e)
        {
            label5.Text = "Bàn đang chọn: ";
            if (int.TryParse(((Button)sender).Tag.ToString(), out tableId))
            {
                string nameTableinfo = ((Button)sender).Text;
                nameTable = nameTableinfo.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)[0];
                listView1.Tag = (sender as Button).Tag;
                label5.Text += nameTable;
                // Hiển thị thông tin chi tiết của bàn trong giao diện
                ShowTableDetails(tableId);
            }
            else
            {
                // Nếu không lấy được mã bàn, hiển thị message box thông báo lỗi
                MessageBox.Show("Không thể lấy mã bàn!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void ShowTableDetails(int id)
        {
            DataContextDataContext dc = new DataContextDataContext();
            // Lấy danh sách các sản phẩm trong hóa đơn của bàn
            var query = from cthd in dc.ChiTietHoaDons
                        join hd in dc.HoaDons on cthd.MaHD equals hd.MaHD
                        join bv in dc.BanCafes on hd.MaSoBan equals bv.MaSoBan
                        join sp in dc.Mons on cthd.MaMon equals sp.MaMon
                        where bv.MaSoBan == id && hd.status == 0
                        select new
                        {
                            MaSP = sp.MaMon,
                            TenSP = sp.Ten,
                            SoLuong = cthd.SoLuong,
                            DonGia = sp.GiaTien,
                            ThanhTien = (int)cthd.SoLuong * sp.GiaTien
                        };
            // Xóa hết các item trong ListView trước khi gán lại dữ liệu
            listView1.Items.Clear();

            double? tongtien = 0;
            tongGiamGia = 0;

            // Điền dữ liệu vào ListView
            foreach (var item in query)
            {
                double? giaTriKhuyenMai = 0;
                double? giatrikmtienmat = 0;
                double? phantrmakhuyenmai = 0;
                var khuyenMais = from km in dc.Khuyenmais
                                 join mk in dc.Monkhuyenmais on km.makm equals mk.MaKm
                                 where mk.MaMon == item.MaSP
                                 select new { Loai = km.maloaikm, GiaTri = km.GiaKhuyenMai };

                giaTriKhuyenMai = khuyenMais.Sum(km => km.Loai == 1 ? km.GiaTri / 100 * item.DonGia * item.SoLuong : 0);
                giatrikmtienmat = khuyenMais.Sum(km => km.Loai == 2 ? km.GiaTri * item.SoLuong : 0);
                phantrmakhuyenmai = khuyenMais.Sum(km => km.Loai == 1 ? km.GiaTri : 0);
                ListViewItem listItem = new ListViewItem(item.MaSP.ToString());
                listItem.SubItems.Add(item.TenSP);
                listItem.SubItems.Add(item.SoLuong.ToString());
                listItem.SubItems.Add(String.Format("{0:0,0} VND", item.DonGia));

                double? thanhTien = item.SoLuong * item.DonGia;
                if (giaTriKhuyenMai > 0)
                {
                    string giamGia = "";
                    giamGia = String.Format("{0}% ", phantrmakhuyenmai);
                    listItem.SubItems.Add(String.Format("- {0}", giamGia));
                    thanhTien -= giaTriKhuyenMai;
                }
                else
                {
                    listItem.SubItems.Add("");
                }

                if (giatrikmtienmat > 0)
                {
                    string giamGiaTien = "";
                    giamGiaTien += String.Format("{0:0,0} VND ", giatrikmtienmat);
                    listItem.SubItems.Add(String.Format("- {0}", giamGiaTien));
                    thanhTien -= giatrikmtienmat;
                }
                else
                {
                    listItem.SubItems.Add("");
                }


                listItem.SubItems.Add(String.Format("{0:0,0} VND", thanhTien));

                listView1.Items.Add(listItem);

                tongtien += thanhTien;

                Total = (double)tongtien;
                tongGiamGia += giaTriKhuyenMai ?? 0;
                tongGiamGia += giatrikmtienmat ?? 0;
            }

            int templist = query.Count();
            if (templist <= 0)
            {
                BanCafe uphd = dc.BanCafes.SingleOrDefault(p => p.MaSoBan == id);
                {
                    uphd.MaSoBan = uphd.MaSoBan;
                    uphd.TenBan = uphd.TenBan;
                    uphd.Trangthai = "Trống";
                };
                dc.SubmitChanges();
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);



            // Hiển thị tổng tiền và tiền giảm giá lên các hộp văn bản tương ứng
            txbTongTien.Text = String.Format("{0:0,0} VNĐ", Total);
            bunifuTextBox1.Text = String.Format("{0:0,0} VNĐ", tongGiamGia);
            LoadTables();
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
                gbChonMon.BorderColor = ThemeColor.SecondaryColor;
                gbChonBan.BorderColor = ThemeColor.SecondaryColor;
                gbDoiBan.BorderColor = ThemeColor.SecondaryColor;
                gbThanhToan.BorderColor = ThemeColor.SecondaryColor;
                gbOrder.BorderColor = ThemeColor.SecondaryColor;
                btnDoiBan.BackColor = ThemeColor.PrimaryColor;
                btnThanhToan.BackColor = ThemeColor.PrimaryColor;
                btnThemMon.BackColor = ThemeColor.PrimaryColor;
            }
        }

        private void FormBanHang_Load(object sender, EventArgs e)
        {

        }

        private void metroComboBox2_Click(object sender, EventArgs e)
        {
            LoadMon();
        }

        private void btnThemMon_Click(object sender, EventArgs e)
        {
            string IDBill;
            if (listView1.Tag == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi đặt món");
                return;
            }
            else
            {
                IDBill = listView1.Tag.ToString();

            }

            if (metroComboBox2.SelectedValue == null)
            {
                MessageBox.Show("Chọn món trước khi thêm");
                return;
            }
            using (DataContextDataContext db = new DataContextDataContext())
            {
                var query = from h in db.HoaDons
                            where h.MaSoBan == int.Parse(IDBill) && h.status == 0
                            select new { h.MaHD };
                int temp = query.Count();
                //int ?sl = (int?)metroComboBox2.SelectedValue;
                if (temp != 0)
                {
                    // Case đã có bill
                    int? MaDH = query.SingleOrDefault()?.MaHD;
                    int? cout = (int)numericUpDown1.Value;
                    var query1 = from m in db.ChiTietHoaDons
                                 where m.MaMon == (int?)metroComboBox2.SelectedValue && m.MaHD == MaDH
                                 select new { m.MaChiTietHD };

                    temp = query1.Count();
                    int? MaCtDH = query1.FirstOrDefault()?.MaChiTietHD;

                    if (temp != 0)
                    {
                        //ShowTableDetails(int.Parse(IDBill));
                        //MessageBox.Show("Đã có món trong bill");
                        ChiTietHoaDon upcthd = db.ChiTietHoaDons.SingleOrDefault(p => p.MaChiTietHD == MaCtDH);
                        if (upcthd != null)
                        {
                            upcthd.MaHD = MaDH;

                            upcthd.MaMon = (int?)metroComboBox2.SelectedValue;

                            upcthd.SoLuong = upcthd.SoLuong + cout;

                            if (upcthd.SoLuong <= 0)
                            {
                                db.ChiTietHoaDons.DeleteOnSubmit(upcthd);
                            }
                            db.SubmitChanges();
                            loadHoaDon();
                            LoadTables();
                        }
                        ShowTableDetails(int.Parse(IDBill));
                    }
                    else
                    {
                        if (cout > 0)
                        {
                            //MessageBox.Show("Chưa món trong bill");
                            ChiTietHoaDon addct = new ChiTietHoaDon
                            {
                                MaHD = MaDH,
                                MaMon = (int?)metroComboBox2.SelectedValue,
                                SoLuong = cout
                            };
                            db.ChiTietHoaDons.InsertOnSubmit(addct);
                            db.SubmitChanges();
                            ShowTableDetails(int.Parse(IDBill));
                            loadHoaDon();
                            LoadTables();
                        }
                        else
                        {
                            return;
                        }

                    }
                }
                else
                {
                    //int? MaDH = query.SingleOrDefault()?.MaHD;
                    int? cout = (int)numericUpDown1.Value;
                    //Case chưa có bill
                    if (cout > 0)
                    {
                        //Phải tạo bill trước
                        HoaDon add = new HoaDon
                        {
                            Ngay = DateTime.Now,
                            MaNhanVien = nhanVienBanHang.Ma,
                            MaSoBan = int.Parse(IDBill),
                            TongTien = Total,
                            GiamGia = tongGiamGia,
                            status = 0
                        };
                        db.HoaDons.InsertOnSubmit(add);
                        db.SubmitChanges();
                        // Lấy MaHD vừa tạo
                        int MaDH = add.MaHD;

                        // Tạo phần InforBill

                        ChiTietHoaDon addct = new ChiTietHoaDon
                        {
                            MaHD = MaDH,
                            MaMon = (int?)metroComboBox2.SelectedValue,
                            SoLuong = cout
                        };
                        db.ChiTietHoaDons.InsertOnSubmit(addct);
                        db.SubmitChanges();
                        ShowTableDetails(int.Parse(IDBill));
                        loadHoaDon();
                        LoadTables();
                        PopupNotifier popup = new PopupNotifier();
                        popup.Image = Properties.Resources.icons8_done_80px;
                        popup.Size = new Size(300, 100);
                        popup.TitleText = "Thông báo";
                        popup.ContentText = "Thao tác thêm món thành công!";
                        popup.ShowCloseButton = true;
                        popup.Delay = 3000;
                        popup.Popup();
                    }
                    else
                    {
                        MessageBox.Show("Chọn số lượng lớn hơn 1");
                    }
                }

            }
            //loadHoaDon();
        }
        void loadHoaDon()
        {
            string IDBill;
            if (listView1.Tag == null)
            {
                return;
            }
            else
            {
                IDBill = listView1.Tag.ToString();
            }


            DataContextDataContext db = new DataContextDataContext();


            var query = from cthd in db.ChiTietHoaDons
                        join hd in db.HoaDons on cthd.MaHD equals hd.MaHD
                        join bv in db.BanCafes on hd.MaSoBan equals bv.MaSoBan
                        join sp in db.Mons on cthd.MaMon equals sp.MaMon
                        where bv.MaSoBan == tableId && hd.status == 0
                        select new
                        {
                            ThanhTien = (int)cthd.SoLuong * sp.GiaTien
                        };

            var query1 = from h in db.HoaDons
                         where h.MaSoBan == int.Parse(IDBill) && h.status == 0
                         select new { h.MaHD };
            int? MaDH = query1.SingleOrDefault()?.MaHD;
            HoaDon uphd = db.HoaDons.SingleOrDefault(p => p.MaHD == MaDH);
            {
                uphd.Ngay = uphd.Ngay;
                uphd.MaNhanVien = nhanVienBanHang.Ma;
                uphd.MaSoBan = int.Parse(IDBill);
                uphd.TongTien = Total;
                uphd.GiamGia = tongGiamGia;
                uphd.status = 0;
            };
            Total = 0;
            db.SubmitChanges();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            string IDBill;
            if (listView1.Tag == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi đặt món");
                return;
            }
            else
            {
                IDBill = listView1.Tag.ToString();
            }

            using (DataContextDataContext db = new DataContextDataContext())
            {
                ShowTableDetails(int.Parse(IDBill));
                var query = from h in db.HoaDons
                            where h.MaSoBan == int.Parse(IDBill) && h.status == 0
                            select new { h.MaHD };
                int temp = query.Count();
                int? MaDH = query.SingleOrDefault()?.MaHD;
                if (temp != 0)
                {
                    if (MessageBox.Show("Bạn có muốn thanh toán cho " + nameTable, "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        loadHoaDon();
                        var query1 = from h in db.HoaDons
                                     where h.MaSoBan == int.Parse(IDBill) && h.status == 0
                                     select new { h.MaHD };
                        //int? MaDH = query1.SingleOrDefault()?.MaHD;
                        HoaDon uphd = db.HoaDons.SingleOrDefault(p => p.MaHD == MaDH);
                        {
                            uphd.status = 1;
                        };
                        db.SubmitChanges();
                        ShowTableDetails(int.Parse(IDBill));
                        LoadTables();
                        PopupNotifier popup = new PopupNotifier();
                        popup.Image = Properties.Resources.icons8_done_80px;
                        popup.Size = new Size(300, 100);
                        popup.TitleText = "Thông báo";
                        popup.ContentText = "Thao tác thanh toán thành công!";
                        popup.ShowCloseButton = true;
                        popup.Delay = 3000;
                        popup.Popup();
                        //MessageBox.Show("đã thanh toán");
                    }
                }
                else
                {
                    // Trường hợp không có hóa hơn thì không phải làm gì
                }
            }
        }

        public void ChuyenBan(int idBanCu, int idBanMoi)
        {

            if (idBanCu == idBanMoi)
            {
                return;
            }

            DataContextDataContext db = new DataContextDataContext();
            // Xác định hoá đơn của bàn cũ
            HoaDon hoaDonCu = db.HoaDons.FirstOrDefault(x => x.MaSoBan == idBanCu && x.status == 0);
            // Nếu không tìm thấy hoá đơn, hiển thị thông báo lỗi và thoát khỏi phương thức.
            if (hoaDonCu == null)
            {
                MessageBox.Show("Bàn cũ không có đơn hàng chưa thanh toán!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Xác định hoá đơn của bàn mới hoặc tạo hoá đơn mới nếu không có hoá đơn cho bàn đó.
            HoaDon hoaDonMoi = db.HoaDons.FirstOrDefault(x => x.MaSoBan == idBanMoi && x.status == 0);
            if (hoaDonMoi == null)
            {
                hoaDonMoi = new HoaDon()
                {
                    MaSoBan = idBanMoi,
                    status = 0,
                    Ngay = DateTime.Now,
                    GiamGia = tongGiamGia,
                    TongTien = Total,
                    MaNhanVien = nhanVienBanHang.Ma
                };
                db.HoaDons.InsertOnSubmit(hoaDonMoi);
                db.SubmitChanges();
            }
            else
            {
                MessageBox.Show("Bàn đã có người ngồi");
                return;
            }

            foreach (var chiTietHoaDon in hoaDonCu.ChiTietHoaDons)
            {
                chiTietHoaDon.MaHD = hoaDonMoi.MaHD;
            }
            BanCafe banCu = db.BanCafes.FirstOrDefault(x => x.MaSoBan == idBanCu);
            BanCafe banMoi = db.BanCafes.FirstOrDefault(x => x.MaSoBan == idBanMoi);
            banCu.Trangthai = "Trống";
            banMoi.Trangthai = "Có người";
            hoaDonCu.MaSoBan = idBanMoi;
            db.HoaDons.DeleteOnSubmit(hoaDonCu);
            db.SubmitChanges();
            LoadTables();
            // Hiển thị thông báo khi chuyển bàn thành công.
            PopupNotifier popup = new PopupNotifier();
            popup.Image = Properties.Resources.icons8_done_80px;
            popup.Size = new Size(300, 100);
            popup.TitleText = "Thông báo";
            popup.ContentText = "Thao tác đổi bàn thành công!";
            popup.ShowCloseButton = true;
            popup.Delay = 3000;
            popup.Popup();
        }

        private void btnDoiBan_Click(object sender, EventArgs e)
        {
            // Lấy thông tin mã bàn cũ và mã bàn mới từ các đối tượng trên giao diện
            string IDBanCu;
            if (listView1.Tag == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi đổi");
                return;
            }
            else
            {
                IDBanCu = listView1.Tag.ToString();
            }
            int idBanMoi = int.Parse(metroComboBox3.SelectedValue.ToString());

            // Thực hiện chức năng chuyển bàn
            ChuyenBan(int.Parse(IDBanCu), idBanMoi);

        }


    }
}

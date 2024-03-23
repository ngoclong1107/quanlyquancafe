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

namespace QuanLyCafe.Forms
{
    public partial class FormChiTietKM : Form
    {
        public FormChiTietKM()
        {
            InitializeComponent();
            DataContextDataContext dc = new DataContextDataContext();
            comboBox1.DataSource = dc.DanhMucs.ToList();
            comboBox1.DisplayMember = "Ten";
            comboBox1.ValueMember = "MaDanhMuc";

            //Hiện combobox cho khuyến mãi
            cbTenvattu.DataSource = dc.Khuyenmais.ToList();
            cbTenvattu.DisplayMember = "tenkm";
            cbTenvattu.ValueMember = "makm";
        }

        void hienthi()
        {
            lbname.Text ="Tên khuyến mãi";
            label1.Text ="Thời gian bắt đầu";
            label2.Text ="Thời gian kết thúc";
            label4.Text ="Giá trị";
            label6.Text = "Loại KM";
            int idKM = int.Parse(cbTenvattu.SelectedValue.ToString());
            using (DataContextDataContext dc = new DataContextDataContext())
            {
                var query = from km in dc.Khuyenmais
                            join ml in dc.Loaikms
                            on km.maloaikm equals ml.maloaikm
                            where km.makm == idKM
                            select new { km.tenkm, km.tgbatdau, km.tgketthuc, km.GiaKhuyenMai ,ml.tenloai, km.maloaikm };
                var results = query.ToList();
                var khuyenMai = query.FirstOrDefault();
                if (khuyenMai != null)
                {
                    lbname.Text += ": " + khuyenMai.tenkm;
                    label1.Text += ": " + khuyenMai.tgbatdau;
                    label2.Text += ": " + khuyenMai.tgketthuc;
                    label4.Text += ": " + khuyenMai.GiaKhuyenMai;
                    if(khuyenMai.maloaikm == 1)
                    {
                        label4.Text += " %";
                    }
                    else
                    {
                        label4.Text += " VNĐ";
                    }
                    label6.Text += ": " + khuyenMai.tenloai;

                }
            }
        }

        private void cbTenvattu_Click(object sender, EventArgs e)
        {
            hienthi();
        }

        private void cbTendanhmuc_Click(object sender, EventArgs e)
        {
            DataContextDataContext dc = new DataContextDataContext();
            int maDanhMuc = 0;
            if (comboBox1.SelectedValue != null)
            {
                maDanhMuc = int.Parse(comboBox1.SelectedValue.ToString());
            }

            var query = dc.Mons.Where(m => m.MaDanhMuc == maDanhMuc).ToList();
            cbTendanhmuc.DataSource = query; // Gán giá trị mới cho metroComboBox2
            cbTendanhmuc.ValueMember = "MaMon";
            cbTendanhmuc.DisplayMember = "Ten";
        }

        private void cbTenvattu_SelectionChangeCommitted(object sender, EventArgs e)
        {
            hienthi();
        }
    }
}

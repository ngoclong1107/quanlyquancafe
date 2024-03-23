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
    public partial class FormKho : Form
    {
        public FormKho()
        {
            InitializeComponent();
            LoadTheme();
            LoadData();
        }
        public void hienthi()
        {
            {
                using (DataContextDataContext db = new DataContextDataContext())
                {
                    var query = from Kho in db.Khos
                                select new { Kho.mavt, Kho.tenvt, Kho.sl, Kho.donvi };
                    dataGridView1.DataSource = query.ToList();
                }
                dataGridView1.Columns["mavt"].HeaderText = "Mã vật tư";
                dataGridView1.Columns["tenvt"].HeaderText = "Tên vật tư";
                dataGridView1.Columns["sl"].HeaderText = "Số lượng";
                dataGridView1.Columns["donvi"].HeaderText = "Đơn vị";
                dataGridView1.Refresh();
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView1.RowTemplate.Height = 50;
                dataGridView1.ColumnHeadersHeight = 150;

            }
        }
        private void LoadData()
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {
                ListKho = db.Khos.ToList();
            }

            totalPages = (int)Math.Ceiling((double)ListKho.Count / pageSize);
            currentPage = 1;

            LoadPage();
        }
        private List<Kho> ListKho = new List<Kho>();
        private int pageSize = 5;
        private int totalPages;
        private int currentPage;
        private void LoadPage()
        {
            int skipRows = (currentPage - 1) * pageSize;
            var dataSource = ListKho.Skip(skipRows).Take(pageSize).ToList();
            dataGridView1.DataSource = dataSource;
            Text = "Page " + currentPage.ToString() + " of " + totalPages.ToString();
            btnTrangSau.Enabled = currentPage < totalPages;
            btnTrangTruoc.Enabled = currentPage > 1;
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
                btnSapXepGiamDan.BackColor = ThemeColor.SecondaryColor;
                btnSapXepTangDan.BackColor = ThemeColor.PrimaryColor;
                btnTaiLai.BackColor = ThemeColor.PrimaryColor;
                btnTimKiem.BackColor = ThemeColor.PrimaryColor;
                btnTrangSau.BackColor = ThemeColor.PrimaryColor;
                btnTrangTruoc.BackColor = ThemeColor.PrimaryColor;
            }
        }

        private void FormKho_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = new List<Kho>();
            hienthi();
            LoadData();
            DataContextDataContext gt = new DataContextDataContext();
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            using (DataContextDataContext db = new DataContextDataContext())
            {
                string keyword = txbTimKiem.Text;
                var result = from v in db.Khos
                             where v.tenvt.Contains(keyword)
                             select v;

                dataGridView1.DataSource = result.ToList();
            }
        }

        private void btnTrangTruoc_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadPage();
            }
        }

        private void btnTrangSau_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                LoadPage();
            }
        }

        private void btnTaiLai_Click(object sender, EventArgs e)
        {
            LoadData();
            currentPage = 1;
            LoadPage();
        }

        private void btnSapXepTangDan_Click(object sender, EventArgs e)
        {
            ListKho = ListKho.OrderBy(k => k.sl).ToList();
            dataGridView1.DataSource = ListKho;
            Text = "Page " + currentPage.ToString() + " of " + totalPages.ToString();
            btnTrangSau.Enabled = currentPage < totalPages;
            btnTrangTruoc.Enabled = currentPage > 1;
        }

        private void btnSapXepGiamDan_Click(object sender, EventArgs e)
        {
            ListKho = ListKho.OrderByDescending(k => k.sl).ToList();
            dataGridView1.DataSource = ListKho;
            Text = "Page " + currentPage.ToString() + " of " + totalPages.ToString();
            btnTrangSau.Enabled = currentPage < totalPages;
            btnTrangTruoc.Enabled = currentPage > 1;
        }
    }
}

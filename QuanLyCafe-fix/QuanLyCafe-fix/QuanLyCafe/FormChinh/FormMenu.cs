using QuanLyCafe.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyCafe
{
    public partial class FormMenu : Form
    {
        private Button currentButton;
        private Random random;
        private int tempIndex;
        private Form activeForm;
        private NhanVienModel nhanVienBanHang;

        public FormMenu(NhanVienModel nv)
        {
            InitializeComponent();
            nhanVienBanHang = nv;
            random = new Random();
            btnCloseChildForm.Visible = false;
            this.Text = string.Empty;
            customList();
            this.ControlBox = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            float fontSize = 16.0f;
            FontFamily fontFamily = new FontFamily("Segoe UI");
            FontStyle fontStyle = FontStyle.Regular;
            GraphicsUnit graphicsUnit = GraphicsUnit.Pixel;
            btnDoiThongTin.Font = new Font(fontFamily, fontSize, fontStyle, graphicsUnit);
            btnDoiThongTin.Text += " của " + nhanVienBanHang.Ten;
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private Color SelectThemeColor()
        {
            int index = random.Next(ThemeColor.ColorList.Count);
            while (tempIndex == index)
            {
                index = random.Next(ThemeColor.ColorList.Count);
            }
            tempIndex = index;
            string color = ThemeColor.ColorList[index];
            return ColorTranslator.FromHtml(color);
        }
        private void ActivateButton(Object btnSender)
        {
            if (btnSender != null)
            {
                if (currentButton != btnSender as Button)
                {
                    DisableButton();
                    Color color = SelectThemeColor();
                    currentButton = (Button)btnSender;
                    currentButton.BackColor = color;
                    currentButton.ForeColor = Color.White;
                    currentButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    panelTitle.BackColor = color;
                    panelLogo.BackColor = ThemeColor.ChangeColorBrightness(color, -0.3);
                    ThemeColor.PrimaryColor = color;
                    ThemeColor.SecondaryColor = ThemeColor.ChangeColorBrightness(color, -0.3);
                    btnCloseChildForm.Visible = true;
                }
            }
        }
        private void DisableButton()
        {
            foreach (Control previousBtn in panelMenu.Controls)
            {
                if (previousBtn.GetType() == typeof(Button))
                {
                    previousBtn.BackColor = Color.FromArgb(51, 51, 76);
                    previousBtn.ForeColor = Color.Gainsboro;
                    previousBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }
            }
        }
        private void customList()  //panel dropdown tài khoản
        {
            PanelMoRong.Visible = false;
        }
        private void hideCustomList()
        {
            if (PanelMoRong.Visible == true)
                PanelMoRong.Visible = false;
        }
        private void showCustomlist(Panel subMenu)
        {
            if (PanelMoRong.Visible == false)
            {
                hideCustomList();
                subMenu.Visible = true;
            }
            else
                PanelMoRong.Visible = false;

        }
        private void OpenChildForm(Form childForm, object btnSender)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            ActivateButton(btnSender);
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            this.panelDesktop.Controls.Add(childForm);
            this.panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            //lbTitle.Text = childForm.Text;
        }

        private void FormMenu_Load(object sender, EventArgs e)
        {
            lbTime.Text = DateTime.Now.ToLongTimeString();
            lbDate.Text = DateTime.Now.ToLongDateString();
        }

        private void btnBanHang_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Bán hàng";
            OpenChildForm(new Forms.FormBanHang(nhanVienBanHang), sender);
        }

        private void btnNhanVien_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Nhân viên";
            OpenChildForm(new Forms.FormNhanVien(), sender);
        }

        private void btnDanhMuc_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Danh Mục";
            OpenChildForm(new Forms.FormDanhMuc(), sender);
        }

        private void btnMon_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Món";
            OpenChildForm(new Forms.FormMon(), sender);
        }

        private void btnKho_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Kho";
            OpenChildForm(new Forms.FormKho(), sender);
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Thống kê";
            OpenChildForm(new Forms.FormThongKe(), sender);
        }

        private void btnNhaCungCap_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Nhà cung cấp";
            OpenChildForm(new Forms.FormNhaCungCap(), sender);
        }

        private void btnChiTietHoaDon_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Chi tiết hóa đơn";
            OpenChildForm(new Forms.FormChiTietHoaDon(), sender);
        }

        private void btnTaiKhoan_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Tài khoản";
            ActivateButton(sender);
            showCustomlist(PanelMoRong);
        }

        private void btnCloseChildForm_Click(object sender, EventArgs e)
        {
            if(activeForm != null) 
                activeForm.Close();
            Reset();
        }
        private void Reset()
        {
            DisableButton();
            lbTitle.Text = "HOME";
            panelTitle.BackColor = Color.FromArgb(51, 51, 76);
            panelLogo.BackColor = Color.FromArgb(39, 39, 58);
            currentButton = null;
            btnCloseChildForm.Visible = false;
        }

        private void panelTitle_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát chương trình?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Normal)
                this.WindowState = FormWindowState.Maximized;
            else
                this.WindowState = FormWindowState.Normal;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState |= FormWindowState.Minimized;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbTime.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private void btnDoiThongTin_Click(object sender, EventArgs e)
        {
            btnDoiThongTin.Text = "Đổi thông tin ";
            //lbTitle.Text = "Đổi thông tin";
            float fontSize = 16.0f;
            FontFamily fontFamily = new FontFamily("Segoe UI");
            FontStyle fontStyle = FontStyle.Regular;
            GraphicsUnit graphicsUnit = GraphicsUnit.Pixel;
            btnDoiThongTin.Font = new Font(fontFamily, fontSize, fontStyle, graphicsUnit);
            btnDoiThongTin.Text += "của " + nhanVienBanHang.Ten;
            OpenChildForm(new Forms.FromInfoUserStaff(nhanVienBanHang), sender);
        }

        private void btnKhuyenMai_Click(object sender, EventArgs e)
        {
            lbTitle.Text = "Khuyến mãi";
            OpenChildForm(new Forms.FormKhuyenMai(), sender);
        }
    }
}

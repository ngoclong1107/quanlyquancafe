using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCafe.Model
{
    public class NhanVienModel
    {
        public NhanVienModel(string v1, string v2, int v3, string v4)
        {
            this.TenDangNhap = v1;
            this.Ten = v2;
            this.Ma = v3;
            this.Matkhau = v4;
        }

        public int Ma { get; set; }
        public string Ten { get; set; }
        public string TenDangNhap { get; set; }

        public string Matkhau { get; set; }
    }
}

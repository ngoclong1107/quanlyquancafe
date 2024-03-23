using QuanLyCafe.Data;
using QuanLyQuanCafe.Data;
using QuanLyQuanCafe.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.Data
{
    class NhanVienDal
    {
        private DataContextDataContext quanLyCafeModel;

        public NhanVienDal()
        {
            quanLyCafeModel = new DataContextDataContext();
        }

        //lay tat ca nhan vien
        public IEnumerable<NvViewModel> GetNhanViens()
        {
            var danhSachNhanVien = quanLyCafeModel.NhanViens
                                   .Select(x => new NvViewModel
                                   {
                                       Ma = x.MaNhanVien,
                                       Ten = x.Ten,  
                                       TenDangNhap = x.TenDangNhap,
                                       GioiTinh = (bool)x.Gioitinh,
                                       sdt = x.sdt,
                                       roleview = (int)x.Role

                                   }).ToList();
            return danhSachNhanVien;
        }

    }
}

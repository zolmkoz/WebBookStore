using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBookStore.Models
{
    public class Giohang
    {
        //Tạo 1 đối tượng để quản lý CSDL
        dbQLBookstoreDataContext data = new dbQLBookstoreDataContext();
        public int iMaSach { get; set; }
        public string sTenSach { get; set; }
        public string sAnhBia { get; set; }
        public Double dDonGia { get; set; }
        public int iSoLuong { get; set; }
        public Double dThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        public Giohang(int MaSach)
        {
            iMaSach = MaSach;
            //Truyền mã sách vào Cart
            Sach sach = data.Saches.Single(n => n.MaSach == iMaSach);
            sTenSach = sach.TenSach;
            sAnhBia = sach.AnhBia;
            dDonGia = double.Parse(sach.GiaBan.ToString());
            iSoLuong = 1;
        }

    }
}
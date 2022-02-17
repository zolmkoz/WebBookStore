using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class GioHangController : Controller
    {
        //Tạo 1 đối tượng để quản lý CSDL
        dbQLBookstoreDataContext data = new dbQLBookstoreDataContext();
        // GET: GioHang
        public List<Giohang> LayGioHang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            //Nếu tồn tại, gán vào Card
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGioHang(int iMaSach, string strURL)
        {
            //Get Session
            List<Giohang> lstGiohang = LayGioHang();
            //Checked books add to card
            Giohang SanPham = lstGiohang.Find(n => n.iMaSach == iMaSach);
            if (SanPham == null)
            {
                SanPham = new Giohang(iMaSach);
                lstGiohang.Add(SanPham);
                return Redirect(strURL);
            }
            else
            {
                SanPham.iSoLuong++;
                return Redirect(strURL);
            }

        }
        //Total Quantity
        public int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongSoLuong = lstGiohang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        // Total price
        public double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }
        //Create card
        public ActionResult GioHang()
        {
            List<Giohang> lstGiohang = LayGioHang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");                
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }

        //Delete Card
        public ActionResult XoaGiohang(int iMaSP)
        {
            //Get card from Session
            List<Giohang> lstGiohang = LayGioHang();
            //Checked books in session["Giohang"]
            Giohang SanPham = lstGiohang.SingleOrDefault(n => n.iMaSach == iMaSP);
            //If already exists, edit the quantity
            if (lstGiohang != null)
            {
                lstGiohang.RemoveAll(n => n.iMaSach == iMaSP);
                return RedirectToAction("GioHang");
            }
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BookStore");
            }
            return RedirectToAction("GioHang");
        }
    }
}
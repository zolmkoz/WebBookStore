﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class GioHangController : Controller
    {
        //Create an object to manage the database
        dbQLBookstoreDataContext data = new dbQLBookstoreDataContext();
        // GET: Cart
        public List<Giohang> LayGioHang()
        {
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            //If exists, assign to Cart
            if (lstGiohang == null)
            {
                lstGiohang = new List<Giohang>();
                Session["GioHang"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGioHang(int iMaSach, string strURL)
        {
            //Get Session
            List<Giohang> lstGiohang = LayGioHang();
            //Checked books add to cart
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
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
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
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if (lstGiohang != null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }
        //Create Shopping cart
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

        //Delete Shopping Cart
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

        //Update Shopping Cart
        public ActionResult CapnhatGiohang(int iMaSP, FormCollection f)
        {
            //Get card from Session
            List<Giohang> lstGiohang = LayGioHang();
            //Checked books in session["Giohang"]
            Giohang SanPham = lstGiohang.SingleOrDefault(n => n.iMaSach == iMaSP);
            //If already exists, edit the quantity
            if (SanPham != null)
            {
                SanPham.iSoLuong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        //Clear Shopping Cart
        public ActionResult XoaTatcaGiohang()
        {
            //Get cart from Session
            List<Giohang> lstGiohang = LayGioHang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "BookStore");
        }

        //Order Cart
        [HttpGet]
        public ActionResult DatHang()
        {
            //Check login
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "Customer");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "BookStore");
            }

            //Get card from session
            List<Giohang> lstGiohang = LayGioHang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }

        public ActionResult DatHang(FormCollection collection)
        {
            //Add order
            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["Taikhoan"];
            List<Giohang> gh = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDH = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);
            ddh.NgayGiao = DateTime.Parse(ngaygiao);
            ddh.TinhTrangGiaoHang = false;
            ddh.DaThanhToan = false;
            data.DonDatHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            //Add order detail
            foreach (var item in gh)
            {
                ChiTietDatHang ctdh = new ChiTietDatHang();
                ctdh.SoDH = ddh.SoDH;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                data.ChiTietDatHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("Xacnhandonhang", "GioHang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}
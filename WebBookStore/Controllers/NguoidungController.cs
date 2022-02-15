using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class NguoidungController : Controller
    {
        //Tạo 1 đối tượng để quản lý CSDL
        dbQLBookstoreDataContext db = new dbQLBookstoreDataContext();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        // GET: Nguoidung
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }

        // POST: Nguoidung
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KhachHang kh)
        {
            //Gán giá trị vào form
            var fullname = collection["HotenHK"];
            var user = collection["TenDN"];
            var password = collection["Matkhau"];
            var confirmpassword = collection["Matkhaunhaplai"];
            var email = collection["Email"];
            var address = collection["Diachi"];
            var phone = collection["Dienthoai"];
            var birthday = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if(String.IsNullOrEmpty(fullname))
            {
                ViewData["Loi1"] = "Customer's name cannot empty";
            }
            if (String.IsNullOrEmpty(user))
            {
                ViewData["Loi2"] = "The account cannot be empty";
            }
            if (String.IsNullOrEmpty(password))
            {
                ViewData["Loi3"] = "The password cannot be empty";
            }
            if (String.IsNullOrEmpty(confirmpassword))
            {
                ViewData["Loi4"] = "The confirm password cannot be empty";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = "The email cannot be empty";
            }
            if (String.IsNullOrEmpty(phone))
            {
                ViewData["Loi6"] = "The phone number cannot be empty";
            }
            else
            {
                //Gán giá trị vào Databese
                kh.HoTen = fullname;
                kh.TaiKhoan = user;
                kh.MatKhau = password;
                kh.Email = email;
                kh.DiachiKH = address;
                kh.DienThoaiKH = phone;
                kh.NgaySinh = DateTime.Parse(birthday);
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }

            return this.Dangky();
        }

        // Dangnhap
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var user = collection["TenDN"];
            var password = collection["Matkhau"];
            if (String.IsNullOrEmpty(user))
            {
                ViewData["Loi1"] = "The account cannot be empty";
            }
            else if (String.IsNullOrEmpty(password))
            {
                ViewData["Loi2"] = "The password cannot be empty";
            }
            else
            {
                //Gán giá trị và lấy session
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == user && n.MatKhau == password);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Login successful!";
                    Session["TaiKhoan"] = kh;
                }
                else
                    ViewBag.ThongBao = "User or password is incorrect!";               
            }
            return View();
        }
    }
}
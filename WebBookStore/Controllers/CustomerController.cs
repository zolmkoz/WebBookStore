using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class CustomerController : Controller
    {
        //Create objects to manage Database
        dbQLBookstoreDataContext db = new dbQLBookstoreDataContext();
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
        // GET: Customer
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        // POST: Customer
        [HttpPost]
        public ActionResult Register(FormCollection collection, KhachHang kh)
        {
            //Assign value to form
            var fullname = collection["HotenHK"];
            var user = collection["TenDN"];
            var password = collection["MatKhau"];
            var confirmpassword = collection["Matkhaunhaplai"];
            var email = collection["Email"];
            var address = collection["DiaChi"];
            var phone = collection["DienThoai"];
            var birthday = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            if(String.IsNullOrEmpty(fullname))
            {
                ViewData["Error1"] = "Customer's name cannot empty";
            }
            if (String.IsNullOrEmpty(user))
            {
                ViewData["Error2"] = "The account cannot be empty";
            }
            if (String.IsNullOrEmpty(password))
            {
                ViewData["Error3"] = "The password cannot be empty";
            }
            if (String.IsNullOrEmpty(confirmpassword))
            {
                ViewData["Error4"] = "The confirm password cannot be empty";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["Error5"] = "The email cannot be empty";
            }
            if (String.IsNullOrEmpty(phone))
            {
                ViewData["Error6"] = "The phone number cannot be empty";
            }
            else
            {
                //Assign value to Database
                kh.HoTen = fullname;
                kh.TaiKhoan = user;
                kh.MatKhau = password;
                kh.Email = email;
                kh.DiachiKH = address;
                kh.DienThoaiKH = phone;
                kh.NgaySinh = DateTime.Parse(birthday);
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Login");
            }

            return this.Register();
        }

        // Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var user = collection["TenDN"];
            var password = collection["MatKhau"];
            if (String.IsNullOrEmpty(user))
            {
                ViewData["Error1"] = "The account cannot be empty";
            }
            else if (String.IsNullOrEmpty(password))
            {
                ViewData["Error2"] = "The password cannot be empty";
            }
            else
            {
                //Assign value and get session
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == user && n.MatKhau == password);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Login successful!";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "BookStore");
                }
                else
                    ViewBag.ThongBao = "User or password is incorrect!";               
            }
            return View();
        }
    }
}
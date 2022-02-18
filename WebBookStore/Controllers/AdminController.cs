using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class AdminController : Controller
    {
        //Create objects to manage Database
        dbQLBookstoreDataContext db = new dbQLBookstoreDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(FormCollection collection)
        {
            var user = collection["username"];
            var password = collection["password"];
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
                Admin admin = db.Admins.SingleOrDefault(n => n.UserAdmin == user && n.PassAdmin == password);
                if (admin != null)
                {
                    ViewBag.ThongBao = "Login successful!";
                    Session["TaiKhoan"] = admin;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.ThongBao = "User or password is incorrect!";
            }
            return View();
        }
    }
}
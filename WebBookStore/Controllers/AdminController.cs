using WebBookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace MvcBookStore.Controllers
{
    public class AdminController : Controller
    {
        dbQLBookstoreDataContext db = new dbQLBookstoreDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sach(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            //return View(db.Saches.ToList());
            return View(db.Saches.ToList().OrderBy(n => n.MaSach).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            dbQLBookstoreDataContext db = new dbQLBookstoreDataContext();
            var user = collection["username"].ToString();
            var password = collection["password"].ToString();
            if (String.IsNullOrEmpty(user))
            {
                ViewData["Loi1"] = "Username must be entered";
            }
            else if (String.IsNullOrEmpty(password))
            {
                ViewData["Loi2"] = "Password must be entered";
            }
            else
            {
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == user && n.PassAdmin == password);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Username or password is incorrect";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiSach()
        {
            //Put data into Dropdownlist
            //Get data from tb.ChuDe, sort by id ascending, select id value and then display name
            ViewBag.MaCD = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiSach(Sach sach, HttpPostedFileBase fileupload)
        {
            ViewBag.MaCD = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Please choose cover photo";

                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {

                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/product_imgs/"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Image already exists!!!";

                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sach.AnhBia = fileName;
                    db.Saches.InsertOnSubmit(sach);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }

        }
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == id);

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            return View(sach);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasach(Sach sach, HttpPostedFileBase fileupload)
        {
            ViewBag.MaCD = new SelectList(db.ChuDes.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            if (fileupload == null)
            {
                ViewBag.Thongbao = "Please choose cover photo";
                return View();
            }
            //add into db
            else
            {
                if (ModelState.IsValid)
                {
                    //save the file name
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //save the path of the file
                    var path = Path.Combine(Server.MapPath("~/product_imgs/"), fileName);
                    //check if the image exists
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Image already exists!!!";
                    }
                    else
                    {
                        //Save img into the path
                        fileupload.SaveAs(path);
                    }
                    sach.AnhBia = fileName;
                    //SaveDB
                    UpdateModel(sach);
                    db.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }
        }

        public ActionResult Chitietsach(int id)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == id);

            ViewBag.MaSach = sach.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == id);
            ViewBag.Masach = sach.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        [HttpPost, ActionName("XoaSach")]
        public ActionResult Xacnhanxoa(int id)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == id);
            ViewBag.Masach = sach.MaSach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Saches.DeleteOnSubmit(sach);
            db.SubmitChanges();
            return RedirectToAction("Sach");
        }


    }
}
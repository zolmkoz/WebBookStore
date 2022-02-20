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
            int pageSize = 7;
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
            //Gan cac gia tri nguoi dung nhap lieu cho cac bien
            var user = collection["username"].ToString();
            var password = collection["password"].ToString();
            if (String.IsNullOrEmpty(user))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(password))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //gan giá trị cho đối tượng (ad)
                Admin ad = db.Admins.SingleOrDefault(n => n.UserAdmin == user && n.PassAdmin == password);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult ThemmoiSach()
        {
            //Dua du lieu vao dropdownList
            //Lay ds tu table chu de, sap xep tang dan theo tu chu de, chon lay gia tri Ma CD, hien thi tenchude
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
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";

                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //lưu ý thêm thư viện using System.IO
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/product_imgs"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại!!!";

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
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //thêm vào CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    // lưu tên file , lưu ý bổ sung thư viện using system.IO
                    var fileName = Path.GetFileName(fileupload.FileName);
                    // lưu đường dẫn của file
                    var path = Path.Combine(Server.MapPath("~/Content/images"), fileName);
                    //kiểm tra hình ảnh tồn tại chưa
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại!";
                    }
                    else
                    {
                        //lưu hình ảnh vào đường dẫn
                        fileupload.SaveAs(path);
                    }
                    sach.AnhBia = fileName;
                    //lưu vào CSDL
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBookStore.Models;

namespace WebBookStore.Controllers
{
    public class BookStoreController : Controller
    {
        //Tạo 1 đối tượng để quản lý CSDL
        dbQLBookstoreDataContext data = new dbQLBookstoreDataContext();
        private List<Sach> Laysachmoi(int count)
        {
            //Sắp xếp
            return data.Saches.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        // GET: BookStore
        public ActionResult Index()
        {
            var sachmoi = Laysachmoi(5);
            return View(sachmoi);
        }
        public ActionResult Topic()
        {
            var chude = from cd in data.ChuDes select cd;
            return PartialView(chude);
        }
        public ActionResult Publishingcompany()
        {
            var nhaxuatban = from cd in data.NhaXuatBans select cd;
            return PartialView(nhaxuatban);
        }
    }
}
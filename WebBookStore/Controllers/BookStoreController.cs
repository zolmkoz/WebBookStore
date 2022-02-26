using System;
using System.Web;
using WebBookStore.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using PagedList;
using PagedList.Mvc;
namespace BookStore.Controllers
{
    public class BookStoreController : Controller
    {
        dbQLBookstoreDataContext data = new dbQLBookstoreDataContext();
        private List<Sach> Laysachmoi(int count)
        {
            return data.Saches.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        public ActionResult Index(int? page)
        {
            int pageSize = 3;
            int pageNum = (page ?? 1);
            var sachmoi = Laysachmoi(15);
            return View(sachmoi.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Topic()
        {
            var chude = from cd in data.ChuDes select cd;
            return PartialView(chude);
        }
        public ActionResult Publisher()
        {
            var nhaxuatban = from nxb in data.NhaXuatBans select nxb;
            return PartialView(nhaxuatban);
        }
        public ActionResult BookByTopic(int id)
        {
            var sach = from s in data.Saches where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult BookByPublisher(int id)
        {
            var sach = from s in data.Saches where s.MaNXB == id select s;
            return View(sach);
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.Saches where s.MaSach == id select s;
            return View(sach.Single());
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shopthethao.Models;

namespace shopthethao.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        shopthethaoEntities2 db = new shopthethaoEntities2();
        // GET: Admin/User
        public ActionResult Index()
        {
            return View(db.TaiKhoans.Where(x => x.BiXoa == false).ToList());
        }

        public ActionResult Change(int? ID)
        {
            ViewBag.User = db.LoaiTaiKhoans.ToList();
            var list = db.TaiKhoans.First(x => x.MaTaiKhoan == ID);
            return View(list);
        }

        public ActionResult Delete()
        {
            return View();
        }
    }
}
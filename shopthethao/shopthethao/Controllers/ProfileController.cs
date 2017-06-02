using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace shopthethao.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        shopthethaoEntities1 db = new shopthethaoEntities1();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Info(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }
            var p =  db.TaiKhoans.First(x => x.MaTaiKhoan == ID);
            return View(p);
        }
        public ActionResult ChangeProfile(FormCollection fc)
        {
            var id = int.Parse(fc["id"]);
            var p = db.TaiKhoans.First(x => x.MaTaiKhoan == id);
            p.TenHienThi = fc["name"].ToString();
            p.DiaChi = fc["location"].ToString();
            p.DienThoai = fc["phone"].ToString();
            p.Email = fc["email"].ToString();
            p.GioiTinh = Boolean.Parse(fc["sex"]);
            p.NgaySinh = DateTime.Parse(fc["birthday"]);

            db.SaveChanges();

            if(db.SaveChanges() == 0)
            {
                Session["DoiThongTinThanhCong"] = "";
                return RedirectToAction("Info", "Profile");
            }
            return RedirectToAction("Info", "Profile");
        }
        public ActionResult ChangePassword()
        {
            return View();
        }
    }
}
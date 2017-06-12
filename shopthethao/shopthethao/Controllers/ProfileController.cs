using Common;
using PagedList;
using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace shopthethao.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        shopthethaoEntities db = new shopthethaoEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Info(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Error404");
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
                return RedirectToAction("Info", "Profile", new { ID = id });
            }
            return RedirectToAction("Info", "Profile", new { ID = id });
        }

       
        public ActionResult ChangePassword(FormCollection fc)
        {
            var id = int.Parse(fc["id"]);

            string pass = new CreateMD5().Hash((fc["password"].ToString()));
            string newPass = new CreateMD5().Hash((fc["newPassword"].ToString()));

            var p = db.TaiKhoans.First(x => x.MaTaiKhoan == id);

            if(p.MatKhau == pass)
            {
                p.MatKhau = newPass;
                db.SaveChanges();
                if (db.SaveChanges() == 0)
                {
                    Session.Remove("DoiMatKhauThatBai");
                    Session["DoiMatKhauThanhCong"] = "";
                    return RedirectToAction("Info", "Profile", new { ID = id});
                    
                }
            }
            else
            {
                Session["DoiMatKhauThatBai"] = "";
                return RedirectToAction("Info", "Profile", new { ID = id });
            }

            return RedirectToAction("Info", "Profile", new { ID = id });
        }

        public ActionResult HistoryOrder(int? ID, int? page)
        {
            var listOrder = db.DonDatHangs.Where(x => x.MaTaiKhoan == ID && x.BiXoa == false).ToList();
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(listOrder.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }
    }
}
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
        shopthethaoEntities1 db = new shopthethaoEntities1();
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

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public ActionResult ChangePassword(FormCollection fc)
        {
            var id = int.Parse(fc["id"]);

            string pass = CreateMD5(fc["password"].ToString());
            string newPass = CreateMD5(fc["newPassword"].ToString());

            var p = db.TaiKhoans.First(x => x.MaTaiKhoan == id && x.MatKhau == pass);

            if(p != null)
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
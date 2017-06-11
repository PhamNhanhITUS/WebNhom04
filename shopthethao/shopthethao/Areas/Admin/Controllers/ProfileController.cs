using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace shopthethao.Areas.Admin.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Admin/Profile
        shopthethaoEntities db = new shopthethaoEntities();
        public ActionResult Index(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            var list = db.TaiKhoanAdmins.First(x => x.MaAdmin == ID);
            return View(list);
        }
        public ActionResult ChangeProfile(FormCollection fc)
        {
            var id = int.Parse(fc["id"]);
            var p = db.TaiKhoanAdmins.First(x => x.MaAdmin == id);
            p.TenHienThi = fc["name"].ToString();
            p.Email = fc["email"].ToString();
            db.SaveChanges();

            if (db.SaveChanges() == 0)
            {
                return RedirectToAction("Index", "Profile", new { ID = id });
            }
            return RedirectToAction("Index", "Profile", new { ID = id });
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

            var p = db.TaiKhoanAdmins.First(x => x.MaAdmin == id);

            if (p.MatKhauAdmin == pass)
            {
                p.MatKhauAdmin = newPass;
                db.SaveChanges();
                if (db.SaveChanges() == 0)
                {
                    Session.Remove("DoiMatKhauAdminThatBai");
                    Session["DoiMatKhauAdminThanhCong"] = "";
                    return RedirectToAction("Index", "Profile", new { ID = id });
                }
            }
            else
            {
                Session["DoiMatKhauAdminThatBai"] = "";
                return RedirectToAction("Index", "Profile", new { ID = id });
            }

            return RedirectToAction("Index", "Profile", new { ID = id });
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return RedirectToAction("Index", "Home");
        }
    }
}
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
    public class SystemController : Controller
    {
        // GET: Admin/System
        shopthethaoEntities db = new shopthethaoEntities();
        public ActionResult Index()
        {
            return View(db.TaiKhoanAdmins.ToList());
        }
        public static string CreateMD5(string input)
        {
            //Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            //Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public ActionResult Add(FormCollection fc)
        {
            string password = CreateMD5(fc["password"].ToString());
            string username = fc["username"].ToString();
            string email = fc["email"].ToString();

            if (db.TaiKhoanAdmins.Where(u => u.TaiKhoan == username).SingleOrDefault() != null)
            {
                Session["KTThemNhanVien"] = "Tên đăng nhập đã được đăng ký";
                return RedirectToAction("Index");
            }
            else if (db.TaiKhoanAdmins.Where(u => u.Email == email).SingleOrDefault() != null)
            {
                Session["KTThemNhanVien"] = "Email đã đã được đăng ký";
                return RedirectToAction("Index");
            }
            else
            {
                TaiKhoanAdmin a = new TaiKhoanAdmin
                {

                    TaiKhoan = fc["username"].ToString(),
                    MatKhauAdmin = password.ToString(),
                    TenHienThi = fc["name"].ToString(),
                    Email = fc["email"].ToString(),
                    MaLoaiAdmin = int.Parse(fc["admingroup"]),

                };
                db.TaiKhoanAdmins.Add(a);
                db.SaveChanges();

                if (db.SaveChanges() == 0)
                {
                    Session["ThemNhanVienThanhCong"] = "";
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult Change(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "System");
            }
            ViewBag.Admin = db.LoaiAdmins.ToList();
            var list = db.TaiKhoanAdmins.First(x => x.MaAdmin == ID);
            return View(list);
        }
        public ActionResult ChangeForm(FormCollection fc)
        {
            var id = int.Parse(fc["id"]);
            var p = db.TaiKhoanAdmins.First(x => x.MaAdmin == id);
            p.MaLoaiAdmin = int.Parse(fc["admingroup"]);
            db.SaveChanges();
            if (db.SaveChanges() == 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Change");
        }
        public ActionResult Delete(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "System");
            }
            var p = db.TaiKhoanAdmins.First(x => x.MaAdmin == ID);
            db.TaiKhoanAdmins.Remove(p);
            db.SaveChanges();
            if (db.SaveChanges() == 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
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
        shopthethaoEntities2 db = new shopthethaoEntities2();
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
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

            if (db.Admins.Where(u => u.TaiKhoanAdmin == username).SingleOrDefault() != null)
            {
                Session["KTThemNhanVien"] = "Tên đăng nhập đã được đăng ký";
                return RedirectToAction("Index");
            }
            else if (db.Admins.Where(u => u.Email == email).SingleOrDefault() != null)
            {
                Session["KTThemNhanVien"] = "Email đã đã được đăng ký";
                return RedirectToAction("Index");
            }
            else
            {
                shopthethao.Models.Admin admin = new shopthethao.Models.Admin
                {
                    TaiKhoanAdmin = fc["username"].ToString(),
                    MatKhauAdmin = password.ToString(),
                    TenHienThi = fc["name"].ToString(),
                    Email = fc["email"].ToString(),
                    MaLoaiAdmin = int.Parse(fc["admingroup"])

                };
                db.Admins.Add(admin);
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
        public ActionResult Change()
        {
            return View();
        }
        public ActionResult ChangeForm(FormCollection fc)
        {
            return View();
        }
        
    }
}
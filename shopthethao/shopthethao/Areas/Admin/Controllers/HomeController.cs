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
    public class HomeController : Controller
    {
        // GET: Admin/Home
        shopthethaoEntities5 db = new shopthethaoEntities5();
        public ActionResult Index()
        {
            return View();
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
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            var username = fc["username"].ToString();
            var password = CreateMD5(fc["password"].ToString());
            var t = db.TaiKhoanAdmins.Where(u => u.TaiKhoan == username && u.MatKhauAdmin == password).SingleOrDefault();
            if (t != null)
            {
                Session["TenDangNhapAdmin"] = t.TenHienThi;
                Session["MaTaiKhoanAdmin"] = t.MaAdmin;
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                Session["LoiDangNhapAdmin"] = "Tài khoản hoặc mật khẩu không chính xác!";
                return RedirectToAction("Index", "Home");
            }
        }
        //public JsonResult thongKeDoanhThu(int id)
        //{
        //    var mix = db.LoaiSanPhams.Select(l => new { l.MaLoaiSanPham, l.TenLoaiSanPham,  });
        //}
        public ActionResult Dashboard()
        {
            return View();
        }
    }
}
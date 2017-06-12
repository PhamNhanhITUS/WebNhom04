using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shopthethao.Models;
using System.Security.Cryptography;
using System.Text;
using System.Configuration;
using Common;

namespace shopthethao.Controllers
{
    public class HomeController : Controller
    {
        //GET: Home
        shopthethaoEntities db = new shopthethaoEntities();
        public ActionResult Index()
        {
            ViewBag.BestProduct = BestProduct();
            ViewBag.NewProduct_1 = NewProduct_1();
            ViewBag.NewProduct_2 = NewProduct_2();
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            string taikhoan = fc[0].ToString();
            string matkhau = new CreateMD5().Hash((fc[1].ToString()));
            var t = db.TaiKhoans.Where(u => u.TenDangNhap == taikhoan && u.MatKhau == matkhau && u.BiXoa == false).SingleOrDefault();
            if (t != null)
            {
                Session["TenDangNhap"] = t.TenHienThi;
                Session["MaTaiKhoan"] = t.MaTaiKhoan;
                Session["DangNhapThanhCong"] = "";
                return RedirectToAction("Index");
            }
            else
            {
                Session["LoiDangNhap"] = "Tài khoản hoặc mật khẩu không chính xác!";
                return RedirectToAction("Index");
            }

        }
        public ActionResult ResetPass(FormCollection fc)
        {
            string email = fc["Email"].ToString();
            var ramdomPass = new Random().Next(10000000, 99999999);

            var newPass = new CreateMD5().Hash((ramdomPass.ToString()));
            var u = db.TaiKhoans.Where(x => x.Email == email && x.BiXoa == false).SingleOrDefault();
            if (u != null)
            {
                var name = u.TenHienThi;
                var username = u.TenDangNhap;
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/Template/ResetPass.html"));

                content = content.Replace("{{CustomerName}}", name);
                content = content.Replace("{{Username}}", username);
                content = content.Replace("{{Password}}", ramdomPass.ToString());
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(email, "Yêu cầu cấp lại mật khẩu đăng nhập shopthethao.com", content);
                new MailHelper().SendMail(toEmail, "Yêu cầu cấp lại mật khẩu đăng nhập shopthethao.com", content);

                u.MatKhau = newPass;
                if(db.SaveChanges() == 0)
                {
                    Session["GuiMailThanhCong"] = "";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            else
            {
                Session["MailKhongTonTai"] = "Mail chưa được đăng kí!";
                return RedirectToAction("Index");
            }
            

        }
        
        [HttpPost]
        public ActionResult Register(FormCollection fc)
        {
                string matKhauMaHoa = new CreateMD5().Hash((fc["MatKhau"].ToString()));
                string tenDangNhap = fc["TenDangNhap"].ToString();
                string email = fc["Email"].ToString();
                string dienThoai = fc["DienThoai"].ToString();
                var src = db.TaiKhoans.Where(u => u.TenDangNhap == tenDangNhap && u.BiXoa == false).SingleOrDefault();
                if (src != null)
                {
                    Session["KTDangKy"] = "Tên đăng nhập đã được đăng ký";
                    return RedirectToAction("Index");
                }
                else if (db.TaiKhoans.Where(u => u.Email == email).SingleOrDefault() != null)
                {
                    Session["KTDangKy"] = "Email đã đã được đăng ký";
                    return RedirectToAction("Index");
                }
                else if (db.TaiKhoans.Where(u => u.DienThoai == dienThoai).SingleOrDefault() != null)
                {
                    Session["KTDangKy"] = "Số diện đã được đăng ký";
                    return RedirectToAction("Index");
                }
                else
                {
                    TaiKhoan taiKhoan = new TaiKhoan
                    {
                        TenDangNhap = fc["TenDangNhap"].ToString(),
                        MatKhau = matKhauMaHoa.ToString(),
                        TenHienThi = fc["TenHienThi"].ToString(),
                        Email = fc["Email"].ToString(),
                        DienThoai = fc["DienThoai"].ToString(),
                        GioiTinh = Boolean.Parse(fc["GioiTinh"]),
                        NgaySinh = DateTime.Parse(fc["NgaySinh"]),
                        DiaChi = fc["DiaChi"].ToString(),
                        MaLoaiTaiKhoan = 2,
                        BiXoa = false
                    };
                    db.TaiKhoans.Add(taiKhoan);
                    db.SaveChanges();

                    if (db.SaveChanges() == 0)
                    {
                        Session["DangKyThanhCong"] = "";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        Session["DangKyThatBai"] = "";
                        return RedirectToAction("Index");
                    }
                }
        }

        public List<SanPham> BestProduct()
        {

            return ViewBag.BestProduct = db.SanPhams.OrderBy(y => y.SoLuongBan).Take(8).ToList();
        }
        public List<SanPham> NewProduct_1()
        {
            return ViewBag.NewProduct_1 = db.SanPhams.OrderByDescending(y => y.MaSanPham).Take(4).ToList();
        }
        public List<SanPham> NewProduct_2()
        {
            return ViewBag.NewProduct_2 = db.SanPhams.OrderByDescending(y => y.MaSanPham).Skip(4).Take(4).ToList();
        }
    }
}
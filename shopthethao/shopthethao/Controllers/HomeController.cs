using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shopthethao.Models;
using System.Security.Cryptography;
using System.Text;

namespace shopthethao.Controllers
{
    public class HomeController : Controller
    {
        //GET: Home
        shopthethaoEntities5 db = new shopthethaoEntities5();
        public ActionResult Index()
        {
            ViewBag.BestProduct = BestProduct();
            ViewBag.NewProduct_1 = NewProduct_1();
            ViewBag.NewProduct_2 = NewProduct_2();
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
            string taikhoan = fc[0].ToString();
            string matkhau = CreateMD5(fc[1].ToString());
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
        [HttpPost]
        public ActionResult Register(FormCollection fc)
        {
                string matKhauMaHoa = CreateMD5(fc["MatKhau"].ToString());
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
using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Common;
namespace shopthethao.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: Admin/Home
        shopthethaoEntities db = new shopthethaoEntities();
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            var username = fc["username"].ToString();
            var password = new CreateMD5().Hash((fc["password"].ToString()));
            var t = db.TaiKhoanAdmins.Where(u => u.TaiKhoan == username && u.MatKhauAdmin == password).SingleOrDefault();
            if (t != null)
            {

                Session["TenHienThiAdmin"] = t.TenHienThi;
                Session["MaTaiKhoanAdmin"] = t.MaAdmin;
                if(t.MaLoaiAdmin == 1)
                {
                    Session["Admin"] = "";
                }
                return RedirectToAction("Dashboard", "Home");
            }
            else
            {
                Session["LoiDangNhapAdmin"] = "Tài khoản hoặc mật khẩu không chính xác!";
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Dashboard()
        {
            ViewBag.totalOrder = db.DonDatHangs.Count();
            ViewBag.totalProduct = db.SanPhams.Count();
            ViewBag.totalWraning = db.DonDatHangs.Where(x=>x.MaTinhTrang == 1).Count();
            ViewBag.totalUser = db.TaiKhoans.Count();
            var ddh = db.DonDatHangs.OrderByDescending(s => s.NgayLap.Value.Year).Select(s => s.NgayLap.Value.Year).Distinct();
            List<int> lstyear = new List<int>();
            foreach (var item in ddh)
            {
                lstyear.Add(item);
            }
            ViewData["yearDDH"] = lstyear;
            return View();
        }
        //Thống kê doanh thu theo 1: Loại sản phẩm   2: Hãng sản xuất  3: Sản phẩm
        public JsonResult thongKeDoanhThu(int id, int? nam,int? thang)
        {
            if(nam==null)
            {
                nam = DateTime.Now.Year;
                thang = 13;
            }
            if (id==1)
            {
                var mix = (from lsp in db.LoaiSanPhams.DefaultIfEmpty()
                           join sp in db.SanPhams on lsp.MaLoaiSanPham equals sp.MaLoaiSanPham
                           join ctddh in db.ChiTietDonDatHangs on sp.MaSanPham equals ctddh.MaSanPham
                           where ctddh.DonDatHang.MaTinhTrang == 3 &&ctddh.DonDatHang.NgayLap.Value.Year==nam
                           select new { MaLoai = lsp.MaLoaiSanPham, TenLoaiSP = lsp.TenLoaiSanPham, Gia = ctddh.GiaBan * ctddh.SoLuong , NgayLap=ctddh.DonDatHang.NgayLap})
                           .ToList() ;
                if(thang!=13)
                {
                    mix = mix.Where(m=>m.NgayLap.Value.Month==thang).ToList();
                }
                var dataChart = (
                            from rst in mix
                            group rst by new { rst.MaLoai, rst.TenLoaiSP } into gr
                            select new ThongKeDoanhThu { Label = gr.Key.TenLoaiSP, DoanhThu = Convert.ToDecimal(gr.Sum(dt => dt.Gia)) }
                    ).ToList();
                return Json(dataChart, JsonRequestBehavior.AllowGet);
            }
            else if(id==2)
            {
                var mix = (from lsp in db.HangSanXuats.DefaultIfEmpty()
                           join sp in db.SanPhams on lsp.MaHangSanXuat equals sp.MaHangSanXuat
                           join ctddh in db.ChiTietDonDatHangs on sp.MaSanPham equals ctddh.MaSanPham
                           where ctddh.DonDatHang.MaTinhTrang == 3
                           select new { MaLoai = lsp.MaHangSanXuat, TenLoaiSP = lsp.TenHangSanXuat, Gia = ctddh.GiaBan * ctddh.SoLuong, NgayLap = ctddh.DonDatHang.NgayLap }
                    ).ToList();
                if (thang != 13)
                {
                    mix = mix.Where(m => m.NgayLap.Value.Month == thang).ToList();
                }
                var dataChart = (
                                    from rst in mix
                                    group rst by new { rst.MaLoai, rst.TenLoaiSP } into gr
                                    select new ThongKeDoanhThu { Label = gr.Key.TenLoaiSP, DoanhThu = Convert.ToDecimal(gr.Sum(dt => dt.Gia)) }
                                 ).ToList();
                return Json(dataChart, JsonRequestBehavior.AllowGet);
            }
            else if(id==3)
            {
                var mix = (from sp in db.SanPhams
                           join ctddh in db.ChiTietDonDatHangs on sp.MaSanPham equals ctddh.MaSanPham
                           where ctddh.DonDatHang.MaTinhTrang == 3
                           select new { MaLoai = sp.MaSP, TenLoaiSP = sp.TenSanPham, Gia = ctddh.GiaBan * ctddh.SoLuong, NgayLap = ctddh.DonDatHang.NgayLap }
                    ).ToList();
                if (thang != 13)
                {
                    mix = mix.Where(m => m.NgayLap.Value.Month == thang).ToList();
                }
                var dataChart = (
                                    from rst in mix
                                    group rst by new { rst.MaLoai, rst.TenLoaiSP } into gr
                                    select new ThongKeDoanhThu { Label = gr.Key.MaLoai, DoanhThu = Convert.ToDecimal(gr.Sum(dt => dt.Gia)) }
                                 ).ToList();
                return Json(dataChart, JsonRequestBehavior.AllowGet);
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        //Thống kê doanh thu theo tháng trong năm
        public JsonResult thongkeTheoThang(int id)
        {
            var mix = (from ddh in db.DonDatHangs
                       where ddh.MaTinhTrang == 3 && ddh.NgayLap.Value.Year == id
                       select new { NgayLap = ddh.NgayLap, Gia = ddh.TongThanhTien }
                    ).ToList();
            var ma = mix.OrderBy(s => s.NgayLap.Value.Month);
            var dataChart = (
                                   from rst in ma
                                   group rst by new { rst.NgayLap.Value.Month } into gr
                                   select new ThongKeDoanhThu { Label = gr.Key.Month.ToString(), DoanhThu = Convert.ToDecimal(gr.Sum(dt => dt.Gia))/1000000 }
                                ).ToList();
            return Json(dataChart, JsonRequestBehavior.AllowGet);
        }
    }
}
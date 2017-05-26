using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopthethao.Controllers
{
    public class ProductController : Controller
    {
        //GET: Product
       shopthethaoEntities db = new shopthethaoEntities();
        public ActionResult Index()
        {
            ViewBag.ShowCategory = ShowCategory();
            ViewBag.ShowManufacturer = ShowManufacturer();
            return View();
        }
        public ActionResult Detail(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Product");
            }

            var p = db.SanPhams.First(x => x.MaSanPham == id);
            return View(p);
        }
        public List<LoaiSanPham> ShowCategory()
        {
            return db.LoaiSanPhams.ToList();
        }
        public ActionResult Category(int? id)
        {
            ViewBag.Category = db.LoaiSanPhams.Where(u=>u.MaLoaiSanPham == id).ToList();
            return View();
        }
        public List<HangSanXuat> ShowManufacturer()
        {
            return db.HangSanXuats.ToList();
        }
        public ActionResult Manufacturer(int? id)
        {
            ViewBag.Manufacturer =  db.HangSanXuats.Where(u=>u.MaHangSanXuat == id).ToList();
            return View();
        }
    }
}
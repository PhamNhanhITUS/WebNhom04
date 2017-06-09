using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopthethao.Areas.Admin.Controllers
{
    public class DeleteController : Controller
    {
        shopthethaoEntities5 db = new shopthethaoEntities5();
        // GET: Admin/Delete
        public ActionResult Index()
        {
            ViewBag.Category = db.LoaiSanPhams.Where(x => x.BiXoa == true).ToList();
            ViewBag.Manufacturer = db.HangSanXuats.Where(x => x.BiXoa == true).ToList();
            return View(db.SanPhams.Where(x=>x.BiXoa == true).ToList());
        }

        public ActionResult RestoreCate(int? ID)
        {
            var p = db.LoaiSanPhams.First(x => x.MaLoaiSanPham == ID);
            p.BiXoa = false;
            db.SaveChanges();
            if(db.SaveChanges() == 0)
            {
                Session["PhucHoiThanhCong"] = "";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCate(int? ID)
        {

            var p = db.LoaiSanPhams.First(x => x.MaLoaiSanPham == ID);
            db.LoaiSanPhams.Remove(p);
            db.SaveChanges();
            if(db.SaveChanges() == 0)
            {
                Session["XoaThanhCong"] = "";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult RestoreManu(int? ID)
        {

            var p = db.HangSanXuats.First(x => x.MaHangSanXuat == ID);
            p.BiXoa = false;
            db.SaveChanges();
            if (db.SaveChanges() == 0)
            {
                Session["PhucHoiThanhCong"] = "";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteManu(int? ID)
        {

            var p = db.HangSanXuats.First(x => x.MaHangSanXuat == ID);
            db.HangSanXuats.Remove(p);
            db.SaveChanges();
            if (db.SaveChanges() == 0)
            {
                Session["XoaThanhCong"] = "";
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult RestoreProduct(int? ID)
        {

            var p = db.SanPhams.First(x => x.MaSanPham == ID);
            p.BiXoa = false;
            db.SaveChanges();
            if (db.SaveChanges() == 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteProduct(int? ID)
        {

            var p = db.SanPhams.First(x => x.MaSanPham == ID);
            db.SanPhams.Remove(p);
            db.SaveChanges();
            if (db.SaveChanges() == 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
    }
}
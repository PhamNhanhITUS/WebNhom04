﻿using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopthethao.Areas.Admin.Controllers
{
    public class DeleteController : Controller
    {
        shopthethaoEntities db = new shopthethaoEntities();
        // GET: Admin/Delete
        public ActionResult Index()
        {
            ViewBag.Category = db.LoaiSanPhams.Where(x => x.BiXoa == true).ToList();
            ViewBag.Manufacturer = db.HangSanXuats.Where(x => x.BiXoa == true).ToList();
            return View(db.SanPhams.Where(x=>x.BiXoa == true).ToList());
        }

        public ActionResult RestoreCate(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Delete");
            }
            var p = db.LoaiSanPhams.First(x => x.MaLoaiSanPham == ID);
            p.BiXoa = false;
            db.SaveChanges();
            if(db.SaveChanges() == 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCate(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Delete");
            }
            var p = db.LoaiSanPhams.First(x => x.MaLoaiSanPham == ID);
            db.LoaiSanPhams.Remove(p);
            db.SaveChanges();
            if(db.SaveChanges() == 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult RestoreManu(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Delete");
            }
            var p = db.HangSanXuats.First(x => x.MaHangSanXuat == ID);
            p.BiXoa = false;
            db.SaveChanges();
            if (db.SaveChanges() == 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteManu(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Delete");
            }
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
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Delete");
            }
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
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Delete");
            }
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
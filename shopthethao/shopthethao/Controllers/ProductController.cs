﻿using PagedList;
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
        public ActionResult Index(int? page)
        {
            ViewBag.ShowCategory = ShowCategory();
            ViewBag.ShowManufacturer = ShowManufacturer();
            ViewBag.ShowProduct = ShowProduct();
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(ShowProduct().ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Detail(int? id)
        {
            
            if (db.SanPhams.Where(x=>x.MaSanPham == id) == null)
            {
                return RedirectToAction("Index", "Product");
            }
            else
            {
                var p = db.SanPhams.First(x => x.MaSanPham == id);
                p.SoLuongXem++;
                db.SaveChanges();
                ViewBag.Detail = db.SanPhams.Where(x => x.MaLoaiSanPham == p.MaLoaiSanPham && x.BiXoa == false).Take(4).ToList();
                return View(p);
            }
            
        }
        public List<SanPham> ShowProduct()
        {
            return db.SanPhams.ToList();
        }
        public List<LoaiSanPham> ShowCategory()
        {
            return db.LoaiSanPhams.ToList();
        }
        public ActionResult Category(int? id, int? page)
        {
            ViewBag.ShowCategory = ShowCategory();
            ViewBag.ShowManufacturer = ShowManufacturer();
            var list = db.SanPhams.Where(u => u.MaLoaiSanPham == id && u.BiXoa == false).ToList();
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }
        public List<HangSanXuat> ShowManufacturer()
        {
            return db.HangSanXuats.ToList();
        }
        public ActionResult Manufacturer(int? id, int? page)
        {
            ViewBag.ShowCategory = ShowCategory();
            ViewBag.ShowManufacturer = ShowManufacturer();
            var list = db.SanPhams.Where(u => u.MaHangSanXuat == id && u.BiXoa == false).ToList();
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Search(string Keyword, int? page)
        {
            ViewBag.ShowCategory = ShowCategory();
            ViewBag.ShowManufacturer = ShowManufacturer();

            var list = db.SanPhams.Where(x => x.TenSanPham.ToLower().Contains(Keyword) || x.TenSanPham.Contains(Keyword) || x.TenSanPham.ToLower().StartsWith(Keyword) || x.TenSanPham.StartsWith(Keyword) && x.BiXoa == false).ToList();

            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Filter(FormCollection fc, int? page)
        {
            int toPrice;
            int formPrice;
            string[] txtPrice = fc["txtPrice"].ToString().Split(',');
            toPrice = int.Parse(txtPrice[0] + "000");
            formPrice = int.Parse(txtPrice[1] + "000");
            int category = int.Parse(fc["cbbCategory"]);
            int manufacturer = int.Parse(fc["cbbManufacture"]);
            
            ViewBag.ShowCategory = ShowCategory();
            ViewBag.ShowManufacturer = ShowManufacturer();
            
            var list = db.SanPhams.Where(x => x.GiaSanPham >= toPrice && x.GiaSanPham <= formPrice).ToList();
            if (category != 0)
                list = list.Where(x=>x.MaLoaiSanPham==category && x.BiXoa == false).ToList();
            if (manufacturer != 0)
                list = list.Where(x=>x.MaHangSanXuat== manufacturer && x.BiXoa == false).ToList();
            int pageSize = 9;
            int pageNumber = (page ?? 1);
            return View(list.ToPagedList(pageNumber, pageSize));
        }

    }
}
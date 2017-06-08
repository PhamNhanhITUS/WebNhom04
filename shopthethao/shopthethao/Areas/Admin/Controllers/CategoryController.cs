﻿using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopthethao.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        shopthethaoEntities2 db = new shopthethaoEntities2();
        // GET: Admin/Category
        public ActionResult Index()
        {
            return View(db.LoaiSanPhams.Where(x => x.BiXoa == false).ToList());
        }
        public ActionResult Add(FormCollection fc)
        {
            var name = fc["name"].ToString();
            var id = fc["id"].ToString();

            if (db.LoaiSanPhams.Where(x => x.MaLoai == id).SingleOrDefault() != null)
            {
                Session["TrungLoaiSP"] = "";
                return RedirectToAction("Index");
            }
            else
            {
                LoaiSanPham category = new LoaiSanPham
                {
                    MaLoai = id,
                    TenLoaiSanPham = name,
                    BiXoa = false
                };
                db.LoaiSanPhams.Add(category);
                db.SaveChanges();
                if (db.SaveChanges() == 0)
                {
                    Session["ThemLoaiSPThanhCong"] = "";
                    return RedirectToAction("Index");
                }
                else
                {
                    Session["ThemLoaiSPThatBai"] = "";
                    return RedirectToAction("Index");
                }
            }
            
        }
        public ActionResult Change(int? ID)
        {
            var list = db.LoaiSanPhams.First(x => x.MaLoaiSanPham == ID);
            return View(list);
        }

        public ActionResult ChangeForm(FormCollection fc)
        {
            var id = int.Parse(fc["id"]);
            var idCate = fc["idCate"].ToString();

            if (db.LoaiSanPhams.Where(x => x.MaLoai == idCate).SingleOrDefault() != null)
            {
                Session["TrungLoaiSP"] = "";
                return RedirectToAction("Change", "Category", new { ID = id });
            }
            else
            {
                var p = db.LoaiSanPhams.First(x => x.MaLoaiSanPham == id);
                p.MaLoai = fc["id"].ToString();
                p.TenLoaiSanPham = fc["name"].ToString();
                db.SaveChanges();

                if (db.SaveChanges() == 0)
                {
                    Session["SuaLoaiSPThanhCong"] = "";
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    Session["SuaLoaiSPThatBai"] = "";
                    return RedirectToAction("Index", "Category");
                }
            }
        }
        public ActionResult Delete(int? ID)
        {
            var p = db.LoaiSanPhams.First(x => x.MaLoaiSanPham == ID);
            p.BiXoa = true;
            db.SaveChanges();

            if (db.SaveChanges() == 0)
            {
                Session["XoaSPThanhCong"] = "";
                return RedirectToAction("Index", "Category");
            }
            else
            {
                Session["SuaLoaiSPThatBai"] = "";
                return RedirectToAction("Index", "Category");
            }
        }
    }
}
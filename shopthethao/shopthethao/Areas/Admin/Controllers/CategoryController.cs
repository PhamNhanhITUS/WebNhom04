using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopthethao.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        shopthethaoEntities db = new shopthethaoEntities();
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
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Category");
            }
            var list = db.LoaiSanPhams.First(x => x.MaLoaiSanPham == ID);
            return View(list);
        }

        public ActionResult ChangeForm(FormCollection fc)
        {
            var id = int.Parse(fc["id"]);
            var idCate = fc["idCate"].ToString();

           
                var p = db.LoaiSanPhams.First(x => x.MaLoaiSanPham == id);
                p.MaLoai = fc["idCate"].ToString();
                p.TenLoaiSanPham = fc["name"].ToString();
                db.SaveChanges();

                if (db.SaveChanges() == 0)
                {
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    return RedirectToAction("Index", "Category");
                }
        }
        public ActionResult Delete(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Category");
            }
            var p = db.LoaiSanPhams.First(x => x.MaLoaiSanPham == ID);

            if(p.SanPhams.Count == 0)
            {
                p.BiXoa = true;
                db.SaveChanges();

                if (db.SaveChanges() == 0)
                {
                    return RedirectToAction("Index", "Category");
                }
                else
                {
                    return RedirectToAction("Index", "Category");
                }
            }
            else
            {
                return RedirectToAction("Index", "Category");
            }
        }
    }
}
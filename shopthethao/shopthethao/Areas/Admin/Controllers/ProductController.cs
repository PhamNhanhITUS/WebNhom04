using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopthethao.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        shopthethaoEntities5 db = new shopthethaoEntities5();
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddProduct()
        {
            ViewBag.Category = db.LoaiSanPhams.Where(x => x.BiXoa == false).ToList();
            ViewBag.Manufacturer = db.HangSanXuats.Where(x => x.BiXoa == false).ToList();
            ViewBag.Sale = db.KhuyenMaiSanPhams.ToList();
            return View();
        }
        public ActionResult Add(FormCollection fc, HttpPostedFileBase picture)
        {
            var id = fc["id"].ToString();
            var name = fc["name"].ToString();
            var category = int.Parse(fc["category"]);
            var manufacturer = int.Parse(fc["manufacturer"]);
            var price = int.Parse(fc["price"]);
            var quantity = int.Parse(fc["quantity"]);
            var sale = int.Parse(fc["sale"]);
            var description = fc["description"].ToString();


            if (db.SanPhams.Where(x => x.MaSP == id).SingleOrDefault() != null)
            {
                Session["TrungSanPham"] = "";
                return RedirectToAction("Index");
            }
            else
            {
                SanPham product = new SanPham
                {
                    MaSP = id,
                    TenSanPham = name,
                    GiaSanPham = price,
                    SoLuongTon = quantity,
                    NgayNhap = DateTime.Now,
                    MoTa = description,
                    MaHangSanXuat = manufacturer,
                    MaLoaiSanPham = category,
                    MaKhuyenMai = sale,
                    TrangThai = true,
                    BiXoa = false
                };

                db.SanPhams.Add(product);
                db.SaveChanges();

                var lastInsertName = product.MaSP;
                var lastInsertId = product.MaSanPham;

                if (picture != null && picture.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(lastInsertId.ToString() + "_" + picture.FileName);

                    string dirPath = Server.MapPath("~/Assets/Product");
                    string targetDirPath = Path.Combine(dirPath, lastInsertName.ToString());
                    Directory.CreateDirectory(targetDirPath);

                    string path = Path.Combine(targetDirPath, fileName);

                    picture.SaveAs(path);
                    product.URLAnhDaiDien = fileName;

                    db.SaveChanges();

                    if (db.SaveChanges() == 0)
                    {
                        Session["ThemSanPhamThanhCong"] = "";
                        return RedirectToAction("Index", "Product");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Product");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Product");
                }
            }
        }
        public ActionResult DeleteAll()
        {
            return View();
        }
    }
}
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
        shopthethaoEntities db = new shopthethaoEntities();
        private object lastInsertId;

        // GET: Admin/Product
        public ActionResult Index()
        {
            return View(db.SanPhams.Where(x => x.BiXoa == false).ToList());
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
                Session["TrungSanPham"] = "Mã sản phẩm đã tồn tại";
                return RedirectToAction("AddProduct", "Product");
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
                        Session.Remove("TrungSanPham");
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

        public ActionResult Change(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Product");
            }
            ViewBag.Category = db.LoaiSanPhams.Where(x => x.BiXoa == false).ToList();
            ViewBag.Manufacturer = db.HangSanXuats.Where(x => x.BiXoa == false).ToList();
            ViewBag.Sale = db.KhuyenMaiSanPhams.ToList();
            var list = db.SanPhams.First(x => x.MaSanPham == ID);
            return View(list);
        }
        public ActionResult ChangeForm(FormCollection fc, HttpPostedFileBase picture)
        {
            var id = int.Parse(fc["id"]);
            var idProduct = fc["idProduct"].ToString();
            var name = fc["name"].ToString();
            var category = int.Parse(fc["category"]);
            var manufacturer = int.Parse(fc["manufacturer"]);
            var price = int.Parse(fc["price"]);
            var quantity = int.Parse(fc["quantity"]);
            var sale = int.Parse(fc["sale"]);
            var description = fc["description"].ToString();

            var p = db.SanPhams.First(x => x.MaSanPham == id);
            p.MaSP = idProduct;
            p.TenSanPham = name;
            p.GiaSanPham = price;
            p.SoLuongTon = quantity;
            p.MoTa = description;
            p.MaHangSanXuat = manufacturer;
            p.MaLoaiSanPham = category;
            p.MaKhuyenMai = sale;

            db.SaveChanges();
            Session["SuaSanPhamThanhCong"] = "";
            var lastInsertName = p.MaSP;
            var lastInsertId = p.MaSanPham;

            if (picture != null && picture.ContentLength > 0)
            {
                string fileName = Path.GetFileName(lastInsertId.ToString() + "_" + picture.FileName);

                string dirPath = Server.MapPath("~/Assets/Product");
                string targetDirPath = Path.Combine(dirPath, lastInsertName.ToString());
                Directory.CreateDirectory(targetDirPath);

                string path = Path.Combine(targetDirPath, fileName);

                picture.SaveAs(path);
                p.URLAnhDaiDien = fileName;

                db.SaveChanges();

                if (db.SaveChanges() == 0)
                {
                    Session["SuaSanPhamThanhCong"] = "";
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    return RedirectToAction("Index", "Product");
                }
            }
            return RedirectToAction("Index", "Product");
        }
        public ActionResult Delete(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Product");
            }
            var p = db.SanPhams.First(x => x.MaSanPham== ID);
            p.BiXoa = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Picture(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "Product");
            }
            var p = db.SanPhams.First(x => x.MaSanPham == ID);
            ViewBag.NameProduct = p.TenSanPham;
            ViewBag.IDProduct = p.MaSanPham;
            ViewBag.ID = p.MaSP;
            return View(db.HinhAnhs.Where(x=>x.MaSanPham == ID).ToList());
        }
        public ActionResult AddPicture(FormCollection fc, HttpPostedFileBase picture)
        {
            var id = int.Parse(fc["id"]);
            var idPro = fc["idPro"].ToString();

            if (picture != null && picture.ContentLength > 0)
            {
                string fileName = Path.GetFileName(id.ToString() + "_" + picture.FileName);

                string dirPath = Server.MapPath("~/Assets/Product");
                string targetDirPath = Path.Combine(dirPath, idPro.ToString());
                //Directory.CreateDirectory(targetDirPath);

                string path = Path.Combine(targetDirPath, fileName);

                picture.SaveAs(path);
                HinhAnh pic = new HinhAnh
                {
                    MaSanPham = id,
                    URL = fileName,
                };
                db.HinhAnhs.Add(pic);
                db.SaveChanges();

                if (db.SaveChanges() == 0)
                {
                    return RedirectToAction("Picture", "Product", new { ID = id});
                }
                else
                {
                    return RedirectToAction("Picture", "Product", new { ID = id });
                }
            }
            else
            {
                return RedirectToAction("Picture", "Product", new { ID = id });
            }
        }
        public ActionResult DeletePicture(int? ID, int? idPro)
        {
            var p = db.HinhAnhs.First(x => x.MaHinhAnh == ID);
            db.HinhAnhs.Remove(p);
            db.SaveChanges();
            return RedirectToAction("Picture", "Product", new { ID = idPro });
        }


    }
}
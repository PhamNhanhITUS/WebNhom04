using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopthethao.Areas.Admin.Controllers
{
    public class ManufacturerController : Controller
    {
        shopthethaoEntities4 db = new shopthethaoEntities4();
        // GET: Admin/Manufacturer
        public ActionResult Index()
        {
            return View(db.HangSanXuats.Where(x => x.BiXoa == false).ToList());
        }
        [HttpPost]
        public ActionResult Add(FormCollection fc, HttpPostedFileBase picture)
        {
            var name = fc["name"].ToString();
            var id = fc["id"].ToString();

            if (db.HangSanXuats.Where(x => x.MaHSX == id).SingleOrDefault() != null)
            {
                Session["TrungHSX"] = "";
                return RedirectToAction("Index");
            }
            else
            {
                HangSanXuat manufacturer = new HangSanXuat
                {
                    MaHSX = id,
                    TenHangSanXuat = name,
                    BiXoa = false
                };

                db.HangSanXuats.Add(manufacturer);
                db.SaveChanges();

                var lastInsertName = manufacturer.TenHangSanXuat;

                if (picture != null && picture.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(lastInsertName.ToString() + "_" + picture.FileName);

                    string dirPath = Server.MapPath("~/Assets/Logo");
                    string targetDirPath = Path.Combine(dirPath, lastInsertName.ToString());
                    Directory.CreateDirectory(targetDirPath);

                    string path = Path.Combine(targetDirPath, fileName);

                    picture.SaveAs(path);
                    manufacturer.LogoURL = fileName;

                    db.SaveChanges();

                    if (db.SaveChanges() == 0)
                    {
                        Session["ThemHSXThanhCong"] = "";
                        return RedirectToAction("Index", "Manufacturer");
                    }
                    else
                    {
                        Session["ThemHSXThatBai"] = "";
                        return RedirectToAction("Index", "Manufacturer");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Manufacturer");
                }
            }
        }
        public ActionResult Change(int? ID)
        {
            var list = db.HangSanXuats.First(x => x.MaHangSanXuat == ID);
            return View(list);
        }
        public ActionResult ChangeForm(FormCollection fc, HttpPostedFileBase picture)
        {
            var id = int.Parse(fc["id"]);
            var idManu = fc["idManu"].ToString();

            if (db.HangSanXuats.Where(x => x.MaHSX == idManu).SingleOrDefault() != null)
            {
                Session["TrungHSX"] = "";
                return RedirectToAction("Change", "Manufacturer", new { ID = id });
            }
            else
            {
                var p = db.HangSanXuats.First(x => x.MaHangSanXuat == id);
                p.MaHSX = fc["idManu"].ToString();
                p.TenHangSanXuat = fc["name"].ToString();
                db.SaveChanges();

                var lastInsertName = p.TenHangSanXuat;

                if (picture != null && picture.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(lastInsertName.ToString() + "_" + picture.FileName);

                    string dirPath = Server.MapPath("~/Assets/Logo");
                    string targetDirPath = Path.Combine(dirPath, lastInsertName.ToString());
                    Directory.CreateDirectory(targetDirPath);

                    string path = Path.Combine(targetDirPath, fileName);

                    picture.SaveAs(path);
                    p.LogoURL = fileName;

                    db.SaveChanges();

                    if (db.SaveChanges() == 0)
                    {
                        return RedirectToAction("Index", "Manufacturer");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manufacturer");
                    }
                }
                return RedirectToAction("Index", "Manufacturer");
            }
        }
        public ActionResult Delete(int? ID)
        {
            var p = db.HangSanXuats.First(x => x.MaHangSanXuat == ID);
            p.BiXoa = true;
            db.SaveChanges();

            if (db.SaveChanges() == 0)
            {
                Session["XoaHSXThanhCong"] = "";
                return RedirectToAction("Index", "Manufacturer");
            }
            else
            {
                return RedirectToAction("Index", "Manufacturer");
            }
        }
    }
}
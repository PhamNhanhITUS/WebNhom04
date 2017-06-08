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
        shopthethaoEntities2 db = new shopthethaoEntities2();
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

                var lastInsertID = manufacturer.MaHangSanXuat;

                if (picture != null && picture.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(lastInsertID.ToString() + "_" + picture.FileName);

                    string dirPath = Server.MapPath("~/Assets/HinhAnh");
                    string targetDirPath = Path.Combine(dirPath, lastInsertID.ToString());
                    Directory.CreateDirectory(targetDirPath);

                    string path = Path.Combine(targetDirPath, fileName);

                    picture.SaveAs(path);
                    manufacturer.LogoURL = fileName;

                    db.SaveChanges();
                }

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



            
            return View();
        }
        public ActionResult Change()
        {
            return View();
        }
    }
}
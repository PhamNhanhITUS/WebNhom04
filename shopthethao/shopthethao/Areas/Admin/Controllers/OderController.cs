using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopthethao.Areas.Admin.Controllers
{
    public class OderController : Controller
    {
        // GET: Admin/Oder
        shopthethaoEntities5 db = new shopthethaoEntities5();
        public ActionResult Index()
        {
            return View(db.DonDatHangs.Where(x => x.BiXoa == false).ToList());
        }
        public ActionResult Detail(int? ID)
        {
            return View(db.DonDatHangs.First(x => x.MaDonDatHang == ID && x.BiXoa == false));
        }
        public ActionResult Change(int? ID)
        {
            ViewBag.Status = db.TinhTrangs.ToList();
            var list = db.DonDatHangs.First(x => x.MaDonDatHang == ID);
            return View(list);
        }
        
        public ActionResult ChangeForm(FormCollection fc)
        {
            var id = int.Parse(fc["id"]);
            var p = db.DonDatHangs.First(x => x.MaDonDatHang == id);
            p.MaTinhTrang = int.Parse(fc["status"]);
            db.SaveChanges();
            if (db.SaveChanges() == 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Change");
        }
        public ActionResult Delete(int? ID)
        {
            var p = db.DonDatHangs.First(x => x.MaDonDatHang== ID);
            p.BiXoa = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ListDelete()
        {
            return View(db.DonDatHangs.Where(x => x.BiXoa == true).ToList());
        }
        public ActionResult RestoreOder(int? ID)
        {
            var p = db.DonDatHangs.First(x => x.MaDonDatHang == ID);
            p.BiXoa = false;
            db.SaveChanges();
            return RedirectToAction("ListDelete", "Oder");
        }
        public ActionResult DeleteOder(int? ID)
        {
            var p = db.DonDatHangs.First(x => x.MaDonDatHang == ID);
            db.DonDatHangs.Remove(p);
            db.SaveChanges();
            return RedirectToAction("ListDelete", "Oder");
        }
    }
}
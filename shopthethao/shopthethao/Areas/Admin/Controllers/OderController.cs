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
        shopthethaoEntities db = new shopthethaoEntities();
        public ActionResult Index()
        {
            return View(db.DonDatHangs.OrderBy(x=>x.TinhTrang.MaTinhTrang).Where(x => x.BiXoa == false).ToList());
        }
        public ActionResult Detail(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                return RedirectToAction("Index", "Oder");
            }
            return View(db.DonDatHangs.First(x => x.MaDonDatHang == ID && x.BiXoa == false));
        }
        public ActionResult Change(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                return RedirectToAction("Index", "Oder");
            }
            ViewBag.Status = db.TinhTrangs.ToList();
            var list = db.DonDatHangs.First(x => x.MaDonDatHang == ID);
            return View(list);
        }
        
        public ActionResult ChangeForm(FormCollection fc)
        {
            var id = (fc["id"]).ToString();
            var p = db.DonDatHangs.First(x => x.MaDonDatHang == id);
            if (p.ChiTietDonDatHangs.Where(ct => ct.SoLuong <= ct.SanPham.SoLuongTon).Count() == p.ChiTietDonDatHangs.Count)
            {
                p.MaTinhTrang = int.Parse(fc["status"]);
                if (db.SaveChanges() == 0)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Change");
            }
            return RedirectToAction("Change");
        }
        public ActionResult Delete(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                return RedirectToAction("Index", "Oder");
            }
            var p = db.DonDatHangs.First(x => x.MaDonDatHang == ID);
            p.BiXoa = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ListDelete()
        {
            return View(db.DonDatHangs.Where(x => x.BiXoa == true).ToList());
        }
        public ActionResult RestoreOder(string ID)
        {
            if (string.IsNullOrEmpty(ID))
            {
                return RedirectToAction("ListDelete", "Oder");
            }
            var p = db.DonDatHangs.First(x => x.MaDonDatHang == ID);
            p.BiXoa = false;
            db.SaveChanges();
            return RedirectToAction("ListDelete", "Oder");
        }
        public ActionResult DeleteOder(string ID)
        {
            if (string.IsNullOrWhiteSpace(ID))
            {
                return RedirectToAction("ListDelete", "Oder");
            }
            var p = db.DonDatHangs.First(x => x.MaDonDatHang == ID);
            db.DonDatHangs.Remove(p);
            db.SaveChanges();
            return RedirectToAction("ListDelete", "Oder");
        }
    }
}
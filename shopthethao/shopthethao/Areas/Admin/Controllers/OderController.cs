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
        shopthethaoEntities4 db = new shopthethaoEntities4();
        public ActionResult Index()
        {
            return View(db.DonDatHangs.Where(x => x.BiXoa == false).ToList());
        }
        public ActionResult Detail(int? ID)
        {
            return View(db.DonDatHangs.First(x => x.MaDonDatHang == ID && x.BiXoa == false));
        }
        public ActionResult Change()
        {
            return View();
        }
        public ActionResult Delete()
        {
            return View();
        }
    }
}
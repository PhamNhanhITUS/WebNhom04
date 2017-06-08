using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopthethao.Areas.Admin.Controllers
{
    public class SystemController : Controller
    {
        // GET: Admin/System
        shopthethaoEntities2 db = new shopthethaoEntities2();
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }
    }
}
﻿using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace shopthethao.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        shopthethaoEntities2 db = new shopthethaoEntities2();
        // GET: Admin/Product
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Category()
        {
            return View();
        }
        public ActionResult Manufacturer()
        {
            return View();
        }
        public ActionResult DeleteAll()
        {
            return View();
        }
    }
}
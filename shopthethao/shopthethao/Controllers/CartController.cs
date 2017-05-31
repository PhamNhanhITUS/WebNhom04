﻿using shopthethao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace shopthethao.Controllers
{
    public class CartController : Controller
    {
        shopthethaoEntities1 db = new shopthethaoEntities1();

        private const string CartSession = "CartSession";
        // GET: Cart
        public ActionResult Index()
        {
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }

        public ActionResult AddItem(int productId, int quantity)
        {
            shopthethaoEntities1 db = new shopthethaoEntities1();
            var product = db.SanPhams.First(x => x.MaSanPham == productId);
            var cart = Session[CartSession];
            if (cart != null)
            {
                var list = (List<CartItem>)cart;
                if (list.Exists(x => x.Product.MaSanPham == productId))
                {

                    foreach (var item in list)
                    {
                        if (item.Product.MaSanPham == productId)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    list.Add(item);
                }
                //Gán vào session
                Session[CartSession] = list;
            }
            else
            {
                //tạo mới đối tượng cart item
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                var list = new List<CartItem>();
                list.Add(item);
                //Gán vào session
                Session[CartSession] = list;
            }
            return RedirectToAction("Index");
        }
        public JsonResult Delete(int id)
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Product.MaSanPham == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Update(string cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x.Product.MaSanPham == item.Product.MaSanPham);
                if (jsonItem != null)
                {
                    item.Quantity = jsonItem.Quantity;
                }
            }
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }

        public ActionResult Payment(int? ID)
        {
            shopthethaoEntities1 db = new shopthethaoEntities1();

            ViewBag.Info = db.TaiKhoans.Where(x => x.MaTaiKhoan == ID).ToList();
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
   
        //[HttpPost]
        //public ActionResult Paymenting(FormCollection fc)
        //{

           

        //    DonDatHang donhang = new DonDatHang
        //    {

        //        NgayLap = DateTime.Parse("2017-06-02"),
        //        MaTaiKhoan = int.Parse(fc["id"]),
        //        MaTinhTrang = 1,
        //        TongThanhTien = 1000000,
        //        DiaChiNhanHang = fc["address"].ToString(),
        //        HoTenNhanHang = fc["shipName"].ToString(),
        //        EmailNhanhang = fc["email"].ToString(),
        //        DienThoaiNhanHang = fc["phone"].ToString(),
        //    };

        //    db.DonDatHangs.Add(donhang);

        //    db.SaveChanges();

        //    if (db.SaveChanges() == 0)
        //    {
        //        Session["DangKyThanhCong"] = "";
        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        Session["DangKyThatBai"] = "";
        //        return RedirectToAction("Index", "Product");
        //    }
        //    //try
        //    //{
        //    //    var idOder = db.DonDatHangs.OrderByDescending(x => x.MaDonDatHang).Take(1);
        //    //    var cart = (List<CartItem>)Session[CartSession];
        //    //    var detailDao = new Model.Dao.OrderDetailDao();
        //    //    decimal total = 0;
        //    //    foreach (var item in cart)
        //    //    {
        //    //        var orderDetail = new OrderDetail();
        //    //        orderDetail.ProductID = item.Product.ID;
        //    //        orderDetail.OrderID = id;
        //    //        orderDetail.Price = item.Product.Price;
        //    //        orderDetail.Quantity = item.Quantity;
        //    //        detailDao.Insert(orderDetail);

        //    //        total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
        //    //    }

        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    ghi log
        //    //    return Redirect("/loi-thanh-toan");
        //    //}

        //}
    }
}
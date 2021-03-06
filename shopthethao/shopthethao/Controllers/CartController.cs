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
        shopthethaoEntities db = new shopthethaoEntities();

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

        public ActionResult AddItem(int productId, int quantity, int price)
        {
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
                    item.price = price;
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
                item.price = price;
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

        public ActionResult Payment()
        {
            var ID = (int)Session["MaTaiKhoan"];
            ViewBag.Info = db.TaiKhoans.Where(x => x.MaTaiKhoan == ID).ToList();
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public string MaDonHang()
        {
            string ma = "";
            DateTime now = DateTime.Now;
          
            ma +=""+ now.Year + (now.Month>9?now.Month.ToString() : "0"+now.Month) + (now.Day > 9 ? now.Day.ToString() : "0" + now.Day) + (now.Hour>9?now.Hour.ToString():"0"+now.Hour) + (now.Minute > 9 ? now.Minute.ToString() : "0" + now.Minute) + (now.Second > 9 ? now.Second.ToString() : "0" + now.Second);
            return ma;
        }
        [HttpPost]
        public ActionResult Payment(FormCollection fc)
        {

            var MaTaiKhoan = int.Parse(fc["id"]);

            DonDatHang order = new DonDatHang
            {
                MaDonDatHang = MaDonHang(),
                NgayLap = DateTime.Now,
                MaTaiKhoan = int.Parse(fc["id"]),
                MaTinhTrang = 1,
                TongThanhTien = int.Parse(fc["total"]),
                DiaChiNhanHang = fc["address"].ToString(),
                HoTenNhanHang = fc["shipName"].ToString(),
                EmailNhanhang = fc["email"].ToString(),
                DienThoaiNhanHang = fc["phone"].ToString(),
                BiXoa = false
            };

            db.DonDatHangs.Add(order);

            db.SaveChanges();

            var idOrder = order.MaDonDatHang;
            try
            {
                var cart = (List<CartItem>)Session[CartSession];
                foreach (var item in cart)
                {
                    ChiTietDonDatHang orderDetail = new ChiTietDonDatHang
                    {
                        SoLuong = item.Quantity,
                        GiaBan = (int)(item.price),
                        MaDonDatHang = idOrder,
                        MaSanPham = item.Product.MaSanPham
                    };
                    db.ChiTietDonDatHangs.Add(orderDetail);
                    db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Session["ThanhToanThatBai"] = "";
                return RedirectToAction("Index", "Home");
            }
            Session["ThanhToanThanhCong"] = "";
            Session.Remove("CartSession");
            return RedirectToAction("Index", "Home");
        }
    }
}
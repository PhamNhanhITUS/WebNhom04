﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using shopthethao.Models;
using System.Text;
using System.Security.Cryptography;
using Common;

namespace shopthethao.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        shopthethaoEntities db = new shopthethaoEntities();
        // GET: Admin/User
        public ActionResult Index()
        {
            return View(db.TaiKhoans.Where(x => x.BiXoa == false).ToList());
        }
        public ActionResult Add(FormCollection fc)
        {
            string password = new CreateMD5().Hash((fc["password"].ToString()));
            string username = fc["username"].ToString();
            string email = fc["email"].ToString();
            string phone = fc["phone"].ToString();

            if (db.TaiKhoans.Where(u => u.TenDangNhap == username && u.BiXoa == false).SingleOrDefault() != null)
            {
                Session["KTThemTaiKhoan"] = "Tên đăng nhập đã được đăng ký";
                return RedirectToAction("Index");
            }
            else if (db.TaiKhoans.Where(u => u.Email == email).SingleOrDefault() != null)
            {
                Session["KTThemTaiKhoan"] = "Email đã đã được đăng ký";
                return RedirectToAction("Index");
            }
            else if (db.TaiKhoans.Where(u => u.DienThoai == phone).SingleOrDefault() != null)
            {
                Session["KTThemTaiKhoan"] = "Số diện đã được đăng ký";
                return RedirectToAction("Index");
            }
            else
            {
                TaiKhoan taiKhoan = new TaiKhoan
                {
                    TenDangNhap = fc["username"].ToString(),
                    MatKhau = password.ToString(),
                    TenHienThi = fc["name"].ToString(),
                    Email = fc["email"].ToString(),
                    DienThoai = fc["phone"].ToString(),
                    MaLoaiTaiKhoan = int.Parse(fc["usergroup"]),
                    BiXoa = false
                };
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();

                if (db.SaveChanges() == 0)
                {
                    Session["ThemTaiKhoanThanhCong"] = "";
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }
        public ActionResult Change(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "User");
            }
            ViewBag.User = db.LoaiTaiKhoans.ToList();
            var list = db.TaiKhoans.First(x => x.MaTaiKhoan == ID);
            return View(list);
        }

        public ActionResult ChangeForm(FormCollection fc)
        {

            var id = int.Parse(fc["id"]);
            var p = db.TaiKhoans.First(x => x.MaTaiKhoan == id);
            p.MaLoaiTaiKhoan = int.Parse(fc["usergroup"]);
            db.SaveChanges();
            if(db.SaveChanges() == 0)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Change");
        }
        public ActionResult Delete(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("Index", "User");
            }
            var p = db.TaiKhoans.First(x => x.MaTaiKhoan == ID);
            p.BiXoa = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ListDelete()
        {
            return View(db.TaiKhoans.Where(x => x.BiXoa == true).ToList());
        }
        public ActionResult RestoreUser(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("ListDelete", "User");
            }
            var p = db.TaiKhoans.First(x => x.MaTaiKhoan == ID);
            p.BiXoa = false;
            db.SaveChanges();
            return RedirectToAction("ListDelete", "User");
        }
        public ActionResult DeleteUser(int? ID)
        {
            if (ID.HasValue == false)
            {
                return RedirectToAction("ListDelete", "User");
            }
            var p = db.TaiKhoans.First(x => x.MaTaiKhoan == ID);
            db.TaiKhoans.Remove(p);
            db.SaveChanges();
            return RedirectToAction("ListDelete", "User");
        }
    }
}
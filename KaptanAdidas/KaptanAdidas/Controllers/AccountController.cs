using KaptanAdidas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace KaptanAdidas.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        KaptansporAdidasEntities6 db = new KaptansporAdidasEntities6();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Account");
        }
        [HttpPost]
        public ActionResult Login(Kullanici p)
        {
            var bilgiler = db.Kullanici.FirstOrDefault(x=>x.EMail==p.EMail && x.Sifre==p.Sifre);
            if (bilgiler.State != 1)
            {
                ViewBag.hata = "Kullanıcı Onaylanmamış.";
            }
            else if (bilgiler.State == 2)
            {
                ViewBag.hata = "Kullanıcı Reddedilmiş.";
            }
            else if(bilgiler!=null)
            {
                FormsAuthentication.SetAuthCookie(bilgiler.EMail,false);
                Session["Mail"] = bilgiler.EMail.ToString();
                Session["Ad"] = bilgiler.Ad.ToString();
                Session["Soyad"] = bilgiler.Soyad.ToString();
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewBag.hata = "Kullanici  Adı veya Sifre hatalı";
            }
            return View();
        }

        public ActionResult UserReject(int id)
        {
            Kullanici user = db.Kullanici.Where(s => s.Id == id).FirstOrDefault();
            user.State = 2;//Reddedildi!!!
           
            try
            {
                db.SaveChanges();
                return RedirectToAction("UserListNotAccepted");
            }
            catch (Exception ex)
            {
                return RedirectToAction("UserListNotAccepted");
            }

        }

        public ActionResult UserAccept(int id)
        {
            Kullanici user = db.Kullanici.Where(s => s.Id == id).FirstOrDefault();
            user.State = 1;//Onaylandı!!!
         
            try
            {
                db.SaveChanges();
                return RedirectToAction("UserListNotAccepted");
            }
            catch (Exception ex)
            {
                return RedirectToAction("UserListNotAccepted");
            }   
           
        }

        public ActionResult UserListNotAccepted()
        {
            List<Kullanici> userlist = db.Kullanici.Where(s => s.State == 0 || s.State == 2).ToList();
            return View(userlist);
        }
        [HttpGet]
        public ActionResult SignUp()
        {

            return View();
        }
        [HttpPost]
        public ActionResult SignUp(Kullanici Data)
        {
            //State Değişkeni Kullanıcının Onay Durumudur.  0 ise bekleyen 1 ise Onaylı 2 Reddedilmiş demektir.
            try
            {
                Data.State = 0;
                db.Kullanici.Add(Data);
                db.SaveChanges();
                ViewData["message"] = "Kayıt İşlemi Başarılı";
                return View();
            }
            catch (Exception ex)
            {
                ViewData["message"] = "Kayıt İşlemi Başarısız";
                return View();
            }
          
           
        }
        public ActionResult Register()
        {
            var admins = db.Kullanici.ToList();
          return View(admins);
        }
        [HttpPost]
        public ActionResult Register(Kullanici Data)
        {
            db.Kullanici.Add(Data);
            Data.Rol = "U";
            db.SaveChanges();
            return RedirectToAction("Login","Account");
        }
        public ActionResult Edit(int id)
        {
            var kullanici = db.Kullanici.Where(s => s.Id == id).FirstOrDefault();
            return View(kullanici);
        }
        [HttpPost]
        public ActionResult Edit(int id, Kullanici kullanici)
        {
            var user = db.Kullanici.Where(s => s.Id == id).FirstOrDefault();
            user.KuallaniciAd = kullanici.KuallaniciAd;
            user.Rol = kullanici.Rol;
            user.Sifre = kullanici.Sifre;
            user.SifreTekrar = kullanici.SifreTekrar;
            user.Soyad = kullanici.Soyad;
            db.SaveChanges();
            return RedirectToAction(nameof(Register));
        }
        public ActionResult Delete(int id)
        {
            var kullanici = db.Kullanici.Where(s => s.Id == id).FirstOrDefault();
            return View(kullanici);
        }
        [HttpPost]
        public ActionResult Delete(int id, Kullanici kullanici)
        {
            var user = db.Kullanici.Where(s => s.Id == id).FirstOrDefault();
            db.Kullanici.Remove(user);
            db.SaveChanges();
            return RedirectToAction(nameof(Register));
        }
        public ActionResult Guncelle()
        {
            var kullanicilar = (string)Session["Mail"];
            var degerler = db.Kullanici.FirstOrDefault(x=>x.EMail==kullanicilar);
            return View(degerler);
        }
        [HttpPost]
        public ActionResult Guncelle(Kullanici data)
        {
            var kullanicilar = (string)Session["Mail"];
            var user = db.Kullanici.Where(x=>x.EMail==kullanicilar).FirstOrDefault();
            user.Ad = data.Ad;
            user.Soyad = data.Soyad;
            user.EMail = data.EMail;
            user.KuallaniciAd = data.KuallaniciAd;
            user.Sifre = data.Sifre;
            user.SifreTekrar = data.SifreTekrar;
            db.SaveChanges();
            return RedirectToAction("Index","Home");
        }

    }
}
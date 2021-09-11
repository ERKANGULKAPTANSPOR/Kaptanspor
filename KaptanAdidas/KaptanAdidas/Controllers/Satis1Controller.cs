using KaptanAdidas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
namespace KaptanAdidas.Controllers
{
    public class Satis1Controller : Controller
    {
        // GET: Satis1
        KaptansporAdidasEntities6 db = new KaptansporAdidasEntities6();
        public ActionResult Index(int sayfa = 1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kulanici = db.Kullanici.FirstOrDefault(x => x.EMail == kullaniciadi);
                var model = db.Satislar.Where(x => x.KullaniciId == kulanici.Id).ToList().ToPagedList(sayfa, 5);
                return View(model);
            }
            return HttpNotFound();
        }
        public ActionResult SatinAl(int id)
        {
            var model = db.Sepet.FirstOrDefault(x => x.Id == id);
            return View(model);

        }
        [HttpPost]
         public ActionResult SatinAl2(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var model = db.Sepet.FirstOrDefault(x => x.Id == id);
                    var satis = new Satislar
                    {
                        KullaniciId = model.KullaniciId,
                        UrunId = model.UrunId,
                        Adet = model.UrunId,
                        Fiyat = model.Fiyat,
                        Tarih = model.Tarih,
                    };
                    db.Sepet.Remove(model);
                    db.Satislar.Add(satis);
                    db.SaveChanges();
                    ViewBag.islem = "Satın Alma İşlemi Başarılı Bir Şekilde Gerçekleştirilmiştir ";

                }

            }
            catch (Exception)
            {
                ViewBag.islem = "Satın Alma İşlemi Başarısız";

            }
            return View("islem");
        }
        [HttpPost]
        public ActionResult HepsiniSatınAl(decimal? Tutar)
        {
            var a = Request;
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var kullanici = db.Kullanici.FirstOrDefault(x => x.EMail == kullaniciadi);
                var model = db.Sepet.Where(x => x.KullaniciId == kullanici.Id).ToList();
                var kid = db.Sepet.FirstOrDefault(x => x.KullaniciId == kullanici.Id);
                if (model != null)
                {
                    if (kid == null)
                    {
                        ViewBag.Tutar = "Sepetinizde Ürün Bulunmamaktadır";
                    }
                    else if (kid != null)
                    {
                        Tutar = db.Sepet.Where(x => x.KullaniciId == kid.KullaniciId).Sum(x => x.Urun.Fiyat * x.Adet);
                        ViewBag.Tutar = "Toplam Tutar=" + Tutar + "TL";
                    }
                    return View(model);
                }
                return View();
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult HepsiniSatınAl2()
        {
            var username = User.Identity.Name;
            var kullanici = db.Kullanici.FirstOrDefault(x => x.EMail == username);
            var model = db.Sepet.Where(x => x.KullaniciId == kullanici.Id).ToList();
            int satir = 0;
            foreach (var item in model)
            {
                var satis = new Satislar
                {
                    KullaniciId = model[satir].KullaniciId,
                    UrunId = model[satir].UrunId,
                    Adet = model[satir].Adet,
                    Fiyat = model[satir].Fiyat,
                    Resim = model[satir].Resim,
                    Tarih = DateTime.Now
                };
                db.Satislar.Add(satis);
                db.SaveChanges();
                satir++;
            }
            db.Sepet.RemoveRange(model);
            db.SaveChanges();
            return RedirectToAction("Index", "Sepet");
        }

    }

}

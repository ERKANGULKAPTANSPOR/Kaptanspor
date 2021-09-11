using KaptanAdidas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KaptanAdidas.Controllers
{
    public class SepetController : Controller
    {
        // GET: Sepet
        KaptansporAdidasEntities6 db = new KaptansporAdidasEntities6();

        public ActionResult Index(decimal?Tutar)
        {
            if(User.Identity.IsAuthenticated)
            {
                var kulaniciadi = User.Identity.Name;
                var kullanici = db.Kullanici.FirstOrDefault(x=>x.EMail==kulaniciadi);
                var model = db.Sepet.Where(x=>x.KullaniciId==kullanici.Id).ToList();
                var kid = db.Sepet.FirstOrDefault(x => x.KullaniciId == kullanici.Id);
                if (model!=null)
                {
                    if (kid == null)
                    {
                        ViewBag.Tutar = "Sepetinizde Ürün Bulunmamaktadır.";

                    }
                    else if(kid!=null)
                    {
                        Tutar = db.Sepet.Where(x => x.KullaniciId == kid.KullaniciId).Sum(x => x.Urun.Fiyat * x.Adet);
                        ViewBag.Tutar = "Toplam Tutar="+Tutar+"TL";
                    }
                    return View(model);
                }
            }
            return HttpNotFound();
        }
        public ActionResult SepeteEkle(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanici.FirstOrDefault(x=>x.EMail==kullaniciadi);
                var u = db.Urun.Find(id);
                var sepet = db.Sepet.FirstOrDefault(x=>x.KullaniciId==model.Id && x.UrunId==id);
                if (model!=null)
                {
                    if (sepet!=null)
                    {
                        sepet.Adet++;
                        sepet.Fiyat = u.Fiyat * sepet.Adet;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    var s = new Sepet
                    {
                        KullaniciId = model.Id,
                        UrunId = u.Id,
                        Adet = 1,
                        Fiyat = u.Fiyat,
                        Tarih = DateTime.Now

                    };
                    db.Sepet.Add(s);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
            return HttpNotFound();
        }
        public ActionResult SepetCount(int?count)
        {
            if (User.Identity.IsAuthenticated)
            {
                var model = db.Kullanici.FirstOrDefault(x => x.EMail == User.Identity.Name);
                count = db.Sepet.Where(x => x.KullaniciId == model.Id).Count();
                ViewBag.count = count;
                if (count == 0)
                {
                    ViewBag.count = "";
                }
                return PartialView();
            }
            return HttpNotFound();
        }
        public ActionResult arttir(int id)
        {
            var model = db.Sepet.Find(id);
            model.Adet++;
            model.Fiyat = model.Fiyat * model.Adet;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult azalt(int id)
        {
            var model = db.Sepet.Find();
            if (model.Adet==1)
            {
                db.Sepet.Remove(model);
                db.SaveChanges();
            }
            model.Adet--;
            model.Fiyat = model.Fiyat * model.Adet;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void AdetYaz(int id,int miktar)
        {
            var model = db.Sepet.Find(id);
            model.Adet = miktar;
            model.Fiyat = model.Fiyat * model.Adet;
            db.SaveChanges();
        }
        public ActionResult Sil(int id)
        {
            var sil = db.Sepet.Find(id);
            db.Sepet.Remove(sil);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult HepsiniSil()
        {
            if (User.Identity.IsAuthenticated)
            {
                var kullaniciadi = User.Identity.Name;
                var model = db.Kullanici.FirstOrDefault(x=>x.EMail==kullaniciadi);
                var sil = db.Sepet.Where(x => x.KullaniciId == model.Id);
                db.Sepet.RemoveRange(sil);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return HttpNotFound();
        }


    }
}
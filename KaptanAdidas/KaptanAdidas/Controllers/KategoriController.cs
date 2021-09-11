using KaptanAdidas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KaptanAdidas.Controllers
{
    public class KategoriController : Controller
    {
        // GET: Kategori
        KaptansporAdidasEntities6 db = new KaptansporAdidasEntities6();
        public ActionResult Index()
        {
            return View(db.Kategori.Where(x=>x.Durum==true).ToList());
        }
        [Authorize(Roles = "A")]
        public ActionResult Ekle()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Ekle(Kategori data)
        {
            db.Kategori.Add(data);
            data.Durum = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            var urun = db.Urun.Where(x => x.Id == id).FirstOrDefault();
            db.Urun.Remove(urun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Guncelle(Kategori Data)
        {
            var guncelle = db.Kategori.Where(x => x.Id == Data.Id).FirstOrDefault();
            guncelle.Aciklama = Data.Aciklama;
            guncelle.Ad = Data.Ad;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
};
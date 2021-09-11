using KaptanAdidas.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KaptanAdidas.Controllers
{
    public class MusteriController : Controller
    {
        Soap Soap;
        public MusteriController()
        {
            Soap = new Soap();
        }
        // GET: Musteri
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DegisenMusteriGetir()
        {
            // son 15 gün getirilecek
            ViewBag.Musteriler = Soap.DegisenMusteriGetir();
            ViewBag.Date = DateTime.Now.AddDays(-15);
            return View();
        }
        [HttpPost]
        public ActionResult DegisenMusteriGetir(string date)
        {
            // son 15 gün getirilecek
            ViewBag.Musteriler = Soap.DegisenMusteriGetir(date);
            ViewBag.Date = Convert.ToDateTime(date);
            return View();
        }
        public ActionResult MusteriEkle()
        {
            MusteriAraNesnesi musteri = new MusteriAraNesnesi();
            return View(musteri);
        }
        [HttpPost]
        public ActionResult MusteriEkle(MusteriAraNesnesi musteri)
        {
            var result = Soap.MusteriKaydet(musteri);
            return RedirectToAction(nameof(DegisenMusteriGetir));
        }

    }
}
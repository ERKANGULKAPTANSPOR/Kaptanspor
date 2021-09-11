using BarcodeLib;
using KaptanAdidas.Models;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;



namespace KaptanAdidas.Controllers
{
    public class UrunController : Controller
    {
        // GET: Urun
        KaptansporAdidasEntities6 db = new KaptansporAdidasEntities6();
        Soap soap;
        public UrunController()
        {
            soap = new Soap();
        }
        [Authorize]
        public ActionResult Index(string ara, int sayfa = 1)
        {
            var list = db.Urun.ToList().ToPagedList(sayfa, 100);
            if (!string.IsNullOrEmpty(ara))
            {
                var araobj2 = db.Urun.Where(s => s.Barkod != null && s.Ad != null && s.Aciklama != null && s.StokKodu != null)
                    .Where(x => x.Ad.Contains(ara) || x.Aciklama.Contains(ara) || x.Barkod == ara || x.StokKodu == ara).ToList();
                list = araobj2.ToPagedList(1, 1);
            }
            return View(list);
        }
        [HttpGet]
        public ActionResult BarkodYaz(string id)
        {
            Image image = null;

            BarcodeLib.Barcode b = new BarcodeLib.Barcode();
            string result = string.Empty;
            try
            {
              
                b.BackColor = System.Drawing.Color.White;//图片背景颜色
                b.ForeColor = System.Drawing.Color.Black;//条码颜色
                b.IncludeLabel = true;
                b.Alignment = BarcodeLib.AlignmentPositions.LEFT;
                b.LabelPosition = BarcodeLib.LabelPositions.BOTTOMCENTER;//code的显示位置
                b.ImageFormat = System.Drawing.Imaging.ImageFormat.Jpeg;//图片格式
                System.Drawing.Font font = new System.Drawing.Font("verdana", 10f);//字体设置
                b.LabelFont = font;
                b.Height = 120;//图片高度设置(px单位)
                b.Width = 290;//图片宽度设置(px单位)

                var url =Path.Combine("C:\\Users\\Web"+" "+"Tasarım\\Desktop\\KaptanAdidas\\KaptanAdidas", @"barcodes");

                image = b.Encode(TYPE.EAN13, id);//生成图片
                image.Save(url+"\\"+id+".jpeg", System.Drawing.Imaging.ImageFormat.Jpeg);
              
              
                var response = new { success = true, image = url + "\\" + id + ".jpeg" };
                 
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception err)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }


            //Barcode barcodeAPI = new Barcode();

            //// Define basic settings of the image
            //int imageWidth = 290;
            //int imageHeight = 120;
            //Color foreColor = Color.Black;
            //Color backColor = Color.Transparent;
            //string data = "978020137962";

            //// Generate the barcode with your settings
            //Image barcodeImage = barcodeAPI.Encode(TYPE.EAN13, data, foreColor, backColor, imageWidth, imageHeight);
            //try
            //{
            //    barcodeImage.Save("d:\\BarCodeImage.png", System.Drawing.Imaging.ImageFormat.Png);
            //}
            //catch (Exception ex)
            //{

            //}

        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }

        //public static Byte[] BitmapToBytes(Bitmap img)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        //        return stream.ToArray();
        //    }
        //}

        [Authorize(Roles = "A")]
        public ActionResult Ekle()
        {
            List<SelectListItem> deger1 = (from x in db.Kategori.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Ad,
                                               Value = x.Id.ToString()

                                           }).ToList();

            ViewBag.ktgr = deger1;
            return View();
        }
        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Ekle(Urun Data, HttpPostedFileBase File)
        {
            string path = Path.Combine("~/Content/Image/" + File.FileName);
            File.SaveAs(Server.MapPath(path));
            Data.Resim = File.FileName.ToString();
            db.Urun.Add(Data);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            Urun urun = db.Urun.Where(x => x.Id == id).FirstOrDefault();
            db.Urun.Remove(urun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "A")]
        public ActionResult Guncelle(int id)
        {
            var guncelle = db.Urun.Where(x => x.Id == id).FirstOrDefault();
            List<SelectListItem> deger1 = (from x in db.Kategori.ToList()
                                           select new SelectListItem
                                           {
                                               Text = x.Ad,
                                               Value = x.Id.ToString()

                                           }).ToList();

            ViewBag.ktgr = deger1;
            return View(guncelle);

        }
        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult Guncelle(int id, Urun model, HttpPostedFileBase File)
        {
            var urun = db.Urun.Find(model.Id);
            if (File == null)
            {
                urun.Ad = model.Ad;
                urun.Aciklama = model.Aciklama;
                urun.Fiyat = model.Fiyat;
                urun.Fiyat = model.Stok;
                urun.KategoriId = model.KategoriId;
                //db.Urun.(urun);
                db.Urun.Attach(urun);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                urun.Resim = File.FileName.ToString();
                urun.Ad = model.Ad;
                urun.Aciklama = model.Aciklama;
                urun.Fiyat = model.Fiyat;
                urun.Fiyat = model.Stok;
                urun.KategoriId = model.KategoriId;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

        }
        [Authorize(Roles = "A")]
        public ActionResult KritikStok()
        {
            var kritik = db.Urun.Where(x => x.Stok <= 50).ToList();
            return View(kritik);

        }
        public PartialViewResult StokCount()
        {
            if (User.Identity.IsAuthenticated)
            {
                var count = db.Urun.Where(x => x.Stok < 50).Count();
                ViewBag.count = count;
                var azalan = db.Urun.Where(x => x.Stok == 50).Count();
                ViewBag.count = azalan;
            }
            return PartialView();
        }
        public ActionResult StokGrafik()
        {
            ArrayList deger1 = new ArrayList();
            ArrayList deger2 = new ArrayList();
            var veriler = db.Urun.ToList();
            veriler.ToList().ForEach(x => deger1.Add(x.Ad));
            veriler.ToList().ForEach(x => deger2.Add(x.Stok));
            var grafik = new Chart(width: 500, height: 500).AddTitle("Ürün Stok Grafiği").AddSeries(chartType: "Column", name: "Ad", xValue: deger1, yValues: deger2);
            return File(grafik.ToWebImage().GetBytes(), "image/jpeg");
        }

        public ActionResult Urunler()
        {
            string stokkodu = "";
            Soap soap = new Soap();
            var a = soap.UrunBazliStokBilgisiGetir(70831, stokkodu);
            ViewBag.ResultObj = a;
            return View();
        }
        [HttpPost]
        public ActionResult Urunler(int stokid = 0)
        {
            string stokkodu = "";
            Soap soap = new Soap();
            var a = soap.UrunBazliStokBilgisiGetir(stokid, stokkodu);
            ViewBag.ResultObj = a;
            return View();
        }
        public ActionResult Urunler2()
        {
            Soap soap = new Soap();
            var myresult = soap.SipariseAcikStokKartlari(DateTime.Now.ToString("yyyy-MM-dd"));
            ViewBag.ResultObj = myresult;
            ViewBag.Date = DateTime.Now.ToString("yyyy-MM-dd");
            return View();
        }
        [HttpPost]
        public ActionResult Urunler2(string date)
        {
            Soap soap = new Soap();
            var myresult = soap.SipariseAcikStokKartlari(date);
            DBExecuter(date);
            ViewBag.ResultObj = myresult;
            ViewBag.Date = date;
            return View();
        }

        public ActionResult Urunler3()
        {
            Soap soap = new Soap();
            var myresult = soap.StokYeriSorgulama();
            ViewBag.ResultObj = myresult;
            return View();
        }
        public ActionResult Urunler4(string stokoid = "70831")
        {
            Soap soap = new Soap();
            var myresult = soap.StokMiktarVeMaliyetGetir(stokoid);
            var myresull2 = soap.UrunBazliStokBilgisiGetir(int.Parse(stokoid));
            var myresull3 = soap.StokFiyatGetir(stokoid);

            ViewBag.ResultObj = myresult;
            ViewBag.ResultObj2 = myresull2;
            ViewBag.ResultObj3 = myresull3;
            return View();
        }
        [HttpPost]
        public ActionResult Urunler4(string stokoid2 = "70831", int post = 1)
        {
            Soap soap = new Soap();
            var myresult = soap.StokMiktarVeMaliyetGetir(stokoid2);
            var myresull2 = soap.UrunBazliStokBilgisiGetir(int.Parse(stokoid2));
            var myresull3 = soap.StokFiyatGetir(stokoid2);
            DBExecuter2(stokoid2);

            ViewBag.ResultObj = myresult;
            ViewBag.ResultObj2 = myresull2;
            ViewBag.ResultObj3 = myresull3;
            return View();
        }
        public ActionResult Urunler5(string stokoid = "70831")
        {
            Soap soap = new Soap();
            var myresult = soap.StokMiktarVeMaliyetGetir(stokoid);
            var myresull2 = soap.UrunBazliStokBilgisiGetir(int.Parse(stokoid));

            ViewBag.ResultObj = myresult;
            ViewBag.ResultObj2 = myresull2;
            return View();
        }
        [HttpPost]
        public ActionResult Urunler5(string stokoid2 = "70831", string barkod = "")
        {
            Soap soap = new Soap();
            var myresult = new object();
            var myresull2 = new object();
            var stokList = new object();
            if (string.IsNullOrEmpty(barkod))
            {
                myresult = soap.StokMiktarVeMaliyetGetir(stokoid2);
                myresull2 = soap.UrunBazliStokBilgisiGetir(int.Parse(stokoid2));
                DBExecuter2(stokoid2);
            }
            else
            {
                stokList = db.Urun.Where(s => s.Barkod == barkod).FirstOrDefault();

            }

            ViewBag.ResultObj = myresult;
            ViewBag.ResultObj2 = myresull2;
            ViewBag.StokList = stokList;
            return View();
        }
        public ActionResult Urunler6()
        {
            ViewBag.Urun = new Urun();
            return View();
        }
        public ActionResult Urunler7()
        {
            var a = soap.GetStokMiktarMaliyetGetir();
            ViewBag.Siparisler = a;
            ViewBag.Date = DateTime.Now.AddDays(-15);
            return View();
        }
        public ActionResult MarkalariGetir()
        {
            var obj = soap.MarkalariGetir();
            ViewBag.Markalar = obj;
            return View();
        }
        [HttpPost]
        public ActionResult Urunler6(string barkod = "")
        {
            ViewBag.Urun = db.Urun.Where(s => s.Barkod == barkod).FirstOrDefault();
            return View();
        }

        public void DBExecuter(string date)
        {
            var stokList = db.Urun.ToList();

            Soap soap = new Soap();
            var list = soap.SipariseAcikStokKartlari(date);



            foreach (var item in list.StokKartlari.ToList())
            {
                Urun urunExist = stokList.Where(s => s.OID == item.OID).FirstOrDefault();
                if (urunExist == null)
                {
                    Urun urun = new Urun();
                    string barkodText = "";
                    if (item.Barkodlar != null && item.Barkodlar.Length > 0)
                    {
                        foreach (var sub in item.Barkodlar)
                        {
                            barkodText += sub.BARKOD + " - ";
                        }
                    }


                    var fiyatlistesi = soap.StokFiyatGetir(item.OID);
                    var mktar_maliyet = soap.StokMiktarVeMaliyetGetir(item.OID);
                    //mktar_maliyet.StokList[0].
                    //", PARA BIRIMI : " + fiyatlistesi.FiyatListesi.Length > 0 ? fiyatlistesi.FiyatListesi[0].PARA_BIRIMI : null
                    db.Urun.Add(new Urun
                    {
                        OID = item.OID,
                        Ad = item.STOK_ADI,
                        Aciklama = "RENK : " + item.RENK_ACIKLAMA + ", Barkodlar :  " + barkodText,
                        Barkod = item.Barkodlar != null && item.Barkodlar.Length > 0 ? item.Barkodlar[0].BARKOD : "BARKODYOK",
                        Fiyat = fiyatlistesi.FiyatListesi.Length > 0 ? decimal.Parse(fiyatlistesi.FiyatListesi[0].FIYAT) : 0,
                        KategoriId = 1,
                        MarkaAdi = item.MARKA_ADI,
                        Miktar = mktar_maliyet.StokList.Length > 0 ? mktar_maliyet.StokList[0].STOK_MIKTARI : 0,
                        Resim = "",
                        Populer = true,
                        Stok = 0,
                        StokAdi = item.STOK_ADI,
                        StokKodu = item.STOK_KODU,
                        Maliyet = mktar_maliyet.StokList.Length > 0 ? mktar_maliyet.StokList[0].MALIYET.ToString() : "0",
                        AcikSiparisMiktari = mktar_maliyet.StokList.Length > 0 ? mktar_maliyet.StokList[0].ACIK_SIPARIS_MIKTARI : 0,
                        SevkEmriMiktari = mktar_maliyet.StokList.Length > 0 ? mktar_maliyet.StokList[0].SEVK_EMRI_MIKTARI : 0
                    });
                    db.SaveChanges();
                }

            }

        }
        public void DBExecuter2(string stokoid2 = "70831")
        {
            var stokList = db.Urun.ToList();

            Soap soap = new Soap();
            var mktar_maliyet = soap.StokMiktarVeMaliyetGetir(stokoid2);
            var myresull2 = soap.UrunBazliStokBilgisiGetir(int.Parse(stokoid2));
            var fiyatlistesi = soap.StokFiyatGetir(stokoid2);
            var myresult4 = soap.UrunBazliStokBilgisiGetir(int.Parse(stokoid2), "");

            if (fiyatlistesi.FiyatListesi.Length > 0)
            {
                Urun urun = stokList.Where(s => s.OID == fiyatlistesi.FiyatListesi[0].HSTK_STOK_OID).FirstOrDefault();

                if (urun == null)
                {
                    decimal maliyet = 0;
                    string anamaliyet = "0";
                    if (mktar_maliyet.StokList.Length > 0)
                    {
                        maliyet = mktar_maliyet.StokList[0].MALIYET;
                        anamaliyet = maliyet.ToString("F2");
                    }
                    CultureInfo culture = new CultureInfo("en-US");
                    db.Urun.Add(new Urun
                    {
                        OID = fiyatlistesi.FiyatListesi[0].HSTK_STOK_OID,
                        Ad = myresull2.StokAdi,
                        Aciklama = "AÇIKLAMASIZ",
                        Barkod = mktar_maliyet.StokList.Length > 0 ? mktar_maliyet.StokList[0].BARKOD : "YOK",
                        Fiyat = fiyatlistesi.FiyatListesi.Length > 0 ? decimal.Parse(fiyatlistesi.FiyatListesi[0].FIYAT) : 0,
                        KategoriId = 1,
                        MarkaAdi = myresull2.MarkaAdi,
                        Miktar = mktar_maliyet.StokList.Length > 0 ? mktar_maliyet.StokList[0].STOK_MIKTARI : 0,
                        Resim = "",
                        Populer = true,
                        Stok = 0,
                        StokAdi = myresull2.StokAdi,
                        StokKodu = myresull2.StokKodu,
                        Maliyet = anamaliyet,
                        AcikSiparisMiktari = mktar_maliyet.StokList.Length > 0 ? mktar_maliyet.StokList[0].ACIK_SIPARIS_MIKTARI : 0,
                        SevkEmriMiktari = mktar_maliyet.StokList.Length > 0 ? mktar_maliyet.StokList[0].SEVK_EMRI_MIKTARI : 0
                    });
                    db.SaveChanges();
                }
            }


        }
    }
}
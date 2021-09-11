using KaptanAdidas.Objects;
using KaptanAdidas.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KaptanAdidas
{
    public class Soap
    {
        Kullanici Kullanici;
        public Soap()
        {
            //TEST KULLANICISI
            //Kullanici = new Kullanici
            //{
            //    KullaniciKodu = "erkan",
            //    Parola = "erkan"
            //};
            //CANLI KULLANICI
            Kullanici = new Kullanici
            {
                KullaniciKodu = "erkan",
                Parola = "erkan"
            };
        }
        HititR5PSMusteriSiparisSoapClient Client = new HititR5PSMusteriSiparisSoapClient();

        public StokBilgisiGetirResult UrunBazliStokBilgisiGetir(int sistemstokno = 0, string stokKodu = "F36219")
        {
            StokBilgisiGetirResult result = Client.StokBilgisiGetir(sistemstokno, stokKodu, Kullanici);
            return result;
        }

        public SipariseAcikStokKartiGetirResult SipariseAcikStokKartlari(string date)
        {
            SipariseAcikStokKartiGetirArgs args = new SipariseAcikStokKartiGetirArgs();
            args.BuZamandanItibarenGetirilsin = date + "T00:00:00";//"2021-05-30T00:00:00";
            SipariseAcikStokKartiGetirResult result = Client.SipariseAcikStoklariGetir(args, Kullanici);
            return result;
        }

        public SipariseAcikStokFiyatlariniGetirResult StokYeriSorgulama(string stokoid = "70831")
        {
            // buarada hitit uygulaması stok bazlı olarak sorgulama yapmaktadır.
            HititR5PSMusteriSiparisSoapClient Client = new HititR5PSMusteriSiparisSoapClient();
            SipariseAcikStokFiyatlariniGetirArgs args = new SipariseAcikStokFiyatlariniGetirArgs
            {
                StokOid = int.Parse(stokoid),
                //MarkaOid 
            };
            SipariseAcikStokFiyatlariniGetirResult result = Client.SipariseAcikStokFiyatlariniGetir(args, Kullanici);
            return result;
        }
        public StokMiktarMaliyetGetirResult StokMiktarVeMaliyetGetir(string stokoid = "70831")
        {
            // buarada hitit uygulaması stok bazlı olarak miktar ve maliyet bilgilerini getirmektedir.
            HititR5PSMusteriSiparisSoapClient Client = new HititR5PSMusteriSiparisSoapClient();
            StokMikPrm param = new StokMikPrm
            {
                StokOid = int.Parse(stokoid),

            };
            StokMiktarMaliyetGetirResult result = Client.StokMiktarMaliyetGetir(param, Kullanici);
            return result;
        }
        public SipariseAcikStokFiyatlariniGetirResult StokFiyatGetir(string stokoid = "70831")
        {
            HititR5PSMusteriSiparisSoapClient Client = new HititR5PSMusteriSiparisSoapClient();
            SipariseAcikStokFiyatlariniGetirArgs args = new SipariseAcikStokFiyatlariniGetirArgs
            {
                StokOid = int.Parse(stokoid)
            };
            SipariseAcikStokFiyatlariniGetirResult result = Client.SipariseAcikStokFiyatlariniGetir(args, Kullanici);
            return result;
        }
        public SipariseAcikStokMiktarMaliyetAcikSiparisGetirResult GetStokMiktarMaliyetGetir()
        {
            HititR5PSMusteriSiparisSoapClient Client = new HititR5PSMusteriSiparisSoapClient();
            SipariseAcikStokMiktarMaliyetAcikSiparisGetirArgs args = new SipariseAcikStokMiktarMaliyetAcikSiparisGetirArgs
            {
                BuZamandanItibarenGetirilsin = DateTime.Now.AddDays(-1).ToString("yyyy-MM-ddTHH:mm:ss")
            };
            SipariseAcikStokMiktarMaliyetAcikSiparisGetirResult result = Client.SipariseAcikStokMiktarMaliyetGetir(args, Kullanici);
            return result;
        }
        public DegisenMusteriSorgulaResult DegisenMusteriGetir(string date = "")
        {
            DateTime newDate;
            if (!string.IsNullOrEmpty(date))
            {
                newDate = Convert.ToDateTime(date + "T00:00:00");
            }
            else
            {
                newDate = DateTime.Now.AddDays(-15);
            }
            HititR5PSMusteriSiparisSoapClient Client = new HititR5PSMusteriSiparisSoapClient();
            DegisenMusteriSorgulaResult a = Client.DegisenMusteriSorgula(Kullanici, newDate);
            return a;
        }
        public MusteriKaydetResult MusteriKaydet(MusteriAraNesnesi musteri)
        {
            HititR5PSMusteriSiparisSoapClient Client = new HititR5PSMusteriSiparisSoapClient();
            MusteriKaydetResult result = Client.MusteriEkle(new Musteri
            {
                ADI = musteri.ADI,
                SOYADI = musteri.SOYADI,
                ADRES1 = musteri.ADRES1,
                EPOSTA = musteri.EPOSTA,
                TELEFON1 = musteri.TELEFON1,
                CINSIYETI = 0,
                ILCE_SEMT = musteri.ILCE_SEMT,
                MEDENI_DURUMU = 0,
                UNVAN1 = musteri.UNVAN1
            }, Kullanici);
            return result;
        }
        public MusteriSorgulaResult MusteriSorgula(string cepKartNo = "", string epostaAdresi = "")
        {
            HititR5PSMusteriSiparisSoapClient Client = new HititR5PSMusteriSiparisSoapClient();
            MusteriSorgulaResult result = Client.MusteriSorgula(Kullanici, cepKartNo, epostaAdresi);
            return result;
        }
        //75127
        public StokDepoAdetBilgisiGetirResult StokDepoAdetBilgisiGetirResult()
        {
            HititR5PSMusteriSiparisSoapClient Client = new HititR5PSMusteriSiparisSoapClient();
            StokDepoAdetBilgisiGetirResult result = Client.StokDepoAdetBilgisiGetir("Marka", "SBS007", "ALTSTOKKODU", 1, Kullanici);
            return new StokDepoAdetBilgisiGetirResult{ };
        }
        public SipariseAcikMarkalariGetirResult MarkalariGetir()
        {
            HititR5PSMusteriSiparisSoapClient Client = new HititR5PSMusteriSiparisSoapClient();
            SipariseAcikMarkalariGetirArgs args = new SipariseAcikMarkalariGetirArgs {
                
            };
            SipariseAcikMarkalariGetirResult result = Client.SipariseAcikMarkalariGetir(args, Kullanici);
            return result;
        }
        private void Hititislemler4()
        {
           
        }
        private void Hititislemler5()
        {
            HititR5PSMusteriSiparisSoapClient Client = new HititR5PSMusteriSiparisSoapClient();
            //Client.SiparisNodanSiparisSistemNoBul();
        }
    }
}
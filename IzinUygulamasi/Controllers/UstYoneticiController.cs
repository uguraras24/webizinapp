using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IzinUygulamasi.Models;

namespace IzinUygulamasi.Controllers
{
    [AdminAuthorize]
    public class UstYoneticiController : Controller
    {
        private izinApp2Entities db = new izinApp2Entities();
        // GET: UstYonetici
        public ActionResult Index()
        {
            ViewBag.personelSayisi = db.personeller.Count(m => m.p_aktiflik == 1);
            ViewBag.altYoneticiSayisi = db.altYoneticiler.Count(m => m.a_aktiflik == 1);
            ViewBag.onayBekleyenIzin = db.izinler.Count(m => m.izin_durumu == 1);

            var izinData = (from iz in db.izinler
                            join p in db.personeller on iz.personel_id equals p.p_id
                            join ay in db.altYoneticiler on p.altYoneticiId equals ay.a_id
                            join it in db.izinTurleri on iz.izin_turu equals it.id
                            where iz.izin_durumu == 1
                            select new Models.Custom.OnayIzinler
                            {
                                izin_id = iz.izin_id,
                                baslangic_tarihi = iz.baslangic_tarihi.Value,
                                bitis_tarihi = iz.bitis_tarihi.Value,
                                izin_turu = it.izin_adi,
                                izin_aciklamasi = iz.izin_aciklamasi,
                                adi_soyadi = p.p_adi_soyadi
                            }).Take(5).ToList();
            return View(izinData);
        }

        public ActionResult OnayBekleyenIzinler()
        {
            var izinData = (from iz in db.izinler
                            join p in db.personeller on iz.personel_id equals p.p_id
                            join ay in db.altYoneticiler on p.altYoneticiId equals ay.a_id
                            join it in db.izinTurleri on iz.izin_turu equals it.id
                            where iz.izin_durumu == 1
                            select new Models.Custom.OnayIzinler
                            {
                                izin_id = iz.izin_id,
                                baslangic_tarihi = iz.baslangic_tarihi.Value,
                                bitis_tarihi = iz.bitis_tarihi.Value,
                                izin_turu = it.izin_adi,
                                izin_aciklamasi = iz.izin_aciklamasi,
                                adi_soyadi = p.p_adi_soyadi
                            }).ToList();
            return View(izinData);
        }

        public ActionResult IzinOnayla(int izinId)
        {
            izinler izinDuzenle = db.izinler.Find(izinId);
            izinDuzenle.izin_durumu = 4;
            db.SaveChanges();
            return Json(new { result = 1, message = "Başarılı." });
        }

        [HttpPost]
        public ActionResult IzinReddet(int izinId)
        {
            izinler izinDuzenle = db.izinler.Find(izinId);
            izinDuzenle.izin_durumu = 5;
            db.SaveChanges();
            return Json(new { result = 1, message = "Başarılı." });
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Admin", "Login");
        }

    }
}
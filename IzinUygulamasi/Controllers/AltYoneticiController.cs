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
    [YoneticiAuthorize]
    public class AltYoneticiController : Controller
    {
        private izinApp2Entities db = new izinApp2Entities();
        // GET: AltYonetici
        public ActionResult Index()
        {
            string kullaniciAdi = System.Web.HttpContext.Current.User.Identity.Name;
            int yoneticiId = db.altYoneticiler.Where(m => m.a_kullanici_adi == kullaniciAdi)
                                      .Select(m => m.a_id)
                                      .SingleOrDefault();
            int isGunSayisi = int.Parse(db.altYoneticiler.Where(m => m.a_kullanici_adi == kullaniciAdi).Select(p => EntityFunctions.DiffDays(p.a_ise_giris_tarihi, DateTime.Now).Value.ToString()).SingleOrDefault());
            int isGunSayisiSayac = isGunSayisi;
            int toplamIzinGunSayisi = 0;
            int kullanilanIzinGunSayisi = 0;

            while (isGunSayisiSayac >= 365)
            {
                if (isGunSayisi >= 365 && isGunSayisi < 365 * 5) // 1-5 yıl arası ise 14 günlük izin hakkı
                    toplamIzinGunSayisi += 14;
                else if (isGunSayisi >= 365 * 5 && isGunSayisi < 365 * 15) // 5-15 yıl arası ise 20 günlük izin hakkı
                    toplamIzinGunSayisi += 20;
                else // 15 yıl veya daha fazla ise 26 günlük izin hakkı
                    toplamIzinGunSayisi += 26;

                isGunSayisiSayac -= 365;
            }
            var izinDatalar = db.izinler.Where(m => m.alt_yonetici_id == yoneticiId).OrderBy(m => m.izin_id).ToList();
            foreach (var item in izinDatalar)
            {
                kullanilanIzinGunSayisi += (int)(item.bitis_tarihi.Value - item.baslangic_tarihi.Value).TotalDays;
            }


            ViewBag.kalanIzinGunSayisi = toplamIzinGunSayisi - kullanilanIzinGunSayisi;
            ViewBag.toplamKullanilanIzinGunSayisi = kullanilanIzinGunSayisi;
            ViewBag.enSonIzinTarihi = db.izinler.OrderBy(m => m.izin_id)
                                      .Take(1)
                                      .Select(m => m.olusturulma_tarihi)
                                      .SingleOrDefault();
            return View(db.izinler.OrderByDescending(m => m.izin_id).Where(m => m.alt_yonetici_id == yoneticiId).Take(5).ToList());
        }

        public ActionResult OnayBekleyenIzinler()
        {
            var izinData = (from iz in db.izinler
                              join p in db.personeller on iz.personel_id equals p.p_id
                              join ay in db.altYoneticiler on p.altYoneticiId equals ay.a_id
                              join it in db.izinTurleri on iz.izin_turu equals it.id
                              where iz.izin_durumu == 0
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

        [HttpPost]
        public ActionResult IzinOnayla(int izinId)
        {
            izinler izinDuzenle = db.izinler.Find(izinId);
            izinDuzenle.izin_durumu = 1;
            db.SaveChanges();
            return Json(new { result = 1, message = "Başarılı." });
        }

        [HttpPost]
        public ActionResult IzinReddet(int izinId)
        {
            izinler izinDuzenle = db.izinler.Find(izinId);
            izinDuzenle.izin_durumu = 2;
            db.SaveChanges();
            return Json(new { result = 1, message = "Başarılı." });
        }

        public ActionResult IzinAl()
        {
            var turList = db.izinTurleri.Where(m => m.izin_aktiflik == 1).ToList();
            ViewBag.turList = new SelectList(turList, "id", "izin_adi");

            ViewBag.UstYonetici = db.sysAdmin.Select(m => m.adi_soyadi).SingleOrDefault();

            ViewBag.SysAdmin = db.sysAdmin.Select(m => m.adi_soyadi).SingleOrDefault();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IzinAl([Bind(Include = "izin_id,personel_id,baslangic_tarihi,bitis_tarihi,izin_turu,izin_aciklamasi,olusturulma_tarihi")] izinler izinler)
        {
            if (ModelState.IsValid)
            {
                db.izinler.Add(izinler);
                db.SaveChanges();
                TempData["IzinAl"] = "basari";
                return RedirectToAction("IzinAl");

            }
            else
            {
                TempData["IzinAl"] = "hata";
                return View();
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("AltYonetici", "Login");
        }
    }
}
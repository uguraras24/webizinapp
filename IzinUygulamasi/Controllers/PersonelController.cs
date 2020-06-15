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
    [PersonelAuthorize]
    public class PersonelController : Controller
    {
        private izinApp2Entities db = new izinApp2Entities();

        // GET: personeller
        public ActionResult Index()
        {
            string kullaniciAdi = System.Web.HttpContext.Current.User.Identity.Name;
            var personelData = db.personeller.Where(m => m.p_kullanici_adi == kullaniciAdi)
                                      .Select(m => new {m.p_id,m.altYoneticiId})
                                      .ToList();
            int personelId = (int)personelData[0].p_id;
            int altYoneticiId = (int)personelData[0].altYoneticiId;
            int isGunSayisi = int.Parse(db.personeller.Where(m => m.p_kullanici_adi == kullaniciAdi).Select(p => EntityFunctions.DiffDays(p.p_ise_giris_tarihi,DateTime.Now).Value.ToString()).SingleOrDefault());
            int isGunSayisiSayac = isGunSayisi;
            int toplamIzinGunSayisi = 0;
            int kullanilanIzinGunSayisi = 0;

            while(isGunSayisiSayac>=365)
            {
                if (isGunSayisi >= 365 && isGunSayisi < 365 * 5) // 1-5 yıl arası ise 14 günlük izin hakkı
                    toplamIzinGunSayisi += 14;
                else if (isGunSayisi >= 365 * 5 && isGunSayisi < 365 * 15) // 5-15 yıl arası ise 20 günlük izin hakkı
                    toplamIzinGunSayisi += 20;
                else // 15 yıl veya daha fazla ise 26 günlük izin hakkı
                    toplamIzinGunSayisi += 26;

                isGunSayisiSayac -= 365;
            }
            var izinDatalar = db.izinler.Where(m => m.personel_id == personelId).OrderBy(m => m.izin_id).ToList();
            foreach(var item in izinDatalar)
            {
                kullanilanIzinGunSayisi += (int)(item.bitis_tarihi.Value - item.baslangic_tarihi.Value).TotalDays;
            }

            ViewBag.kalanIzinGunSayisi = toplamIzinGunSayisi-kullanilanIzinGunSayisi;
            ViewBag.toplamKullanilanIzinGunSayisi = kullanilanIzinGunSayisi;
            ViewBag.enSonIzinTarihi = db.izinler.OrderBy(m => m.izin_id)
                                      .Take(1)
                                      .Select(m => m.olusturulma_tarihi)
                                      .SingleOrDefault();
            return View(db.izinler.OrderByDescending(m => m.izin_id).Where(m => m.personel_id == personelId).Take(5).ToList());
        }

        public ActionResult TumIzinlerim()
        {
            string kullaniciAdi = System.Web.HttpContext.Current.User.Identity.Name;
            int personelId = db.personeller.Where(m => m.p_kullanici_adi == kullaniciAdi)
                                      .Select(m => m.p_id)
                                      .SingleOrDefault();
            return View(db.izinler.OrderByDescending(m => m.izin_id).Where(m => m.personel_id == personelId).ToList());
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult IzinAl()
        {
            var turList = db.izinTurleri.Where(m => m.izin_aktiflik == 1).ToList();
            ViewBag.turList = new SelectList(turList, "id", "izin_adi");

            string kullaniciAdi = System.Web.HttpContext.Current.User.Identity.Name;
            var personelData = db.personeller.Where(m => m.p_kullanici_adi == kullaniciAdi)
                                      .Select(m => new { m.altYoneticiId })
                                      .ToList();
            int altYoneticiId = (int)personelData[0].altYoneticiId;

            ViewBag.AltYonetici = db.altYoneticiler.Where(m => m.a_id == altYoneticiId)
                                      .Select(m => m.a_adi_soyadi)
                                      .SingleOrDefault();

            ViewBag.SysAdmin = db.sysAdmin.Select(m => m.adi_soyadi).SingleOrDefault();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IzinAl([Bind(Include = "izin_id,personel_id,baslangic_tarihi,bitis_tarihi,izin_turu,izin_aciklamasi,olusturulma_tarihi")] izinler izinler)
        {
            if (ModelState.IsValid)
            {
                string kullaniciAdi = System.Web.HttpContext.Current.User.Identity.Name;
                izinler.personel_id = db.personeller.Where(m => m.p_kullanici_adi == kullaniciAdi)
                                      .Select(m => m.p_id)
                                      .SingleOrDefault();
                izinler.izin_durumu = 0;
                izinler.olusturulma_tarihi = DateTime.Now;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

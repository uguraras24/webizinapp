using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using IzinUygulamasi.Models;

namespace IzinUygulamasi.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AltYonetici()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }
        public ActionResult PersonelGiris(personeller personelFormu)
        {
            using (izinApp2Entities db = new izinApp2Entities())
            {
                var kullaniciKontrol = db.personeller.FirstOrDefault(
                    x => x.p_kullanici_adi == personelFormu.p_kullanici_adi && x.p_kullanici_sifre == personelFormu.p_kullanici_sifre
                    );

                if (kullaniciKontrol != null)
                {
                    FormsAuthentication.SetAuthCookie(personelFormu.p_kullanici_adi,false);
                    Session["PersonelGiris"] = "basari";
                    TempData["PersonelGiris"] = "basari";
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    Session["PersonelGiris"] = "hata";
                    TempData["PersonelGiris"] = "hata";
                    return RedirectToAction("Index", "Login");
                }

            }
        }

        public ActionResult AYGiris(altYoneticiler altYonetici)
        {
            using (izinApp2Entities db = new izinApp2Entities())
            {
                var kullaniciKontrol = db.altYoneticiler.FirstOrDefault(
                    x => x.a_kullanici_adi == altYonetici.a_kullanici_adi && x.a_kullanici_sifre == altYonetici.a_kullanici_sifre
                    );

                if (kullaniciKontrol != null)
                {
                    FormsAuthentication.SetAuthCookie(altYonetici.a_kullanici_adi, false);
                    Session["AYGiris"] = "basari";
                    TempData["AYGiris"] = "basari";
                    return RedirectToAction("AltYonetici", "Login");
                }
                else
                {
                    Session["AYGiris"] = "hata";
                    TempData["AYGiris"] = "hata";
                    return RedirectToAction("AltYonetici", "Login");
                }

            }
        }

        public ActionResult UYGiris(sysAdmin ustYonetici)
        {
            using (izinApp2Entities db = new izinApp2Entities())
            {
                var kullaniciKontrol = db.sysAdmin.FirstOrDefault(
                    x => x.kullanici_adi == ustYonetici.kullanici_adi && x.kullanici_sifre == ustYonetici.kullanici_sifre
                    );

                if (kullaniciKontrol != null)
                {
                    FormsAuthentication.SetAuthCookie(ustYonetici.kullanici_adi, false);
                    Session["UYGiris"] = "basari";
                    TempData["UYGiris"] = "basari";
                    return RedirectToAction("Admin", "Login");
                }
                else
                {
                    Session["UYGiris"] = "hata";
                    TempData["UYGiris"] = "hata";
                    return RedirectToAction("Admin", "Login");
                }

            }
        }


    }
}
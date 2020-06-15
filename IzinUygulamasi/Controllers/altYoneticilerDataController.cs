using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IzinUygulamasi.Models;

namespace IzinUygulamasi.Controllers
{
    [AdminAuthorize]
    public class altYoneticilerDataController : Controller
    {
        private izinApp2Entities db = new izinApp2Entities();

        // GET: altYoneticilerData
        public ActionResult Index()
        {
            return View(db.altYoneticiler.ToList());
        }

        // GET: altYoneticilerData/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            altYoneticiler altYoneticiler = db.altYoneticiler.Find(id);
            if (altYoneticiler == null)
            {
                return HttpNotFound();
            }
            return View(altYoneticiler);
        }

        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "Aktif", Value = "1" });
            items.Add(new SelectListItem() { Text = "Pasif", Value = "0" });
            ViewBag.aktifList = new SelectList(items, "a_aktiflik", "a_aktifval",null);
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "a_id,a_kullanici_adi,a_kullanici_sifre,a_adi_soyadi,a_email,a_aktiflik,a_ise_giris_tarihi,a_olusturulma_tarihi")] altYoneticiler altYoneticiler)
        {
            if (ModelState.IsValid)
            {
                db.altYoneticiler.Add(altYoneticiler);
                db.SaveChanges();
                TempData["YoneticiEkle"] = "basari";
                return RedirectToAction("Create");
            }

            return View(altYoneticiler);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            altYoneticiler altYoneticiler = db.altYoneticiler.Find(id);
            if (altYoneticiler == null)
            {
                return HttpNotFound();
            }
            return View(altYoneticiler);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "a_id,a_kullanici_adi,a_kullanici_sifre,a_adi_soyadi,a_email,a_aktiflik,a_ise_giris_tarihi,a_olusturulma_tarihi")] altYoneticiler altYoneticiler)
        {
            if (ModelState.IsValid)
            {
                db.Entry(altYoneticiler).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(altYoneticiler);
        }

        public ActionResult Delete(int yoneticiId)
        {
            altYoneticiler aYoneticiler = db.altYoneticiler.Find(yoneticiId);
            db.altYoneticiler.Remove(aYoneticiler);
            db.SaveChanges();
            return Json(new { result = 1, message = "Başarılı." });
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

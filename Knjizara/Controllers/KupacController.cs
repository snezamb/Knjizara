using Knjizara.Models; // dodato da bi se koristilo Kupac
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knjizara.Controllers
{
    [Autorizacija(Roles = "Nezaposleni")]
    public class KupacController : Controller
    {
        // GET: KupacView
        public ActionResult KupacView()
        {
            return View();
        }

        // metoda koja poziva metodu Izbor iz Kupac i kreira listu artikala sa podacima unetim u formu
        [HttpPost]
        public ActionResult KupacView(FormCollection forma)
        {
            if (ModelState.IsValid)
            {
                string[] sifre = forma["Sifra"].Split(',');
                int[] brSifre = new int[sifre.Length];
                string[] kolicine = forma["Kolicina"].Split(',');
                int[] brKolicine = new int[kolicine.Length];

                for (int i = 0; i < sifre.Length; i++)
                {
                    Int32.TryParse(sifre[i].ToString(), out brSifre[i]);
                }

                for (int i = 0; i < kolicine.Length; i++)
                {
                    Int32.TryParse(kolicine[i].ToString(), out brKolicine[i]);
                }

                List<Artikal> kupovina = Kupac.Izbor(brSifre, brKolicine);

                float ukupnaCena = Kupac.UkupnaCenaKreiranRacun(kupovina);

                TempData["Kupovina"] = kupovina;
                TempData["UkupnaCena"] = ukupnaCena;

                return RedirectToAction("KupacView");
            }
            else
            {
                return View("KupacView");
            }
        }
    }
}
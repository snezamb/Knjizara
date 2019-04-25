using Knjizara.Models; // dodato da bi se koristilo StavkaRacun i StavkaRacunZaposleni
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knjizara.Controllers
{
    [Autorizacija(Roles = "Zaposleni")]
    public class ZaposleniStavkaRacunController : Controller
    {
        // GET: ZaposleniStavkaRacunView
        public ActionResult ZaposleniStavkaRacunView()
        {
            IzborArtikla();
            TempData.Keep("SifraRacun");
            return View();
        }

        // GET: StavkaRacunIzmeni
        public ActionResult StavkaRacunIzmeni()
        {
            IzborArtikla();
            TempData.Keep("SifraRacun");

            return View("IzmenaZaposleniStavkaRacunView");
        }

        // metoda koja popunjava dropdownlist podacima iz tabele Artikal
        public void IzborArtikla()
        {
            SelectList sviArtikli = StavkaRacunZaposleni.IzaberiArtikal(Convert.ToInt32(TempData["SifraRacun"]));

            if (sviArtikli != null)
            {
                ViewBag.podaciArtikli = sviArtikli;
            }
        }

        // metoda koja poziva metodu KreirajStavkaRacun iz StavkaRacunZaposleni i kreira novu stavku racuna sa podacima unetim u formu
        [HttpPost]
        public ActionResult StavkaRacunKreiraj(FormCollection forma)
        {
            IzborArtikla();
            TempData.Keep("SifraRacun");

            if (string.IsNullOrEmpty(forma["RedniBroj"]))
            {
                ModelState.AddModelError("RedniBroj", "Obavezan je redni broj stavke racuna");
            }
            if (string.IsNullOrEmpty(forma["Kolicina"]))
            {
                ModelState.AddModelError("Kolicina", "Obavezna je kolicina stavke racuna");
            }

            if (ModelState.IsValid)
            {
                StavkaRacun novaStavkaRacun = new StavkaRacun();

                novaStavkaRacun.Racun = Convert.ToInt32(TempData["SifraRacun"]);
                novaStavkaRacun.RedniBroj = Convert.ToInt32(forma["RedniBroj"]);
                novaStavkaRacun.Artikal = Convert.ToInt32(forma["listaArtikala"]);
                novaStavkaRacun.Kolicina = Convert.ToInt32(forma["Kolicina"]);
                novaStavkaRacun.Cena = 0;

                StavkaRacunZaposleni.KreirajStavkaRacun(novaStavkaRacun);

                TempData["RacunStavka"] = Convert.ToInt32(forma["RedniBroj"]);

                return RedirectToAction("ZaposleniStavkaRacunView");
            }
            else
            {
                return View("ZaposleniStavkaRacunView");
            }
        }

        // metoda koja bira stavku racuna za izmenu iz dropdownlist
        [HttpPost]
        public ActionResult StavkaRacunIzmeni(FormCollection forma)
        {
            TempData["RedniBroj"] = Convert.ToInt32(forma["listaStavkiRacuna"]);

            StavkaRacun izborStavkaRacun = StavkaRacunZaposleni.IzaberiStavka(Convert.ToInt32(TempData["SifraRacun"]), Convert.ToInt32(forma["listaStavkiRacuna"]));
            StavkaRacunZaposleni.ArtikalIzbor(izborStavkaRacun);

            return View("IzmenaZaposleniStavkaRacunView", izborStavkaRacun);
        }

        // metoda koja poziva metodu IzmeniStavkaRacun iz StavkaRacunZaposleni i menja stavku racuna izabranu iz dropdownlist
        [HttpPost]
        public ActionResult IzmeniStavkaRacun(StavkaRacun staraStavkaRacun)
        {
            if (ModelState.IsValid)
            {
                StavkaRacunZaposleni.IzmeniStavkaRacun(staraStavkaRacun);

                RacunZaposleni.UkupnaCenaKreiranRacun(staraStavkaRacun.Racun);

                TempData["SifraRacun"] = staraStavkaRacun.Racun;
                TempData["StavkaRacunIzmena"] = staraStavkaRacun.RedniBroj;

                return RedirectToAction("KreiranRacun", "ZaposleniRacun");
            }
            else
            {
                StavkaRacunZaposleni.ArtikalIzbor(staraStavkaRacun);
                return View("IzmenaZaposleniStavkaRacunView", staraStavkaRacun);
            }
        }
    }
}
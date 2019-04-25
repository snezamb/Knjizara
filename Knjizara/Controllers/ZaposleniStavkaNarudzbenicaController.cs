using Knjizara.Models; // dodato da bi se koristilo StavkaNarudzbenica i StavkaNarudzbenicaZaposleni
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knjizara.Controllers
{
    [Autorizacija(Roles = "Zaposleni")]
    public class ZaposleniStavkaNarudzbenicaController : Controller
    {
        // GET: ZaposleniStavkaNarudzbenicaView
        public ActionResult ZaposleniStavkaNarudzbenicaView()
        {
            IzborArtikla();
            TempData.Keep("SifraNarudzbenica");
            return View();
        }

        // GET: StavkaNarudzbenicaIzmeni
        public ActionResult StavkaNarudzbenicaIzmeni()
        {
            IzborArtikla();
            TempData.Keep("SifraNarudzbenica");

            return View("IzmenaZaposleniStavkaNarudzbenicaView");
        }

        // metoda koja popunjava dropdownlist podacima iz tabele Artikal
        public void IzborArtikla()
        {
            SelectList sviArtikli = StavkaNarudzbenicaZaposleni.IzaberiArtikal(Convert.ToInt32(TempData["SifraNarudzbenica"]));

            if (sviArtikli != null)
            {
                ViewBag.podaciArtikli = sviArtikli;
            }
        }

        // metoda koja poziva metodu KreirajStavkaNarudzbenica iz StavkaNarudzbenicaZaposleni i kreira novu stavku narudzbenice sa podacima unetim u formu
        [HttpPost]
        public ActionResult StavkaNarudzbenicaKreiraj(FormCollection forma)
        {
            IzborArtikla();
            TempData.Keep("SifraNarudzbenica");

            if (string.IsNullOrEmpty(forma["RedniBroj"]))
            {
                ModelState.AddModelError("RedniBroj", "Obavezan je redni broj stavke narudzbenice");
            }
            if (string.IsNullOrEmpty(forma["Kolicina"]))
            {
                ModelState.AddModelError("Kolicina", "Obavezna je kolicina stavke narudzbenice");
            }

            if (ModelState.IsValid)
            {
                StavkaNarudzbenica novaStavkaNarudzbenica = new StavkaNarudzbenica();

                novaStavkaNarudzbenica.Narudzbenica = Convert.ToInt32(TempData["SifraNarudzbenica"]);
                novaStavkaNarudzbenica.RedniBroj = Convert.ToInt32(forma["RedniBroj"]);
                novaStavkaNarudzbenica.Artikal = Convert.ToInt32(forma["listaArtikala"]);
                novaStavkaNarudzbenica.Kolicina = Convert.ToInt32(forma["Kolicina"]);
                novaStavkaNarudzbenica.Cena = 0;

                StavkaNarudzbenicaZaposleni.KreirajStavkaNarudzbenica(novaStavkaNarudzbenica);

                TempData["NarudzbenicaStavka"] = Convert.ToInt32(forma["RedniBroj"]);

                return RedirectToAction("ZaposleniStavkaNarudzbenicaView");
            }
            else
            {
                return View("ZaposleniStavkaNarudzbenicaView");
            }
        }

        // metoda koja bira stavku narudzbenice za izmenu iz dropdownlist
        [HttpPost]
        public ActionResult StavkaNarudzbenicaIzmeni(FormCollection forma)
        {
            TempData["RedniBroj"] = Convert.ToInt32(forma["listaStavkiNarudzbenice"]);

            StavkaNarudzbenica izborStavkaNarudzbenica = StavkaNarudzbenicaZaposleni.IzaberiStavka(Convert.ToInt32(TempData["SifraNarudzbenica"]), Convert.ToInt32(forma["listaStavkiNarudzbenice"]));
            StavkaNarudzbenicaZaposleni.ArtikalIzbor(izborStavkaNarudzbenica);

            return View("IzmenaZaposleniStavkaNarudzbenicaView", izborStavkaNarudzbenica);
        }

        // metoda koja poziva metodu IzmeniStavkaNarudzbenica iz StavkaNarudzbenicaZaposleni i menja stavku narudzbenice izabranu iz dropdownlist
        [HttpPost]
        public ActionResult IzmeniStavkaNarudzbenica(StavkaNarudzbenica staraStavkaNarudzbenica)
        {
            if (ModelState.IsValid)
            {
                StavkaNarudzbenicaZaposleni.IzmeniStavkaNarudzbenica(staraStavkaNarudzbenica);

                NarudzbenicaZaposleni.UkupnaCenaKreiranaNarudzbenica(staraStavkaNarudzbenica.Narudzbenica);

                TempData["SifraNarudzbenica"] = staraStavkaNarudzbenica.Narudzbenica;
                TempData["StavkaNarudzbenicaIzmena"] = staraStavkaNarudzbenica.RedniBroj;

                return RedirectToAction("KreiranaNarudzbenica", "ZaposleniNarudzbenica");
            }
            else
            {
                StavkaNarudzbenicaZaposleni.ArtikalIzbor(staraStavkaNarudzbenica);
                return View("IzmenaZaposleniStavkaNarudzbenicaView", staraStavkaNarudzbenica);
            }
        }
    }
}
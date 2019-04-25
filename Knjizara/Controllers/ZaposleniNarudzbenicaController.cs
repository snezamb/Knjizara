using Knjizara.Models; // dodato da bi se koristilo Narudzbenica i NarudzbenicaZaposleni
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knjizara.Controllers
{
    [Autorizacija(Roles = "Zaposleni")]
    public class ZaposleniNarudzbenicaController : Controller
    {
        // GET: ZaposleniSveNarudzbeniceView
        public ActionResult ZaposleniSveNarudzbeniceView()
        {
            IzborNarudzbenice();

            return View();
        }

        // GET: ZaposleniNarudzbenicaView
        public ActionResult ZaposleniNarudzbenicaView()
        {
            return View();
        }

        // GET: KreiranaNarudzbenica
        public ActionResult KreiranaNarudzbenica()
        {
            IzborNarudzbenice();
            IzborStavkeNarudzbenice();

            return View("PrikazZaposleniNarudzbenicaView");
        }

        // GET: IzmenaZaposleniNarudzbenicaView
        public ActionResult IzmenaZaposleniNarudzbenicaView()
        {
            IzborStavkeNarudzbenice();
            TempData.Keep("SifraNarudzbenica");

            return View();
        }

        // metoda koja popunjava dropdownlist podacima iz tabele Narudzbenica za korisnicko ime
        public void IzborNarudzbenice()
        {
            SelectList sveNarudzbenice = NarudzbenicaZaposleni.IzaberiNarudzbenica(User.Identity.Name);

            if (sveNarudzbenice != null)
            {
                ViewBag.podaciNarudzbenice = sveNarudzbenice;
            }
        }

        // metoda koja popunjava dropdownlist podacima iz tabele StavkaNarudzbenica za sifru narudzbenice
        public void IzborStavkeNarudzbenice()
        {
            SelectList sveStavkeNarudzbenice = NarudzbenicaZaposleni.IzaberiStavkaNarudzbenica(Convert.ToInt32(TempData["SifraNarudzbenica"]));

            if (sveStavkeNarudzbenice != null)
            {
                ViewBag.podaciStavkeNarudzbenice = sveStavkeNarudzbenice;
            }
        }

        // metoda koja ili upucuje na kreiranje narudzbenice ili upucuje na izmenu stavke narudzbenice
        public ActionResult SveNarudzbenice(FormCollection forma)
        {
            switch (forma["dugme"])
            {
                case "Kreirajte novu narudzbenicu":
                    {
                        return RedirectToAction("ZaposleniNarudzbenicaView");
                    }
                case "Izmenite stavke postojece narudzbenice":
                    {
                        return RedirectToAction("KreiranaNarudzbenica");
                    }
                default: return View();
            }
        }

        // metoda koja poziva metodu IzmeniNarudzbenica iz NarudzbenicaZaposleni i menja narudzbenicu sa podacima unetim u formu
        public ActionResult IzmenaNarudzbenica(FormCollection forma)
        {
            IzborNarudzbenice();

            if (string.IsNullOrEmpty(forma["Datum"]))
            {
                ModelState.AddModelError("Datum", "Obavezan je datum narudzbenice");
            }

            if (ModelState.IsValid)
            {
                Narudzbenica izmenaNarudzbenica = new Narudzbenica();

                izmenaNarudzbenica.NarudzbenicaID = Convert.ToInt32(forma["listaNarudzbenica"]);
                izmenaNarudzbenica.Datum = Convert.ToDateTime(forma["Datum"]);
                izmenaNarudzbenica.Zaposleni = User.Identity.Name;

                NarudzbenicaZaposleni.IzmeniNarudzbenica(izmenaNarudzbenica);

                NarudzbenicaZaposleni.UkupnaCenaKreiranaNarudzbenica(Convert.ToInt32(forma["listaNarudzbenica"]));

                TempData["IzmenaNarudzbenica"] = Convert.ToInt32(forma["listaNarudzbenica"]);

                return RedirectToAction("ZaposleniSveNarudzbeniceView");
            }
            else
            {
                return View("ZaposleniSveNarudzbeniceView");
            }
        }

        // metoda koja poziva metodu KreirajNarudzbenica iz NarudzbenicaZaposleni i kreira novu narudzbenicu sa podacima unetim u formu
        [HttpPost]
        public ActionResult NarudzbenicaKreiraj(Narudzbenica novaNarudzbenica)
        {
            if (ModelState.IsValid)
            {
                novaNarudzbenica.Zaposleni = User.Identity.Name;

                NarudzbenicaZaposleni.KreirajNarudzbenica(novaNarudzbenica);

                TempData["SifraNarudzbenica"] = novaNarudzbenica.NarudzbenicaID;

                return RedirectToAction("ZaposleniStavkaNarudzbenicaView", "ZaposleniStavkaNarudzbenica");
            }
            else
            {
                return View("ZaposleniNarudzbenicaView");
            }
        }

        // metoda koja ili prikazuje narudzbenicu izabranu iz dropdownlist ili bira narudzbenicu za izmenu iz dropdownlist
        [HttpPost]
        public ActionResult KreiranaNarudzbenica(FormCollection forma)
        {
            IzborNarudzbenice();
            TempData.Remove("SifraNarudzbenica");

            switch (forma["dugme"])
            {
                case "Prikazite kreiranu narudzbenicu":
                    {
                        TempData["SifraNarudzbenica"] = Convert.ToInt32(forma["listaNarudzbenica"]);

                        NarudzbenicaZaposleni.UkupnaCenaKreiranaNarudzbenica(Convert.ToInt32(forma["listaNarudzbenica"]));

                        return RedirectToAction("KreiranaNarudzbenica");
                    }
                case "Izmenite kreiranu narudzbenicu":
                    {
                        TempData["SifraNarudzbenica"] = Convert.ToInt32(forma["listaNarudzbenica"]);

                        return RedirectToAction("IzmenaZaposleniNarudzbenicaView");
                    }
                default: return View("PrikazZaposleniNarudzbenicaView");
            }
        }
    }
}
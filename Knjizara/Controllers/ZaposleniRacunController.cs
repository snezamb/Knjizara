using Knjizara.Models; // dodato da bi se koristilo Racun i RacunZaposleni
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knjizara.Controllers
{
    [Autorizacija(Roles = "Zaposleni")]
    public class ZaposleniRacunController : Controller
    {
        // GET: ZaposleniSviRacuniView
        public ActionResult ZaposleniSviRacuniView()
        {
            IzborRacuna();

            return View();
        }

        // GET: ZaposleniRacunView
        public ActionResult ZaposleniRacunView()
        {
            return View();
        }

        // GET: KreiranRacun
        public ActionResult KreiranRacun()
        {
            IzborRacuna();
            IzborStavkeRacuna();

            return View("PrikazZaposleniRacunView");
        }

        // GET: IzmenaZaposleniRacunView
        public ActionResult IzmenaZaposleniRacunView()
        {
            IzborStavkeRacuna();
            TempData.Keep("SifraRacun");

            return View();
        }

        // metoda koja popunjava dropdownlist podacima iz tabele Racun za korisnicko ime
        public void IzborRacuna()
        {
            SelectList sviRacuni = RacunZaposleni.IzaberiRacun(User.Identity.Name);

            if (sviRacuni != null)
            {
                ViewBag.podaciRacuni = sviRacuni;
            }
        }

        // metoda koja popunjava dropdownlist podacima iz tabele StavkaRacun za sifru racuna
        public void IzborStavkeRacuna()
        {
            SelectList sveStavkeRacuna = RacunZaposleni.IzaberiStavkaRacun(Convert.ToInt32(TempData["SifraRacun"]));

            if (sveStavkeRacuna != null)
            {
                ViewBag.podaciStavkeRacuna = sveStavkeRacuna;
            }
        }

        // metoda koja ili upucuje na kreiranje racuna ili upucuje na izmenu stavke racuna
        public ActionResult SviRacuni(FormCollection forma)
        {
            switch (forma["dugme"])
            {
                case "Kreirajte nov racun":
                    {
                        return RedirectToAction("ZaposleniRacunView");
                    }
                case "Izmenite stavke postojeceg racuna":
                    {
                        return RedirectToAction("KreiranRacun");
                    }
                default: return View();
            }
        }

        // metoda koja poziva metodu IzmeniRacun iz RacunZaposleni i menja racun sa podacima unetim u formu 
        public ActionResult IzmenaRacun(FormCollection forma)
        {
            IzborRacuna();

            if (string.IsNullOrEmpty(forma["Datum"]))
            {
                ModelState.AddModelError("Datum", "Obavezan je datum racuna");
            }

            if (ModelState.IsValid)
            {
                Racun izmenaRacun = new Racun();

                izmenaRacun.RacunID = Convert.ToInt32(forma["listaRacuna"]);
                izmenaRacun.Datum = Convert.ToDateTime(forma["Datum"]);
                izmenaRacun.Zaposleni = User.Identity.Name;

                RacunZaposleni.IzmeniRacun(izmenaRacun);

                RacunZaposleni.UkupnaCenaKreiranRacun(Convert.ToInt32(forma["listaRacuna"]));

                TempData["IzmenaRacun"] = Convert.ToInt32(forma["listaRacuna"]);

                return RedirectToAction("ZaposleniSviRacuniView");
            }
            else
            {
                return View("ZaposleniSviRacuniView");
            }
        }

        // metoda koja poziva metodu KreirajRacun iz RacunZaposleni i kreira nov racun sa podacima unetim u formu
        [HttpPost]
        public ActionResult RacunKreiraj(Racun novRacun)
        {
            if (ModelState.IsValid)
            {
                novRacun.Zaposleni = User.Identity.Name;

                RacunZaposleni.KreirajRacun(novRacun);

                TempData["SifraRacun"] = novRacun.RacunID;

                return RedirectToAction("ZaposleniStavkaRacunView", "ZaposleniStavkaRacun");
            }
            else
            {
                return View("ZaposleniRacunView");
            }
        }

        // metoda koja ili prikazuje racun izabran iz dropdownlist ili bira racun za izmenu iz dropdownlist
        [HttpPost]
        public ActionResult KreiranRacun(FormCollection forma)
        {
            IzborRacuna();
            TempData.Remove("SifraRacun");

            switch (forma["dugme"])
            {
                case "Prikazite kreiran racun":
                    {
                        TempData["SifraRacun"] = Convert.ToInt32(forma["listaRacuna"]);

                        RacunZaposleni.UkupnaCenaKreiranRacun(Convert.ToInt32(forma["listaRacuna"]));

                        return RedirectToAction("KreiranRacun");
                    }
                case "Izmenite kreiran racun":
                    {
                        TempData["SifraRacun"] = Convert.ToInt32(forma["listaRacuna"]);

                        return RedirectToAction("IzmenaZaposleniRacunView");
                    }
                default: return View("PrikazZaposleniRacunView");
            }
        }
    }
}
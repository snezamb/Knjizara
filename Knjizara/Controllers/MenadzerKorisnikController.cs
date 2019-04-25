using Knjizara.Models; // dodato da bi se koristilo Korisnik i KorisnikMenadzer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knjizara.Controllers
{
    [Autorizacija(Roles = "Menadzer")]
    public class MenadzerKorisnikController : Controller
    {
        // GET: MenadzerKorisnikView
        public ActionResult MenadzerKorisnikView()
        {
            IzborKorisnika();
            return View();
        }

        // metoda koja popunjava dropdownlist podacima iz tabele Korisnik
        public void IzborKorisnika()
        {
            SelectList sviKorisnici = KorisnikMenadzer.IzaberiKorisnika();

            if (sviKorisnici != null)
            {
                ViewBag.podaciKorisnici = sviKorisnici;
            }
        }

        // akciona metoda koja poziva metodu UnesiKorisnika iz KorisnikMenadzer i unosi novog korisnika sa podacima unetim u formu
        [HttpPost]
        public ActionResult KorisnikUnesi(Korisnik novKorisnik)
        {
            IzborKorisnika();

            if (ModelState.IsValid)
            {
                KorisnikMenadzer.UnesiKorisnika(novKorisnik);

                TempData["ImeKorisnika"] = novKorisnik.Ime;
                TempData["PrezimeKorisnika"] = novKorisnik.Prezime;

                return RedirectToAction("MenadzerKorisnikView");
            }
            else
            {
                return View("MenadzerKorisnikView");
            }
        }

        // akciona metoda koja poziva ili metodu BrisiKorisnika ili metodu PrikaziKorisnika iz KorisnikMenadzer i ili brise korisnika ili prikazuje korisnika izabranog iz dropdownlist
        [HttpPost]
        public ActionResult KorisnikBrisiPrikazi(FormCollection forma)
        {
            if (ModelState.IsValid)
            {
                switch (forma["dugme"])
                {
                    case "Izbrisite korisnika":
                        {
                            bool izbrisan = KorisnikMenadzer.BrisiKorisnika(forma["listaKorisnika"]);

                            TempData["KorisnikKorisnickoIme"] = forma["listaKorisnika"];

                            return RedirectToAction("MenadzerKorisnikView");
                        }
                    case "Prikazite korisnika":
                        {
                            Korisnik prikazKorisnik = KorisnikMenadzer.PrikaziKorisnika(forma["listaKorisnika"]);

                            return View("PrikazKorisnikView", prikazKorisnik);
                        }
                    default: return View("MenadzerKorisnikView");
                }
            }

            return RedirectToAction("MenadzerKorisnikView");
        }

        // akciona metoda koja poziva metodu IzmeniKorisnika iz KorisnikMenadzer i menja korisnika izabranog iz dropdownlist
        [HttpPost]
        public ActionResult KorisnikIzmeni(Korisnik starKorisnik)
        {
            if (ModelState.IsValid)
            {
                KorisnikMenadzer.IzmeniKorisnika(starKorisnik);

                TempData["KorisnickoImeIzmena"] = starKorisnik.KorisnickoIme;

                return RedirectToAction("MenadzerKorisnikView");
            }
            else
            {
                return View("PrikazKorisnikView", starKorisnik);
            }
        }
    }
}
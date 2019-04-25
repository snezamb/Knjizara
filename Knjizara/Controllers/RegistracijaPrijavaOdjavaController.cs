using Knjizara.Models; // dodato da bi se koristilo Korisnik
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security; // dodato da bi se koristilo Roles

namespace Knjizara.Controllers
{
    public class RegistracijaPrijavaOdjavaController : Controller
    {
        // GET: Registracija
        public ActionResult Registracija()
        {
            return View();
        }

        // GET: Prijava
        public ActionResult Prijava()
        {
            return View();
        }

        // GET: KnjizaraView
        public ActionResult KnjizaraView()
        {
            return View();
        }

        // akciona metoda koja poziva metodu RegistracijaKorisnika iz RegistracijaPrijava i unosi novog korisnika sa podacima unetim u formu
        [HttpPost]
        public ActionResult Registracija(Korisnik novKorisnik)
        {
            if (ModelState.IsValid)
            {
                RegistracijaPrijavaOdjava.RegistracijaKorisnika(novKorisnik);

                FormsAuthentication.SetAuthCookie(novKorisnik.KorisnickoIme, false);

                return RedirectToAction("KnjizaraView");
            }
            else
            {
                return View("Registracija");
            }
        }

        // akciona metoda koja poziva metodu PrijavaKorisnika iz RegistracijaPrijava i prijavljuje korisnika sa podacima unetim u formu
        [HttpPost]
        public ActionResult Prijava(FormCollection forma)
        {
            if (string.IsNullOrEmpty(forma["KorisnickoIme"]))
            {
                ModelState.AddModelError("KorisnickoIme", "Obavezno je korisnicko ime korisnika");
            }
            if (string.IsNullOrEmpty(forma["Lozinka"]))
            {
                ModelState.AddModelError("Lozinka", "Obavezna je lozinka korisnika");
            }

            if (ModelState.IsValid)
            {
                Korisnik prijavljenKorisnik = new Korisnik();

                prijavljenKorisnik.KorisnickoIme = forma["KorisnickoIme"];
                prijavljenKorisnik.Lozinka = forma["Lozinka"];

                bool prijavljen = RegistracijaPrijavaOdjava.PrijavaKorisnika(prijavljenKorisnik);

                if (prijavljen)
                {
                    FormsAuthentication.SetAuthCookie(forma["KorisnickoIme"], false);
                    return RedirectToAction("KnjizaraView");
                }
                else
                {
                    ModelState.AddModelError("Prijava", "Korisnicko ime i lozinka nisu odgovarajuci");
                    return View("Prijava");
                }
            }
            else
            {
                return View("Prijava");
            }
        }

        // akciona metoda koja odjavljuje korisnika
        public ActionResult Odjava()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
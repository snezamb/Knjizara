using Knjizara.Models; // dodato da bi se koristilo RacunMenadzer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knjizara.Controllers
{
    [Autorizacija(Roles = "Menadzer")]
    public class MenadzerRacunController : Controller
    {
        // GET: MenadzerRacunView
        public ActionResult MenadzerRacunView()
        {
            IzborZaposlenog();

            return View();
        }

        // metoda koja popunjava dropdownlist podacima iz tabele Korisnik
        public void IzborZaposlenog()
        {
            SelectList sviZaposleni = RacunMenadzer.IzaberiZaposleni();

            if (sviZaposleni != null)
            {
                ViewBag.podaciZaposleni = sviZaposleni;
            }
        }

        // metoda koja prikazuje racune zaposlenog izabranog iz dropdownlist
        [HttpPost]
        public ActionResult RacuniZaposlenog(FormCollection forma)
        {
            IzborZaposlenog();

            TempData["zaposleni"] = forma["listaZaposleni"];

            return View("MenadzerRacunView");
        }
    }
}
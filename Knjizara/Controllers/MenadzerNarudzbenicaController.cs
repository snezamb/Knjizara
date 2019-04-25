using Knjizara.Models; // dodato da bi se koristilo NarudzbenicaMenadzer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knjizara.Controllers
{
    [Autorizacija(Roles = "Menadzer")]
    public class MenadzerNarudzbenicaController : Controller
    {
        // GET: MenadzerNarudzbenicaView
        public ActionResult MenadzerNarudzbenicaView()
        {
            IzborZaposlenog();

            return View();
        }

        // metoda koja popunjava dropdownlist podacima iz tabele Korisnik
        public void IzborZaposlenog()
        {
            SelectList sviZaposleni = NarudzbenicaMenadzer.IzaberiZaposleni();

            if (sviZaposleni != null)
            {
                ViewBag.podaciZaposleni = sviZaposleni;
            }
        }

        // metoda koja prikazuje racune zaposlenog izabranog iz dropdownlist
        [HttpPost]
        public ActionResult NarudzbeniceZaposlenog(FormCollection forma)
        {
            IzborZaposlenog();

            TempData["zaposleni"] = forma["listaZaposleni"];

            return View("MenadzerNarudzbenicaView");
        }
    }
}
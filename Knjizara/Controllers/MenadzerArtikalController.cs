using Knjizara.Models; // dodato da bi se koristilo Artikal i ArtikalMenadzer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knjizara.Controllers
{
    [Autorizacija(Roles = "Menadzer")]
    public class MenadzerArtikalController : Controller
    {
        // GET: MenadzerArtikalView
        public ActionResult MenadzerArtikalView()
        {
            IzborDobavljaca();
            IzborArtikla();
            return View();
        }

        // metoda koja popunjava dropdownlist podacima iz tabele Dobavljaci
        public void IzborDobavljaca()
        {
            SelectList sviDobavljaci = ArtikalMenadzer.IzaberiDobavljaca();

            if (sviDobavljaci != null)
            {
                ViewBag.podaciDobavljaci = sviDobavljaci;
            }
        }

        // metoda koja popunjava dropdownlist podacima iz tabele Artikal
        public void IzborArtikla()
        {
            SelectList sviArtikli = ArtikalMenadzer.IzaberiArtikal();

            if (sviArtikli != null)
            {
                ViewBag.podaciArtikli = sviArtikli;
            }
        }

        // akciona metoda koja poziva metodu UnesiArtikal iz ArtikalMenadzer i unosi nov artikal sa podacima unetim u formu
        [HttpPost]
        public ActionResult ArtikalUnesi(FormCollection forma)
        {
            IzborDobavljaca();
            IzborArtikla();

            if (string.IsNullOrEmpty(forma["ArtikalID"]))
            {
                ModelState.AddModelError("ArtikalID", "Obavezna je sifra artikla");
            }
            if (string.IsNullOrEmpty(forma["NazivArtikla"]))
            {
                ModelState.AddModelError("NazivArtikla", "Obavezan je naziv artikla");
            }
            if (string.IsNullOrEmpty(forma["Cena"]))
            {
                ModelState.AddModelError("Cena", "Obavezna je cena artikla");
            }
            if (string.IsNullOrEmpty(forma["Kolicina"]))
            {
                ModelState.AddModelError("Kolicina", "Obavezna je kolicina artikla");
            }

            if (ModelState.IsValid)
            {
                Artikal novArtikal = new Artikal();

                novArtikal.ArtikalID = Convert.ToInt32(forma["ArtikalID"]);
                novArtikal.NazivArtikla = forma["NazivArtikla"];
                novArtikal.Cena = float.Parse(forma["Cena"]);
                novArtikal.Kolicina = Convert.ToInt32(forma["Kolicina"]);
                novArtikal.Dobavljac = Convert.ToInt32(forma["listaDobavljaca"]);
                novArtikal.Aktivan = 1;

                ArtikalMenadzer.UnesiArtikal(novArtikal);

                TempData["SifraArtikla"] = Convert.ToInt32(forma["ArtikalID"]);
                TempData["NazivArtikla"] = forma["NazivArtikla"];

                return RedirectToAction("MenadzerArtikalView");
            }
            else
            {
                return View("MenadzerArtikalView");
            }
        }

        // akciona metoda koja poziva ili metodu BrisiArtikal ili metodu PrikaziArtikal iz ArtikalMenadzer i ili brise artikal ili prikazuje artikal izabran iz dropdownlist
        [HttpPost]
        public ActionResult ArtikalBrisiPrikazi(FormCollection forma)
        {
            if (ModelState.IsValid)
            {
                switch (forma["dugme"])
                {
                    case "Izbrisite artikal":
                        {
                            bool izbrisan = ArtikalMenadzer.BrisiArtikal(Convert.ToInt32(forma["listaArtikala"]));

                            TempData["ArtikalSifra"] = forma["listaArtikala"];

                            return RedirectToAction("MenadzerArtikalView");
                        }
                    case "Prikazite artikal":
                        {
                            Artikal prikazArtikal = ArtikalMenadzer.PrikaziArtikal(Convert.ToInt32(forma["listaArtikala"]));
                            ArtikalMenadzer.DobavljacIzbor(prikazArtikal);

                            return View("PrikazArtikalView", prikazArtikal);
                        }
                    default: return View("MenadzerArtikalView");
                }
            }

            return RedirectToAction("MenadzerArtikalView");
        }

        // akciona metoda koja poziva metodu IzmeniArtikal iz ArtikalMenadzer i menja artikal izabran iz dropdownlist
        [HttpPost]
        public ActionResult ArtikalIzmeni(Artikal starArtikal)
        {
            if (ModelState.IsValid)
            {
                ArtikalMenadzer.IzmeniArtikal(starArtikal);

                TempData["SifraIzmena"] = starArtikal.ArtikalID;

                return RedirectToAction("MenadzerArtikalView");
            }
            else
            {
                ArtikalMenadzer.DobavljacIzbor(starArtikal);
                return View("PrikazArtikalView", starArtikal);
            }
        }
    }
}
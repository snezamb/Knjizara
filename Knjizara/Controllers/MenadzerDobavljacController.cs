using Knjizara.Models; // dodato da bi se koristilo Dobavljaci i DobavljacMenadzer
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knjizara.Controllers
{
    [Autorizacija(Roles = "Menadzer")]
    public class MenadzerDobavljacController : Controller
    {
        // GET: MenadzerDobavljacView
        public ActionResult MenadzerDobavljacView()
        {
            IzborDobavljaca();
            return View();
        }

        // metoda koja popunjava dropdownlist podacima iz tabele Dobavljaci
        public void IzborDobavljaca()
        {
            SelectList sviDobavljaci = DobavljacMenadzer.IzaberiDobavljaca();

            if (sviDobavljaci != null)
            {
                ViewBag.podaciDobavljaci = sviDobavljaci;
            }
        }

        // akciona metoda koja poziva metodu UnesiDobavljaca iz DobavljacMenadzer i unosi novog dobavljaca sa podacima unetim u formu
        [HttpPost]
        public ActionResult DobavljacUnesi(Dobavljaci novDobavljac)
        {
            IzborDobavljaca();

            if (ModelState.IsValid)
            {
                DobavljacMenadzer.UnesiDobavljaca(novDobavljac);

                TempData["SifraDobavljaca"] = novDobavljac.DobavljacID;
                TempData["NazivDobavljaca"] = novDobavljac.NazivDobavljaca;

                return RedirectToAction("MenadzerDobavljacView");
            }
            else
            {
                return View("MenadzerDobavljacView");
            }
        }

        // akciona metoda koja poziva ili metodu BrisiDobavljac ili metodu PrikaziDobavljac iz DobavljacMenadzer i ili brise dobavljaca ili prikazuje dobavljaca izabranog iz dropdownlist
        [HttpPost]
        public ActionResult DobavljacBrisiPrikazi(FormCollection forma)
        {
            if (ModelState.IsValid)
            {
                switch (forma["dugme"])
                {
                    case "Izbrisite dobavljaca":
                        {
                            bool izbrisan = DobavljacMenadzer.BrisiDobavljac(Convert.ToInt32(forma["listaDobavljaca"]));

                            TempData["DobavljacSifra"] = forma["listaDobavljaca"];

                            return RedirectToAction("MenadzerDobavljacView");
                        }
                    case "Prikazite dobavljaca":
                        {
                            Dobavljaci prikazDobavljac = DobavljacMenadzer.PrikaziDobavljac(Convert.ToInt32(forma["listaDobavljaca"]));

                            return View("PrikazDobavljacView", prikazDobavljac);
                        }
                    default: return View("MenadzerDobavljacView");
                }
            }

            return RedirectToAction("MenadzerDobavljacView");
        }

        // akciona metoda koja poziva metodu IzmeniDobavljac iz DobavljacMenadzer i menja dobavljaca izabranog iz dropdownlist
        [HttpPost]
        public ActionResult DobavljacIzmeni(Dobavljaci starDobavljac)
        {
            if (ModelState.IsValid)
            {
                DobavljacMenadzer.IzmeniDobavljac(starDobavljac);

                TempData["SifraIzmena"] = starDobavljac.DobavljacID;

                return RedirectToAction("MenadzerDobavljacView");
            }
            else
            {
                return View("PrikazDobavljacView",starDobavljac);
            }
        }
    }
}
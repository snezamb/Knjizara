using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; // dodato da bi se koristilo SelectList

namespace Knjizara.Models
{
    public class RacunMenadzer
    {
        // izabrana vrednost u metodi IzaberiZaposleni
        public static int IdZaposlenog { get; set; }

        // metoda koja vraca listu racunviewmodel
        public static List<RacunViewModel> PrikaziSveRacune(string zaposleni)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                List<RacunViewModel> racuni = (from r in knjizaraEntitet.Racuns
                                               where r.Zaposleni.Equals(zaposleni)
                                               select new RacunViewModel { RacunID = r.RacunID, Datum = r.Datum, UkupnaCena = r.UkupnaCena, Zaposleni = r.Zaposleni }).ToList();
                return racuni;
            }
        }

        // metoda koja vraca listu stavkaracunviewmodel
        public static List<StavkaRacunViewModel> PrikaziSveStavkeRacuna(int sifra)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                List<StavkaRacunViewModel> stavkeRacuna = (from s in knjizaraEntitet.StavkaRacuns
                                                           where s.Racun == sifra
                                                           select new StavkaRacunViewModel { Racun = s.Racun, RedniBroj = s.RedniBroj, Artikal = s.Artikal1.NazivArtikla, Kolicina = s.Kolicina, Cena = s.Cena }).ToList();
                return stavkeRacuna;
            }
        }

        // metoda koja vraca listu racuna za dropdownlist
        public static SelectList IzaberiZaposleni()
        {
            KnjizaraEntities knjizaraEntitet = new KnjizaraEntities();
            IEnumerable<SelectListItem> zaposleni = (from k in knjizaraEntitet.Korisniks
                                                     where k.Uloge.Opis.Equals("Zaposleni")
                                                     select k).AsEnumerable().Select(k => new SelectListItem()
                                                     { Text = k.Ime + " " + k.Prezime, Value = k.KorisnickoIme });
            return new SelectList(zaposleni, "Value", "Text", IdZaposlenog);
        }
    }
}
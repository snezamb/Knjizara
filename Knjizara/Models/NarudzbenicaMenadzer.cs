using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; // dodato da bi se koristilo SelectList

namespace Knjizara.Models
{
    public class NarudzbenicaMenadzer
    {
        // izabrana vrednost u metodi IzaberiZaposleni
        public static int IdZaposlenog { get; set; }

        // metoda koja vraca listu narudzbenicaviewmodel
        public static List<NarudzbenicaViewModel> PrikaziSveNarudzbenice(string zaposleni)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                List<NarudzbenicaViewModel> narudzbenice = (from n in knjizaraEntitet.Narudzbenicas
                                                            where n.Zaposleni.Equals(zaposleni)
                                                            select new NarudzbenicaViewModel { NarudzbenicaID = n.NarudzbenicaID, Datum = n.Datum, UkupnaCena = n.UkupnaCena, Zaposleni = n.Zaposleni }).ToList();
                return narudzbenice;
            }
        }

        // metoda koja vraca listu stavkanarudzbenicaviewmodel
        public static List<StavkaNarudzbenicaViewModel> PrikaziSveStavkeNarudzbenice(int sifra)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                List<StavkaNarudzbenicaViewModel> stavkeNarudzbenice = (from s in knjizaraEntitet.StavkaNarudzbenicas
                                                                        where s.Narudzbenica == sifra
                                                                        select new StavkaNarudzbenicaViewModel { Narudzbenica = s.Narudzbenica, RedniBroj = s.RedniBroj, Artikal = s.Artikal1.NazivArtikla, Kolicina = s.Kolicina, Cena = s.Cena }).ToList();
                return stavkeNarudzbenice;
            }
        }

        // metoda koja vraca listu narudzbenica za dropdownlist
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
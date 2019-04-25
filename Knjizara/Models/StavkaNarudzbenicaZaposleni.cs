using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; // dodato da bi se koristilo SelectList

namespace Knjizara.Models
{
    public class StavkaNarudzbenicaZaposleni
    {
        // izabrana vrednost u metodi IzaberiArtikal
        public static int IdArtikla { get; set; }

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

        // metoda koja vraca listu artikala
        public static void ArtikalIzbor(StavkaNarudzbenica stavkaNarudzbenica)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                List<int> artikalStavkaNarudzbenica = (from s in knjizaraEntitet.StavkaNarudzbenicas
                                                       where s.Narudzbenica == stavkaNarudzbenica.Narudzbenica
                                                       select s.Artikal).ToList();

                stavkaNarudzbenica.ListaArtikal = (from a in knjizaraEntitet.Artikals
                                                   where a.Aktivan == 1 && !artikalStavkaNarudzbenica.Contains(a.ArtikalID) || a.ArtikalID == stavkaNarudzbenica.Artikal
                                                   select a).ToList();
            }
        }

        // metoda koja vraca listu artikala za dropdownlist
        public static SelectList IzaberiArtikal(int sifra)
        {
            KnjizaraEntities knjizaraEntitet = new KnjizaraEntities();
            List<int> artikalStavkaNarudzbenica = (from s in knjizaraEntitet.StavkaNarudzbenicas
                                                   where s.Narudzbenica == sifra
                                                   select s.Artikal).ToList();

            IEnumerable<SelectListItem> naziviArtikala = (from a in knjizaraEntitet.Artikals
                                                          where a.Aktivan == 1 && !artikalStavkaNarudzbenica.Contains(a.ArtikalID)
                                                          select a).AsEnumerable().Select(a => new SelectListItem()
                                                          { Text = a.NazivArtikla, Value = a.ArtikalID.ToString() });
            return new SelectList(naziviArtikala, "Value", "Text", IdArtikla);
        }

        // metoda koja kreira novu stavku narudzbenice u tabeli StavkaNarudzbenica
        public static void KreirajStavkaNarudzbenica(StavkaNarudzbenica novaStavkaNarudzbenica)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Artikal cenaStavkaNarudzbenica = (from a in knjizaraEntitet.Artikals
                                                      where novaStavkaNarudzbenica.Artikal == a.ArtikalID
                                                      select a).Single();

                    novaStavkaNarudzbenica.Cena = cenaStavkaNarudzbenica.Cena * novaStavkaNarudzbenica.Kolicina;

                    knjizaraEntitet.StavkaNarudzbenicas.Add(novaStavkaNarudzbenica);
                    knjizaraEntitet.SaveChanges();

                    Narudzbenica izmenaCenaNarudzbenica = (from n in knjizaraEntitet.Narudzbenicas
                                                           where n.NarudzbenicaID == novaStavkaNarudzbenica.Narudzbenica
                                                           select n).Single();

                    izmenaCenaNarudzbenica.UkupnaCena = izmenaCenaNarudzbenica.UkupnaCena + novaStavkaNarudzbenica.Cena;

                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        // metoda koja vraca stavku narudzbenice s konkretnim Narudzbenica i RedniBroj iz tabele StavkaNarudzbenica
        public static StavkaNarudzbenica IzaberiStavka(int sifraNarudzbenica, int redniBroj)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    StavkaNarudzbenica izborStavka = (from s in knjizaraEntitet.StavkaNarudzbenicas
                                                      where s.Narudzbenica == sifraNarudzbenica && s.RedniBroj == redniBroj
                                                      select s).Single();

                    return izborStavka;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        // metoda koja menja stavku narudzbenice iz tabele StavkaNarudzbenica
        public static void IzmeniStavkaNarudzbenica(StavkaNarudzbenica staraStavkaNarudzbenica)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    StavkaNarudzbenica izmenaStavkaNarudzbenica = (from s in knjizaraEntitet.StavkaNarudzbenicas
                                                                   where s.Narudzbenica == staraStavkaNarudzbenica.Narudzbenica && s.RedniBroj == staraStavkaNarudzbenica.RedniBroj
                                                                   select s).Single();

                    izmenaStavkaNarudzbenica.Artikal = staraStavkaNarudzbenica.Artikal;
                    izmenaStavkaNarudzbenica.Kolicina = staraStavkaNarudzbenica.Kolicina;

                    knjizaraEntitet.SaveChanges();

                    izmenaStavkaNarudzbenica.Cena = izmenaStavkaNarudzbenica.Artikal1.Cena * izmenaStavkaNarudzbenica.Kolicina;

                    knjizaraEntitet.SaveChanges();

                    Narudzbenica izmenaCenaNarudzbenica = (from n in knjizaraEntitet.Narudzbenicas
                                                           where n.NarudzbenicaID == staraStavkaNarudzbenica.Narudzbenica
                                                           select n).Single();

                    izmenaCenaNarudzbenica.UkupnaCena = 0;

                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
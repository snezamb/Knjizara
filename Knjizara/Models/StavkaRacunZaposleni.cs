using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; // dodato da bi se koristilo SelectList

namespace Knjizara.Models
{
    public class StavkaRacunZaposleni
    {
        // izabrana vrednost u metodi IzaberiArtikal
        public static int IdArtikla { get; set; }

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

        // metoda koja vraca listu artikala
        public static void ArtikalIzbor(StavkaRacun stavkaRacun)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                List<int> artikalStavkaRacun = (from s in knjizaraEntitet.StavkaRacuns
                                                where s.Racun == stavkaRacun.Racun
                                                select s.Artikal).ToList();

                stavkaRacun.ListaArtikal = (from a in knjizaraEntitet.Artikals
                                            where a.Aktivan == 1 && !artikalStavkaRacun.Contains(a.ArtikalID) || a.ArtikalID == stavkaRacun.Artikal
                                            select a).ToList();
            }
        }

        // metoda koja vraca listu artikala za dropdownlist
        public static SelectList IzaberiArtikal(int sifra)
        {
            KnjizaraEntities knjizaraEntitet = new KnjizaraEntities();
            List<int> artikalStavkaRacun = (from s in knjizaraEntitet.StavkaRacuns
                                            where s.Racun == sifra
                                            select s.Artikal).ToList();

            IEnumerable<SelectListItem> naziviArtikala = (from a in knjizaraEntitet.Artikals
                                                          where a.Aktivan == 1 && !artikalStavkaRacun.Contains(a.ArtikalID)
                                                          select a).AsEnumerable().Select(a => new SelectListItem()
                                                          { Text = a.NazivArtikla, Value = a.ArtikalID.ToString() });
            return new SelectList(naziviArtikala, "Value", "Text", IdArtikla);
        }

        // metoda koja kreira novu stavku racuna u tabeli StavkaRacun
        public static void KreirajStavkaRacun(StavkaRacun novaStavkaRacun)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Artikal cenaStavkaRacun = (from a in knjizaraEntitet.Artikals
                                               where novaStavkaRacun.Artikal == a.ArtikalID
                                               select a).Single();

                    novaStavkaRacun.Cena = cenaStavkaRacun.Cena * novaStavkaRacun.Kolicina;

                    knjizaraEntitet.StavkaRacuns.Add(novaStavkaRacun);
                    knjizaraEntitet.SaveChanges();

                    Racun izmenaCenaRacun = (from r in knjizaraEntitet.Racuns
                                             where r.RacunID == novaStavkaRacun.Racun
                                             select r).Single();

                    izmenaCenaRacun.UkupnaCena = izmenaCenaRacun.UkupnaCena + novaStavkaRacun.Cena;

                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        // metoda koja vraca stavku racuna s konkretnim Racun i RedniBroj iz tabele StavkaRacun
        public static StavkaRacun IzaberiStavka(int sifraRacun, int redniBroj)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    StavkaRacun izborStavka = (from s in knjizaraEntitet.StavkaRacuns
                                               where s.Racun == sifraRacun && s.RedniBroj == redniBroj
                                               select s).Single();

                    return izborStavka;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        // metoda koja menja stavku racuna iz tabele StavkaRacun
        public static void IzmeniStavkaRacun(StavkaRacun staraStavkaRacun)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    StavkaRacun izmenaStavkaRacun = (from s in knjizaraEntitet.StavkaRacuns
                                                     where s.Racun == staraStavkaRacun.Racun && s.RedniBroj == staraStavkaRacun.RedniBroj
                                                     select s).Single();

                    izmenaStavkaRacun.Artikal = staraStavkaRacun.Artikal;
                    izmenaStavkaRacun.Kolicina = staraStavkaRacun.Kolicina;

                    knjizaraEntitet.SaveChanges();

                    izmenaStavkaRacun.Cena = izmenaStavkaRacun.Artikal1.Cena * izmenaStavkaRacun.Kolicina;

                    knjizaraEntitet.SaveChanges();

                    Racun izmenaCenaRacun = (from r in knjizaraEntitet.Racuns
                                             where r.RacunID == staraStavkaRacun.Racun
                                             select r).Single();

                    izmenaCenaRacun.UkupnaCena = 0;

                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
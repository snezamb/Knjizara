using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; // dodato da bi se koristilo SelectList

namespace Knjizara.Models
{
    public class RacunZaposleni
    {
        // izabrana vrednost u metodi IzaberiRacun
        public static int IdRacuna { get; set; }

        // izabrana vrednost u metodi IzaberiStavkaRacun
        public static int IdStavkeRacuna { get; set; }

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

        // metoda koja vraca listu racuna za dropdownlist kada se prosledi korisnicko ime
        public static SelectList IzaberiRacun(string ime)
        {
            KnjizaraEntities knjizaraEntitet = new KnjizaraEntities();
            IEnumerable<SelectListItem> racuni = (from r in knjizaraEntitet.Racuns
                                                  where r.Zaposleni.Equals(ime)
                                                  select r).AsEnumerable().Select(r => new SelectListItem()
                                                  { Text = r.RacunID.ToString(), Value = r.RacunID.ToString() });
            return new SelectList(racuni, "Value", "Text", IdRacuna);
        }

        // metoda koja vraca listu stavki racuna za dropdownlist kada se prosledi sifra racuna
        public static SelectList IzaberiStavkaRacun(int sifra)
        {
            KnjizaraEntities knjizaraEntitet = new KnjizaraEntities();
            IEnumerable<SelectListItem> stavkeRacuna = (from s in knjizaraEntitet.StavkaRacuns
                                                        where s.Racun == sifra
                                                        select s).AsEnumerable().Select(s => new SelectListItem()
                                                        { Text = s.Artikal1.NazivArtikla, Value = s.RedniBroj.ToString() });
            return new SelectList(stavkeRacuna, "Value", "Text", IdStavkeRacuna);
        }

        // metoda koja kreira nov racun u tabeli Racun
        public static void KreirajRacun(Racun novRacun)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    novRacun.UkupnaCena = 0;
                    knjizaraEntitet.Racuns.Add(novRacun);
                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        // metoda koja racuna ukupnu cenu racuna
        public static void UkupnaCenaKreiranRacun(int sifraKreiranRacun)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Racun ukupnaCenaRacun = (from r in knjizaraEntitet.Racuns
                                             where r.RacunID == sifraKreiranRacun
                                             select r).Single();

                    if (ukupnaCenaRacun.UkupnaCena == 0)
                    {
                        foreach (StavkaRacunViewModel stavka in StavkaRacunZaposleni.PrikaziSveStavkeRacuna(ukupnaCenaRacun.RacunID))
                        {
                            ukupnaCenaRacun.UkupnaCena = ukupnaCenaRacun.UkupnaCena + stavka.Cena;
                        }
                    }

                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        // metoda koja vraca racun s konkretnim RacunID iz tabele Racun
        public static Racun IzaberiRacun(int sifra)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Racun izborRacun = (from r in knjizaraEntitet.Racuns
                                        where r.RacunID == sifra
                                        select r).Single();

                    return izborRacun;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        // metoda koja menja racun iz tabele Racun
        public static void IzmeniRacun(Racun starRacun)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Racun izmenaRacun = (from r in knjizaraEntitet.Racuns
                                         where r.RacunID == starRacun.RacunID
                                         select r).Single();

                    izmenaRacun.Datum = starRacun.Datum;
                    izmenaRacun.UkupnaCena = 0;
                    izmenaRacun.Zaposleni = starRacun.Zaposleni;

                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
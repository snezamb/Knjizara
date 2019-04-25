using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; // dodato da bi se koristilo SelectList

namespace Knjizara.Models
{
    public class NarudzbenicaZaposleni
    {
        // izabrana vrednost u metodi IzaberiNarudzbenica
        public static int IdNarudzbenice { get; set; }

        // izabrana vrednost u metodi IzaberiStavkaNarudzbenica
        public static int IdStavkeNarudzbenice { get; set; }

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

        // metoda koja vraca listu narudzbenica za dropdownlist kada se prosledi korisnicko ime
        public static SelectList IzaberiNarudzbenica(string ime)
        {
            KnjizaraEntities knjizaraEntitet = new KnjizaraEntities();
            IEnumerable<SelectListItem> narudzbenice = (from n in knjizaraEntitet.Narudzbenicas
                                                        where n.Zaposleni.Equals(ime)
                                                        select n).AsEnumerable().Select(n => new SelectListItem()
                                                        { Text = n.NarudzbenicaID.ToString(), Value = n.NarudzbenicaID.ToString() });
            return new SelectList(narudzbenice, "Value", "Text", IdNarudzbenice);
        }

        // metoda koja vraca listu stavki narudzbenice za dropdownlist kada se prosledi sifra narudzbenice
        public static SelectList IzaberiStavkaNarudzbenica(int sifra)
        {
            KnjizaraEntities knjizaraEntitet = new KnjizaraEntities();
            IEnumerable<SelectListItem> stavkeNarudzbenice = (from s in knjizaraEntitet.StavkaNarudzbenicas
                                                              where s.Narudzbenica == sifra
                                                              select s).AsEnumerable().Select(s => new SelectListItem()
                                                              { Text = s.Artikal1.NazivArtikla, Value = s.RedniBroj.ToString() });
            return new SelectList(stavkeNarudzbenice, "Value", "Text", IdStavkeNarudzbenice);
        }

        // metoda koja kreira novu narudzbenicu u tabeli Narudzbenica
        public static void KreirajNarudzbenica(Narudzbenica novaNarudzbenica)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    novaNarudzbenica.UkupnaCena = 0;
                    knjizaraEntitet.Narudzbenicas.Add(novaNarudzbenica);
                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        // metoda koja racuna ukupnu cenu narudzbenice
        public static void UkupnaCenaKreiranaNarudzbenica(int sifraKreiranaNarudzbenica)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Narudzbenica ukupnaCenaNarudzbenica = (from n in knjizaraEntitet.Narudzbenicas
                                                           where n.NarudzbenicaID == sifraKreiranaNarudzbenica
                                                           select n).Single();

                    if (ukupnaCenaNarudzbenica.UkupnaCena == 0)
                    {
                        foreach (StavkaNarudzbenicaViewModel stavka in StavkaNarudzbenicaZaposleni.PrikaziSveStavkeNarudzbenice(ukupnaCenaNarudzbenica.NarudzbenicaID))
                        {
                            ukupnaCenaNarudzbenica.UkupnaCena = ukupnaCenaNarudzbenica.UkupnaCena + stavka.Cena;
                        }
                    }

                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        // metoda koja vraca narudzbenicu s konkretnim NarudzbenicaID iz tabele Narudzbenica
        public static Narudzbenica IzaberiNarudzbenica(int sifra)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Narudzbenica izborNarudzbenica = (from n in knjizaraEntitet.Narudzbenicas
                                                      where n.NarudzbenicaID == sifra
                                                      select n).Single();

                    return izborNarudzbenica;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        // metoda koja menja narudzbenicu iz tabele Narudzbenica
        public static void IzmeniNarudzbenica(Narudzbenica staraNarudzbenica)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Narudzbenica izmenaNarudzbenica = (from n in knjizaraEntitet.Narudzbenicas
                                                       where n.NarudzbenicaID == staraNarudzbenica.NarudzbenicaID
                                                       select n).Single();

                    izmenaNarudzbenica.Datum = staraNarudzbenica.Datum;
                    izmenaNarudzbenica.UkupnaCena = 0;
                    izmenaNarudzbenica.Zaposleni = staraNarudzbenica.Zaposleni;

                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
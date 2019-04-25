using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; // dodato da bi se koristilo SelectList

namespace Knjizara.Models
{
    public class KorisnikMenadzer
    {
        // izabrana vrednost u metodi IzaberiKorisnika
        public static int IdKorisnika { get; set; }

        // metoda koja vraca listu korisnikviewmodel
        public static List<KorisnikViewModel> PrikaziSveKorisnike()
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                List<KorisnikViewModel> korisnici = (from k in knjizaraEntitet.Korisniks
                                                     select new KorisnikViewModel { KorisnickoIme = k.KorisnickoIme, Ime = k.Ime, Prezime = k.Prezime, JMBG = k.JMBG, Lozinka = k.Lozinka, Uloga = k.Uloge.Opis }).ToList();
                return korisnici;
            }
        }

        // metoda koja vraca listu korisnika za dropdownlist
        public static SelectList IzaberiKorisnika()
        {
            KnjizaraEntities knjizaraEntitet = new KnjizaraEntities();
            IEnumerable<SelectListItem> korisnici = (from k in knjizaraEntitet.Korisniks
                                                     where k.Uloga != (from u in knjizaraEntitet.Uloges
                                                                       where u.Opis.Equals("Nezaposleni")
                                                                       select u.UlogaID).FirstOrDefault()
                                                     select k).AsEnumerable().Select(k => new SelectListItem
                                                     { Text = k.Ime + " " + k.Prezime, Value = k.KorisnickoIme });
            return new SelectList(korisnici, "Value", "Text", IdKorisnika);
        }

        // metoda koja unosi novog korisnika u tabelu Korisnik
        public static void UnesiKorisnika(Korisnik novKorisnik)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    novKorisnik.Uloga = (from u in knjizaraEntitet.Uloges
                                         where u.Opis.Equals("Zaposleni")
                                         select u.UlogaID).Single();

                    knjizaraEntitet.Korisniks.Add(novKorisnik);
                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        // metoda koja brise korisnika iz tabele Korisnik
        public static bool BrisiKorisnika(string korisnickoIme)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Korisnik brisanjeKorisnik = (from k in knjizaraEntitet.Korisniks
                                                 where k.KorisnickoIme == korisnickoIme
                                                 select k).Single();

                    if (brisanjeKorisnik != null)
                    {
                        brisanjeKorisnik.Uloga = (from u in knjizaraEntitet.Uloges
                                                  where u.Opis.Equals("Nezaposleni")
                                                  select u.UlogaID).Single();

                        knjizaraEntitet.SaveChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        // metoda koja prikazuje korisnika iz tabele Korisnik
        public static Korisnik PrikaziKorisnika(string korisnickoIme)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Korisnik prikazKorisnik = (from k in knjizaraEntitet.Korisniks
                                               where k.KorisnickoIme == korisnickoIme
                                               select k).Single();

                    return prikazKorisnik;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        // metoda koja menja korisnika iz tabele Korisnik
        public static void IzmeniKorisnika(Korisnik starKorisnik)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Korisnik izmenaKorisnik = (from k in knjizaraEntitet.Korisniks
                                               where k.KorisnickoIme == starKorisnik.KorisnickoIme
                                               select k).Single();

                    izmenaKorisnik.Ime = starKorisnik.Ime;
                    izmenaKorisnik.Prezime = starKorisnik.Prezime;
                    izmenaKorisnik.JMBG = starKorisnik.JMBG;
                    izmenaKorisnik.Lozinka = starKorisnik.Lozinka;

                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
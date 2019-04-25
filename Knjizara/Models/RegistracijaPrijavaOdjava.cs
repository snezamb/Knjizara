using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjizara.Models
{
    public class RegistracijaPrijavaOdjava
    {
        // metoda koja registracijom unosi novog korisnika u tabelu Korisnik
        public static void RegistracijaKorisnika(Korisnik novKorisnik)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    novKorisnik.Uloga = (from u in knjizaraEntitet.Uloges
                                         where u.Opis.Equals("Nezaposleni")
                                         select u.UlogaID).Single();
                    knjizaraEntitet.Korisniks.Add(novKorisnik);
                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        // metoda koja 
        public static bool PrijavaKorisnika(Korisnik prijavljenKorisnik)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Korisnik poredjenje = knjizaraEntitet.Korisniks.Single(k => (k.KorisnickoIme.Equals(prijavljenKorisnik.KorisnickoIme)) && (k.Lozinka.Equals(prijavljenKorisnik.Lozinka)));

                    if (poredjenje != null)
                    {
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
    }
}
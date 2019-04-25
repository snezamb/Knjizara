using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; // dodato da bi se koristilo SelectList

namespace Knjizara.Models
{
    public class DobavljacMenadzer
    {
        // izabrana vrednost u metodi IzaberiDobavljaca
        public static int IdDobavljaca { get; set; }

        // metoda koja vraca listu dobavljacviewmodel
        public static List<DobavljacViewModel> PrikaziSveDobavljace()
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                List<DobavljacViewModel> dobavljaci = (from d in knjizaraEntitet.Dobavljacis
                                                       select new DobavljacViewModel { DobavljacID = d.DobavljacID, NazivDobavljaca = d.NazivDobavljaca, Adresa = d.Adresa, Telefon = d.Telefon, Aktivan = d.Aktivan}).ToList();
                return dobavljaci;
            }
        }

        // metoda koja unosi novog dobavljaca u tabelu Dobavljaci
        public static void UnesiDobavljaca(Dobavljaci novDobavljac)
        {
            using (KnjizaraEntities knjizaraEntitet =  new KnjizaraEntities())
            {
                try
                {
                    novDobavljac.Aktivan = 1;
                    knjizaraEntitet.Dobavljacis.Add(novDobavljac);
                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        // metoda koja vraca listu dobavljaca za dropdownlist
        public static SelectList IzaberiDobavljaca()
        {
            KnjizaraEntities knjizaraEntitet = new KnjizaraEntities();
            IEnumerable<SelectListItem> dobavljaci = (from d in knjizaraEntitet.Dobavljacis
                                                      where d.Aktivan == 1 || d.Aktivan == null
                                                      select d).AsEnumerable().Select(d => new SelectListItem()
                                                      { Text = d.NazivDobavljaca, Value = d.DobavljacID.ToString() });
            return new SelectList(dobavljaci, "Value", "Text", IdDobavljaca);
        }

        // metoda koja brise dobavljaca iz tabele Dobavljaci
        public static bool BrisiDobavljac(int sifra)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Dobavljaci brisanjeDobavljac = (from d in knjizaraEntitet.Dobavljacis
                                                    where d.DobavljacID == sifra
                                                    select d).Single();

                    if (brisanjeDobavljac != null)
                    {
                        brisanjeDobavljac.Aktivan = 0;
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

        // metoda koja prikazuje dobavljaca iz tabele Dobavljaci
        public static Dobavljaci PrikaziDobavljac(int sifra)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Dobavljaci prikazDobavljac = (from d in knjizaraEntitet.Dobavljacis
                                                   where d.DobavljacID == sifra
                                                   select d).Single();

                    return prikazDobavljac;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        // metoda koja menja dobavljaca iz tabele Dobavljaci
        public static void IzmeniDobavljac(Dobavljaci starDobavljac)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Dobavljaci izmeniDobavljac = (from d in knjizaraEntitet.Dobavljacis
                                                  where d.DobavljacID == starDobavljac.DobavljacID
                                                  select d).Single();

                    izmeniDobavljac.NazivDobavljaca = starDobavljac.NazivDobavljaca;
                    izmeniDobavljac.Adresa = starDobavljac.Adresa;
                    izmeniDobavljac.Telefon = starDobavljac.Telefon;

                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; // dodato da bi se koristilo SelectList

namespace Knjizara.Models
{
    public class ArtikalMenadzer
    {
        // izabrana vrednost u metodi IzaberiDobavljaca
        public static int IdDobavljaca { get; set; }

        // izabrana vrednost u metodi IzaberiArtikal
        public static int IdArtikla { get; set; }

        // metoda koja vraca listu artikalviewmodel
        public static List<ArtikalViewModel> PrikaziSveArtikle()
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                List<ArtikalViewModel> artikli = (from a in knjizaraEntitet.Artikals
                                                  select new ArtikalViewModel { ArtikalID = a.ArtikalID, NazivArtikla = a.NazivArtikla, Cena = a.Cena, Kolicina = a.Kolicina, Dobavljac = a.Dobavljaci.NazivDobavljaca, Aktivan = a.Aktivan }).ToList();
                return artikli;
            }
        }

        // metoda koja vraca listu dobavljaca
        public static void DobavljacIzbor(Artikal artikal)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                artikal.ListaDobavljaci = (from d in knjizaraEntitet.Dobavljacis
                                           select d).ToList();
            }
        }

        // metoda koja vraca listu dobavljaca za dropdownlist
        public static SelectList IzaberiDobavljaca()
        {
            KnjizaraEntities knjizaraEntitet = new KnjizaraEntities();
            IEnumerable<SelectListItem> dobavljaci = (from d in knjizaraEntitet.Dobavljacis
                                                      select d).AsEnumerable().Select(d => new SelectListItem()
                                                      { Text = d.NazivDobavljaca, Value = d.DobavljacID.ToString()});
            return new SelectList(dobavljaci, "Value", "Text", IdDobavljaca);
        }

        // metoda koja vraca listu artikala za dropdownlist
        public static SelectList IzaberiArtikal()
        {
            KnjizaraEntities knjizaraEntitet = new KnjizaraEntities();
            IEnumerable<SelectListItem> naziviArtikala = (from a in knjizaraEntitet.Artikals
                                                          where a.Aktivan == 1 || a.Aktivan == null
                                                          select a).AsEnumerable().Select(a => new SelectListItem()
                                                          { Text = a.NazivArtikla, Value = a.ArtikalID.ToString() });
            return new SelectList(naziviArtikala, "Value", "Text", IdArtikla);
        }

        // metoda koja unosi nov artikal u tabelu Artikal
        public static void UnesiArtikal(Artikal novArtikal)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    knjizaraEntitet.Artikals.Add(novArtikal);
                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }

        // metoda koja brise artikal iz tabele Artikal
        public static bool BrisiArtikal(int sifra)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Artikal brisanjeArtikal = (from a in knjizaraEntitet.Artikals
                                               where a.ArtikalID == sifra
                                               select a).Single();

                    if (brisanjeArtikal != null)
                    {
                        brisanjeArtikal.Aktivan = 0;
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

        // metoda koja prikazuje artikal iz tabele Artikal
        public static Artikal PrikaziArtikal(int sifra)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Artikal prikazArtikal = (from a in knjizaraEntitet.Artikals
                                             where a.ArtikalID == sifra
                                             select a).Single();

                    return prikazArtikal;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        // metoda koja menja artikal iz tabele Artikal
        public static void IzmeniArtikal(Artikal starArtikal)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    Artikal izmenaArtikal = (from a in knjizaraEntitet.Artikals
                                             where a.ArtikalID == starArtikal.ArtikalID
                                             select a).Single();

                    izmenaArtikal.NazivArtikla = starArtikal.NazivArtikla;
                    izmenaArtikal.Cena = starArtikal.Cena;
                    izmenaArtikal.Kolicina = starArtikal.Kolicina;
                    izmenaArtikal.Dobavljac = starArtikal.Dobavljac;

                    knjizaraEntitet.SaveChanges();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}
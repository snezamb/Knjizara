using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjizara.Models
{
    public class Kupac
    {
        // metoda koja vraca listu artikalviewmodel
        public static List<ArtikalViewModel> PrikaziSveArtikleKnjizare()
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                List<ArtikalViewModel> artikli = (from a in knjizaraEntitet.Artikals
                                                  select new ArtikalViewModel { ArtikalID = a.ArtikalID, NazivArtikla = a.NazivArtikla, Cena = a.Cena, Kolicina = a.Kolicina, Dobavljac = a.Dobavljaci.NazivDobavljaca, Aktivan = a.Aktivan }).ToList();
                return artikli;
            }
        }

        // metoda koja vraca listu artikalviewmodel
        public static List<ArtikalViewModel> PrikaziSveArtikle()
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                List<ArtikalViewModel> artikli = (from a in knjizaraEntitet.Artikals
                                                  where a.Aktivan == 1
                                                  select new ArtikalViewModel { ArtikalID = a.ArtikalID, NazivArtikla = a.NazivArtikla, Cena = a.Cena, Kolicina = a.Kolicina, Dobavljac = a.Dobavljaci.NazivDobavljaca, Aktivan = a.Aktivan }).ToList();
                return artikli;
            }
        }

        // metoda koja vraca listu kupovinaviewmodel kada se prosledi lista artikala
        public static List<KupacViewModel> PrikaziSveArtikle(List<Artikal> izbor)
        {
            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                List<KupacViewModel> kupovina = new List<KupacViewModel>();

                foreach (Artikal artikal in izbor)
                {
                    KupacViewModel knjiga = (from a in knjizaraEntitet.Artikals
                                                where a.ArtikalID == artikal.ArtikalID
                                                select new KupacViewModel { NazivArtikla = a.NazivArtikla, Cena = a.Cena, Kolicina = artikal.Kolicina, UkupnaCena = (a.Cena * artikal.Kolicina), Dobavljac = a.Dobavljaci.NazivDobavljaca }).Single();
                    kupovina.Add(knjiga);
                }
                
                return kupovina;
            }
        }

        // metoda koja vraca listu artikala na osnovu podataka unetih u formu
        public static List<Artikal> Izbor(int[] sifre, int[] kolicine)
        {
            List<int> izborArtikli = new List<int>();
            int[] izborKolicine = new int[kolicine.Length];
            int brojac = 0;

            for (int i = 0; i < kolicine.Length; i++)
            {
                for (int j = 0; j < sifre.Length; j++)
                {
                    if (i == j && kolicine[i] != 0)
                    {
                        izborArtikli.Add(Convert.ToInt32(sifre[j]));
                        izborKolicine[brojac] = Convert.ToInt32(kolicine[i]);
                        brojac++;
                    }
                }
            }

            using (KnjizaraEntities knjizaraEntitet = new KnjizaraEntities())
            {
                try
                {
                    List<Artikal> izborKnjige = new List<Artikal>();
                    int brojacKolicina = 0;

                    foreach (int sifra in izborArtikli)
                    {
                        Artikal artikal = (from a in knjizaraEntitet.Artikals
                                           where a.ArtikalID == sifra
                                           select a).Single();

                        artikal.Kolicina = izborKolicine[brojacKolicina];

                        izborKnjige.Add(artikal);
                        brojacKolicina++;
                    }

                    return izborKnjige;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        // metoda koja racuna ukupnu cenu kupovine kada se prosledi lista artikala
        public static float UkupnaCenaKreiranRacun(List<Artikal> kupovina)
        {
            float ukupnaCenaKupovine = 0;

            List<KupacViewModel> knjigeKupovina = PrikaziSveArtikle(kupovina);

            foreach (KupacViewModel knjiga in knjigeKupovina)
            {
                ukupnaCenaKupovine = ukupnaCenaKupovine + knjiga.UkupnaCena;
            }

            return ukupnaCenaKupovine;
        }
    }
}
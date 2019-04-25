using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjizara.Models
{
    public class KupacViewModel
    {
        public string NazivArtikla { get; set; }

        public float Cena { get; set; }

        public int Kolicina { get; set; }

        public float UkupnaCena { get; set; }

        public string Dobavljac { get; set; }
    }
}
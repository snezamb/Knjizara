using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjizara.Models
{
    public class RacunViewModel
    {
        public int RacunID { get; set; }

        public DateTime Datum { get; set; }

        public float UkupnaCena { get; set; }

        public string Zaposleni { get; set; }
    }
}
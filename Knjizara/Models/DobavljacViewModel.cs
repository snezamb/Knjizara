using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // dodato da bi se koristila anotacija
using System.Linq;
using System.Web;

namespace Knjizara.Models
{
    public class DobavljacViewModel
    {
        public int DobavljacID { get; set; }

        public string NazivDobavljaca { get; set; }

        public string Adresa { get; set; }

        public string Telefon { get; set; }

        public Nullable<int> Aktivan { get; set; }
    }
}
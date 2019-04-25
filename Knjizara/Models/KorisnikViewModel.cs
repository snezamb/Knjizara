using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // dodato da bi se koristila anotacija
using System.Linq;
using System.Web;

namespace Knjizara.Models
{
    public class KorisnikViewModel
    {
        public string KorisnickoIme { get; set; }

        public string Ime { get; set; }

        public string Prezime { get; set; }

        public string JMBG { get; set; }

        public string Lozinka { get; set; }

        public string Uloga { get; set; }
    }
}
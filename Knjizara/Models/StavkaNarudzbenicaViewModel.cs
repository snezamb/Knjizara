using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Knjizara.Models
{
    public class StavkaNarudzbenicaViewModel
    {
        public int Narudzbenica { get; set; }

        public int RedniBroj { get; set; }

        public string Artikal { get; set; }

        public int Kolicina { get; set; }

        public float Cena { get; set; }
    }
}
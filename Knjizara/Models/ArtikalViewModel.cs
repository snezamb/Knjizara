using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // dodato da bi se koristila anotacija
using System.ComponentModel.DataAnnotations.Schema; // dodato da bi se koristilo NotMapped
using System.Linq;
using System.Web;

namespace Knjizara.Models
{
    public class ArtikalViewModel
    {
        public int ArtikalID { get; set; }

        public string NazivArtikla { get; set; }

        public float Cena { get; set; }

        public int Kolicina { get; set; }

        public string Dobavljac { get; set; }

        public Nullable<int> Aktivan { get; set; }
    }
}
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Knjizara.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations; // dodato da bi se koristila anotacija
    using System.ComponentModel.DataAnnotations.Schema; // dodato da bi se koristilo NotMapped

    public partial class Narudzbenica
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Narudzbenica()
        {
            this.StavkaNarudzbenicas = new HashSet<StavkaNarudzbenica>();
        }

        [Required(ErrorMessage = "Sifra narudzbenice je obavezna")]
        [RegularExpression("[0-9]+", ErrorMessage = "Moze se uneti cifra")]
        public int NarudzbenicaID { get; set; }

        [Required(ErrorMessage = "Datum narudzbenice je obavezan")]
        [ValidacijaDatum(ErrorMessage = "Format datuma narudzbenice")]
        public System.DateTime Datum { get; set; }

        public float UkupnaCena { get; set; }

        public string Zaposleni { get; set; }

        [NotMapped]
        public List<Korisnik> ListaKorisnik { get; set; }
        public virtual Korisnik Korisnik { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StavkaNarudzbenica> StavkaNarudzbenicas { get; set; }
    }
}
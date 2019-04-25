using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // dodato da bi se koristilo ValidationAttribute
using System.Linq;
using System.Web;

namespace Knjizara.Models
{
    public class ValidacijaKolicinaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int kolicina = Convert.ToInt32(value);

            if (kolicina <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
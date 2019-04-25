using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // dodato da bi se koristilo ValidationAttribute
using System.Linq;
using System.Web;

namespace Knjizara.Models
{
    public class ValidacijaCenaAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            double cena = Convert.ToDouble(value);

            if (cena <= 0)
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
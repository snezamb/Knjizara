using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // dodato da bi se koristilo ValidationAttribute
using System.Linq;
using System.Web;

namespace Knjizara.Models
{
    public class ValidacijaDatumAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime datum = Convert.ToDateTime(value);

            if (datum >= DateTime.Now.AddMonths(-6) && datum <= DateTime.Now.AddMonths(6))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
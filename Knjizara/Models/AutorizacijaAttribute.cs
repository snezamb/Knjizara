using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; // dodato da bi se koristilo AuthorizeAttribute

namespace Knjizara.Models
{
    public class AutorizacijaAttribute : AuthorizeAttribute
    {
        public string Pogled { get; set; }

        public AutorizacijaAttribute()
        {
            this.Pogled = "Autorizacija";
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            KorisnikAutorizacija(filterContext);
        }

        public void KorisnikAutorizacija(AuthorizationContext filterContext)
        {
            // ako je korisnik autorizovan
            if (filterContext.Result == null)
            {
                return;
            }

            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                ViewDataDictionary recnik = new ViewDataDictionary();
                recnik.Add("Autorizacija", "Niste autorizovani da pristupite sadrzaju");
                var rezultat = new ViewResult { ViewName = this.Pogled, ViewData = recnik};
                filterContext.Result = rezultat;
            }
        }
    }
}
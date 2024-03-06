using HalloDoc.Entity.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalloDoc.Repository
{

    public class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public CustomAuthorize(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            string Hallocookie = context.HttpContext.Request.Cookies["HalloCookie"];


            if (Hallocookie == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Patient", Action = "PatientLogin" }));
                return;
            }

            var role = context.HttpContext.Request.Cookies["CookieRole"];
            if (_role != role)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Patient", Action = "PatientLogin" }));
                return;
            }
        }
    }
}

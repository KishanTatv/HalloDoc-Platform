using HalloDoc.Entity.Models;
using HalloDoc.Repository.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
            var jswtService = context.HttpContext.RequestServices.GetService<IJwtToken>();

            if (jswtService == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Patient", Action = "PatientLogin" }));
                return;
            }

            if(Hallocookie == null || !jswtService.ValidateToken(Hallocookie, out JwtSecurityToken jwtSecurityToken))
            {
                //context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Patient", Action = "SubmitReq" }));
                if (IsAjaxRequest(context.HttpContext.Request))
                {
                    context.Result = new JsonResult(
                        new { Message = "Unauthorization" }
                    )
                    { StatusCode = StatusCodes.Status401Unauthorized };
                }
                else
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Patient", action = "SubmitReq" }));
                }
                return;
            }

            var roleClaims = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
            if(roleClaims == null)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Patient", Action = "PatientLogin" }));
                return;
            }

            //var role = context.HttpContext.Request.Cookies["CookieRole"];
            if (roleClaims.Value.Contains(_role) || string.IsNullOrWhiteSpace(_role))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Patient", Action = "PatientLogin" }));
                return;
            }

        }

        private bool IsAjaxRequest(HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}

﻿using HalloDoc.Entity.Models;
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
        private readonly string _page;

        public CustomAuthorize(string role, string page)
        {
            _role = role;
            _page = page;
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
            if (!_role.Contains(roleClaims.Value) || string.IsNullOrWhiteSpace(_role))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Patient", Action = "PatientLogin" }));
                return;
            }

            var menuList = jwtSecurityToken.Claims.FirstOrDefault(x => x.Type == "Menu");
            if (!menuList.Value.Contains(_page))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { Controller = "Home", Action = "AccessDenied" }));
                return;
            }

        }

        private bool IsAjaxRequest(HttpRequest request)
        {
            return request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}

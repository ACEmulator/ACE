using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ACE.Web.Models.Account;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace ACE.Web.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            RestClient authClient = new RestClient(ConfigurationManager.AppSettings["Ace.Api"]);
            var authRequest = new RestRequest("/Account/Authenticate", Method.POST);
            authRequest.AddJsonBody(new { model.Username, model.Password });
            var authResponse = authClient.Execute(authRequest);

            if (authResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                model.ErrorMessage = "Incorrect Username or Password";
                return View(model);
            }
            else if (authResponse.StatusCode != HttpStatusCode.OK)
            {
                model.ErrorMessage = "Error connecting to API";
                return View(model);
            }
            
            // else we got an OK response
            JObject response = JObject.Parse(authResponse.Content);
            var authToken = (string)response.SelectToken("authToken");

            if (!string.IsNullOrWhiteSpace(authToken))
            {
                JwtCookieManager.SetCookie(authToken);
                return RedirectToAction("Index", "Home", null);
            }

            return View(model);
        }
        
        [HttpGet]
        public ActionResult LogOff()
        {
            JwtCookieManager.SignOut();
            return RedirectToAction("Index", "Home", null);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View(new RegisterModel());
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            // create the user
            
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ACE.Web.Models.Account;
using Newtonsoft.Json;

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
            if (ModelState.IsValid)
            {
                var newUser = new object(); // attempt the login here
                string token = "jwt here";

                if (newUser != null)
                {
                    JwtCookieManager.SetCookie(token);
                    return RedirectToAction("Index", "Home", null);
                }
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace ACE.Web.Controllers
{
    public class ErrorController : BaseController
    {
        /// <summary>
        /// Displays a generic error message
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="redirectTo"></param>
        /// <returns></returns>
        public ActionResult ShowError(string title, string message, string redirectTo)
        {
            if (string.IsNullOrEmpty(message))
                message = "We are sorry, but an unspecified error occurred in the application. The error has been logged and forwarded to be checked out as soon as possible.";

            ErrorViewModel model = new ErrorViewModel
            {
                Message = message,
                Title = title,
                RedirectTo = redirectTo != null ? Url.Content(redirectTo) : "",
                MessageIsHtml = true
            };

            // Explicitly point at Error View regardless of action
            return View("Error", model);
        }

        /// <summary>
        /// Displays a generic error message but allows passing a view model directly for 
        /// additional flexibility
        /// </summary>
        /// <param name="errorModel"></param>
        /// <returns></returns>
        public ActionResult ShowErrorFromModel(ErrorViewModel errorModel)
        {
            return View("Error", errorModel);
        }

        public ActionResult ShowMessage(string title, string message, string redirectTo)
        {
            return this.ShowError(title, message, redirectTo);
        }

        public ActionResult CauseError()
        {
            ErrorController controller = null;
            controller.CauseError();  // cause exception
            // never called
            return View("Error");
        }


        // *** The following are STATIC controller methods that allow 
        // *** triggering the error display outside of a controller context
        // *** (from a module or global.asax handler for example)



        /// <summary>
        /// Static method that can be called from outside of MVC requests
        /// (like in Application_Error) to display an error View.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        /// <param name="redirectTo"></param>
        /// <param name="messageIsHtml"></param>
        public static void ShowErrorPage(string title, string message, string redirectTo)
        {
            ErrorController controller = new ErrorController();

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "ShowError");
            routeData.Values.Add("title", title);
            routeData.Values.Add("message", message);
            routeData.Values.Add("redirectTo", redirectTo);

            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), routeData));
        }

        /// <summary>
        /// Static method that can be called from outside of MVC requests
        /// (like in Application_Error) to display an error View.
        /// </summary>
        public static void ShowErrorPage(ErrorViewModel errorModel)
        {
            ErrorController controller = new ErrorController();

            RouteData routeData = new RouteData();
            routeData.Values.Add("controller", "Error");
            routeData.Values.Add("action", "ShowErrorFromModel");
            routeData.Values.Add("errorModel", errorModel);

            ((IController)controller).Execute(new RequestContext(new HttpContextWrapper(System.Web.HttpContext.Current), routeData));
        }

        /// <summary>
        /// Static method that can be called from outside of MVC requests
        /// (like in Application_Error) to display an error View.
        /// </summary>
        public static void ShowErrorPage(string title, string message)
        {
            ShowErrorPage(title, message, null);
        }

    }
}
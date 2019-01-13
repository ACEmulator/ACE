using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.Server.API;
using ACE.Server.Web.Entities;
using Nancy;
using Nancy.Security;

namespace ACE.Server.Web.Modules
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            this.RequiresAuthentication();

             Get("/", args => "Hello " + this.Context.CurrentUser.Identity.Name);
            //Get("/", parameters =>
            //{

            //    return View["index", BuildIndexModel()];
            //});


            //Post("/login", parameters =>
            //{
            //    //set user

            //    //IUserMapper
            //    LoginRequest request = this.BindAndValidate<LoginRequest>();
            //    if (!ModelValidationResult.IsValid)
            //    {
            //        //return Negotiate.WithModel(ModelValidationResult).WithStatusCode(HttpStatusCode.BadRequest);
            //        return View["index", new { error = ModelValidationResult.ToString() }];
            //    }


            //    return View["index",new {error=ModelValidationResult.ToString() }];
            //});
        }
        private IndexModel BuildIndexModel()
        {
            if (!EnsureLoggedIn())
            {
                return new IndexModel();
            }
            else
            {

                var model = new IndexModel();
                Account acct = null;
                Gate.RunGatedAction(() =>
                {
                    acct = DatabaseManager.Authentication.GetAccountByName(Context.CurrentUser.Identity.Name);
                });
                if (acct != null)
                {
                    acct.PasswordHash = null;
                    acct.PasswordSalt = null;
                    model.Account = acct;
                }
                return model;

            }
        }
        private bool EnsureLoggedIn()
        {
            if (string.IsNullOrWhiteSpace(Context.CurrentUser.Identity.Name))
            {
                return false;
            }
            else
            {
                return true;
            }
            // commented session usage because either the nancy object serializer is a broken piece of ##%$ or i'm just stupid, or both
            //if (Context.Request.Session["AccountId"] == null)
            //{
            //    Account acct = null;
            //    Gate.RunGatedAction(() =>
            //    {
            //        acct = DatabaseManager.Authentication.GetAccountByName(Context.CurrentUser.Identity.Name);
            //    });
            //    if (acct != null)
            //    {
            //        Context.Request.Session["AccountId"] = (int)acct.AccountId;
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}
            //else
            //{
            //    return true;
            //}
        }
    }
}

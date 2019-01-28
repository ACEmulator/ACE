using ACE.Entity.Enum;
using ACE.Server.Command;
using ACE.Server.Managers;
using ACE.WebApiServer.Model;
using ACE.WebApiServer.Model.Admin;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using System.Linq;

namespace ACE.WebApiServer.Modules
{
    public class AdminAuthenticatedModule : BaseAuthenticatedModule
    {
        public AdminAuthenticatedModule()
        {
            this.RequiresAuthentication();

            this.RequiresClaims(k => k.Type == AccessLevel.Admin.ToString());

            Get("/api/command", async (_) =>
            {
                AdminCommandRequestModel request = this.BindAndValidate<AdminCommandRequestModel>();
                if (!ModelValidationResult.IsValid)
                {
                    return Negotiate.WithModel(ModelValidationResult).WithStatusCode(HttpStatusCode.BadRequest);
                }
                CommandManager.ConsoleCommandOverallResult result = null;

                Gate.RunGatedAction(() =>
                {
                    result = CommandManager.ConsoleCommand(request.Command);
                });

                return new AdminCommandResponseModel()
                {
                    Success = result.ConsoleCommandResult == CommandManager.ConsoleCommandResult.Success,
                    CommandResult = result.ConsoleCommandResult.ToString(),
                    CommandHandlerResponse = result.CommandHandlerResponse?.ToString(),
                    SubmittedCommand = request.Command
                }.AsJsonWebResponse();
            });

            Get("/api/playerLocations", async (_) =>
            {
                PlayerLocationsResponseModel resp = new PlayerLocationsResponseModel();
                Gate.RunGatedAction(() => resp.Locations = PlayerManager.GetAllOnline().Select(k => new PlayerNameAndLocation() { Location = k.Location.ToString(), Name = k.Name }).ToList());
                return resp.AsJsonWebResponse();
            });
        }
    }
}

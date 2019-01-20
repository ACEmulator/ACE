using ACE.Entity.Enum;
using ACE.Server.Command;
using ACE.WebApiServer.Model.Admin;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace ACE.WebApiServer.Modules
{
    public class AdminAuthenticatedModule : BaseAuthenticatedModule
    {
        public AdminAuthenticatedModule()
        {
            this.RequiresAuthentication();

            this.RequiresClaims(k => k.Type == AccessLevel.Admin.ToString());

            Get("/api/admin/command", async (_) =>
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
                }.AsJson();
            });
        }
    }
}

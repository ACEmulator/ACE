using ACE.Entity.Enum;
using ACE.Server.Command;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;
using ACE.WebApiServer.Model;
using ACE.WebApiServer.Model.Admin;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using System.Collections.Generic;
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

            Get("/api/landblockStatus", async (_) =>
            {
                LandblockStatusResponseModel resp = new LandblockStatusResponseModel();
                Gate.RunGatedAction(() =>
                {
                    List<Server.Entity.Landblock> activeLandblocks = LandblockManager.GetActiveLandblocks();
                    List<LandblockStatus> lbsl = new List<LandblockStatus>();
                    foreach (Server.Entity.Landblock landblock in activeLandblocks)
                    {
                        LandblockStatus lbs = new LandblockStatus()
                        {
                            Id = landblock.Id.ToString(),
                            Creatures = new List<WorldObjectStatus>(),
                            Missiles = new List<WorldObjectStatus>(),
                            Other = new List<WorldObjectStatus>(),
                            Players = new List<WorldObjectStatus>()
                        };
                        foreach (WorldObject worldObject in landblock.GetAllWorldObjectsForDiagnostics())
                        {
                            if (worldObject is Player)
                            {
                                lbs.Players.Add(new WorldObjectStatus() { Id = worldObject.Guid.Full, Name = worldObject.Name });
                            }
                            else if (worldObject is Creature)
                            {
                                lbs.Creatures.Add(new WorldObjectStatus() { Id = worldObject.Guid.Full, Name = worldObject.Name });
                            }
                            else if (worldObject.Missile ?? false)
                            {
                                lbs.Missiles.Add(new WorldObjectStatus() { Id = worldObject.Guid.Full, Name = worldObject.Name });
                            }
                            else
                            {
                                lbs.Other.Add(new WorldObjectStatus() { Id = worldObject.Guid.Full, Name = worldObject.Name });
                            }
                        }
                        lbsl.Add(lbs);
                    }
                    resp.Active = lbsl;
                });
                return resp.AsJsonWebResponse();
            });
        }
    }
}

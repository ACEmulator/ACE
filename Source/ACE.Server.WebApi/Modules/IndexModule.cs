using ACE.Common;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WebApi.Model;
using ACE.Server.WebApi.Util;
using AutoMapper;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using System.IO;
using System.Threading.Tasks;

namespace ACE.Server.WebApi.Modules
{
    public class IndexModule : BaseModule
    {
        public async Task<CharacterListModel> GetModelCharacterListAsync()
        {
            CharacterListModel model = Mapper.Map<CharacterListModel>(BaseModel);
            TaskCompletionSource<object> tsc = new TaskCompletionSource<object>();
            Gate.RunGatedAction(() =>
            {
                DatabaseManager.Shard.GetCharacters(uint.Parse(Context.CurrentUser.FindFirst("AccountId").Value), true, (chars) =>
                {
                    model.Characters = chars;
                    tsc.SetResult(new object());
                });
            });
            await tsc.Task;
            return model;
        }

        public IndexModule()
        {
            this.RequiresAuthentication();

            this.RequiresAnyClaim(
                k => k.Type == AccessLevel.Admin.ToString(),
                k => k.Type == AccessLevel.Advocate.ToString(),
                k => k.Type == AccessLevel.Developer.ToString(),
                k => k.Type == AccessLevel.Envoy.ToString(),
                k => k.Type == AccessLevel.Player.ToString(),
                k => k.Type == AccessLevel.Sentinel.ToString());

            Get("/characters", async (_) => { return (await GetModelCharacterListAsync()).AsJson(); });

            Get("/CharacterBackup", async (_) =>
            {
                CharacterBackupRequestModel request = this.BindAndValidate<CharacterBackupRequestModel>();
                if (!ModelValidationResult.IsValid)
                {
                    return Negotiate.WithModel(ModelValidationResult).WithStatusCode(HttpStatusCode.BadRequest);
                }
                TransferManager.PackageMetadata metadata = new TransferManager.PackageMetadata
                {
                    CharacterId = request.CharacterId,
                    AccountId = uint.Parse(Context.CurrentUser.FindFirst("AccountId").Value),
                    PackageType = TransferManager.PackageType.Backup
                };
                Gate.RunGatedAction(() =>
                {
                    metadata = TransferManager.CreatePackage(metadata).Result;
                });
                if (metadata == null)
                {
                    return Negotiate.WithStatusCode(HttpStatusCode.InternalServerError);
                }
                if (!File.Exists(metadata.FilePath))
                {
                    return Negotiate.WithStatusCode(HttpStatusCode.InternalServerError);
                }
                CharacterBackupResponseModel resp = new CharacterBackupResponseModel
                {
                    SnapshotPackage = File.ReadAllBytes(metadata.FilePath)
                };
                File.Delete(metadata.FilePath);
                return resp.AsJson();
            });

            Get("/CharacterMigrationBegin", async (_) =>
            {
                CharacterMigrationBeginRequestModel request = this.BindAndValidate<CharacterMigrationBeginRequestModel>();
                if (!ModelValidationResult.IsValid)
                {
                    return Negotiate.WithModel(ModelValidationResult).WithStatusCode(HttpStatusCode.BadRequest);
                }
                TransferManager.PackageMetadata metadata = new TransferManager.PackageMetadata
                {
                    CharacterId = request.CharacterId,
                    AccountId = uint.Parse(Context.CurrentUser.FindFirst("AccountId").Value),
                    PackageType = TransferManager.PackageType.Migrate
                };
                Gate.RunGatedAction(() =>
                {
                    metadata = TransferManager.CreatePackage(metadata).Result;
                });
                if (metadata == null)
                {
                    return Negotiate.WithStatusCode(HttpStatusCode.InternalServerError);
                }
                if (!File.Exists(metadata.FilePath))
                {
                    return Negotiate.WithStatusCode(HttpStatusCode.InternalServerError);
                }
                CharacterMigrationBeginResponseModel resp = new CharacterMigrationBeginResponseModel
                {
                    BaseURL = $"https://{ConfigManager.Config.Transfer.ExternalIPAddressOrDNSName}:{ConfigManager.Config.Server.Network.Port + 2}",
                    Cookie = metadata.Cookie
                };
                File.Delete(metadata.FilePath);
                return resp.AsJson();
            });
        }
    }
}

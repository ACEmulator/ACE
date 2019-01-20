using ACE.Common;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.WebApi.Model;
using AutoMapper;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ACE.Server.WebApi.Modules
{
    public class CharacterModule : BaseModule
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
        public CharacterModule()
        {
            this.RequiresAuthentication();

            this.RequiresAnyClaim(
                k => k.Type == AccessLevel.Admin.ToString(),
                k => k.Type == AccessLevel.Advocate.ToString(),
                k => k.Type == AccessLevel.Developer.ToString(),
                k => k.Type == AccessLevel.Envoy.ToString(),
                k => k.Type == AccessLevel.Player.ToString(),
                k => k.Type == AccessLevel.Sentinel.ToString());

            Get("/api/character", async (_) => { return (await GetModelCharacterListAsync()).AsJson(); });

            Get("/api/character/backup", async (_) =>
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
                    return new CharacterBackupResponseModel
                    {
                        Success = false,
                        CharacterId = request.CharacterId
                    }.AsJson();
                }
                if (!File.Exists(metadata.FilePath))
                {
                    return new CharacterBackupResponseModel
                    {
                        Success = false,
                        CharacterId = request.CharacterId
                    }.AsJson();
                }
                CharacterBackupResponseModel resp = new CharacterBackupResponseModel
                {
                    SnapshotPackage = File.ReadAllBytes(metadata.FilePath),
                    Success = true,
                    CharacterId = request.CharacterId
                };
                File.Delete(metadata.FilePath);
                return resp.AsJson();
            });

            Get("/api/character/migrationBegin", async (_) =>
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
                    return new CharacterMigrationBeginResponseModel()
                    {
                        CharacterId = request.CharacterId,
                        Success = false
                    }.AsJson();
                }
                if (!File.Exists(metadata.FilePath))
                {
                    return new CharacterMigrationBeginResponseModel()
                    {
                        CharacterId = request.CharacterId,
                        Success = false
                    }.AsJson();
                }
                CharacterMigrationBeginResponseModel resp = new CharacterMigrationBeginResponseModel
                {
                    BaseURL = $"https://{ConfigManager.Config.Transfer.ExternalIPAddressOrDNSName}:{ConfigManager.Config.Server.Network.Port + 2}",
                    Cookie = metadata.Cookie,
                    Success = true,
                    CharacterId = request.CharacterId
                };
                return resp.AsJson();
            });

            Get("/api/character/migrationCancel", async (_) =>
            {
                CharacterMigrationCancelRequestModel request = this.BindAndValidate<CharacterMigrationCancelRequestModel>();
                if (!ModelValidationResult.IsValid)
                {
                    return Negotiate.WithModel(ModelValidationResult).WithStatusCode(HttpStatusCode.BadRequest);
                }
                TransferManager.PackageMetadata metadata = new TransferManager.PackageMetadata
                {
                    Cookie = request.Cookie,
                    AccountId = uint.Parse(Context.CurrentUser.FindFirst("AccountId").Value)
                };
                TransferManager.MigrateCloseResult result = null;
                Gate.RunGatedAction(() =>
                {
                    result = TransferManager.CloseMigration(metadata, TransferManager.MigrationCloseType.Cancel);
                });
                CharacterMigrationCancelResponseModel resp = new CharacterMigrationCancelResponseModel()
                {
                    Cookie = request.Cookie,
                    Success = result.Success
                };
                return resp.AsJson();
            });

            Get("/api/character/migrationComplete", async (_) =>
            {
                CharacterMigrationCompleteRequestModel request = this.BindAndValidate<CharacterMigrationCompleteRequestModel>();
                if (!ModelValidationResult.IsValid)
                {
                    return Negotiate.WithModel(ModelValidationResult).WithStatusCode(HttpStatusCode.BadRequest);
                }
                TransferManager.PackageMetadata metadata = new TransferManager.PackageMetadata
                {
                    NewCharacterName = request.NewCharacterName,
                    Cookie = request.Cookie,
                    AccountId = uint.Parse(Context.CurrentUser.FindFirst("AccountId").Value),
                    ImportUrl = new Uri(request.BaseURL),
                    PackageType = TransferManager.PackageType.Migrate
                };
                TransferManager.ImportAndMigrateResult result = null;
                Gate.RunGatedAction(() =>
                {
                    result = TransferManager.ImportAndMigrate(metadata);
                });
                return new CharacterMigrationCompleteResponseModel()
                {
                    Cookie = request.Cookie,
                    Success = result.Success,
                    CharacterName = result.NewCharacterName,
                    FailureReason = result.Success ? null : result.FailReason.ToString(),
                    CharacterId = result.NewCharacterId
                }.AsJson();
            });

            Post("/api/character/import", async (_) =>
            {
                CharacterImportRequestModel request = this.BindAndValidate<CharacterImportRequestModel>();
                if (!ModelValidationResult.IsValid)
                {
                    return Negotiate.WithModel(ModelValidationResult).WithStatusCode(HttpStatusCode.BadRequest);
                }
                TransferManager.PackageMetadata metadata = new TransferManager.PackageMetadata
                {
                    NewCharacterName = request.NewCharacterName,
                    AccountId = uint.Parse(Context.CurrentUser.FindFirst("AccountId").Value),
                    PackageType = TransferManager.PackageType.Backup
                };
                TransferManager.ImportAndMigrateResult result = null;
                Gate.RunGatedAction(() =>
                {
                    result = TransferManager.ImportAndMigrate(metadata, Convert.FromBase64String(request.SnapshotPackageBase64));
                });
                return new CharacterImportResponseModel()
                {
                    Success = result.Success,
                    CharacterName = result.NewCharacterName,
                    CharacterId = result.NewCharacterId,
                    FailureReason = result.Success ? null : result.FailReason.ToString()
                }.AsJson();
            });
        }
    }
}

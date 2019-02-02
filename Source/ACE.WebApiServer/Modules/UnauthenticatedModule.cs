using ACE.Common;
using ACE.Server.Command.Handlers;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.WebApiServer.Model;
using ACE.WebApiServer.Model.Character.Migration;
using Nancy;
using Nancy.ModelBinding;

namespace ACE.WebApiServer.Modules
{
    public class UnauthenticatedModule : NancyModule
    {
        public UnauthenticatedModule()
        {
            Get("/api/character/migrationCheck", async (_) =>
            {
                TransferManager.TransferManagerMigrationCheckRequestModel request = this.BindAndValidate<TransferManager.TransferManagerMigrationCheckRequestModel>();
                if (!ModelValidationResult.IsValid)
                {
                    return Negotiate.WithModel(ModelValidationResult).WithStatusCode(HttpStatusCode.BadRequest);
                }
                TransferManager.MigrationReadyStatus readyStatus = TransferManager.MigrationReadyStatus.Unknown;
                Gate.RunGatedAction(() =>
                {
                    readyStatus = TransferManager.CheckReadyStatusOfMigration(request.Cookie);
                }, 1); // inter-server request must use a different queue

                TransferManager.TransferManagerMigrationCheckResponseModel payload = new TransferManager.TransferManagerMigrationCheckResponseModel()
                {
                    Config = new TransferManager.TransferManagerTransferConfigResponseModel()
                    {
                        MyThumbprint = CryptoManager.Thumbprint,
                        AllowImportFrom = ConfigManager.Config.Transfer.AllowImportFrom,
                        AllowMigrationFrom = ConfigManager.Config.Transfer.AllowMigrationFrom,
                        AllowBackup = ConfigManager.Config.Transfer.AllowBackup,
                        AllowImport = ConfigManager.Config.Transfer.AllowImport,
                        AllowMigrate = ConfigManager.Config.Transfer.AllowMigrate,
                    },
                    Cookie = request.Cookie,
                    Nonce = request.Nonce,
                    Ready = readyStatus == TransferManager.MigrationReadyStatus.Ready,
                    ReadyStatus = readyStatus.ToString()
                };

                TransferManager.SignedTransferManagerMigrationCheckResponseModel model = new TransferManager.SignedTransferManagerMigrationCheckResponseModel()
                {
                    Result = payload,
                    Signature = CryptoManager.SignData(ModelTools.ToJson(payload)),
                    Signer = CryptoManager.ExportCertAsBytes()
                };

                return model.AsJsonWebResponse();
            });

            // this needs to be unathenticated
            Get("/api/character/migrationDownload", async (_) =>
            {
                CharacterMigrationDownloadRequestModel request = this.BindAndValidate<CharacterMigrationDownloadRequestModel>();
                if (!ModelValidationResult.IsValid)
                {
                    return Negotiate.WithModel(ModelValidationResult).WithStatusCode(HttpStatusCode.BadRequest);
                }
                TransferManager.PackageMetadata metadata = new TransferManager.PackageMetadata
                {
                    Cookie = request.Cookie
                };
                TransferManager.MigrateCloseResult result = null;
                Gate.RunGatedAction(() =>
                {
                    result = TransferManager.CloseMigration(metadata, TransferManager.MigrationCloseType.Download);
                }, 1); // inter-server request must use a different queue
                CharacterMigrationDownloadResponseModel resp = new CharacterMigrationDownloadResponseModel()
                {
                    Cookie = request.Cookie,
                    SnapshotPackage = result.SnapshotPackage,
                    Success = result.Success
                };
                return resp.AsJsonWebResponse();
            });


            Get("/api/playerCount", async (_) =>
            {
                return new PlayerCountResponseModel()
                {
                    Online = PlayerManager.GetOnlineCount(),
                    Offline = PlayerManager.GetOfflineCount()
                }.AsJsonWebResponse();
            });

            Get("/api/networkStats", async (_) =>
            {
                return new NetworkStatsResponseModel()
                {
                    C2S_CRCErrors_Aggregate = NetworkStatistics.C2S_CRCErrors_Aggregate,
                    C2S_Packets_Aggregate = NetworkStatistics.C2S_Packets_Aggregate,
                    C2S_RequestsForRetransmit_Aggregate = NetworkStatistics.C2S_RequestsForRetransmit_Aggregate,
                    S2C_Packets_Aggregate = NetworkStatistics.S2C_Packets_Aggregate,
                    S2C_RequestsForRetransmit_Aggregate = NetworkStatistics.S2C_RequestsForRetransmit_Aggregate,
                    Summary = NetworkStatistics.Summary()
                }.AsJsonWebResponse();
            });

            Get("/api/serverStatus", async (_) =>
            {
                string resp = null;
                Gate.RunGatedAction(() => resp = AdminCommands.GetServerStatus());
                return resp.AsJsonWebResponse();
            });

            Get("/api/serverInfo", async (_) =>
            {
                ServerInfoResponseModel resp = null;
                Gate.RunGatedAction(() => resp = new ServerInfoResponseModel()
                {
                    PlayerCount = new PlayerCountResponseModel()
                    {
                        Online = PlayerManager.GetOnlineCount(),
                        Offline = PlayerManager.GetOfflineCount()
                    },
                    Welcome = ConfigManager.Config.Server.Welcome,
                    Uptime = Timers.RunningTime,
                    WorldName = ConfigManager.Config.Server.WorldName,
                    Transfers = new TransferManager.TransferManagerTransferConfigResponseModel()
                    {
                        MyThumbprint = CryptoManager.Thumbprint,
                        AllowImportFrom = ConfigManager.Config.Transfer.AllowImportFrom,
                        AllowMigrationFrom = ConfigManager.Config.Transfer.AllowMigrationFrom,
                        AllowBackup = ConfigManager.Config.Transfer.AllowBackup,
                        AllowImport = ConfigManager.Config.Transfer.AllowImport,
                        AllowMigrate = ConfigManager.Config.Transfer.AllowMigrate,
                    },
                    AccountDefaults = ConfigManager.Config.Server.Accounts
                });
                return resp.AsJsonWebResponse();
            });
        }
    }
}

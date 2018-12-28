using ACE.Database;
using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Server.Managers;
using ACE.Server.Network;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACE.Server.Command.Handlers.Processors
{


    internal class InterServer
    {

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void RunExportAsync(Session session)
        {
            Task.Run(() => RunExport(session));
        }
        private async void RunExport(Session session)
        {
            session.WorldBroadcast($"Preparing for character transfer.");
            // log.Info($"Character being exported. name: {session.Player.Name}");
            uint accountId = 1; // session.AccountId
            uint charId = 1342177281; // session.Player.Guid.Full
            CharacterSnapshot snapshot = new CharacterSnapshot() { };
            TaskCompletionSource<object> tsc = new TaskCompletionSource<object>();
            DatabaseManager.Shard.GetCharacters(accountId, false, new Action<List<Character>>(new Action<List<Character>>((chars) =>
            {
                snapshot.Character = chars.FirstOrDefault(k => k.Id == charId);
                DatabaseManager.Shard.GetPossessedBiotasInParallel(snapshot.Character.Id, new Action<PossessedBiotas>((pb) =>
                {
                    snapshot.Character = null;
                   // snapshot.PossessedBiotas = pb;
                    snapshot.PlayerBiotas = DatabaseManager.Shard.GetAllPlayerBiotasInParallel();
                    tsc.SetResult(new object());
                }));
            })));
            await tsc.Task;
            session.WorldBroadcast($"Character snapshot created.  Preparing character transfer.");
            string cookie = await TransferManager.PrepareTransfer(snapshot);
            session.WorldBroadcast($"Character ready for transfer.  A secret cookie has been assigned for this transfer.  " +
                "Use the secret cookie to either download your character or transfer your character to another server by logging into " + "" +
                $"the target server and entering the command (replace <hostname> with the ip address or domain name of THIS server): @import-char ACE://<hostname>/{cookie}");
        }
    }
}

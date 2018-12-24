using ACE.Database;
using ACE.Database.Entity;
using ACE.Database.Models.Shard;
using ACE.Server.Network;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ACE.Server.Command.Handlers.Processors
{
    public class CharacterSnapshot
    {
        public CharacterSnapshot() { }
        public Character Character { get; set; }
        public PossessedBiotas PossessedBiotas { get; set; }
        public List<Biota> PlayerBiotas { get; set; }
    }
    public class CharacterSnapshotWrapper
    {
        public CharacterSnapshot CharacterSnapshot { get; set; }
        public Signature Signature { get; set; }
    }
    public class Signature
    {
        public byte[] Snapshot { get; set; }
    }

    internal class InterServer
    {
        private static readonly JsonSerializerSettings serializationSettings = new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.All
        };
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void RunAsync(Session session)
        {
            Task.Run(() => Run(session));
        }
        private async void Run(Session session)
        {
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
                    snapshot.PossessedBiotas = pb;
                    snapshot.PlayerBiotas = DatabaseManager.Shard.GetAllPlayerBiotasInParallel();
                    tsc.SetResult(new object());
                }));
                
            })));
            await tsc.Task;
            string snapshotSerialized = JsonConvert.SerializeObject(snapshot, serializationSettings);
            System.IO.File.WriteAllText(@"C:\ACE\player.json", snapshotSerialized);
        }
    }

}

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

using log4net;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Network.Handlers
{
    public static class CharacterHandler
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [GameMessage(GameMessageOpcode.CharacterCreate, SessionState.AuthConnected)]
        public static void CharacterCreate(ClientMessage message, Session session)
        {
            string clientString = message.Payload.ReadString16L();

            if (clientString != session.Account)
                return;

            CharacterCreateEx(message, session);
        }

        private static void CharacterCreateEx(ClientMessage message, Session session)
        {
            var characterCreateInfo = new CharacterCreateInfo();
            characterCreateInfo.Unpack(message.Payload);

            // TODO: Check for Banned Name Here
            //DatabaseManager.Shard.IsCharacterNameBanned(characterCreateInfo.Name, isBanned =>
            //{
            //    if (!isBanned)
            //    {
            //        SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameBanned);
            //        return;
            //    }
            //});

            // Disable OlthoiPlay characters for now. They're not implemented yet.
            // FIXME: Restore OlthoiPlay characters when properly handled.
            if (characterCreateInfo.Heritage == (int)HeritageGroup.Olthoi || characterCreateInfo.Heritage == (int)HeritageGroup.OlthoiAcid)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Pending);
                return;
            }

            var isAdmin = characterCreateInfo.IsAdmin && (session.AccessLevel >= AccessLevel.Developer);
            var isEnvoy = characterCreateInfo.IsEnvoy && (session.AccessLevel >= AccessLevel.Sentinel);

            Weenie weenie;
            if (ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (session.AccessLevel >= AccessLevel.Developer && session.AccessLevel <= AccessLevel.Admin)
                    weenie = DatabaseManager.World.GetCachedWeenie("admin");
                else if (session.AccessLevel >= AccessLevel.Sentinel && session.AccessLevel <= AccessLevel.Envoy)
                    weenie = DatabaseManager.World.GetCachedWeenie("sentinel");
                else
                    weenie = DatabaseManager.World.GetCachedWeenie("human");

                if (characterCreateInfo.Heritage == (int)HeritageGroup.Olthoi && weenie.Type == (int)WeenieType.Admin)
                    weenie = DatabaseManager.World.GetCachedWeenie("olthoiadmin");

                if (characterCreateInfo.Heritage == (int)HeritageGroup.OlthoiAcid && weenie.Type == (int)WeenieType.Admin)
                    weenie = DatabaseManager.World.GetCachedWeenie("olthoiacidadmin");
            }
            else
                weenie = DatabaseManager.World.GetCachedWeenie("human");

            if (characterCreateInfo.Heritage == (int)HeritageGroup.Olthoi && weenie.Type == (int)WeenieType.Creature)
                weenie = DatabaseManager.World.GetCachedWeenie("olthoiplayer");

            if (characterCreateInfo.Heritage == (int)HeritageGroup.OlthoiAcid && weenie.Type == (int)WeenieType.Creature)
                weenie = DatabaseManager.World.GetCachedWeenie("olthoiacidplayer");

            if (isEnvoy)
                weenie = DatabaseManager.World.GetCachedWeenie("sentinel");

            if (isAdmin)
                weenie = DatabaseManager.World.GetCachedWeenie("admin");

            if (weenie == null)
                weenie = DatabaseManager.World.GetCachedWeenie("human"); // Default catch-all

            if (weenie == null) // If it is STILL null after the above catchall, the database is missing critical data and cannot continue with character creation.
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.DatabaseDown);
                log.Error("Database does not contain the weenie for human (1). Characters cannot be created until the missing weenie is restored.");
                return;
            }

            // Removes the generic knife and buckler, hidden Javelin, 30 stack of arrows, and 5 stack of coins that are given to all characters
            // Starter Gear from the JSON file are added to the character later in the CharacterCreateEx() process
            weenie.WeeniePropertiesCreateList.Clear();

            var guid = GuidManager.NewPlayerGuid();

            // If Database didn't have Sentinel/Admin weenies, alter the weenietype coming in.
            if (ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (session.AccessLevel >= AccessLevel.Developer && session.AccessLevel <= AccessLevel.Admin && weenie.Type != (int)WeenieType.Admin)
                    weenie.Type = (int)WeenieType.Admin;
                else if (session.AccessLevel >= AccessLevel.Sentinel && session.AccessLevel <= AccessLevel.Envoy && weenie.Type != (int)WeenieType.Sentinel)
                    weenie.Type = (int)WeenieType.Sentinel;
            }


            var result = PlayerFactory.Create(characterCreateInfo, weenie, guid, session.AccountId, out var player);

            if (result != PlayerFactory.CreateResult.Success || player == null)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Corrupt);
                return;
            }


            DatabaseManager.Shard.IsCharacterNameAvailable(characterCreateInfo.Name, isAvailable =>
            {
                if (!isAvailable)
                {
                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);
                    return;
                }

                var possessions = player.GetAllPossessions();
                var possessedBiotas = new Collection<(Biota biota, ReaderWriterLockSlim rwLock)>();
                foreach (var possession in possessions)
                    possessedBiotas.Add((possession.Biota, possession.BiotaDatabaseLock));

                // We must await here -- 
                DatabaseManager.Shard.AddCharacterInParallel(player.Biota, player.BiotaDatabaseLock, possessedBiotas, player.Character, player.CharacterDatabaseLock, saveSuccess =>
                {
                    if (!saveSuccess)
                    {
                        SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.DatabaseDown);
                        return;
                    }

                    PlayerManager.AddOfflinePlayer(player);
                    session.Characters.Add(player.Character);

                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Ok, player.Guid, characterCreateInfo.Name);
                });
            });
        }

        private static void SendCharacterCreateResponse(Session session, CharacterGenerationVerificationResponse response, ObjectGuid guid = default(ObjectGuid), string charName = "")
        {
            session.Network.EnqueueSend(new GameMessageCharacterCreateResponse(response, guid, charName));
        }


        [GameMessage(GameMessageOpcode.CharacterEnterWorldRequest, SessionState.AuthConnected)]
        public static void CharacterEnterWorldRequest(ClientMessage message, Session session)
        {
            session.Network.EnqueueSend(new GameMessageCharacterEnterWorldServerReady());
        }

        [GameMessage(GameMessageOpcode.CharacterEnterWorld, SessionState.AuthConnected)]
        public static void CharacterEnterWorld(ClientMessage message, Session session)
        {
            var guid = message.Payload.ReadUInt32();

            string clientString = message.Payload.ReadString16L();

            if (clientString != session.Account)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            var character = session.Characters.SingleOrDefault(c => c.Id == guid);
            if (character == null)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            if (PlayerManager.GetOnlinePlayer(guid) != null)
            {
                // If this happens, it could be that the previous session for this Player terminated in a way that didn't transfer the player to offline via PlayerManager properly.
                session.SendCharacterError(CharacterError.EnterGameCharacterInWorld);
                return;
            }

            if (PlayerManager.GetOfflinePlayer(guid) == null)
            {
                // This would likely only happen if the account tried to log in a character that didn't exist.
                session.SendCharacterError(CharacterError.EnterGameGeneric);
                return;
            }

            session.InitSessionForWorldLogin();

            session.State = SessionState.WorldConnected;

            WorldManager.PlayerEnterWorld(session, character);
        }


        [GameMessage(GameMessageOpcode.CharacterLogOff, SessionState.WorldConnected)]
        public static void CharacterLogOff(ClientMessage message, Session session)
        {
            session.LogOffPlayer();
        }


        [GameMessage(GameMessageOpcode.CharacterDelete, SessionState.AuthConnected)]
        public static void CharacterDelete(ClientMessage message, Session session)
        {
            string clientString = message.Payload.ReadString16L();
            uint characterSlot = message.Payload.ReadUInt32();

            if (clientString != session.Account)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            var character = session.Characters[(int)characterSlot];
            if (character == null)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            session.Network.EnqueueSend(new GameMessageCharacterDelete());

            var charRestoreTime = PropertyManager.GetLong("char_delete_time", 3600).Item;
            character.DeleteTime = (ulong)(Time.GetUnixTime() + charRestoreTime);
            character.IsDeleted = false;

            DatabaseManager.Shard.SaveCharacter(character, new ReaderWriterLockSlim(), result =>
            {
                if (result)
                {
                    session.Network.EnqueueSend(new GameMessageCharacterList(session.Characters, session));
                    PlayerManager.HandlePlayerDelete(character.Id);
                }
                else
                    session.SendCharacterError(CharacterError.Delete);
            });
        }

        [GameMessage(GameMessageOpcode.CharacterRestore, SessionState.AuthConnected)]
        public static void CharacterRestore(ClientMessage message, Session session)
        {
            var guid = message.Payload.ReadUInt32();

            var character = session.Characters.SingleOrDefault(c => c.Id == guid);
            if (character == null)
                return;

            DatabaseManager.Shard.IsCharacterNameAvailable(character.Name, isAvailable =>
            {
                if (!isAvailable)
                {
                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);
                }
                else
                {
                    character.DeleteTime = 0;
                    character.IsDeleted = false;

                    DatabaseManager.Shard.SaveCharacter(character, new ReaderWriterLockSlim(), result =>
                    {
                        var name = character.Name;

                        if (ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions && session.AccessLevel > AccessLevel.Advocate)
                            name = "+" + name;
                        else if (!ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions && character.IsPlussed)
                            name = "+" + name;

                        if (result)
                            session.Network.EnqueueSend(new GameMessageCharacterRestore(guid, name, 0u));
                        else
                            SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Corrupt);
                    });
                }
            });
        }
    }
}

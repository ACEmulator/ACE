using System;
using System.Globalization;
using System.Threading;
using ACE.Common;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.WorldObjects;

namespace ACE.Server.Command.Handlers
{
    public static class CharacterCommands
    {
        // set-characteraccess charactername (accesslevel)
        [CommandHandler("set-characteraccess", AccessLevel.Admin, CommandHandlerFlag.None, 1,
            "Sets the access level for the character",
            "charactername (accesslevel)\n" +
            "accesslevel can be a number or enum name\n" +
            "0 = Player | 1 = Advocate | 2 = Sentinel | 3 = Envoy | 4 = Developer | 5 = Admin")]
        public static void HandleCharacterTokenization(Session session, params string[] parameters)
        {
            string characterName = parameters[0];

            AccessLevel accessLevel = AccessLevel.Player;
            if (parameters.Length > 1)
                if (Enum.TryParse(parameters[1], true, out accessLevel))
                    if (!Enum.IsDefined(typeof(AccessLevel), accessLevel))
                        accessLevel = AccessLevel.Player;

            DatabaseManager.Shard.SetCharacterAccessLevelByName(characterName.ToLower(), accessLevel, ((uint characterId) =>
            {
                if (characterId > 0)
                {
                    string articleAorAN = "a";
                    if (accessLevel == AccessLevel.Advocate || accessLevel == AccessLevel.Admin || accessLevel == AccessLevel.Envoy)
                        articleAorAN = "an";

                    CommandHandlerHelper.WriteOutputInfo(session, "Character " + characterName + " has been made " + articleAorAN + " " + Enum.GetName(typeof(AccessLevel), accessLevel) + ".", ChatMessageType.Broadcast);
                }
                else
                {
                    CommandHandlerHelper.WriteOutputInfo(session, "There is no character by the name of " + characterName + " found in the database. Has it been deleted?", ChatMessageType.Broadcast);
                }
            }));
        }

        // deletecharacter character name
        [CommandHandler("deletecharacter", AccessLevel.Admin, CommandHandlerFlag.None, 1,
            "Deletes a character and removes it from players restore list",
            "[character name]\n" +
            "Given the name of a character, this command deletes that character, booting it if in game, and removes it from character's restore list.  (You can find the name for a character using the @finger command.)\n")]
        public static void HandleCharacterForcedDelete(Session session, params string[] parameters)
        {
            var characterName = string.Join(" ", parameters);

            var foundPlayer = PlayerManager.FindByName(characterName, out var isOnline);

            if (foundPlayer == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"There is no character named {characterName} in the database.", ChatMessageType.Broadcast);
                return;
            }

            if (isOnline && foundPlayer is Player player)
            {
                player.Character.DeleteTime = (ulong)Time.GetUnixTime();
                player.Character.IsDeleted = true;
                player.CharacterChangesDetected = true;                
                player.Session.LogOffPlayer(true);
                PlayerManager.HandlePlayerDelete(player.Character.Id);

                var success = PlayerManager.ProcessDeletedPlayer(player.Character.Id);
                if (success)
                    CommandHandlerHelper.WriteOutputInfo(session, $"Successfully {(isOnline ? "booted and " : "")}deleted character {foundPlayer.Name} (0x{foundPlayer.Guid}).", ChatMessageType.Broadcast);
                else
                    CommandHandlerHelper.WriteOutputInfo(session, $"Unable to {(isOnline ? "boot and " : "")}delete character {foundPlayer.Name} (0x{foundPlayer.Guid}) due to PlayerManager failure.", ChatMessageType.Broadcast);
            }
            else
            {
                var existingCharId = foundPlayer.Guid.Full; //DatabaseManager.Shard.BaseDatabase.GetCharacterStubByName(foundPlayer.Name).Id;

                DatabaseManager.Shard.GetCharacter(existingCharId, character =>
                {
                    if (character != null)
                    {
                        character.DeleteTime = (ulong)Time.GetUnixTime();
                        character.IsDeleted = true;
                        DatabaseManager.Shard.SaveCharacter(character, new ReaderWriterLockSlim(), result =>
                        {
                            if (result)
                            {
                                var deleteOfflineChain = new ActionChain();
                                deleteOfflineChain.AddAction(WorldManager.ActionQueue, () => PlayerManager.HandlePlayerDelete(character.Id));
                                deleteOfflineChain.AddDelayForOneTick();
                                deleteOfflineChain.AddAction(WorldManager.ActionQueue, () =>
                                {
                                    var success = PlayerManager.ProcessDeletedPlayer(character.Id);
                                    if (success)
                                        CommandHandlerHelper.WriteOutputInfo(session, $"Successfully {(isOnline ? "booted and " : "")}deleted character {foundPlayer.Name} (0x{foundPlayer.Guid}).", ChatMessageType.Broadcast);
                                    else
                                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to {(isOnline ? "boot and " : "")}delete character {foundPlayer.Name} (0x{foundPlayer.Guid}) due to PlayerManager failure.", ChatMessageType.Broadcast);
                                });
                                deleteOfflineChain.EnqueueChain();
                            }
                            else
                                CommandHandlerHelper.WriteOutputInfo(session, $"Unable to {(isOnline ? "boot and " : "")}delete character {foundPlayer.Name} (0x{foundPlayer.Guid}) due to shard database SaveCharacter failure.", ChatMessageType.Broadcast);
                        });
                    }
                    else
                        CommandHandlerHelper.WriteOutputInfo(session, $"Unable to {(isOnline ? "boot and " : "")}delete character {foundPlayer.Name} (0x{foundPlayer.Guid}) due to shard database GetCharacter failure.", ChatMessageType.Broadcast);
                });
            }
        }
    }
}

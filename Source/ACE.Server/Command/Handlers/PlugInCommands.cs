using System;
using System.Linq;
using System.Collections.Generic;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Database.Models.Auth;

using Microsoft.EntityFrameworkCore;

namespace ACE.Server.Command.Handlers
{
    public class PlugInCommands
    {
        /// <summary>
        /// Command to send all your attackable status. For the ACE Admin plugin
        /// </summary>
        /// <remarks>
        ///   Currently this is the only thing we need on login, if more are added we will change this command and group everything in one message.
        /// </remarks>
        [CommandHandler("attackableStatus", AccessLevel.Advocate, CommandHandlerFlag.RequiresWorld, 0, "Gets your Attackable Status")]
        public static void HandleAttackableStatus(Session session, params string[] parameters)
        {
            var aparam = Convert.ToString(session.Player.Attackable.Value);
            session.Network.EnqueueSend(new GameMessageSystemChat("#AceAdmin.attackable="+aparam, ChatMessageType.Broadcast));
        }

        /// <summary>
        /// Command to send all Points of Interests in the DB. For the ACE Admin plugin.
        /// </summary>
        [CommandHandler("AceAlistpoi", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Sends all Points of Interests to the ACE Admin plugin.")]
        public static void HandleListPois(Session session, params string[] parameters)
        {
            string message = "#AceAdmin.PoiList=";

            foreach (PointsOfInterest poi in DatabaseManager.World.GetAllPointsOfInterests())
                      {
                message += $"{poi.Name}|";
            }

            // remove the "|" from the end of the message.
            message = message.Remove(message.Length - 1);

            var listPlayersMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
            session.Network.EnqueueSend(listPlayersMessage);

        }

        /// <summary>
        /// Command to send all accounts in the DB. 
        /// </summary>
        [CommandHandler("AceAlistAllAccounts", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Sends list of all Accounts and Account Access Level to the ACE Admin plugin.")]
        public static void HandleListAllAccoounts(Session session, params string[] parameters)
        { 
            string message = "#AceAdmin.AccountList=";

            foreach (Account account in DatabaseManager.Authentication.GetAllAccounts())
            {
                message += $"{account.AccountName}:{account.AccessLevel}|";
            }
            
            // remove the "|" from the end of the message.
            message = message.Remove(message.Length - 1);

            var listPlayersMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
            session.Network.EnqueueSend(listPlayersMessage);

        }

        /// <summary>
        /// Command to send all active players connected to the server. For the ACE Admin plugin
        /// </summary>
        [CommandHandler("AceAlistLoggedOnplayers", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Sends all the players {name|True if online|AccessLevel|AccountName} connected to the server to the ACE Admin plugin.")]
        public static void HandleListPlayers(Session session, params string[] parameters)
        {
            string message = "#AceAdmin.PlayerList=";

            foreach (Session playerSession in WorldManager.GetAll(true))
            {
                message += $"{playerSession.Player.Name}:True:{playerSession.AccessLevel}:{playerSession.Account}|";

            }

            // remove the "|" from the end of the message.
            message = message.Remove(message.Length - 1);

            var listPlayersMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
            session.Network.EnqueueSend(listPlayersMessage);

        }

        /// <summary>
        /// Command to send all players in the DB. For the ACE Admin plugin
        /// </summary>
        [CommandHandler("AceAlistAllPlayers", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 0,
            "Sends ALL players {name|True if online|AccessLevel|AccountName} in the DB to the ACE Admin plugin.")]
        public static void HandleListAllPlayers(Session session, params string[] parameters)
        {
            string message = "#AceAdmin.PlayerList=";

            foreach (Character character in GetAllCharactersFromDB())
            {
                string accname = DatabaseManager.Authentication.GetAccountById(character.AccountId).AccountName;
                uint accessLevel = DatabaseManager.Authentication.GetAccountById(character.AccountId).AccessLevel;
                string CharName = null;

                if (accessLevel > 4)
                    CharName = "+" + character.Name;
                else
                    CharName = character.Name;

                string onLine = "False";
                Session playerSession = WorldManager.FindByPlayerName(CharName);
                
                if (playerSession != null) onLine = "True";

                message += $"{CharName}:{onLine}:{accessLevel.ToString()}:{accname}|";
                
            }

            // remove the "|" from the end of the message.
            message = message.Remove(message.Length - 1);

            var listPlayersMessage = new GameMessageSystemChat(message, ChatMessageType.Broadcast);
            session.Network.EnqueueSend(listPlayersMessage);

        }


        public static List<Character> GetAllCharactersFromDB()
        {
            using (var context = new ShardDbContext())
            {
                var results = context.Character.
                    AsNoTracking();

                if (results != null)
                    return results.ToList();

            }

            return null;
        }


    }
}

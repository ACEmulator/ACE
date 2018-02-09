using System;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Command.Handlers
{
    public static class ShardCommands
    {
        // // commandname parameters
        // [CommandHandler("commandname", AccessLevel.Admin, CommandHandlerFlag.RequiresWorld, 0)]
        // public static void HandleHelp(Session session, params string[] parameters)
        // {
        //     //TODO: output
        // }
   
        /// <summary>
        /// Cancels an in-progress shutdown event.
        /// </summary>
        [CommandHandler("cancel-shutdown", AccessLevel.Admin, CommandHandlerFlag.None, 0,
            "Stops an active server shutdown.",
            "")]
        public static void HandleCancelShutdown(Session session, params string[] parameters)
        {
            ServerManager.CancelShutdown();
        }

        /// <summary>
        /// Increase or decrease the server shutdown interval in seconds
        /// </summary>
        [CommandHandler("set-shutdown-interval", AccessLevel.Admin, CommandHandlerFlag.None, 1,
            "Changes the delay before the server will shutdown.",
            "< 0-99999 >")]
        public static void HandleSetShutdownInterval(Session session, params string[] parameters)
        {
            if (parameters?.Length > 0)
            {
                // delay server shutdown for up to x minutes
                // limit to uint length 65535
                string parseInt = parameters[0].Length > 5 ? parameters[0].Substring(0, 5) : parameters[0];
                if (uint.TryParse(parseInt, out var newShutdownInterval))
                {
                    // newShutdownInterval is represented as a time element
                    if (newShutdownInterval > uint.MaxValue) newShutdownInterval = uint.MaxValue;

                    // set the interval
                    ServerManager.SetShutdownInterval(Convert.ToUInt32(newShutdownInterval));

                    // message the admin
                    ChatPacket.SendServerMessage(session, $"Shutdown Interval (seconds to shutdown server) has been set to {ServerManager.ShutdownInterval}.", ChatMessageType.Broadcast);
                    return;
                }
            }
            ChatPacket.SendServerMessage(session, "Usage: /change-shutdown-interval <00000>", ChatMessageType.Broadcast);
        }

        /// <summary>
        /// Immediately begins the shutdown process by setting the shutdown interval to 0 before executing the shutdown method
        /// </summary>
        [CommandHandler("stop-now", AccessLevel.Admin, CommandHandlerFlag.None, -1,
            "Shuts the server down, immediately!",
            "\nThis command will attempt to safely logoff all players, before shutting down the server.")]
        public static void ShutdownServerNow(Session session, params string[] parameters)
        {
            ServerManager.SetShutdownInterval(0);
            ShutdownServer(session, parameters);
        }

        /// <summary>
        /// Function to shutdown the server from console or in-game.
        /// </summary>
        [CommandHandler("shutdown", AccessLevel.Admin, CommandHandlerFlag.None, 0,
            "Begins the server shutdown process. Optionally displays a shutdown message, if a string is passed.",
            "< Optional Shutdown Message >\n" +
            "\tUse @cancel-shutdown too abort an active shutdown!\n" +
            "\tSet the shutdown delay with @set-shutdown-interval < 0-99999 >")]
        public static void ShutdownServer(Session session, params string[] parameters)
        {
            // inform the world that a shutdown is about to take place
            string shutdownInitiator = (session == null ? "Server" : session.Player.Name);
            string shutdownText = "";
            string adminShutdownText = "";
            TimeSpan timeTillShutdown = TimeSpan.FromSeconds(ServerManager.ShutdownInterval);
            string timeRemaining = (timeTillShutdown.TotalSeconds > 120 ? $"The server will go down in {(int)timeTillShutdown.TotalMinutes} minutes."
                : $"The server will go down in {timeTillShutdown.TotalSeconds} seconds.");

            // add admin shutdown text
            if (parameters?.Length > 0)
            {
                foreach (var word in parameters)
                {
                    if (adminShutdownText.Length > 0)
                        adminShutdownText += " " + word;
                    else
                        adminShutdownText += word;
                }
            }

            shutdownText += $"{shutdownInitiator} initiated a complete server shutdown @ {DateTime.UtcNow} UTC";

            // output to console (log in the future)
            Console.WriteLine(shutdownText);
            Console.WriteLine(timeRemaining);

            if (adminShutdownText.Length > 0)
                Console.WriteLine("Admin message: " + adminShutdownText);

            // send a message to each player that the server will go down in x interval
            foreach (var player in WorldManager.GetAll())
            {
                // send server shutdown message and time remaining till shutdown
                player.Network.EnqueueSend(new GameMessageSystemChat(shutdownText + "\n" + timeRemaining, ChatMessageType.Broadcast));

                if (adminShutdownText.Length > 0)
                    player.Network.EnqueueSend(new GameMessageSystemChat($"Message from {shutdownInitiator}: {adminShutdownText}", ChatMessageType.Broadcast));
            }
            ServerManager.BeginShutdown();
        }
    }
}

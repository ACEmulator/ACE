using System;

using log4net;

using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Command.Handlers
{
    public static class AdminShardCommands
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            var adminName = (session == null) ? "CONSOLE" : session.Player.Name;
            var msg = $"{adminName} has requested the pending shut down @ {ServerManager.ShutdownTime.ToLocalTime()} ({ServerManager.ShutdownTime} UTC) be cancelled.";
            log.Info(msg);
            PlayerManager.BroadcastToAuditChannel(session?.Player, msg);

            ServerManager.CancelShutdown();
        }

        /// <summary>
        /// Increase or decrease the server shutdown interval in seconds
        /// </summary>
        [CommandHandler("set-shutdown-interval", AccessLevel.Admin, CommandHandlerFlag.None, 1,
            "Changes the delay, in seconds, before the server will shutdown.",
            "< 0-99999 > in seconds")]
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

                    var adminName = (session == null) ? "CONSOLE" : session.Player.Name;
                    var msg = $"{adminName} has requested the shut down interval be changed from {ServerManager.ShutdownInterval} seconds to {newShutdownInterval} seconds.";
                    //log.Info(msg);
                    PlayerManager.BroadcastToAuditChannel(session?.Player, msg);

                    // set the interval
                    ServerManager.SetShutdownInterval(Convert.ToUInt32(newShutdownInterval));

                    // message the admin
                    CommandHandlerHelper.WriteOutputInfo(session, $"Shutdown Interval (seconds to shutdown server) has been set to {ServerManager.ShutdownInterval}.", ChatMessageType.Broadcast);
                    return;
                }
            }
            CommandHandlerHelper.WriteOutputInfo(session, "Usage: /set-shutdown-interval <00000>", ChatMessageType.Broadcast);
        }

        /// <summary>
        /// Immediately begins the shutdown process by setting the shutdown interval to 0 before executing the shutdown method
        /// </summary>
        [CommandHandler("stop-now", AccessLevel.Admin, CommandHandlerFlag.None, -1,
            "Shuts the server down, immediately!",
            "\nThis command will attempt to safely logoff all players, before shutting down the server.")]
        public static void ShutdownServerNow(Session session, params string[] parameters)
        {
            var adminName = (session == null) ? "CONSOLE" : session.Player.Name;
            var msg = $"{adminName} has initiated an immediate server shut down.";
            //log.Info(msg);
            PlayerManager.BroadcastToAuditChannel(session?.Player, msg);

            ServerManager.SetShutdownInterval(0);
            ShutdownServer(session, parameters);
        }

        /// <summary>
        /// Function to shutdown the server from console or in-game.
        /// </summary>
        [CommandHandler("shutdown", AccessLevel.Admin, CommandHandlerFlag.None, 0,
            "Begins the server shutdown process. Optionally displays a shutdown message, if a string is passed.",
            "< Optional Shutdown Message >\n" +
            "\tUse @cancel-shutdown to abort an active shutdown!\n" +
            "\tSet the shutdown delay in seconds with @set-shutdown-interval < 0-99999 >")]
        public static void ShutdownServer(Session session, params string[] parameters)
        {
            var adminText = "";
            if (parameters.Length > 0)
                adminText = string.Join(" ", parameters);

            var adminName = (session == null) ? "CONSOLE" : session.Player.Name;
            var hideName = string.IsNullOrEmpty(adminText);

            var timeTillShutdown = TimeSpan.FromSeconds(ServerManager.ShutdownInterval);
            var timeRemaining = "The server will shut down in " + (timeTillShutdown.TotalSeconds > 120 ? $"{(int)timeTillShutdown.TotalMinutes} minutes." : $"{timeTillShutdown.TotalSeconds} seconds.");

            log.Info($"{adminName} initiated a complete server shutdown @ {DateTime.Now} ({DateTime.UtcNow} UTC)");
            log.Info(timeRemaining);
            PlayerManager.BroadcastToAuditChannel(session?.Player, $"{adminName} initiated a complete server shutdown @ {DateTime.Now} ({DateTime.UtcNow} UTC)");

            if (adminText.Length > 0)
            {
                log.Info("Admin message: " + adminText);
                PlayerManager.BroadcastToAuditChannel(session?.Player, $"{adminName} sent the following message for the shutdown: {adminText}");
            }

            var sdt = timeTillShutdown;
            var timeHrs = $"{(sdt.Hours >= 1 ? $"{sdt.ToString("%h")}" : "")}{(sdt.Hours >= 2 ? $" hours" : sdt.Hours == 1 ? " hour" : "")}";
            var timeMins = $"{(sdt.Minutes != 0 ? $"{sdt.ToString("%m")}" : "")}{(sdt.Minutes >= 2 ? $" minutes" : sdt.Minutes == 1 ? " minute" : "")}";
            var timeSecs = $"{(sdt.Seconds != 0 ? $"{sdt.ToString("%s")}" : "")}{(sdt.Seconds >= 2 ? $" seconds" : sdt.Seconds == 1 ? " second" : "")}";
            var time = $"{(timeHrs != "" ? timeHrs : "")}{(timeMins != "" ? $"{((timeHrs != "") ? ", " : "")}" + timeMins : "")}{(timeSecs != "" ? $"{((timeHrs != "" || timeMins != "") ? " and " : "")}" + timeSecs : "")}";

            if (adminName.Equals("CONSOLE"))
                adminName = "System";

            var genericMsgToPlayers = $"Broadcast from {(hideName ? "System": $"{adminName}")}> {(timeTillShutdown.TotalMinutes > 1.5 ? "ATTENTION" : "WARNING")} - This Asheron's Call Server is shutting down in {time}.{(timeTillShutdown.TotalMinutes <= 3 ? " Please log out." : "")}";

            if (sdt.TotalMilliseconds == 0)
                genericMsgToPlayers = $"Broadcast from {(hideName ? "System" : $"{adminName}")}> ATTENTION - This Asheron's Call Server is shutting down NOW!!!!";

            if (!hideName)
                PlayerManager.BroadcastToAll(new GameMessageSystemChat($"Broadcast from {adminName}> {adminText}\n" + genericMsgToPlayers, ChatMessageType.WorldBroadcast));
            else
                PlayerManager.BroadcastToAll(new GameMessageSystemChat(genericMsgToPlayers, ChatMessageType.WorldBroadcast));

            ServerManager.BeginShutdown();
        }

        [CommandHandler("world", AccessLevel.Admin, CommandHandlerFlag.None, 0,
            "Open or Close world to player access.",
            "[open | close] <boot>\nIf closing world, using @world close boot will force players to logoff immediately")]
        public static void HandleHelp(Session session, params string[] parameters)
        {
            var open = false;
            var close = false;
            var bootPlayers = false;

            var message = $"World is currently {WorldManager.WorldStatus.ToString()}\nPlease specify state to change\n@world [open | close] <boot>\nIf closing world, using @world close boot will force players to logoff immediately";
            if (parameters.Length >= 1)
            {
                switch (parameters[0].ToLower())
                {
                    case "open":
                        if (WorldManager.WorldStatus != WorldManager.WorldStatusState.Open)
                        {                            
                            message = "Opening world to players...";
                            open = true;
                        }
                        else
                        {
                            message = "World is already open.";
                        }
                        break;
                    case "close":
                        if (WorldManager.WorldStatus != WorldManager.WorldStatusState.Closed)
                        {
                            if (parameters.Length > 1)
                            {
                                if (parameters[1].ToLower() == "boot")
                                    bootPlayers = true;
                            }
                            message = "Closing world";
                            if (bootPlayers)
                                message += ", and booting all online players.";
                            else
                                message += "...";

                            close = true;
                        }
                        else
                        {
                            message = "World is already closed.";
                        }
                        break;
                }
            }

            CommandHandlerHelper.WriteOutputInfo(session, message, ChatMessageType.WorldBroadcast);

            if (open)
                WorldManager.Open(session == null ? null : session.Player);
            else if (close)
                WorldManager.Close(session == null ? null : session.Player, bootPlayers);
        }
    }
}

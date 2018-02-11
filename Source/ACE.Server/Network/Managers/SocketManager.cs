using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using ACE.Common;
using log4net;

namespace ACE.Server.Network.Managers
{
    public static class SocketManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly List<ConnectionListener> Listeners = new List<ConnectionListener>();

        public static void Initialize()
        {
            IPAddress host;

            try
            {
                host = IPAddress.Parse(ConfigManager.Config.Server.Network.Host);
            }
            catch (Exception ex)
            {
                log.Error($"Unable to use {ConfigManager.Config.Server.Network.Host} as host due to: {ex}");
                log.Error("Using IPAddress.Any as host instead.");
                host = IPAddress.Any;
            }

            Listeners.Add(new ConnectionListener(host, ConfigManager.Config.Server.Network.Port));
            log.Info($"Binding ConnectionListener to {host}:{ConfigManager.Config.Server.Network.Port}");
            Listeners.Add(new ConnectionListener(host, ConfigManager.Config.Server.Network.Port + 1));
            log.Info($"Binding ConnectionListener to {host}:{ConfigManager.Config.Server.Network.Port + 1}");

            foreach (var listener in Listeners)
                listener.Start();
        }

        public static Socket GetSocket(int id = 1)
        {
            return Listeners[id].Socket;
        }
    }
}

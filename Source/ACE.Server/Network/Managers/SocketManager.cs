using System;
using System.Net;
using System.Net.Sockets;

using log4net;

using ACE.Common;

namespace ACE.Server.Network.Managers
{
    public static class SocketManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ConnectionListener[] listeners = new ConnectionListener[2];

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

            listeners[0] = new ConnectionListener(host, ConfigManager.Config.Server.Network.Port);
            log.Info($"Binding ConnectionListener to {host}:{ConfigManager.Config.Server.Network.Port}");

            listeners[1] = new ConnectionListener(host, ConfigManager.Config.Server.Network.Port + 1);
            log.Info($"Binding ConnectionListener to {host}:{ConfigManager.Config.Server.Network.Port + 1}");

            listeners[0].Start();
            listeners[1].Start();
        }

        /// <summary>
        /// We use a single socket because the use of dual unidirectional sockets doesn't work for some client firewalls
        /// </summary>
        public static Socket GetMainSocket()
        {
            return listeners[0].Socket;
        }
    }
}

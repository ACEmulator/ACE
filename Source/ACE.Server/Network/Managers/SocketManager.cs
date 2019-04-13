using System;
using System.Net;
using System.Net.Sockets;

using log4net;

using ACE.Common;

namespace ACE.Server.Network.Managers
{
    /// <summary>
    /// We use a single socket because the use of dual unidirectional sockets doesn't work for some client firewalls
    /// </summary>
    public static class SocketManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly ConnectionListener[] listeners = new ConnectionListener[2];

        public static InboundPacketQueue InboundQueue { get; private set; }
        public static OutboundPacketQueue OutboundQueue { get; private set; }

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

            InboundQueue = new InboundPacketQueue();

            listeners[0] = new ConnectionListener(host, ConfigManager.Config.Server.Network.Port);
            log.Info($"Binding ConnectionListener to {host}:{ConfigManager.Config.Server.Network.Port}");

            listeners[1] = new ConnectionListener(host, ConfigManager.Config.Server.Network.Port + 1);
            log.Info($"Binding ConnectionListener to {host}:{ConfigManager.Config.Server.Network.Port + 1}");

            listeners[0].Start();
            listeners[1].Start();

            OutboundQueue = new OutboundPacketQueue(listeners[0].Socket);
        }
        public static void Shutdown()
        {
            InboundQueue.Shutdown();
        }
    }
}

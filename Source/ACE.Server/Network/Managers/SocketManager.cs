using System;
using System.Collections.Generic;
using System.Net;

using log4net;

using ACE.Common;

namespace ACE.Server.Network.Managers
{
    public static class SocketManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static ConnectionListener[] listeners;

        public static void Initialize()
        {
            var hosts = new List<IPAddress>();

            try
            {
                var splits = ConfigManager.Config.Server.Network.Host.Split(",");

                foreach (var split in splits)
                    hosts.Add(IPAddress.Parse(split));
            }
            catch (Exception ex)
            {
                log.Error($"Unable to use {ConfigManager.Config.Server.Network.Host} as host due to: {ex}");
                log.Error("Using IPAddress.Any as host instead.");
                hosts.Clear();
                hosts.Add(IPAddress.Any);
            }

            listeners = new ConnectionListener[hosts.Count * 2];

            for (int i = 0; i < hosts.Count; i++)
            {
                listeners[(i * 2) + 0] = new ConnectionListener(hosts[i], ConfigManager.Config.Server.Network.Port);
                log.Info($"Binding ConnectionListener to {hosts[i]}:{ConfigManager.Config.Server.Network.Port}");

                listeners[(i * 2) + 1] = new ConnectionListener(hosts[i], ConfigManager.Config.Server.Network.Port + 1);
                log.Info($"Binding ConnectionListener to {hosts[i]}:{ConfigManager.Config.Server.Network.Port + 1}");

                listeners[(i * 2) + 0].Start();
                listeners[(i * 2) + 1].Start();
            }
        }
    }
}

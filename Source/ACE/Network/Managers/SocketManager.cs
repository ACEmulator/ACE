using System.Collections.Generic;
using System.Net.Sockets;

using ACE.Common;

namespace ACE.Network.Managers
{
    public static class SocketManager
    {
        private static readonly List<ConnectionListener> Listeners = new List<ConnectionListener>();

        public static void Initialise()
        {
            Listeners.Add(new ConnectionListener(ConfigManager.Config.Server.Network.Port));
            Listeners.Add(new ConnectionListener(ConfigManager.Config.Server.Network.Port + 1));

            foreach (var loginListener in Listeners)
                loginListener.Start();
        }

        public static Socket GetSocket()
        {
            return Listeners[0].Socket;
        }
    }
}

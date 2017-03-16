using System.Collections.Generic;
using System.Net.Sockets;

using ACE.Common;

namespace ACE.Network.Managers
{
    public static class SocketManager
    {
        private static List<ConnectionListener> loginListeners = new List<ConnectionListener>();

        public static void Initialise()
        {
            loginListeners.Add(new ConnectionListener(ConfigManager.Config.Server.Network.Port, ConnectionType.Login));
            loginListeners.Add(new ConnectionListener(ConfigManager.Config.Server.Network.Port + 1, ConnectionType.Login));

            foreach (var loginListener in loginListeners)
                loginListener.Start();
        }
        
        public static Socket GetSocket()
        {
            return loginListeners[0].Socket;
        }
    }
}

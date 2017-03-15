using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Common.Extensions;
using ACE.Common;
using ACE.Common.Cryptography;

namespace ACE.Network.Managers
{
    public static class SocketManager
    {
        private static List<ConnectionListener> loginListeners = new List<ConnectionListener>();

        public static void Initialise()
        {
            loginListeners.Add(new ConnectionListener(ConfigManager.Config.Server.Network.LoginPort, ConnectionType.Login));
            loginListeners.Add(new ConnectionListener(ConfigManager.Config.Server.Network.LoginPort + 1, ConnectionType.Login));

            foreach (var loginListener in loginListeners)
                loginListener.Start();
        }
        
        public static Socket GetSocket()
        {
            return loginListeners[0].Socket;
        }
    }
}

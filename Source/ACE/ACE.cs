using System;

using ACE.Command;
using ACE.Common;
using ACE.Database;
using ACE.Managers;
using ACE.Network.Managers;

namespace ACE
{
    public class ACE
    {
        public static void Main(string[] args)
        {
            Console.Title = "ACEmulator";

            ConfigManager.Initialise();
            DatabaseManager.Initialise();
            AssetManager.Initialise();
            InboundMessageManager.Initialise();
            SocketManager.Initialise();
            WorldManager.Initialise();
            CommandManager.Initialise();
        }
    }
}

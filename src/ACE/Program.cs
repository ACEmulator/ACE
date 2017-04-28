using System;
using ACE.Command;
using ACE.Common;
using ACE.Database;
using ACE.DatLoader;
using ACE.Managers;
using ACE.Network.Managers;

namespace ACE
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ACEmulator";

            ConfigManager.Initialise();
            DatabaseManager.Initialise();
            AssetManager.Initialise();
            InboundMessageManager.Initialise();
            DatManager.Initialize();
            SocketManager.Initialise();
            WorldManager.Initialise();
            CommandManager.Initialise();
        }
    }
}

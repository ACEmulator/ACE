using System;

using ACE.Command;
using ACE.Common;
using ACE.Database;
using ACE.Managers;
using ACE.Network.Managers;

namespace ACE
{
    class ACE
    {
        static void Main(string[] args)
        {
            Console.Title = "ACEmulator";

            ConfigManager.Initialise();
            DatabaseManager.Initialise();
            AssetManager.Initialise();
            PacketManager.Initialise();
            NetworkManager.Initialise();
            WorldManager.Initialise();
            CommandManager.Initialise();
        }
    }
}

using ACE.Command;
using ACE.Database;
using ACE.Managers;
using ACE.Network;
using System;

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

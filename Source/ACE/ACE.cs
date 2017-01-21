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
            PacketManager.Initialise();
            NetworkMgr.Initialise();
            WorldManager.Initalise();
            CommandManager.Initialise();
        }
    }
}

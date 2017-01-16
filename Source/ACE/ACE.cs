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

            PacketManager.Initialise();
            NetworkMgr.Initialise();
            WorldManager.Initalise();
        }
    }
}

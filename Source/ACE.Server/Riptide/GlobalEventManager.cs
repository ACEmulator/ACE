using System;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.WorldObjects;

namespace ACE.Server.Riptide
{
    //public class GlobalEventFlags
    //{
    //    public bool PK_DEATH_ANNOUNCEMENT = true;
    //}
    public static class GlobalEventManager
    {
        //public static GlobalEventFlags Enabled = new GlobalEventFlags();

        public static void GlobalWorldBroadcast(string message)
        {
            foreach (var player in PlayerManager.GetAllOnline())
            {
                player.Session.WorldBroadcast(message);
            }
        }
        public static void OnPKDeath(Player killer, Player victim, DeathMessage deathMessage)
        {
            try
            {
                if (killer != null && victim != null)
                {
                    string msg = string.Format(deathMessage.Broadcast, victim.Name, killer.Name);
                    foreach (var player in PlayerManager.GetAllOnline())
                    {
                        if (player != killer && player != victim)
                        {
                            player.Session.WorldBroadcast(msg);
                        }
                    }
                    //GlobalWorldBroadcast(msg);
                }
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

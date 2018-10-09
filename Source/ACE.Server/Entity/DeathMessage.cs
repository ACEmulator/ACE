using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.Entity
{
    public class DeathMessage
    {
        public string Killer;
        public string Victim;

        public string Broadcast;

        public DeathMessage(string killer, string victim, string broadcast)
        {
            Killer = killer;
            Victim = victim;

            Broadcast = broadcast;
        }
    }
}

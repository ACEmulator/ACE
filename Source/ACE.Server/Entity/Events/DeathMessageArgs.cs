using System;
using ACE.Entity;

namespace ACE.Server.Entity.Events
{
    public class DeathMessageArgs : EventArgs
    {
        public string Message { get; set; }

        public ObjectGuid Victim { get; set; }

        public ObjectGuid Killer { get; set; }

        public DeathMessageArgs(string message, ObjectGuid victimId, ObjectGuid killerId)
        {
            this.Message = message;
            this.Victim = victimId;
            this.Killer = killerId;
        }
    }
}

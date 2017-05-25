using System;

namespace ACE.Entity.Events
{
    public class DeathMessageArgs : EventArgs
    {
        public string Message { get; set; }

        public ObjectGuid Victim { get; set; }

        public ObjectGuid Killer { get; set; }

        public DeathMessageArgs(string message, ObjectGuid victimId, ObjectGuid killerId)
        {
            Message = message;
            Victim = victimId;
            Killer = killerId;
        }
    }
}

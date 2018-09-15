using System;

using ACE.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class QueuedEmote
    {
        public Emote Data;
        public WorldObject Target;
        public DateTime ExecuteTime;

        public QueuedEmote() { }

        public QueuedEmote(Emote data, WorldObject target, DateTime executeTime)
        {
            Data = data;
            Target = target;
            ExecuteTime = executeTime;
        }
    }
}

using ACE.Entity;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class QueuedEmote
    {
        public Emote Data;
        public WorldObject Target;
        public double ExecuteTime;

        public QueuedEmote() { }

        public QueuedEmote(Emote data, WorldObject target, double executeTime)
        {
            Data = data;
            Target = target;
            ExecuteTime = executeTime;
        }
    }
}

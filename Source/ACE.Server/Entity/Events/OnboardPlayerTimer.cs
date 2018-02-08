using System.Collections.Generic;

namespace ACE.Server.Entity.Events
{
    public class OnboardPlayerTimer : System.Timers.Timer
    {
        public Player Player { get; private set; }

        public List<WorldObject> Objects { get; private set; }

        public OnboardPlayerTimer(Player player, List<WorldObject> objects)
            : base()
        {
            this.Player = player;
            this.Objects = objects;
        }

        public OnboardPlayerTimer(Player player, List<WorldObject> objects, double interval)
            : base(interval)
        {
            this.Player = player;
            this.Objects = objects;
        }
    }
}

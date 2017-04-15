using ACE.Entity;

namespace ACE.Network.GameAction
{
    public abstract class TargetedGameAction : GameAction
    {
        public ObjectGuid Target { get; protected set; }

        public float Range { get; protected set; } = 20;

        protected TargetedGameAction(ClientMessage message) : base(message)
        { }
    }
}
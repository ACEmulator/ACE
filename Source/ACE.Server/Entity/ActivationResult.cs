using ACE.Server.Network.GameEvent;

namespace ACE.Server.Entity
{
    public class ActivationResult
    {
        public bool Success;
        public GameEventMessage Message;

        public ActivationResult(bool success)
        {
            Success = success;
        }

        public ActivationResult(GameEventMessage message)
        {
            Message = message;
        }
    }
}

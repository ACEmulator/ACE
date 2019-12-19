using ACE.Entity.Enum;

namespace ACE.Server.Physics.Command
{
    public class CommandListElement
    {
        public CommandListElement Next;
        public CommandListElement Prev;

        public MotionCommand Command;
        public float Speed;
        public bool HoldRun;

        public CommandListElement()
        {
            Speed = 1.0f;
        }

        public CommandListElement(MotionCommand command, float speed, bool holdRun)
        {
            Command = command;
            Speed = speed;
            HoldRun = holdRun;
        }
    }
}

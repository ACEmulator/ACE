using ACE.Entity.Enum;

namespace ACE.Server.Physics.Command
{
    public class CommandList
    {
        public CommandListElement Head;
        public CommandListElement MouseCommand;
        public CommandListElement Current;

        public void AddCommand(MotionCommand command, float speed, bool mouse, bool holdRun)
        {
            var element = new CommandListElement(command, speed, holdRun);

            CommandListElement mouse_cmd = null;

            if (mouse)
            {
                mouse_cmd = MouseCommand;

                if (mouse_cmd == null)
                {
                    SetMouseCommand(element);
                    return;
                }

                var prevNext = mouse_cmd.Prev.Next;
                if (prevNext != null)
                {
                    prevNext = mouse_cmd.Next;
                }
                else
                {
                    var next = mouse_cmd.Next;
                    var nextNull = next == null;
                    Head = mouse_cmd.Next;
                    if (nextNull)
                    {
                        // label_12
                        mouse_cmd.Next = null;
                        mouse_cmd.Prev = null;

                        SetMouseCommand(element);
                        return;
                    }
                    next.Prev = null;
                }
                if (mouse_cmd.Next != null)
                    mouse_cmd.Next.Prev = mouse_cmd.Prev;
                // goto label_12
            }
        }

        public void ClearAllCommands()
        {
            if (Head != null)
            {
                CommandListElement headPrevNext = null;

                while (true)
                {
                    var head = Head;
                    headPrevNext = Head.Prev.Next;
                    if (headPrevNext != null)
                        break;

                    var headNext = head.Next;
                    var headNextNull = head.Next == null;
                    Head = head.Next;

                    if (!headNextNull)
                    {
                        headNext.Prev = null;

                        // label_6
                        if (head.Next != null)
                            head.Next.Prev = head.Prev;
                    }

                    head.Next = null;
                    head.Prev = null;

                    if (Head == null)
                    {
                        MouseCommand = null;
                        return;
                    }
                }
                headPrevNext = Head.Next;
                // goto label_6
            }
            MouseCommand = null;
        }

        public void ClearKeyboardCommands()
        {

        }

        public CommandListElement GetHead()
        {
            return Head;
        }

        public bool HeadIsMouse()
        {
            if (Head == null)
                return false;

            return Head == MouseCommand;
        }

        public bool RemoveCommand(MotionCommand command, float speed, bool mouse)
        {
            return false;
        }

        public void SetMouseCommand(CommandListElement element)
        {
            MouseCommand = element;
            element.Next = Head;
            if (Head != null)
                Head.Prev = element;
            Head = element;
        }
    }
}

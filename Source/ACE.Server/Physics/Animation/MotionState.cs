using System.Collections.Generic;
using ACE.Server.Physics.Animation.Internal;

namespace ACE.Server.Physics.Animation
{
    public class MotionState
    {
        public uint Style;
        public uint Substate;
        public float SubstateMod;
        public LinkedList<Motion> Modifiers;
        public LinkedList<Motion> Actions;

        public MotionState()
        {
            SubstateMod = 1.0f;
            Modifiers = new LinkedList<Motion>();
            Actions = new LinkedList<Motion>();
        }

        public MotionState(MotionState state)
        {
            Style = state.Style;
            Substate = state.Substate;
            SubstateMod = state.SubstateMod;

            Modifiers = new LinkedList<Motion>();
            foreach (var modifier in state.Modifiers)
                Modifiers.AddLast(modifier);

            Actions = new LinkedList<Motion>();
            foreach (var action in state.Actions)
                Actions.AddLast(action);
        }

        public void add_action(uint action, float speedMod)
        {
            Actions.AddLast(new Motion(action, speedMod));
        }

        public bool add_modifier(uint _modifier, float speedMod)
        {
            if (Substate == _modifier)
                return false;

            foreach (var modifier in Modifiers)
            {
                if (modifier.ID == _modifier)
                    return false;
            }
            add_modifier_no_check(_modifier, speedMod);
            return true;
        }

        public void add_modifier_no_check(uint modifier, float speedMod)
        {
            Modifiers.AddFirst(new Motion(modifier, speedMod));
        }

        public void clear_actions()
        {
            Actions.Clear();
        }

        public void clear_modifiers()
        {
            Modifiers.Clear();
        }

        public void remove_action_head()
        {
            if (Actions.Count > 0)
                Actions.RemoveFirst();
        }

        public void remove_modifier(LinkedListNode<Motion> modifier)
        {
            if (modifier == null) return;

            Modifiers.Remove(modifier);
        }
    }
}

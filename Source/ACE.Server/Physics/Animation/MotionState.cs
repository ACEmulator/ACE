using System.Collections.Generic;
using System.Linq;

namespace ACE.Server.Physics.Animation
{
    public class MotionState
    {
        public int Style;
        public int Substate;
        public float SubstateMod;
        public LinkedList<Motion> Modifiers;
        public LinkedList<Motion> Actions;

        public MotionState()
        {
            SubstateMod = 1.0f;
        }

        public void add_action(int action, float speedMod)
        {
            Actions.AddLast(new Motion(action, speedMod));
        }

        public bool add_modifier(int _modifier, float speedMod)
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

        public void add_modifier_no_check(int modifier, float speedMod)
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
            Actions.RemoveFirst();
        }

        public void remove_modifier(Motion modifier)
        {
            Modifiers.Remove(modifier);
        }
    }
}

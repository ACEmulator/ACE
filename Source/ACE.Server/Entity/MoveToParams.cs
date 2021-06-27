using System;

using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class MoveToParams
    {
        public Action<bool> Callback;

        public WorldObject Target;

        public float? UseRadius;

        public MoveToParams(Action<bool> callback, WorldObject target, float? useRadius = null)
        {
            Callback = callback;

            Target = target;

            UseRadius = useRadius;
        }
    }
}

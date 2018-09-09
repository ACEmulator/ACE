
namespace ACE.Server.WorldObjects
{
    partial class Corpse
    {
        /// <summary>
        /// The maximum number of seconds for an empty corpse to stick around
        /// </summary>
        private static readonly double emptyDecayTime = 15.0;

        private bool rotCompleted;

        /// <summary>
        /// Handles corpse decay and removal
        /// </summary>
        public override void HeartBeat()
        {
            if (rotCompleted)
                return;

            TimeToRot -= HeartbeatInterval ?? 5;

            // empty corpses decay faster?
            if (Inventory.Count == 0 && TimeToRot > emptyDecayTime)
                TimeToRot = emptyDecayTime;

            if (TimeToRot <= 0)
            {
                rotCompleted = true;

                // TODO: if items are left on corpse,
                // create these items in the world
                // http://asheron.wikia.com/wiki/Item_Decay

                Destroy();
                return;
            }

            base.HeartBeat();
        }
    }
}


namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public override void Tick(double currentUnixTime)
        {
            foreach (var wo in EquippedObjects.Values)
                wo.Tick(currentUnixTime);

            Monster_Tick(currentUnixTime);

            base.Tick(currentUnixTime);
        }

        /// <summary>
        /// Called every ~5 seconds for Creatures
        /// </summary>
        public override void HeartBeat(double currentUnixTime)
        {
            VitalHeartBeat();

            // item enchantment ticks?

            base.HeartBeat(currentUnixTime);
        }
    }
}

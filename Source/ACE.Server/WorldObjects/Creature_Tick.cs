
namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public override void Tick(double lastTickDuration, double currentUnixTime)
        {
            foreach (var wo in EquippedObjects.Values)
                wo.Tick(lastTickDuration, currentUnixTime);

            Monster_Tick(lastTickDuration, currentUnixTime);

            base.Tick(lastTickDuration, currentUnixTime);
        }

        /// <summary>
        /// Called every ~5 seconds for Creatures
        /// </summary>
        public override void HeartBeat(double currentUnixTime)
        {
            VitalTick();

            // item enchantment ticks?

            base.HeartBeat(currentUnixTime);
        }
    }
}

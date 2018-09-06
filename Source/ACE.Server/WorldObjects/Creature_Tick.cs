
namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public override void Tick(double lastTickDuration, long currentTimeTick)
        {
            foreach (var wo in EquippedObjects.Values)
                wo.Tick(lastTickDuration, currentTimeTick);

            Monster_Tick(lastTickDuration, currentTimeTick);

            base.Tick(lastTickDuration, currentTimeTick);
        }

        /// <summary>
        /// Called every ~5 seconds for Creatures
        /// </summary>
        public override void HeartBeat()
        {
            VitalTick();

            // item enchantment ticks?

            base.HeartBeat();
        }
    }
}

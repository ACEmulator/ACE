
namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public override void Tick(double lastTickDuration)
        {
            foreach (var wo in EquippedObjects.Values)
                wo.Tick(lastTickDuration);

            Monster_Tick(lastTickDuration);

            base.Tick(lastTickDuration);
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

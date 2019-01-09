using ACE.Server.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// Called every ~5 seconds for Creatures
        /// </summary>
        public override void HeartBeat()
        {
            //PerfTimer.StartTimer("Creature_HeartBeat");

            //foreach (var wo in EquippedObjects.Values)
                //wo.HeartBeat();   // exclude

            EmoteManager.HeartBeat();   // only needed for creatures?

            VitalHeartBeat();

            //PerfTimer.StopTimer("Creature_HeartBeat");

            base.HeartBeat();
        }
    }
}

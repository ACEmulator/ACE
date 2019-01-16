
namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// Called every ~5 seconds for Creatures
        /// </summary>
        public override void HeartBeat(double currentUnixTime)
        {
            foreach (var wo in EquippedObjects.Values)
                wo.HeartBeat(currentUnixTime);

            VitalHeartBeat();

            EmoteManager.HeartBeat();

            base.HeartBeat(currentUnixTime);
        }
    }
}

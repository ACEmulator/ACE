
namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// Called every ~5 seconds for Creatures
        /// </summary>
        public override void Heartbeat(double currentUnixTime)
        {
            foreach (var wo in EquippedObjects.Values)
                wo.Heartbeat(currentUnixTime);

            VitalHeartBeat();

            EmoteManager.HeartBeat();

            base.Heartbeat(currentUnixTime);
        }
    }
}

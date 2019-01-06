
namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// Called every ~5 seconds for Creatures
        /// </summary>
        public override void HeartBeat()
        {
            //foreach (var wo in EquippedObjects.Values)
                //wo.HeartBeat();   // exclude

            EmoteManager.HeartBeat();   // only needed for creatures?

            VitalHeartBeat();

            base.HeartBeat();
        }
    }
}

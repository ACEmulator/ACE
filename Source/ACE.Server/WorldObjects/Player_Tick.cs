
namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// Called every ~5 seconds for Players
        /// </summary>
        public override void HeartBeat()
        {
            NotifyLandblocks();

            ManaConsumersTick();

            ItemEnchantmentTick();

            base.HeartBeat();
        }
    }
}

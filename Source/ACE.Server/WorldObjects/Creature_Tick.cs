using System.Linq;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// Called every ~5 seconds for Creatures
        /// </summary>
        public override void HeartBeat()
        {
            // TODO: EnchantmentManager caching for HasEnchantments
            foreach (var wo in EquippedObjects.Values.Where(i => i.EnchantmentManager.HasEnchantments))
                wo.EnchantmentManager.HeartBeat();

            EmoteManager.HeartBeat();   // only needed for creatures?

            VitalHeartBeat();

            base.HeartBeat();
        }
    }
}

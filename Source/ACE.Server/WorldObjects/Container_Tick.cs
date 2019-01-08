using System.Linq;

namespace ACE.Server.WorldObjects
{
    partial class Container
    {
        public override void HeartBeat()
        {
            // TODO: EnchantmentManager caching for HasEnchantments
            foreach (var wo in Inventory.Values.Where(i => i.EnchantmentManager.HasEnchantments))
                wo.EnchantmentManager.HeartBeat();

            base.HeartBeat();
        }
    }
}

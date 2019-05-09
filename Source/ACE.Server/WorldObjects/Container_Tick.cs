using System.Linq;

namespace ACE.Server.WorldObjects
{
    partial class Container
    {
        public override void Heartbeat(double currentUnixTime)
        {
            // added where clause
            foreach (var wo in Inventory.Values.Where(i => i.EnchantmentManager.HasEnchantments))
            {
                // FIXME: wo.NextHeartbeatTime is double.MaxValue here
                //if (wo.NextHeartbeatTime <= currentUnixTime)
                //wo.Heartbeat(currentUnixTime);

                // just go by parent heartbeats, only for enchantments?
                wo.EnchantmentManager.HeartBeat(HeartbeatInterval);
            }

            // for landblock containers
            if (IsOpen && CurrentLandblock != null)
            {
                var viewer = CurrentLandblock.GetObject(Viewer) as Player;
                if (viewer == null)
                {
                    Close(null);
                    return;
                }
                var withinUseRadius = CurrentLandblock.WithinUseRadius(viewer, Guid, out var targetValid);
                if (!withinUseRadius)
                {
                    Close(viewer);
                    return;
                }
            }
            base.Heartbeat(currentUnixTime);
        }
    }
}

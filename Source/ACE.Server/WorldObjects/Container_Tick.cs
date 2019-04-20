namespace ACE.Server.WorldObjects
{
    partial class Container
    {
        public override void Heartbeat(double currentUnixTime)
        {
            foreach (var wo in Inventory.Values)
            {
                if (wo.NextHeartbeatTime <= currentUnixTime)
                    wo.Heartbeat(currentUnixTime);
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

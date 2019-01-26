
namespace ACE.Server.WorldObjects
{
    partial class Container
    {
        public override void Heartbeat(double currentUnixTime)
        {
            foreach (var wo in Inventory.Values)
                wo.Heartbeat(currentUnixTime);

            base.Heartbeat(currentUnixTime);
        }
    }
}

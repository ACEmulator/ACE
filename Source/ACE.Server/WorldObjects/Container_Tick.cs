
namespace ACE.Server.WorldObjects
{
    partial class Container
    {
        public override void HeartBeat(double currentUnixTime)
        {
            foreach (var wo in Inventory.Values)
                wo.HeartBeat(currentUnixTime);

            base.HeartBeat(currentUnixTime);
        }
    }
}

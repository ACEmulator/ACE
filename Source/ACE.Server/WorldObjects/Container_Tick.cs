
namespace ACE.Server.WorldObjects
{
    partial class Container
    {
        public override void Tick(double lastTickDuration, double currentUnixTime)
        {
            foreach (var wo in Inventory.Values)
                wo.Tick(lastTickDuration, currentUnixTime);

            base.Tick(lastTickDuration, currentUnixTime);
        }
    }
}

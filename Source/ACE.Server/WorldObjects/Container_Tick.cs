
namespace ACE.Server.WorldObjects
{
    partial class Container
    {
        public override void Tick(double currentUnixTime)
        {
            foreach (var wo in Inventory.Values)
                wo.Tick(currentUnixTime);

            base.Tick(currentUnixTime);
        }
    }
}


namespace ACE.Server.WorldObjects
{
    partial class Container
    {
        public override void Tick(double lastTickDuration)
        {
            foreach (var wo in Inventory.Values)
                wo.Tick(lastTickDuration);

            base.Tick(lastTickDuration);
        }
    }
}

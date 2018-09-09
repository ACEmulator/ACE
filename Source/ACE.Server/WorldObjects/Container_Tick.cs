
namespace ACE.Server.WorldObjects
{
    partial class Container
    {
        public override void Tick(double lastTickDuration, long currentTimeTick)
        {
            foreach (var wo in Inventory.Values)
                wo.Tick(lastTickDuration, currentTimeTick);

            base.Tick(lastTickDuration, currentTimeTick);
        }
    }
}

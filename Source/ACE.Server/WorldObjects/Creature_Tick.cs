
namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public override void Tick(double lastTickDuration, long currentTimeTick)
        {
            foreach (var wo in EquippedObjects.Values)
                wo.Tick(lastTickDuration, currentTimeTick);

            base.Tick(lastTickDuration, currentTimeTick);
        }
    }
}

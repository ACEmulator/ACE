
namespace ACE.Server.WorldObjects
{
    partial class Container
    {
        public override void HeartBeat()
        {
            //foreach (var wo in Inventory.Values)
                //wo.Tick(currentUnixTime);

            base.HeartBeat();
        }
    }
}

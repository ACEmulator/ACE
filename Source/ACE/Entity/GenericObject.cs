using System.Threading.Tasks;

namespace ACE.Entity
{
    public class GenericObject : WorldObject
    {
        public GenericObject()
        {
        }

        protected override async Task Init(AceObject aceObject)
        {
            await base.Init(aceObject);
        }

        ////public GenericObject(ObjectGuid guid, AceObject aceObject)
        ////    : base(guid, aceObject)
        ////{
        ////}

        ////public override void HandleActionOnCollide(ObjectGuid playerId)
        ////{
        ////    // TODO: Implement
        ////}

        ////public override void HandleActionOnUse(ObjectGuid playerId)
        ////{
        ////    // TODO: Implement
        ////}

        ////public override void OnUse(Session session)
        ////{
        ////    // TODO: Implement
        ////}
    }
}

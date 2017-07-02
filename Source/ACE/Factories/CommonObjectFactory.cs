using ACE.Entity;
using ACE.Managers;

namespace ACE.Factories
{
    public class CommonObjectFactory
    {
        // private static uint nextObjectId = 0x80000001;

        public static uint DynamicObjectId
        {
            get { return new ObjectGuid(GuidManager.NewItemGuid()).Full; }
        }
    }
}

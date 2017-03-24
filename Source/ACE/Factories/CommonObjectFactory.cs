namespace ACE.Factories
{
    public class CommonObjectFactory
    {
        private static uint nextObjectId = 0x80000001;

        public static uint DynamicObjectId
        {
            get { return nextObjectId++; }
        }
    }
}
namespace ACE.Server.Physics.Common
{
    public class ShadowObj
    {
        public PhysicsObj PhysicsObj;
        public ObjCell Cell;

        public ShadowObj(PhysicsObj physicsObj, ObjCell cell)
        {
            PhysicsObj = physicsObj;
            Cell = cell;
        }
    }
}

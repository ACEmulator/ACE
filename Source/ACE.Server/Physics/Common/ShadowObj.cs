namespace ACE.Server.Physics.Common
{
    public class ShadowObj
    {
        public PhysicsObj PhysObj;
        public ObjCell Cell;

        public ShadowObj(PhysicsObj physObj, ObjCell cell)
        {
            PhysObj = physObj;
            Cell = cell;
        }
    }
}

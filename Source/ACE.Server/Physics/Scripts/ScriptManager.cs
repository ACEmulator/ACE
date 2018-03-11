namespace ACE.Server.Physics
{
    public class ScriptManager
    {
        public PhysicsObj PhysObj;
        public ScriptData CurrData;
        public ScriptData LastData;
        public int HookIndex;
        public double NextHookTime;

        public ScriptManager()
        {

        }

        public ScriptManager(PhysicsObj obj)
        {
            PhysObj = obj;
        }

        public bool AddScript(uint scriptID)
        {
            return false;
        }

        public void UpdateScripts()
        {

        }
    }
}

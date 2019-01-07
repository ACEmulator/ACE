
namespace ACE.Server.Entity.Actions.Legacy
{
    public interface IActor
    {
        void RunActions();

        void EnqueueAction(IAction action);
    }
}

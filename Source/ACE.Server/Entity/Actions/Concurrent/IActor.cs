
namespace ACE.Server.Entity.Actions.Concurrent
{
    public interface IActor
    {
        void RunActions();

        void EnqueueAction(IAction action);
    }
}

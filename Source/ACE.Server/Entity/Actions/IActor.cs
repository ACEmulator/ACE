
namespace ACE.Server.Entity.Actions
{
    public interface IActor
    {
        void RunActions();

        void EnqueueAction(IAction action);
    }
}


namespace ACE.Server.Entity.ActionsLegacy
{
    public interface IActor
    {
        void RunActions();

        void EnqueueAction(IAction action);
    }
}

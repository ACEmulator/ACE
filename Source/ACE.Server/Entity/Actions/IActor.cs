
namespace ACE.Server.Entity.Actions
{
    public interface IActor
    {
        void EnqueueAction(IAction action);
    }
}

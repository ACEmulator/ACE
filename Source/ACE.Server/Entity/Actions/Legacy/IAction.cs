using System;

namespace ACE.Server.Entity.ActionsLegacy
{
    public interface IAction
    {
        Tuple<IActor, IAction> Act();

        void RunOnFinish(IActor actor, IAction action);
    }
}

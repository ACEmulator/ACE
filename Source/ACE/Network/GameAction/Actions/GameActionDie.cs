using ACE.Entity;
using ACE.Entity.PlayerActions;

namespace ACE.Network.GameAction.Actions
{
    // Death feels is less morbid then suicide as a human, used "Die" instead.
    public static class GameActionDie
    {
        [GameAction(GameActionType.EvtCharacterSuicide)]
        public static void Handle(ClientMessage message, Session session)
        {
            session.Player.RequestAction(new DelegateAction(() => session.Player.ActOnKill(session)));
        }
    }
}

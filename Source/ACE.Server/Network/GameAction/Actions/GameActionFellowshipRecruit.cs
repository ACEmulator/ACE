using ACE.Entity;
using ACE.Server.Entity.WorldObjects;
using ACE.Server.Managers;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionFellowshipRecruit
    {
        [GameAction(GameActionType.FellowshipRecruit)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint newMemberId = message.Payload.ReadUInt32();
            ObjectGuid newMember = new ObjectGuid(newMemberId);
            Player newPlayer = WorldManager.GetPlayerByGuidId(newMemberId);
            session.Player.FellowshipRecruit(newPlayer);
        }
    }
}

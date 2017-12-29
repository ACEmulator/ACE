using System.Threading.Tasks;

using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionIdentifyObject
    {
        [GameAction(GameActionType.IdentifyObject)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            var id = message.Payload.ReadUInt32();

            ObjectGuid guid = new ObjectGuid(id);

            await session.Player.ExamineObject(guid);
        }
    }
}

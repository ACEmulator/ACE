
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.TrainSkill)]
    public class GameActionTrainSkill : GameActionPacket
    {
        private Skill skill;

        public GameActionTrainSkill(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            skill = (Skill)Fragment.Payload.ReadUInt32();
        }

        public override void Handle()
        {
            Session.Player.SpendSp(skill);
        }
    }
}

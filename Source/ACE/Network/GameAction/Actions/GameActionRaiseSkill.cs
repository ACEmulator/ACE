
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.RaiseSkill)]
    public class GameActionRaiseSkill : GameActionPacket
    {
        private Skill skill;
        private uint xpSpent;

        public GameActionRaiseSkill(Session session, ClientPacketFragment fragment) : base(session, fragment) { }
        
        public override void Read()
        {
            skill = (Skill)Fragment.Payload.ReadUInt32();
            xpSpent = Fragment.Payload.ReadUInt32();
        }

        public override void Handle()
        {
            Session.Player.SpendXp(skill, xpSpent);
        }
    }
}

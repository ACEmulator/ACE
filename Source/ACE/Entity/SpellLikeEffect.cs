using ACE.Network.Enum;
using ACE.Entity.Enum;
using log4net;
using ACE.Network.GameMessages.Messages;
using ACE.Network;

namespace ACE.Entity
{
    public class SpellLikeEffect : WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PlayScript PlayerScript = Network.Enum.PlayScript.Invalid;

        public SpellLikeEffect(ObjectType type,
            ObjectGuid guid,
            string name,
            ushort weenieClassId,
            ObjectDescriptionFlag descriptionFlag,
            WeenieHeaderFlag weenieFlag,
            Position position,
            Spell spellId,
            uint modelId,
            uint soundTableId,
            uint physicsTableId) : base(type, guid)
        {
            Name = name;
            DescriptionFlags = descriptionFlag;
            WeenieFlags = weenieFlag;
            Location = position;
            WeenieClassid = weenieClassId;

            Spell = spellId;
            CSetup = modelId;
            Stable = soundTableId;
            Petable = physicsTableId;
        }

        public override void PlayScript(Session session)
        {
            if (PlayerScript != Network.Enum.PlayScript.Invalid)
            {
                var scriptEvent = new GameMessageScript(Guid, PlayerScript, 1f);
                session.Network.EnqueueSend(scriptEvent);
            }
        }
    }
}

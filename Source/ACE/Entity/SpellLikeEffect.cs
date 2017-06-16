using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            this.Name = name;
            this.DescriptionFlags = descriptionFlag;
            this.WeenieFlags = weenieFlag;
            this.Location = position;
            this.WeenieClassid = weenieClassId;

            this.Spell = spellId;
            this.PhysicsData.CSetup = modelId;
            this.PhysicsData.Stable = soundTableId;
            this.PhysicsData.Petable = physicsTableId;
        }

        public override void PlayScript(Session session)
        {
            if (PlayerScript != Network.Enum.PlayScript.Invalid)
            {
                var scriptEvent = new GameMessageScript(this.Guid, PlayerScript, 1f);
                session.Network.EnqueueSend(scriptEvent);
            }
        }
    }
}

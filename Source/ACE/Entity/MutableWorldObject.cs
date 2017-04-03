using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity.Enum;
using ACE.Managers;
using log4net;
using ACE.Network;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Enum;

namespace ACE.Entity
{
    /// <summary>
    /// any world object that can change a state of some sort that requires clients be updated.  players, monsters,
    /// doors, etc.
    /// </summary>
    public class MutableWorldObject : WorldObject
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PlayScript PlayerScript = Network.Enum.PlayScript.Invalid;

        public MutableWorldObject(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position) : base(type, guid)
        {
            this.Name = name;
            this.DescriptionFlags = descriptionFlag;
            this.WeenieFlags = weenieFlag;
            this.Position = position;
            this.WeenieClassid = weenieClassId;
        }

        /// <summary>
        /// tick-stamp for the last time a movement update was sent
        /// </summary>
        public double LastMovementBroadcastTicks { get; set; }

        /// <summary>
        /// tick-stamp for the server time of the last time the player moved.
        /// TODO: implement
        /// </summary>
        public double LastAnimatedTicks { get; set; }

        public override Position Position
        {
            get { return base.Position; }
            protected set
            {
                if (base.Position != null)
                    LastUpdatedTicks = WorldManager.PortalYearTicks;

                log.Debug($"{Name} moved to {Position}");

                base.Position = value;
            }
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
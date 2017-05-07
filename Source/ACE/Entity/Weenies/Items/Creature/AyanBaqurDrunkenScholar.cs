using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using System;

namespace ACE.Entity
{
    public class AyanBaqurDrunkenScholar : Vendor
    {
        public AyanBaqurDrunkenScholar(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        public AyanBaqurDrunkenScholar(ObjectGuid guid, BaseAceObject baseAceObject)
            : base(guid, baseAceObject)
        {
        }

        public AyanBaqurDrunkenScholar(AceObject aceO)
            : this(new ObjectGuid(aceO.AceObjectId), aceO)
        {
            this.Location = aceO.Position;
            this.WeenieClassid = aceO.WeenieClassId;
        }

        public override void OnUse(Player player)
        {
            // TODO: Implement
            string message = "You know, things here don't seem quite right. I feel like I've been frozen in time.";
            // var useFailMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
            var sendTellToPlayer = new GameEventTell(player.Session, message, Name, Guid.Full, player.Guid.Full, ChatMessageType.Tell);
            var sendUseDoneEvent = new GameEventUseDone(player.Session);
            player.Session.Network.EnqueueSend(sendTellToPlayer, sendUseDoneEvent);
        }
    }
}

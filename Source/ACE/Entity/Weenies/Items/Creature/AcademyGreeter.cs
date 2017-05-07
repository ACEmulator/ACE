using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using System;

namespace ACE.Entity
{
    public class AcademyGreeter : Vendor
    {
        public AcademyGreeter(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        public AcademyGreeter(ObjectGuid guid, BaseAceObject baseAceObject)
            : base(guid, baseAceObject)
        {
        }

        public AcademyGreeter(AceObject aceO)
            : this(new ObjectGuid(aceO.AceObjectId), aceO)
        {
            this.Location = aceO.Position;
            this.WeenieClassid = aceO.WeenieClassId;

            // this.GameData.Type = aceO.WeenieClassId;
        }

        public override void OnUse(Player player)
        {
            // TODO: Implement
            var movementWave = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Wave));
            player.EnqueueMovementEvent(movementWave, Guid);
            string messages1st = "Hail and welcome to Dereth! No time to chat! Time to get on with your training!";
            var send1stTellToPlayer = new GameEventTell(player.Session, messages1st, Name, Guid.Full, player.Guid.Full, ChatMessageType.Tell);
            player.Session.Network.EnqueueSend(send1stTellToPlayer);
            string message2nd = "Click on the Backpack in the lower right corner of your screen to open your Inventory. Click on the blue Calling Stone, and drag it to me.";
            var send2ndTellToPlayer = new GameEventTell(player.Session, message2nd, Name, Guid.Full, player.Guid.Full, ChatMessageType.Tell);
            player.Session.Network.EnqueueSend(send2ndTellToPlayer);

            var sendUseDoneEvent = new GameEventUseDone(player.Session);
            player.Session.Network.EnqueueSend(sendUseDoneEvent);
        }
    }
}

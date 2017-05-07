using ACE.Entity.Enum;
using ACE.Factories;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using System;

namespace ACE.Entity
{
    public class AcademyGuardExitShoushi : Vendor
    {
        public AcademyGuardExitShoushi(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        public AcademyGuardExitShoushi(ObjectGuid guid, BaseAceObject baseAceObject)
            : base(guid, baseAceObject)
        {
        }

        public AcademyGuardExitShoushi(AceObject aceO)
            : this(new ObjectGuid(aceO.AceObjectId), aceO)
        {
            this.Location = aceO.Position;
            this.WeenieClassid = aceO.WeenieClassId;

            // this.GameData.Type = aceO.WeenieClassId;
        }

        public override void OnUse(Player player)
        {
            // TODO: Implement
            // var movementWave = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Wave));
            // player.EnqueueMovementEvent(movementWave, Guid);
            string messages1st = "If you want to skip your training and leave the Academy early, give this token back to me.";
            var send1stTellToPlayer = new GameEventTell(player.Session, messages1st, Name, Guid.Full, player.Guid.Full, ChatMessageType.Tell);
            player.Session.Network.EnqueueSend(send1stTellToPlayer);

            // This isn't right way to give an item from an npc but I want to see order of things, so faking it.
            var exitToken = LootGenerationFactory.CreateTestWorldObject(player, 29335);
            LootGenerationFactory.AddToContainer(exitToken, player);
            var sendGiveMessage = new GameMessageSystemChat($"{Name} gives you {exitToken.Name}.", ChatMessageType.Broadcast);
            player.Session.Network.EnqueueSend(sendGiveMessage);
            player.ActionApplySoundEffect(Sound.ReceiveItem, player.Guid);
            player.TrackObject(exitToken);
            // Again likely not right way to do this...

            string message2nd = "But beware, once you leave the Training Academy, you cannot come back!";
            var send2ndTellToPlayer = new GameEventTell(player.Session, message2nd, Name, Guid.Full, player.Guid.Full, ChatMessageType.Tell);
            player.Session.Network.EnqueueSend(send2ndTellToPlayer);

            var sendUseDoneEvent = new GameEventUseDone(player.Session);
            player.Session.Network.EnqueueSend(sendUseDoneEvent);
        }
    }
}

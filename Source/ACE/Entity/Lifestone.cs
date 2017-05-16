using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.DatLoader.FileTypes;

namespace ACE.Entity
{
    public class Lifestone : UsableObject
    {
        public Lifestone(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        public Lifestone(AceObject aceO)
            : base((ObjectType)aceO.TypeId, new ObjectGuid(aceO.AceObjectId))
        {
            this.Name = aceO.Name;
            this.DescriptionFlags = (ObjectDescriptionFlag)aceO.AceObjectDescriptionFlags;
            this.Location = aceO.Position;
            this.WeenieClassid = aceO.WeenieClassId;
            this.WeenieFlags = (WeenieHeaderFlag)aceO.WeenieHeaderFlags;

            this.PhysicsData.MTableResourceId = aceO.MotionTableId;
            this.PhysicsData.Stable = aceO.SoundTableId;
            this.PhysicsData.CSetup = aceO.ModelTableId;

            // this should probably be determined based on the presence of data.
            this.PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aceO.PhysicsDescriptionFlag;
            this.PhysicsData.PhysicsState = (PhysicsState)aceO.PhysicsState;

            // game data min required flags;
            this.Icon = aceO.IconId;

            this.GameData.Usable = (Usable)aceO.ItemUseable;
            this.GameData.RadarColour = (RadarColor)aceO.BlipColor;
            this.GameData.RadarBehavior = (RadarBehavior)aceO.Radar;
            this.GameData.UseRadius = aceO.UseRadius;

            aceO.AnimationOverrides.ForEach(ao => this.ModelData.AddModel(ao.Index, (ushort)ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => this.ModelData.AddTexture(to.Index, (ushort)to.OldId, (ushort)to.NewId));
            aceO.PaletteOverrides.ForEach(po => this.ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
        }

        public override void OnUse(Player player)
        {
            string serverMessage = null;
            // validate within use range, taking into account the radius of the model itself, as well
            SetupModel csetup = SetupModel.ReadFromDat(this.PhysicsData.CSetup);
            float radiusSquared = ((float)GameData.UseRadius + csetup.Radius) * ((float)GameData.UseRadius + csetup.Radius);

            var motionSanctuary = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Sanctuary));

            var animationEvent = new GameMessageUpdateMotion(player, player.Session, motionSanctuary);

            // This event was present for a pcap in the training dungeon.. Why? The sound comes with animationEvent...
            var soundEvent = new GameMessageSound(this.Guid, Sound.LifestoneOn, 1);

            if (player.Location.SquaredDistanceTo(this.Location) >= radiusSquared)
            {
                serverMessage = "You wandered too far to attune with the Lifestone!";
            }
            else
            {
                player.SetCharacterPosition(PositionType.Sanctuary, player.Location);

                // create the outbound server message
                serverMessage = "You have attuned your spirit to this Lifestone. You will resurrect here after you die.";
                player.EnqueueMovementEvent(motionSanctuary, player.Guid);
                player.Session.Network.EnqueueSend(soundEvent);
            }

            var lifestoneBindMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.Magic);
            // always send useDone event
            var sendUseDoneEvent = new GameEventUseDone(player.Session);
            player.Session.Network.EnqueueSend(lifestoneBindMessage, sendUseDoneEvent);
        }
    }
}

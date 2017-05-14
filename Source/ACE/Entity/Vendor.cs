using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.DatLoader.FileTypes;

namespace ACE.Entity
{
    public class Vendor : UsableObject
    {
        public Vendor(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        public Vendor(AceObject aceO)
            : base((ObjectType)aceO.TypeId, new ObjectGuid(aceO.AceObjectId))
        {
            Name = aceO.Name;
            DescriptionFlags = (ObjectDescriptionFlag)aceO.WdescBitField;
            Location = aceO.Position;
            WeenieClassid = aceO.WeenieClassId;
            WeenieFlags = (WeenieHeaderFlag)aceO.WeenieFlags;

            PhysicsData.MTableResourceId = aceO.MotionTableId;
            PhysicsData.Stable = aceO.SoundTableId;
            PhysicsData.CSetup = aceO.ModelTableId;

            // this should probably be determined based on the presence of data.
            PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aceO.PhysicsBitField;
            PhysicsData.PhysicsState = (PhysicsState)aceO.PhysicsState;

            // game data min required flags;
            Icon = aceO.IconId;

            GameData.Usable = (Usable)aceO.Usability;
            GameData.RadarColour = (RadarColor)aceO.BlipColor;
            GameData.RadarBehavior = (RadarBehavior)aceO.Radar;
            GameData.UseRadius = aceO.UseRadius;

            aceO.AnimationOverrides.ForEach(ao => ModelData.AddModel(ao.Index, (ushort)ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => ModelData.AddTexture(to.Index, (ushort)to.OldId, (ushort)to.NewId));
            aceO.PaletteOverrides.ForEach(po => ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
        }

        public override void OnUse(Player player)
        {
            string serverMessage = null;
            // validate within use range, taking into account the radius of the model itself, as well
            SetupModel csetup = SetupModel.ReadFromDat(PhysicsData.CSetup);
            float radiusSquared = (GameData.UseRadius + csetup.Radius) * (GameData.UseRadius + csetup.Radius);

            if (player.Location.SquaredDistanceTo(Location) >= radiusSquared)
            {
                serverMessage = "You wandered too far from the vendor";
            }
            else
            {
                // create the outbound server message
                serverMessage = "You Break you Buy!,You Break you Buy!";
            }

            // give player money
            var money = new GameMessageUpdateQualityEvent(player.Session);
            player.Session.Network.EnqueueSend(money);

            var vendordebug = new GameMessageSystemChat(serverMessage, ChatMessageType.Magic);
            // always send useDone event
            var sendUseDoneEvent = new GameEventUseDone(player.Session);
            player.Session.Network.EnqueueSend(vendordebug, sendUseDoneEvent);

            player.Session.Network.EnqueueSend(new GameEventApproachVendor(player.Session, this));
        }
    }
}

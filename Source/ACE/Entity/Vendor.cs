﻿using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network.Enum;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum.Properties;

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

        // FIXME(ddevec): I broke vendors -- I need to think about how to reimplement this cleanly
        public override void OnUse(ObjectGuid playerId)
        {
            ActionChain chain = new ActionChain();
            CurrentLandblock.ChainOnObject(chain, playerId, (WorldObject wo) =>
            {
                Player player = wo as Player;
                if (player == null)
                {
                    return;
                }

                string serverMessage = null;
                // validate within use range, taking into account the radius of the model itself, as well
                SetupModel csetup = SetupModel.ReadFromDat(PhysicsData.CSetup);
                float radiusSquared = (GameData.UseRadius + csetup.Radius) * (GameData.UseRadius + csetup.Radius);

                // We're static, so this is safe -- our Location is never written
                if (player.Location.SquaredDistanceTo(Location) >= radiusSquared)
                {
                    serverMessage = "Your way to far away to trust, come closer and buy something or leave me alone!";
                }
                else
                {
                    // create the outbound server message
                    serverMessage = "You Break it, you bought it!";
                }

                // give player starting money
                var money = new GameMessagePrivateUpdatePropertyInt(player.Session, PropertyInt.CoinValue, 5000);
                player.Session.Network.EnqueueSend(money);

                var welcomemsg = new GameMessageSystemChat(serverMessage, ChatMessageType.Tell);
                // always send useDone event
                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                player.Session.Network.EnqueueSend(welcomemsg, sendUseDoneEvent);

                player.Session.Network.EnqueueSend(new GameEventApproachVendor(player.Session, Guid));
            });
            chain.EnqueueChain();
        }
    }
}

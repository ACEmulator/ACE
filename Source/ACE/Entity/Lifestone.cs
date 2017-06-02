using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using ACE.Entity.Enum;
using ACE.Entity.Actions;
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
            : base((ObjectType)aceO.ItemType, new ObjectGuid(aceO.AceObjectId))
        {
            // FIXME(ddevec): These should be inhereted frome aceO not copied...
            Name = aceO.Name;
            DescriptionFlags = (ObjectDescriptionFlag)aceO.AceObjectDescriptionFlags;
            Location = aceO.Location;
            WeenieClassid = aceO.WeenieClassId;
            WeenieFlags = (WeenieHeaderFlag)aceO.WeenieHeaderFlags;

            PhysicsData.MTableResourceId = aceO.MotionTableId;
            PhysicsData.Stable = aceO.SoundTableId;
            PhysicsData.CSetup = aceO.ModelTableId;
            PhysicsData.PhysicsState = (PhysicsState)aceO.PhysicsState;

            // game data min required flags;
            Icon = aceO.IconId;

            Usable = (Usable?)aceO.ItemUseable;
            RadarColor = (RadarColor?)aceO.BlipColor;
            RadarBehavior = (RadarBehavior?)aceO.Radar;
            UseRadius = aceO.UseRadius;

            aceO.AnimationOverrides.ForEach(ao => ModelData.AddModel(ao.Index, (ushort)ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => ModelData.AddTexture(to.Index, (ushort)to.OldId, (ushort)to.NewId));
            aceO.PaletteOverrides.ForEach(po => ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));
        }

        public override void OnUse(ObjectGuid playerId)
        {
            // All data on a lifestone is constant -- therefore we just run in context of player
            ActionChain chain = new ActionChain();
            CurrentLandblock.ChainOnObject(chain, playerId, (WorldObject wo) =>
            {
                Player player = wo as Player;
                string serverMessage = null;
                // validate within use range, taking into account the radius of the model itself, as well
                SetupModel csetup = SetupModel.ReadFromDat(PhysicsData.CSetup);
                float radiusSquared = (GameData.UseRadius + csetup.Radius) * (GameData.UseRadius + csetup.Radius);

                // Run this animation...
                // Player Enqueue: 
                var motionSanctuary = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Sanctuary));

                var animationEvent = new GameMessageUpdateMotion(player.Guid,
                                                                 player.Sequences.GetCurrentSequence(Network.Sequence.SequenceType.ObjectInstance),
                                                                 player.Sequences, motionSanctuary);
                
                // This event was present for a pcap in the training dungeon.. Why? The sound comes with animationEvent...
                var soundEvent = new GameMessageSound(Guid, Sound.LifestoneOn, 1);

                if (player.Location.SquaredDistanceTo(Location) >= radiusSquared)
                {
                    serverMessage = "You wandered too far to attune with the Lifestone!";
                }
                else
                {
                    player.SetCharacterPosition(PositionType.Sanctuary, player.Location);

                    // create the outbound server message
                    serverMessage = "You have attuned your spirit to this Lifestone. You will resurrect here after you die.";
                    player.HandleActionMotion(motionSanctuary);
                    player.Session.Network.EnqueueSend(soundEvent);
                }

                var lifestoneBindMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.Magic);
                // always send useDone event
                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                player.Session.Network.EnqueueSend(lifestoneBindMessage, sendUseDoneEvent);
            });
            chain.EnqueueChain();
        }
    }
}

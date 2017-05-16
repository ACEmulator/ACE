using System;
using ACE.Database;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Entity
{
    public class Portal : CollidableObject
    {
        private Position portalDestination;

        private uint portalMinLvl;

        private uint portalMaxLvl;

        private byte portalSocietyId;

        private bool portalIsTieable;

        private bool portalIsRecallable;

        private bool portalIsSummonable;

        private enum SpecialPortalWCID : ushort
        {
            /// <summary>
            /// Training Academy's Central Courtyard's portal weenieClassID
            /// </summary>
            CentralCourtyard = 31061,

            /// <summary>
            /// Training Academy's Outer Courtyard's portal weenieClassID
            /// </summary>
            OuterCourtyard = 29334
        }

        private enum SpecialPortalLandblockID : uint
        {
            /// <summary>
            /// Shoushi :: Training Academy's Central Courtyard's portal raw LandblockID
            /// </summary>
            ShoushiCCLaunch = 0x7f030273,

            /// <summary>
            /// Shoushi :: Training Academy's Central Courtyard's portal destination raw LandblockID
            /// </summary>
            ShoushiCCLanding = 0x7f03021e,

            /// <summary>
            /// Yaraq :: Training Academy's Central Courtyard's portal raw LandblockID
            /// </summary>
            YaraqCCLaunch = 0x8c040273,

            /// <summary>
            /// Yaraq :: Training Academy's Central Courtyard's portal destination raw LandblockID
            /// </summary>
            YaraqCCLanding = 0x8c04021e,

            /// <summary>
            /// Sanamar :: Training Academy's Central Courtyard's portal raw LandblockID
            /// </summary>
            SanamarCCLaunch = 0x72030273,

            /// <summary>
            /// Sanamar :: Training Academy's Central Courtyard's portal destination raw LandblockID
            /// </summary>
            SanamarCCLanding = 0x7203021e,

            /// <summary>
            /// Holtburg :: Training Academy's Central Courtyard's portal destination raw LandblockID
            /// </summary>
            HoltburgCCLanding = 0x8603021e,

            /// <summary>
            /// Shoushi :: Training Academy's Outer Courtyard's portal raw LandblockID
            /// </summary>
            ShoushiOCLaunch = 0x7f030331,

            /// <summary>
            /// Shoushi :: Training Academy's Outer Courtyard's portal destination raw LandblockID
            /// </summary>
            ShoushiOCLanding = 0x7f0302c3,

            /// <summary>
            /// Yaraq :: Training Academy's Outer Courtyard's portal raw LandblockID
            /// </summary>
            YaraqOCLaunch = 0x8c040331,

            /// <summary>
            /// Yaraq :: Training Academy's Outer Courtyard's portal destination raw LandblockID
            /// </summary>
            YaraqOCLanding = 0x8c0402c3,

            /// <summary>
            /// Sanamar :: Training Academy's Outer Courtyard's portal raw LandblockID
            /// </summary>
            SanamarOCLaunch = 0x72030331,

            /// <summary>
            /// Sanamar :: Training Academy's Outer Courtyard's portal destination raw LandblockID
            /// </summary>
            SanamarOCLanding = 0x720302c3,

            /// <summary>
            /// Holtburg :: Training Academy's Outer Courtyard's portal destination raw LandblockID
            /// </summary>
            HoltburgOCLanding = 0x860302c3
        }

        public Portal(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        public Portal(AcePortalObject aceO)
            : base((ObjectType)aceO.TypeId, new ObjectGuid(aceO.AceObjectId))
        {
            Name = aceO.Name;
            DescriptionFlags = (ObjectDescriptionFlag)aceO.AceObjectDescriptionFlags;
            Location = aceO.Position;
            WeenieClassid = aceO.WeenieClassId;
            WeenieFlags = (WeenieHeaderFlag)aceO.WeenieHeaderFlags;

            PhysicsData.MTableResourceId = aceO.MotionTableId;
            PhysicsData.Stable = aceO.SoundTableId;
            PhysicsData.CSetup = aceO.ModelTableId;

            // this should probably be determined based on the presence of data.
            PhysicsData.PhysicsDescriptionFlag = (PhysicsDescriptionFlag)aceO.PhysicsDescriptionFlag;
            PhysicsData.PhysicsState = (PhysicsState)aceO.PhysicsState;

            PhysicsData.ObjScale = (float)aceO.DefaultScale;

            // game data min required flags;
            Icon = (ushort)aceO.IconId;

            GameData.Usable = (Usable)aceO.ItemUseable;
            GameData.RadarColour = (RadarColor)aceO.BlipColor;
            GameData.RadarBehavior = (RadarBehavior)aceO.Radar;
            GameData.UseRadius = aceO.UseRadius;

            aceO.AnimationOverrides.ForEach(ao => ModelData.AddModel(ao.Index, (ushort)ao.AnimationId));
            aceO.TextureOverrides.ForEach(to => ModelData.AddTexture(to.Index, (ushort)to.OldId, (ushort)to.NewId));
            aceO.PaletteOverrides.ForEach(po => ModelData.AddPalette(po.SubPaletteId, po.Offset, po.Length));

            portalDestination = aceO.DestPosition;
            portalMinLvl = aceO.MinLvl;
            portalMaxLvl = aceO.MaxLvl;
            portalSocietyId = aceO.SocietyId;
            portalIsTieable = Convert.ToBoolean(aceO.IsTieable);
            portalIsRecallable = Convert.ToBoolean(aceO.IsRecallable);
            portalIsSummonable = Convert.ToBoolean(aceO.IsSummonable);
    }

    public override void OnCollide(Player player)
        {
            // validate within use range :: set to a fixed value as static Portals are normally OnCollide usage
            float rangeCheck = 5.0f;

            if (player.Location.SquaredDistanceTo(Location) < rangeCheck)
            {
                if (portalDestination != null)
                {
                    if ((player.Level >= portalMinLvl) && ((player.Level <= portalMaxLvl) || (portalMaxLvl == 0)))
                    {
                        Position portalDest = portalDestination;
                        switch (WeenieClassid)
                        {
                            /// <summary>
                            /// Setup correct racial portal destination for the Central Courtyard in the Training Academy
                            /// </summary>
                            case (ushort)SpecialPortalWCID.CentralCourtyard:
                                {
                                    uint playerLandblockId = player.Location.LandblockId.Raw;
                                    switch (playerLandblockId)
                                    {
                                        case (uint)SpecialPortalLandblockID.ShoushiCCLaunch:    // Shoushi
                                            {
                                                portalDest.LandblockId = new LandblockId((uint)SpecialPortalLandblockID.ShoushiCCLanding);
                                                break;
                                            }
                                        case (uint)SpecialPortalLandblockID.YaraqCCLaunch:    // Yaraq
                                            {
                                                portalDest.LandblockId = new LandblockId((uint)SpecialPortalLandblockID.YaraqCCLanding);
                                                break;
                                            }
                                        case (uint)SpecialPortalLandblockID.SanamarCCLaunch:    // Sanamar
                                            {
                                                portalDest.LandblockId = new LandblockId((uint)SpecialPortalLandblockID.SanamarCCLanding);
                                                break;
                                            }
                                        default:            // Holtburg
                                            {
                                                portalDest.LandblockId = new LandblockId((uint)SpecialPortalLandblockID.HoltburgCCLanding);
                                                break;
                                            }
                                    }

                                    portalDest.PositionX = portalDestination.PositionX;
                                    portalDest.PositionY = portalDestination.PositionY;
                                    portalDest.PositionZ = portalDestination.PositionZ;
                                    portalDest.RotationX = portalDestination.RotationX;
                                    portalDest.RotationY = portalDestination.RotationY;
                                    portalDest.RotationZ = portalDestination.RotationZ;
                                    portalDest.RotationW = portalDestination.RotationW;
                                    break;
                                }
                            /// <summary>
                            /// Setup correct racial portal destination for the Outer Courtyard in the Training Academy
                            /// </summary>
                            case (ushort)SpecialPortalWCID.OuterCourtyard:
                                {
                                    uint playerLandblockId = player.Location.LandblockId.Raw;
                                    switch (playerLandblockId)
                                    {
                                        case (uint)SpecialPortalLandblockID.ShoushiOCLaunch:    // Shoushi
                                            {
                                                portalDest.LandblockId = new LandblockId((uint)SpecialPortalLandblockID.ShoushiOCLanding);
                                                break;
                                            }
                                        case (uint)SpecialPortalLandblockID.YaraqOCLaunch:    // Yaraq
                                            {
                                                portalDest.LandblockId = new LandblockId((uint)SpecialPortalLandblockID.YaraqOCLanding);
                                                break;
                                            }
                                        case (uint)SpecialPortalLandblockID.SanamarOCLaunch:    // Sanamar
                                            {
                                                portalDest.LandblockId = new LandblockId((uint)SpecialPortalLandblockID.SanamarOCLanding);
                                                break;
                                            }
                                        default:            // Holtburg
                                            {
                                                portalDest.LandblockId = new LandblockId((uint)SpecialPortalLandblockID.HoltburgOCLanding);
                                                break;
                                            }
                                    }

                                    portalDest.PositionX = portalDestination.PositionX;
                                    portalDest.PositionY = portalDestination.PositionY;
                                    portalDest.PositionZ = portalDestination.PositionZ;
                                    portalDest.RotationX = portalDestination.RotationX;
                                    portalDest.RotationY = portalDestination.RotationY;
                                    portalDest.RotationZ = portalDestination.RotationZ;
                                    portalDest.RotationW = portalDestination.RotationW;
                                    break;
                                }
                            /// <summary>
                            /// All other portals don't need adjustments.
                            /// </summary>
                            default:
                                {
                                    break;
                                }
                        }

                        player.Session.Player.Teleport(portalDest);
                        // If the portal just used is able to be recalled to,
                        // save the destination coordinates to the LastPortal character position save table
                        if (Convert.ToBoolean(portalIsRecallable) == true)
                            player.SetCharacterPosition(PositionType.LastPortal, portalDest);
                        // always send useDone event
                        var sendUseDoneEvent = new GameEventUseDone(player.Session);
                        player.Session.Network.EnqueueSend(sendUseDoneEvent);
                    }
                    else if ((player.Level > portalMaxLvl) && (portalMaxLvl != 0))
                    {
                        // You are too powerful to interact with that portal!
                        var usePortalMessage = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.Enum_04AC);
                        // always send useDone event
                        var sendUseDoneEvent = new GameEventUseDone(player.Session);
                        player.Session.Network.EnqueueSend(usePortalMessage, sendUseDoneEvent);
                    }
                    else
                    {
                        // You are not powerful enough to interact with that portal!
                        var usePortalMessage = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.Enum_04AB);
                        // always send useDone event
                        var sendUseDoneEvent = new GameEventUseDone(player.Session);
                        player.Session.Network.EnqueueSend(usePortalMessage, sendUseDoneEvent);
                    }
                }
                else
                {
                    string serverMessage = "Portal destination for portal ID " + this.WeenieClassid + " not yet implemented!";
                    var usePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                    // always send useDone event
                    var sendUseDoneEvent = new GameEventUseDone(player.Session);
                    player.Session.Network.EnqueueSend(usePortalMessage, sendUseDoneEvent);
                }
            }
            else
            {
                // always send useDone event
                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                player.Session.Network.EnqueueSend(sendUseDoneEvent);
            }
        }
    }
}

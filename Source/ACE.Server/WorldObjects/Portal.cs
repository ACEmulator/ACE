using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public sealed class Portal : WorldObject
    {
        public Position Destination { get; private set; }

        // private byte portalSocietyId;

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

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Portal(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Portal(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            DescriptionFlags |= ObjectDescriptionFlag.Portal | ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Attackable;

            SetProperty(PropertyBool.Stuck, true);
            SetProperty(PropertyBool.Attackable, true);
        }

        public string AppraisalPortalDestination
        {
            get;
        }

        public bool PortalShowDestination
        {
            get;
        }

        public int MinimumLevel
        {
            get;
        }

        public int MaximumLevel
        {
            get;
        }

        public int SocietyId => 0;

        public bool IsTieable
        {
            get;
        }

        public bool IsSummonable
        {
            get;
        }

        public bool IsRecallable => IsTieable;

        public override void HandleActionOnCollide(ObjectGuid playerId)
        {
            string serverMessage;

            Player player = CurrentLandblock.GetObject(playerId) as Player;
            if (player == null)
            {
                return;
            }
            if (Destination != null)
            {
#if DEBUG
                serverMessage = "Checking requirements for " + this.Name;
                var usePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                player.Session.Network.EnqueueSend(usePortalMessage);
#endif
                // Check player level -- requires remote query to player (ugh)...
                if ((player.Level >= MinimumLevel) && ((player.Level <= MaximumLevel) || (MaximumLevel == 0)))
                {
                    Position portalDest = Destination;
                    switch (WeenieClassId)
                    {
                        // Setup correct racial portal destination for the Central Courtyard in the Training Academy
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

                                portalDest.PositionX = Destination.PositionX;
                                portalDest.PositionY = Destination.PositionY;
                                portalDest.PositionZ = Destination.PositionZ;
                                portalDest.RotationX = Destination.RotationX;
                                portalDest.RotationY = Destination.RotationY;
                                portalDest.RotationZ = Destination.RotationZ;
                                portalDest.RotationW = Destination.RotationW;
                                break;
                            }
                        // Setup correct racial portal destination for the Outer Courtyard in the Training Academy
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

                                portalDest.PositionX = Destination.PositionX;
                                portalDest.PositionY = Destination.PositionY;
                                portalDest.PositionZ = Destination.PositionZ;
                                portalDest.RotationX = Destination.RotationX;
                                portalDest.RotationY = Destination.RotationY;
                                portalDest.RotationZ = Destination.RotationZ;
                                portalDest.RotationW = Destination.RotationW;
                                break;
                            }
                        // All other portals don't need adjustments.
                        default:
                            {
                                break;
                            }
                    }

#if DEBUG
                    serverMessage = "Portal sending player to destination";
                    usePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                    player.Session.Network.EnqueueSend(usePortalMessage);
#endif
                    player.Session.Player.Teleport(portalDest);
                    // If the portal just used is able to be recalled to,
                    // save the destination coordinates to the LastPortal character position save table
                    if (IsRecallable)
                        player.SetCharacterPosition(PositionType.LastPortal, portalDest);
                }
                else if ((player.Level > MaximumLevel) && (MaximumLevel != 0))
                {
                    // You are too powerful to interact with that portal!
                    var failedUsePortalMessage = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.YouAreTooPowerfulToUsePortal);
                    player.Session.Network.EnqueueSend(failedUsePortalMessage);
                }
                else
                {
                    // You are not powerful enough to interact with that portal!
                    var failedUsePortalMessage = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.YouAreNotPowerfulEnoughToUsePortal);
                    player.Session.Network.EnqueueSend(failedUsePortalMessage);
                }
            }
            else
            {
                serverMessage = "Portal destination for portal ID " + this.WeenieClassId + " not yet implemented!";
                var failedUsePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                player.Session.Network.EnqueueSend(failedUsePortalMessage);
            }
        }

        public override void ActOnUse(ObjectGuid playerId)
        {
            Player player = CurrentLandblock.GetObject(playerId) as Player;
            if (player == null)
            {
                return;
            }

            if (!player.IsWithinUseRadiusOf(this))
                player.DoMoveTo(this);
            else
            {
                // TODO: to be removed once physics collisions are implemented
                HandleActionOnCollide(playerId);
                // always send useDone event
                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                player.Session.Network.EnqueueSend(sendUseDoneEvent);
            }
        }
    }
}

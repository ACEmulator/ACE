using System;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Physics;

namespace ACE.Server.WorldObjects
{
    public sealed class Portal : WorldObject
    {
        //public Position Destination { get; private set; }

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
            BaseDescriptionFlags |= ObjectDescriptionFlag.Portal;

            MinLevel = MinLevel ?? 0;
            MaxLevel = MaxLevel ?? 0;
            PortalBitmask = PortalBitmask ?? 0;
        }

        public string AppraisalPortalDestination
        {
            get;
        }

        public bool PortalShowDestination
        {
            get;
        }

        private int? MinLevel
        {
            get => GetProperty(PropertyInt.MinLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MinLevel); else SetProperty(PropertyInt.MinLevel, value.Value); }
        }

        private int? MaxLevel
        {
            get => GetProperty(PropertyInt.MaxLevel);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.MaxLevel); else SetProperty(PropertyInt.MaxLevel, value.Value); }
        }

        private int? PortalBitmask
        {
            get => GetProperty(PropertyInt.PortalBitmask);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.PortalBitmask); else SetProperty(PropertyInt.PortalBitmask, value.Value); }
        }

        public int SocietyId => 0;

        public bool NoRecall
        {
            get
            {
                if ((((int)PortalBitmask & (int)ACE.Entity.Enum.PortalBitmask.NoRecall) >> 5) == 1) return true;
                else return false;
            }
        }

        public bool NoSummon
        {
            get
            {
                if ((((int)PortalBitmask & (int)ACE.Entity.Enum.PortalBitmask.NoSummon) >> 4) == 1) return true;
                else return false;
            }
        }

        public bool NoTie => NoRecall;

        public override void HandleActionOnCollide(ObjectGuid playerId)
        {
            string serverMessage;

            Player player = CurrentLandblock.GetObject(playerId) as Player;

            if (player == null)
                return;

            if (player.Teleporting)
                return;

            player.Teleporting = true;

            if (Destination != null)
            {
#if DEBUG
                serverMessage = "Checking requirements for " + this.Name;
                var usePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                player.Session.Network.EnqueueSend(usePortalMessage);
#endif
                // Check player level -- requires remote query to player (ugh)...
                if ((player.Level >= MinLevel) && ((player.Level <= MaxLevel) || (MaxLevel == 0)) || (player.IgnorePortalRestrictions ?? false))
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
                    if (!NoRecall)
                        player.SetCharacterPosition(PositionType.LastPortal, portalDest);
                }
                else if ((player.Level > MaxLevel) && (MaxLevel != 0))
                {
                    // You are too powerful to interact with that portal!
                    var failedUsePortalMessage = new GameEventWeenieError(player.Session, WeenieError.YouAreTooPowerfulToUsePortal);
                    player.Session.Network.EnqueueSend(failedUsePortalMessage);
                    player.Teleporting = false;
                }
                else
                {
                    // You are not powerful enough to interact with that portal!
                    var failedUsePortalMessage = new GameEventWeenieError(player.Session, WeenieError.YouAreNotPowerfulEnoughToUsePortal);
                    player.Session.Network.EnqueueSend(failedUsePortalMessage);
                    player.Teleporting = false;
                }
            }
            else
            {
                serverMessage = "Portal destination for portal ID " + this.WeenieClassId + " not yet implemented!";
                var failedUsePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                player.Session.Network.EnqueueSend(failedUsePortalMessage);
                player.Teleporting = false;
            }
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem, and is wrapped in ActionChain.<para />
        /// The actor of the ActionChain is the item being used.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(Player player)
        {
            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
        }
    }
}

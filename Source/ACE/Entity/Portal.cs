using System;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Entity
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

        public Portal(AceObject aceO)
            : base(aceO)
        {
            // check to see if this ace object has a destination.  if so, defer to it.
            if (aceO.Destination != null)
            {
                Destination = aceO.Destination;
            }
            else
            {
                // but if not, portals roll up to the weenie
                Destination = Weenie.Destination;
            }

            MinimumLevel = Weenie.MinLevel ?? 0;
            MaximumLevel = Weenie.MaxLevel ?? 0;
            IsTieable = ((Weenie.PortalBitmask ?? 0) & (uint)PortalBitmask.NoRecall) == 0;
            IsSummonable = ((Weenie.PortalBitmask ?? 0) & (uint)PortalBitmask.NoSummon) == 0;
            AppraisalPortalDestination = Weenie.AppraisalPortalDestination;
            PortalShowDestination = Weenie.PortalShowDestination ?? false;
        }

        public string AppraisalPortalDestination
        {
            get;
        }

        public bool PortalShowDestination
        {
            get;
        }

        public uint MinimumLevel
        {
            get;
        }

        public uint MaximumLevel
        {
            get;
        }

        public uint SocietyId
        {
            get { return 0; }
        }

        public bool IsTieable
        {
            get;
        }

        public bool IsSummonable
        {
            get;
        }

        public bool IsRecallable
        {
            get { return IsTieable; }
        }

        public override void HandleActionOnCollide(ObjectGuid playerId)
        {
            HandleActionOnUse(playerId);
        }

        public override void HandleActionOnUse(ObjectGuid playerId)
        {
            string serverMessage;

            ActionChain chain = new ActionChain();
            CurrentLandblock.ChainOnObject(chain, playerId, (WorldObject wo) =>
            {
                Player player = wo as Player;
                if (player == null)
                {
                    return;
                }

                if (!player.IsWithinUseRadiusOf(this))
                    player.DoCollideWith(this);
                else
                {
                    if (Destination != null)
                    {
#if DEBUG
                        serverMessage = "Checking requirements for " + Name;
                        var usePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                        player.Session.Network.EnqueueSend(usePortalMessage);
#endif
                        // Check player level -- requires remote query to player (ugh)...
                        if ((player.Level >= MinimumLevel) && ((player.Level <= MaximumLevel) || (MaximumLevel == 0)))
                        {
                            Position portalDest = Destination;
                            switch (WeenieClassId)
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

                                        portalDest.PositionX = Destination.PositionX;
                                        portalDest.PositionY = Destination.PositionY;
                                        portalDest.PositionZ = Destination.PositionZ;
                                        portalDest.RotationX = Destination.RotationX;
                                        portalDest.RotationY = Destination.RotationY;
                                        portalDest.RotationZ = Destination.RotationZ;
                                        portalDest.RotationW = Destination.RotationW;
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

                                        portalDest.PositionX = Destination.PositionX;
                                        portalDest.PositionY = Destination.PositionY;
                                        portalDest.PositionZ = Destination.PositionZ;
                                        portalDest.RotationX = Destination.RotationX;
                                        portalDest.RotationY = Destination.RotationY;
                                        portalDest.RotationZ = Destination.RotationZ;
                                        portalDest.RotationW = Destination.RotationW;
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
                            // always send useDone event
                            var sendUseDoneEvent = new GameEventUseDone(player.Session);
                            player.Session.Network.EnqueueSend(sendUseDoneEvent);

                            UseTimestamp++;
                        }
                        else if ((player.Level > MaximumLevel) && (MaximumLevel != 0))
                        {
                            // You are too powerful to interact with that portal!
                            var failedUsePortalMessage = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.Enum_04AC);
                            // always send useDone event
                            var sendUseDoneEvent = new GameEventUseDone(player.Session);
                            player.Session.Network.EnqueueSend(failedUsePortalMessage, sendUseDoneEvent);
                        }
                        else
                        {
                            // You are not powerful enough to interact with that portal!
                            var failedUsePortalMessage = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.Enum_04AB);
                            // always send useDone event
                            var sendUseDoneEvent = new GameEventUseDone(player.Session);
                            player.Session.Network.EnqueueSend(failedUsePortalMessage, sendUseDoneEvent);
                        }
                    }
                    else
                    {
                        serverMessage = "Portal destination for portal ID " + this.WeenieClassId + " not yet implemented!";
                        var failedUsePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                        // always send useDone event
                        var sendUseDoneEvent = new GameEventUseDone(player.Session);
                        player.Session.Network.EnqueueSend(failedUsePortalMessage, sendUseDoneEvent);
                    }
                }
            });
            // Run on the player
            chain.EnqueueChain();
        }
    }
}

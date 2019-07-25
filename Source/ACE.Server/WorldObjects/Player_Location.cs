using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        private static readonly Position MarketplaceDrop = DatabaseManager.World.GetCachedWeenie("portalmarketplace").GetPosition(PositionType.Destination);

        /// <summary>
        /// Teleports the player to position
        /// </summary>
        /// <param name="positionType">PositionType to be teleported to</param>
        /// <returns>true on success (position is set) false otherwise</returns>
        public bool TeleToPosition(PositionType positionType)
        {
            var position = GetPosition(positionType);

            if (position != null)
            {
                var teleportDest = new Position(position);
                AdjustDungeon(teleportDest);

                Teleport(teleportDest);
                return true;
            }

            return false;
        }

        private static readonly Motion motionLifestoneRecall = new Motion(MotionStance.NonCombat, MotionCommand.LifestoneRecall);

        private static readonly Motion motionHouseRecall = new Motion(MotionStance.NonCombat, MotionCommand.HouseRecall);

        public static float RecallMoveThreshold = 8.0f;
        public static float RecallMoveThresholdSq = RecallMoveThreshold * RecallMoveThreshold;

        public bool TooBusyToRecall
        {
            get => IsBusy || Teleporting;
        }

        public void HandleActionTeleToHouse()
        {
            if (PKTimerActive)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
                return;
            }

            if (RecallsDisabled)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.ExitTrainingAcademyToUseCommand));
                return;
            }

            if (TooBusyToRecall)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YoureTooBusy));
                return;
            }

            var house = House ?? GetAccountHouse();

            if (house == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustOwnHouseToUseCommand));
                return;
            }

            if (CombatMode != CombatMode.NonCombat)
            {
                // this should be handled by a different thing, probably a function that forces player into peacemode
                var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.NonCombat);
                SetCombatMode(CombatMode.NonCombat);
                Session.Network.EnqueueSend(updateCombatMode);
            }

            EnqueueBroadcast(new GameMessageSystemChat($"{Name} is recalling home.", ChatMessageType.Recall), 96.0f);
            EnqueueBroadcastMotion(motionHouseRecall);

            var startPos = new Position(Location);

            // Wait for animation
            var actionChain = new ActionChain();

            // Then do teleport
            var animLength = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.HouseRecall);
            actionChain.AddDelaySeconds(animLength);
            IsBusy = true;
            actionChain.AddAction(this, () =>
            {
                IsBusy = false;
                var endPos = new Position(Location);
                if (startPos.SquaredDistanceTo(endPos) > RecallMoveThresholdSq)
                {
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveMovedTooFar));
                    return;
                }
                Teleport(house.SlumLord.Location);
            });

            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Handles teleporting a player to the lifestone (/ls or /lifestone command)
        /// </summary>
        public void HandleActionTeleToLifestone()
        {
            if (PKTimerActive)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
                return;
            }

            if (RecallsDisabled)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.ExitTrainingAcademyToUseCommand));
                return;
            }

            if (TooBusyToRecall)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YoureTooBusy));
                return;
            }

            if (Sanctuary == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("Your spirit has not been attuned to a sanctuary location.", ChatMessageType.Broadcast));
                return;
            }

            // FIXME(ddevec): I should probably make a better interface for this
            UpdateVital(Mana, Mana.Current / 2);

            if (CombatMode != CombatMode.NonCombat)
            {
                // this should be handled by a different thing, probably a function that forces player into peacemode
                var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.NonCombat);
                SetCombatMode(CombatMode.NonCombat);
                Session.Network.EnqueueSend(updateCombatMode);
            }

            EnqueueBroadcast(new GameMessageSystemChat($"{Name} is recalling to the lifestone.", ChatMessageType.Recall), 96.0f);
            EnqueueBroadcastMotion(motionLifestoneRecall);

            var startPos = new Position(Location);

            // Wait for animation
            ActionChain lifestoneChain = new ActionChain();

            // Then do teleport
            IsBusy = true;
            lifestoneChain.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.LifestoneRecall));
            lifestoneChain.AddAction(this, () =>
            {
                IsBusy = false;
                var endPos = new Position(Location);
                if (startPos.SquaredDistanceTo(endPos) > RecallMoveThresholdSq)
                {
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveMovedTooFar));
                    return;
                }

                Teleport(Sanctuary);
            });

            lifestoneChain.EnqueueChain();
        }

        private static readonly Motion motionMarketplaceRecall = new Motion(MotionStance.NonCombat, MotionCommand.MarketplaceRecall);

        public void HandleActionTeleToMarketPlace()
        {
            if (PKTimerActive)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
                return;
            }

            if (RecallsDisabled)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.ExitTrainingAcademyToUseCommand));
                return;
            }

            if (TooBusyToRecall)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YoureTooBusy));
                return;
            }

            var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.NonCombat);

            EnqueueBroadcast(new GameMessageSystemChat($"{Name} is recalling to the marketplace.", ChatMessageType.Recall), 96.0f);
            Session.Network.EnqueueSend(updateCombatMode); // this should be handled by a different thing, probably a function that forces player into peacemode
            EnqueueBroadcastMotion(motionMarketplaceRecall);

            var startPos = new Position(Location);

            // TODO: (OptimShi): Actual animation length is longer than in retail. 18.4s
            // float mpAnimationLength = MotionTable.GetAnimationLength((uint)MotionTableId, MotionCommand.MarketplaceRecall);
            // mpChain.AddDelaySeconds(mpAnimationLength);
            ActionChain mpChain = new ActionChain();
            mpChain.AddDelaySeconds(14);

            // Then do teleport
            IsBusy = true;
            mpChain.AddAction(this, () =>
            {
                IsBusy = false;
                var endPos = new Position(Location);
                if (startPos.SquaredDistanceTo(endPos) > RecallMoveThresholdSq)
                {
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveMovedTooFar));
                    return;
                }

                Teleport(MarketplaceDrop);
            });

            // Set the chain to run
            mpChain.EnqueueChain();
        }

        private static readonly Motion motionAllegianceHometownRecall = new Motion(MotionStance.NonCombat, MotionCommand.AllegianceHometownRecall);

        public void HandleActionRecallAllegianceHometown()
        {
            //Console.WriteLine($"{Name}.HandleActionRecallAllegianceHometown()");

            if (PKTimerActive)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
                return;
            }

            if (RecallsDisabled)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.ExitTrainingAcademyToUseCommand));
                return;
            }

            if (TooBusyToRecall)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YoureTooBusy));
                return;
            }

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            if (Allegiance.Sanctuary == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YourAllegianceDoesNotHaveHometown));
                return;
            }

            if (CombatMode != CombatMode.NonCombat)
            {
                // this should be handled by a different thing, probably a function that forces player into peacemode
                var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.NonCombat);
                SetCombatMode(CombatMode.NonCombat);
                Session.Network.EnqueueSend(updateCombatMode);
            }

            EnqueueBroadcast(new GameMessageSystemChat($"{Name} is going to the Allegiance hometown.", ChatMessageType.Recall), 96.0f);
            EnqueueBroadcastMotion(motionAllegianceHometownRecall);

            var startPos = new Position(Location);

            // Wait for animation
            var actionChain = new ActionChain();

            // Then do teleport
            IsBusy = true;
            var animLength = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.AllegianceHometownRecall);
            actionChain.AddDelaySeconds(animLength);
            actionChain.AddAction(this, () =>
            {
                IsBusy = false;
                var endPos = new Position(Location);
                if (startPos.SquaredDistanceTo(endPos) > RecallMoveThresholdSq)
                {
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveMovedTooFar));
                    return;
                }

                if (Allegiance != null)
                    Teleport(Allegiance.Sanctuary);
            });

            actionChain.EnqueueChain();
        }

        /// <summary>
        /// Recalls you to your allegiance's Mansion or Villa
        /// </summary>
        public void HandleActionTeleToMansion()
        {
            //Console.WriteLine($"{Name}.HandleActionTeleToMansion()");

            if (PKTimerActive)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveBeenInPKBattleTooRecently));
                return;
            }

            if (RecallsDisabled)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.ExitTrainingAcademyToUseCommand));
                return;
            }

            if (TooBusyToRecall)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YoureTooBusy));
                return;
            }

            // check if player is in an allegiance
            if (Allegiance == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouAreNotInAllegiance));
                return;
            }

            var allegianceHouse = Allegiance.GetHouse();

            if (allegianceHouse == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YourMonarchDoesNotOwnAMansionOrVilla));
                return;
            }

            if (allegianceHouse.HouseType < ACE.Entity.Enum.HouseType.Villa)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YourMonarchsHouseIsNotAMansionOrVilla));
                return;
            }

            // ensure allegiance housing has allegiance permissions enabled
            if (allegianceHouse.MonarchId == null)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YourMonarchHasClosedTheMansion));
                return;
            }

            if (CombatMode != CombatMode.NonCombat)
            {
                // this should be handled by a different thing, probably a function that forces player into peacemode
                var updateCombatMode = new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.CombatMode, (int)CombatMode.NonCombat);
                SetCombatMode(CombatMode.NonCombat);
                Session.Network.EnqueueSend(updateCombatMode);
            }

            EnqueueBroadcast(new GameMessageSystemChat($"{Name} is recalling to the Allegiance housing.", ChatMessageType.Recall), 96.0f);
            EnqueueBroadcastMotion(motionHouseRecall);

            var startPos = new Position(Location);

            // Wait for animation
            var actionChain = new ActionChain();

            // Then do teleport
            var animLength = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.HouseRecall);
            actionChain.AddDelaySeconds(animLength);

            IsBusy = true;
            actionChain.AddAction(this, () =>
            {
                IsBusy = false;
                var endPos = new Position(Location);
                if (startPos.SquaredDistanceTo(endPos) > RecallMoveThresholdSq)
                {
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouHaveMovedTooFar));
                    return;
                }

                Teleport(allegianceHouse.SlumLord.Location);
            }); 

            actionChain.EnqueueChain();
        }

        public DateTime LastTeleportTime;

        public void Teleport(Position _newPosition)
        {
            var newPosition = new Position(_newPosition);
            //newPosition.PositionZ += 0.005f;
            newPosition.PositionZ += 0.005f * (ObjScale ?? 1.0f);

            //Console.WriteLine($"{Name}.Teleport() - Sending to {newPosition.ToLOCString()}");

            Teleporting = true;
            LastTeleportTime = DateTime.UtcNow;

            Session.Network.EnqueueSend(new GameMessagePlayerTeleport(this));

            // load quickly, but player can load into landblock before server is finished loading

            // send a "fake" update position to get the client to start loading asap,
            // also might fix some decal bugs
            var prevLoc = Location;
            Location = newPosition;
            SendUpdatePosition();
            Location = prevLoc;

            DoTeleportPhysicsStateChanges();

            // force out of hotspots
            PhysicsObj.report_collision_end(true);

            if (UnderLifestoneProtection)
                LifestoneProtectionDispel();

            UpdatePlayerPosition(new Position(newPosition), true);
        }

        public void DoPreTeleportHide()
        {
            if (Teleporting) return;
            PlayParticleEffect(PlayScript.Hide, Guid);
        }

        public void DoTeleportPhysicsStateChanges()
        {
            var broadcastUpdate = false;

            var oldHidden = Hidden.Value;
            var oldIgnore = IgnoreCollisions.Value;
            var oldReport = ReportCollisions.Value;

            Hidden = true;
            IgnoreCollisions = true;
            ReportCollisions = false;

            if (Hidden != oldHidden || IgnoreCollisions != oldIgnore || ReportCollisions != oldReport)
                broadcastUpdate = true;

            if (broadcastUpdate)
                EnqueueBroadcastPhysicsState();
        }

        public void OnTeleportComplete()
        {
            if (CurrentLandblock != null && !CurrentLandblock.CreateWorldObjectsCompleted)
            {
                // If the critical landblock resources haven't been loaded yet, we keep the player in the pink bubble state
                // We'll check periodically to see when it's safe to let them materialize in
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(0.1);
                actionChain.AddAction(this, OnTeleportComplete);
                actionChain.EnqueueChain();
                return;
            }

            // set materialize physics state
            // this takes the player from pink bubbles -> fully materialized
            ReportCollisions = true;
            IgnoreCollisions = false;
            Hidden = false;
            Teleporting = false;
            
            CheckMonsters();
            CheckHouse();

            EnqueueBroadcastPhysicsState();
        }

        public void NotifyLandblocks()
        {
            // the original implementations of this were done on landblock heartbeat,
            // with checks for players in the current landblock, as well as adjacent outdoor landblocks

            // for performance reasons, this is being reimplemented in the reverse manner,
            // with players notifying landblocks of their activity

            // notify current landblock of player activity
            if (CurrentLandblock != null)
                CurrentLandblock?.SetActive();
        }

        public static readonly float RunFactor = 1.5f;

        /// <summary>
        /// Returns the amount of time for player to rotate by the # of degrees
        /// from the input angle, using the omega speed from its MotionTable
        /// </summary>
        public override float GetRotateDelay(float angle)
        {
            return base.GetRotateDelay(angle) / RunFactor;
        }

        /// <summary>
        /// A list of landblocks the player cannot relog directly into
        /// 
        /// If a regular player logs out in one of these landblocks,
        /// they will be transported back to the lifestone when they log back in.
        /// </summary>
        public static HashSet<ushort> NoLog_Landblocks = new HashSet<ushort>()
        {
            // https://asheron.fandom.com/wiki/Special:Search?query=Lifestone+on+Relog%3A+Yes+
            // https://docs.google.com/spreadsheets/d/122xOw3IKCezaTDjC_hggWSVzYJ_9M_zUUtGEXkwNXfs/edit#gid=846612575

            0x0002,     // Viamontian Garrison
            0x0007,     // Town Network
            0x0056,     // Augmentation Realm Main Level
            0x005F,     // Tanada House of Pancakes (Seasonal)
            0x006D,     // Augmentation Realm Upper Level
            0x007D,     // Augmentation Realm Lower Level
            0x00AB,     // Derethian Combat Arena
            0x00AC,     // Derethian Combat Arena
            0x00C3,     // Blighted Putrid Moarsman Tunnels
            0x00D7,     // Jester's Prison
            0x00EA,     // Mhoire Armory
            0x015D,     // Mountain Cavern
            0x027F,     // East Fork Dam Hive
            0x03A7,     // Mount Elyrii Hive
            0x5764,     // Oubliette of Mhoire Castle
            0x634C,     // Tainted Grotto
            0x6544,     // Greater Battle Dungeon
            0x6651,     // Hoshino Tower
            0x7E04,     // Thug Hideout
            0x8A04,     // Night Club (Seasonal Anniversary)
            0x8B04,     // Frozen Wight Lair
            0x9EE5,     // Northwatch Castle Black Market
            0xB5F0,     // Aerfalle's Sanctum
            0xF92F,     // Freebooter Keep Black Market
        };

        /// <summary>
        /// Called when a player first logs in
        /// </summary>
        public static void HandleNoLogLandblock(Biota biota)
        {
            if (biota.WeenieType == (int)WeenieType.Sentinel || biota.WeenieType == (int)WeenieType.Admin) return;

            var location = biota.BiotaPropertiesPosition.FirstOrDefault(i => i.PositionType == (ushort)PositionType.Location);
            if (location == null) return;

            var landblock = (ushort)(location.ObjCellId >> 16);

            if (!NoLog_Landblocks.Contains(landblock))
                return;

            var lifestone = biota.BiotaPropertiesPosition.FirstOrDefault(i => i.PositionType == (ushort)PositionType.Sanctuary);
            if (lifestone == null) return;

            location.ObjCellId = lifestone.ObjCellId;
            location.OriginX = lifestone.OriginX;
            location.OriginY = lifestone.OriginY;
            location.OriginZ = lifestone.OriginZ;
            location.AnglesX = lifestone.AnglesX;
            location.AnglesY = lifestone.AnglesY;
            location.AnglesZ = lifestone.AnglesZ;
            location.AnglesW = lifestone.AnglesW;
        }
    }
}

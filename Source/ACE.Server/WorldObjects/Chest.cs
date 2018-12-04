using System;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using Microsoft.EntityFrameworkCore.Internal;

namespace ACE.Server.WorldObjects
{
    public class Chest : Container, Lock
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Chest(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Chest(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            //BaseDescriptionFlags |= ObjectDescriptionFlag.Door;

            //if (!DefaultOpen)
            //{
            //    CurrentMotionState = motionStateClosed;
            //    IsOpen = false;
            //    //Ethereal = false;
            //}
            //else
            //{
            //    CurrentMotionState = motionStateOpen;
            //    IsOpen = true;
            //    //Ethereal = true;
            //}

            ContainerCapacity = ContainerCapacity ?? 10;
            ItemCapacity = ItemCapacity ?? 120;

            //Adding loot to chests
            //Eventually these case statements would be linked to indivual treasure generators. Each one should be a different profile, but currently it will be the complete appropriate tier profile.
            for (int i = 0; i < GeneratorProfiles.Count; i++)
            {
                int amount = ACE.Server.Physics.Common.Random.RollDice(2, 14);  //r.Next(2, 14);
                var generator = GeneratorProfiles[i];
                switch(generator.Biota.WeenieClassId)
                {
                    case 414:
                    case 459:
                    case 0:
                    case 6:
                    case 18:
                    case 465:
                        for (int j = 0; j < amount; j++)
                        {
                            var wo = LootGenerationFactory.CreateRandomLootObjects(1);
                            TryAddToInventory(wo);
                        }
                        break;
                    case 413:
                    case 410:
                    case 16:
                    case 457:
                    case 4:
                    case 463:
                    case 395:
                        for (int j = 0; j < amount; j++)
                        {
                            var wo = LootGenerationFactory.CreateRandomLootObjects(2);
                            TryAddToInventory(wo);
                        }
                        break;
                    case 411:
                    case 15:
                    case 313:
                    case 462:
                    case 3:
                    case 456:
                    case 340:
                    case 365:
                        for (int j = 0; j < amount; j++)
                        {
                            var wo = LootGenerationFactory.CreateRandomLootObjects(3);
                            TryAddToInventory(wo);
                        }
                        break;
                    case 460:
                    case 412:
                    case 354:
                    case 1:
                    case 13:
                    case 59:
                    case 339:
                        for (int j = 0; j < amount; j++)
                        {
                            var wo = LootGenerationFactory.CreateRandomLootObjects(4);
                            TryAddToInventory(wo);
                        }
                        break;
                    case 334:
                    case 341:
                    case 317:
                        for (int j = 0; j < amount; j++)
                        {
                            var wo = LootGenerationFactory.CreateRandomLootObjects(5);
                            TryAddToInventory(wo);
                        }
                        break;
                    case 449:
                    case 32:
                    case 2:
                    case 421:
                    case 349:
                    case 351:
                    case 422:
                    case 338:
                        for (int j = 0; j < amount; j++)
                        {
                            var wo = LootGenerationFactory.CreateRandomLootObjects(6);
                            TryAddToInventory(wo);
                        }
                        break;
                    default:
                        if(generator.Biota.WeenieClassId > 500)
                        {
                            //If the WeenieClassId is greater than the profile Id's, then it will be an item that is created with that Id.
                            var wo = WorldObjectFactory.CreateNewWorldObject((uint)generator.Biota.WeenieClassId);
                            TryAddToInventory(wo);
                        }
                        break;
                }
            }

            CurrentMotionState = motionClosed; // What chest defaults to open?

            if (UseRadius < 2)
                UseRadius = 2; // Until DoMoveTo (Physics, Indoor/Outside range variance) is smarter, use 2 is safest.
        }

        protected static readonly Motion motionOpen = new Motion(MotionStance.NonCombat, MotionCommand.On);
        protected static readonly Motion motionClosed = new Motion(MotionStance.NonCombat, MotionCommand.Off);

        //private static readonly MotionState motionStateOpen = new Motion(MotionStance.NonCombat, new MotionItem(MotionCommand.On));
        //private static readonly MotionState motionStateClosed = new Motion(MotionStance.NonCombat, new MotionItem(MotionCommand.Off));

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject wo)
        {
            var player = wo as Player;
            if (player == null) return;

            ////if (playerDistanceTo >= 2500)
            ////{
            ////    var sendTooFarMsg = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.Enum_0037);
            ////    player.Session.Network.EnqueueSend(sendTooFarMsg, sendUseDoneEvent);
            ////    return;
            ////}

            if (!IsLocked)
            {
                if (!IsOpen)
                {
                    var rotateTime = player.Rotate(this);

                    var actionChain = new ActionChain();
                    actionChain.AddDelaySeconds(rotateTime);
                    actionChain.AddAction(this, () => Open(player));
                    actionChain.EnqueueChain();
                    return;
                }
                else
                {
                    if (Viewer == player.Guid.Full)
                        Close(player);

                    // else error msg?
                }
            }
            else
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"The {Name} is locked!"));
                EnqueueBroadcast(new GameMessageSound(Guid, Sound.OpenFailDueToLock, 1.0f));
            }

            player.SendUseDoneEvent();
        }

        protected override void DoOnOpenMotionChanges()
        {
            EnqueueBroadcastMotion(motionOpen);
            CurrentMotionState = motionOpen;
        }

        protected override void DoOnCloseMotionChanges()
        {
            EnqueueBroadcastMotion(motionClosed);
            CurrentMotionState = motionClosed;
        }

        public string LockCode
        {
            get => GetProperty(PropertyString.LockCode);
            set { if (value == null) RemoveProperty(PropertyString.LockCode); else SetProperty(PropertyString.LockCode, value); }
        }

        /// <summary>
        /// Used for unlocking a chest via lockpick, so contains a skill check
        /// player.Skills[Skill.Lockpick].Current should be sent for the skill check
        /// </summary>
        public UnlockResults Unlock(uint playerLockpickSkillLvl, ref int difficulty)
        {
            return LockHelper.Unlock(this, playerLockpickSkillLvl, ref difficulty);
        }

        /// <summary>
        /// Used for unlocking a chest via a key
        /// </summary>
        public UnlockResults Unlock(string keyCode)
        {
            return LockHelper.Unlock(this, keyCode);
        }
    }
}

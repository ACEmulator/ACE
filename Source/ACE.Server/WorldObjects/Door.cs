using ACE.Common;
using ACE.Database;
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
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    public class Door : WorldObject, Lock
    {
        private static readonly UniversalMotion motionOpen = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.On));
        private static readonly UniversalMotion motionClosed = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Door(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Door(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            BaseDescriptionFlags |= ObjectDescriptionFlag.Door;

            if (!DefaultOpen)
            {
                CurrentMotionState = motionClosed;
                IsOpen = false;
                //Ethereal = false;
            }
            else
            {
                CurrentMotionState = motionOpen;
                IsOpen = true;
                //Ethereal = true;
            }

            IsLocked = IsLocked ?? false;
            ResetInterval = ResetInterval ?? 30.0f;
            ResistLockpick = ResistLockpick ?? 0;
            LockCode = LockCode ?? "";

            // If we had the base weenies this would be the way to go
            ////if (DefaultLocked)
            ////    IsLocked = true;
            ////else
            ////    IsLocked = false;

            // But since we don't know what doors were DefaultLocked, let's assume for now that any door that starts Locked should default as such.
            if (IsLocked ?? false)
                DefaultLocked = true;

            if (UseRadius < 2)
                UseRadius = 2;
        }

        private bool DefaultLocked
        {
            get;
            set;
        }

        private bool DefaultOpen
        {
            get;
            set;
        }

        private double? ResetInterval
        {
            get => GetProperty(PropertyFloat.ResetInterval);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.ResetInterval); else SetProperty(PropertyFloat.ResetInterval, value.Value); }
        }

        private double? resetTimestamp;
        private double? ResetTimestamp
        {
            get { return resetTimestamp; }
            set { resetTimestamp = Time.GetTimestamp(); }
        }

        private double? useTimestamp;
        private double? UseTimestamp
        {
            get { return useTimestamp; }
            set { useTimestamp = Time.GetTimestamp(); }
        }

        private double? useLockTimestamp;
        private double? UseLockTimestamp
        {
            get { return useLockTimestamp; }
            set { useLockTimestamp = Time.GetTimestamp(); }
        }

        private uint? LastUnlocker
        {
            get;
            set;
        }

        private string KeyCode
        {
            get;
            set;
        }

        public string LockCode
        {
            get => GetProperty(PropertyString.LockCode);
            set { if (value == null) RemoveProperty(PropertyString.LockCode); else SetProperty(PropertyString.LockCode, value); }
        }

        private string ShortDesc
        {
            get;
            set;
        }

        private string UseMessage
        {
            get;
            set;
        }

        public int? ResistLockpick
        {
            get => GetProperty(PropertyInt.ResistLockpick);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ResistLockpick); else SetProperty(PropertyInt.ResistLockpick, value.Value); }
        }

        private int? AppraisalLockpickSuccessPercent
        {
            get;
            set;
        }

        public override void ActOnUse(WorldObject worldObject)
        {
            ////if (playerDistanceTo >= 2500)
            ////{
            ////    var sendTooFarMsg = new GameEventWeenieError(player.Session, WeenieError.CantGetThere);
            ////    player.Session.Network.EnqueueSend(sendTooFarMsg, sendUseDoneEvent);
            ////    return;
            ////}

            ActionChain checkDoorChain = new ActionChain();

            checkDoorChain.AddAction(this, () =>
            {
                if (!IsLocked ?? false)
                {
                    if (!IsOpen ?? false)
                    {
                        Open(worldObject.Guid);
                    }
                    else
                    {
                        Close(worldObject.Guid);
                    }

                    // Create Door auto close timer
                    ActionChain autoCloseTimer = new ActionChain();
                    autoCloseTimer.AddDelaySeconds(ResetInterval ?? 0);
                    autoCloseTimer.AddAction(this, () => Reset());
                    autoCloseTimer.EnqueueChain();
                }
                else
                {
                    if (worldObject is Player)
                    {
                        var player = worldObject as Player;
                        var doorIsLocked = new GameEventCommunicationTransientString(player.Session, "The door is locked!");
                        player.Session.Network.EnqueueSend(doorIsLocked);
                        CurrentLandblock.EnqueueBroadcastSound(this, Sound.OpenFailDueToLock);
                    }
                }

                if (worldObject is Player)
                {
                    var player = worldObject as Player;
                    var sendUseDoneEvent = new GameEventUseDone(player.Session);
                    player.Session.Network.EnqueueSend(sendUseDoneEvent);
                }
            });

            checkDoorChain.EnqueueChain();
        }

        public void Open(ObjectGuid opener = new ObjectGuid())
        {
            if (CurrentMotionState == motionOpen)
                return;

            CurrentLandblock.EnqueueBroadcastMotion(this, motionOpen);
            CurrentMotionState = motionOpen;
            Ethereal = true;
            IsOpen = true;
            //CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(Sequences, Guid, PropertyBool.Ethereal, Ethereal ?? true));
            //CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(Sequences, Guid, PropertyBool.Open, IsOpen ?? true));
            if (opener.Full > 0)
                UseTimestamp++;
        }

        private void Close(ObjectGuid closer = new ObjectGuid())
        {
            if (CurrentMotionState == motionClosed)
                return;

            CurrentLandblock.EnqueueBroadcastMotion(this, motionClosed);
            CurrentMotionState = motionClosed;
            Ethereal = false;
            IsOpen = false;
            //CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(Sequences, Guid, PropertyBool.Ethereal, Ethereal ?? false));
            //CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(Sequences, Guid, PropertyBool.Open, IsOpen ?? false));
            if (closer.Full > 0)
                UseTimestamp++;
        }

        private void Reset()
        {
            if ((Time.GetTimestamp() - UseTimestamp) < ResetInterval)
                return;

            if (!DefaultOpen)
            {
                Close(ObjectGuid.Invalid);
                if (DefaultLocked)
                {
                    IsLocked = true;
                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(this, PropertyBool.Locked, IsLocked ?? true));
                    CurrentLandblock.EnqueueBroadcastSound(this, Sound.OpenFailDueToLock); // TODO: This should probably come 1.5 seconds after the door closes so that sounds don't overlap
                }
            }
            else
                Open(ObjectGuid.Invalid);

            ResetTimestamp++;
        }

        /// <summary>
        /// Used for unlocking a door via lockpick, so contains a skill check
        /// player.Skills[Skill.Lockpick].Current should be sent for the skill check
        /// </summary>
        public UnlockResults Unlock(uint playerLockpickSkillLvl)
        {
            return LockHelper.Unlock(this, playerLockpickSkillLvl);
        }

        /// <summary>
        /// Used for unlocking a door via a key
        /// </summary>
        public UnlockResults Unlock(string keyCode)
        {
            return LockHelper.Unlock(this, keyCode);
        }

        public override void ActivateLinks()
        {
            if (LinkedInstances.Count > 0)
            {
                foreach (var link in LinkedInstances)
                {
                    var wo = WorldObjectFactory.CreateWorldObject(DatabaseManager.World.GetCachedWeenie(link.WeenieClassId), new ObjectGuid(link.Guid));

                    if (wo != null)
                    {
                        wo.Location = new Position(link.ObjCellId, link.OriginX, link.OriginY, link.OriginZ, link.AnglesX, link.AnglesY, link.AnglesZ, link.AnglesW);

                        wo.ActivationTarget = Guid.Full;

                        CurrentLandblock.AddWorldObject(wo);
                    }
                }
            }
        }
    }
}

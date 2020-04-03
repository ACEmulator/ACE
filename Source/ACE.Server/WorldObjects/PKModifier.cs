using System;

using ACE.Common;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class PKModifier : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public PKModifier(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public PKModifier(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        public bool IsPKSwitch  => PkLevelModifier ==  1;
        public bool IsNPKSwitch => PkLevelModifier == -1;

        private void SetEphemeralValues()
        {
            CurrentMotionState = new Motion(MotionStance.NonCombat);

            if (IsNPKSwitch)
                ObjectDescriptionFlags |= ObjectDescriptionFlag.NpkSwitch;

            if (IsPKSwitch)
                ObjectDescriptionFlags |= ObjectDescriptionFlag.PkSwitch;
        }

        public override ActivationResult CheckUseRequirements(WorldObject activator)
        {
            if (!(activator is Player player))
                return new ActivationResult(false);

            if (player.PkLevel > PKLevel.PK || PropertyManager.GetBool("pk_server").Item || PropertyManager.GetBool("pkl_server").Item)
            {
                if (!string.IsNullOrWhiteSpace(GetProperty(PropertyString.UsePkServerError)))
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.UsePkServerError), ChatMessageType.Broadcast));

                return new ActivationResult(false);
            }

            if (player.PlayerKillerStatus == PlayerKillerStatus.PKLite)
            {
                if (!string.IsNullOrWhiteSpace(GetProperty(PropertyString.UsePkServerError)))
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.UsePkServerError), ChatMessageType.Broadcast));

                player.Session.Network.EnqueueSend(new GameMessageSystemChat("Player Killer Lites may not change their PK status.", ChatMessageType.Broadcast)); // not sure how retail handled this case

                return new ActivationResult(false);
            }

            if (player.Teleporting)
                return new ActivationResult(false);

            if (player.IsBusy)
                return new ActivationResult(false);

            if (player.IsAdvocate || player.AdvocateQuest || player.AdvocateState)
            {
                return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.AdvocatesCannotChangePKStatus));
            }

            if (player.MinimumTimeSincePk != null)
            {
                return new ActivationResult(new GameEventWeenieError(player.Session, WeenieError.CannotChangePKStatusWhileRecovering));
            }

            if (IsBusy)
            {
                return new ActivationResult(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.The_IsCurrentlyInUse, Name));
            }

            return new ActivationResult(true);
        }

        public override void ActOnUse(WorldObject activator)
        {
            if (!(activator is Player player))
                return;

            if (IsBusy)
            {
                player.Session.Network.EnqueueSend(new GameEventWeenieErrorWithString(player.Session, WeenieErrorWithString.The_IsCurrentlyInUse, Name));
                return;
            }

            if (player.PkLevel == PKLevel.PK && IsNPKSwitch && (Time.GetUnixTime() - player.PkTimestamp) < MinimumTimeSincePk)
            {
                IsBusy = true;
                player.IsBusy = true;

                var actionChain = new ActionChain();

                if (UseTargetFailureAnimation != MotionCommand.Invalid)
                {
                    var useMotion = UseTargetFailureAnimation;
                    EnqueueBroadcastMotion(new Motion(this, useMotion));

                    var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
                    var useTime = motionTable.GetAnimationLength(useMotion);

                    player.LastUseTime += useTime;

                    actionChain.AddDelaySeconds(useTime);
                }

                actionChain.AddAction(player, () =>
                {
                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouFeelAHarshDissonance));
                    player.IsBusy = false;
                    Reset();
                });

                actionChain.EnqueueChain();

                return;
            }

            if ((player.PkLevel == PKLevel.NPK && IsPKSwitch) || (player.PkLevel == PKLevel.PK && IsNPKSwitch))
            {
                IsBusy = true;
                player.IsBusy = true;

                var useMotion = UseTargetSuccessAnimation != MotionCommand.Invalid ? UseTargetSuccessAnimation : MotionCommand.Twitch1;
                EnqueueBroadcastMotion(new Motion(this, useMotion));

                var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
                var useTime = motionTable.GetAnimationLength(useMotion);

                player.LastUseTime += useTime;

                var actionChain = new ActionChain();

                actionChain.AddDelaySeconds(useTime);

                actionChain.AddAction(player, () =>
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.UseMessage), ChatMessageType.Broadcast));
                    player.PkLevelModifier += PkLevelModifier;

                    if (player.PkLevel == PKLevel.PK)
                        player.PlayerKillerStatus = PlayerKillerStatus.PK;
                    else
                        player.PlayerKillerStatus = PlayerKillerStatus.NPK;

                    player.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(player, PropertyInt.PlayerKillerStatus, (int)player.PlayerKillerStatus));
                    //player.ApplySoundEffects(Sound.Open); // in pcaps, but makes no sound/has no effect. ?
                    player.IsBusy = false;
                    Reset();
                });

                actionChain.EnqueueChain();
            }
            else
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.ActivationFailure), ChatMessageType.Broadcast));
        }

        public void Reset()
        {
            IsBusy = false;
        }

        public double? MinimumTimeSincePk
        {
            get => GetProperty(PropertyFloat.MinimumTimeSincePk);
            set { if (!value.HasValue) RemoveProperty(PropertyFloat.MinimumTimeSincePk); else SetProperty(PropertyFloat.MinimumTimeSincePk, value.Value); }
        }
    }
}

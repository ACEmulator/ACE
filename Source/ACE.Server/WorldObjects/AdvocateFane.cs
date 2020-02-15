using System;

using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class AdvocateFane : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public AdvocateFane(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public AdvocateFane(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            CurrentMotionState = new Motion(MotionStance.NonCombat);
        }

        public override ActivationResult CheckUseRequirements(WorldObject activator)
        {
            if (!(activator is Player player))
                return new ActivationResult(false);

            if (player.Teleporting)
                return new ActivationResult(false);

            if (player.IsBusy)
                return new ActivationResult(false);

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

            IsBusy = true;
            player.IsBusy = true;

            if (player.AdvocateQuest || player.PkLevel != PKLevel.NPK || player is Admin || player.WeenieType == WeenieType.Admin || player is Sentinel || player.WeenieType == WeenieType.Sentinel) // PlayerKillers, Admins and Sentinels can't be Advocates.
            {                                
                var actionChain = new ActionChain();

                var failMotion = UseTargetFailureAnimation != MotionCommand.Invalid ? UseTargetFailureAnimation : MotionCommand.Twitch2;
                EnqueueBroadcastMotion(new Motion(this, failMotion));

                var motionTable = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId);
                var useTime = motionTable.GetAnimationLength(failMotion);

                player.LastUseTime += useTime;

                actionChain.AddDelaySeconds(useTime);

                actionChain.AddAction(player, () =>
                {
                    player.IsBusy = false;
                    Reset();
                });

                actionChain.EnqueueChain();

                return;
            }

            var faneTimer = new ActionChain();

            if (player.CombatMode != CombatMode.NonCombat)
            {
                var stanceTime = player.SetCombatMode(CombatMode.NonCombat);
                faneTimer.AddDelaySeconds(stanceTime);

                player.LastUseTime += stanceTime;
            }

            var useMotion = UseTargetSuccessAnimation != MotionCommand.Invalid ? UseUserAnimation : MotionCommand.BowDeep;
            var animTime = player.EnqueueMotion(faneTimer, useMotion);
            player.LastUseTime += animTime;

            var successMotion = UseTargetSuccessAnimation != MotionCommand.Invalid ? UseTargetSuccessAnimation : MotionCommand.Twitch1;
            var successTime = EnqueueMotion(faneTimer, successMotion);
            player.LastUseTime += successTime;

            faneTimer.AddAction(player, () =>
            {
                player.AdvocateQuest = true;
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.UseMessage), ChatMessageType.Broadcast));

                if (UseCreateItem != null)
                {
                    var useCreateItem = WorldObjectFactory.CreateNewWorldObject(UseCreateItem.Value);

                    if (useCreateItem != null)
                        player.TryCreateInInventoryWithNetworking(useCreateItem);
                }

                if (PropertyManager.GetBool("advocate_fane_auto_bestow").Item)
                    Advocate.Bestow(player, (int)PropertyManager.GetDouble("advocate_fane_auto_bestow_level").Item);
            });

            faneTimer.AddAction(player, () =>
            {
                player.IsBusy = false;
                Reset();
            });

            faneTimer.EnqueueChain();
        }

        public void Reset()
        {
            IsBusy = false;
        }
    }
}

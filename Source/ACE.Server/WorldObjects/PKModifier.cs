using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;

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

        private void SetEphemeralValues()
        {
            if (PkLevelModifier == -1)
                BaseDescriptionFlags |= ObjectDescriptionFlag.NpkSwitch;

            if (PkLevelModifier == 1)
                BaseDescriptionFlags |= ObjectDescriptionFlag.PkSwitch;
        }

        private static readonly UniversalMotion twitch = new UniversalMotion(MotionStance.NonCombat, new MotionItem(MotionCommand.Twitch1));

        public uint? UseTargetSuccessAnimation
        {
            get => GetProperty(PropertyDataId.UseTargetSuccessAnimation);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.UseTargetSuccessAnimation); else SetProperty(PropertyDataId.UseTargetSuccessAnimation, value.Value); }
        }

        public uint? UseTargetFailureAnimation
        {
            get => GetProperty(PropertyDataId.UseTargetFailureAnimation);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.UseTargetFailureAnimation); else SetProperty(PropertyDataId.UseTargetFailureAnimation, value.Value); }
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem, and is wrapped in ActionChain.<para />
        /// The actor of the ActionChain is the item being used.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject worldObject)
        {
            if (worldObject is Player)
            {
                var player = worldObject as Player;
                //if (ServerIsPKServer) // Need some form of config switch in configmanager...
                //{
                //    player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.UsePkServerError), ChatMessageType.Broadcast));
                //    player.SendUseDoneEvent();
                //    return;
                //}

                if (AllowedActivator == null)
                {
                    if (player.IsAdvocate || (player.AdvocateQuest ?? false) || (player.AdvocateState ?? false))
                    {
                        // Advocates cannot change their PK status
                        if (PkLevelModifier == 1)
                        {
                            player.SendUseDoneEvent();
                            return; // maybe send error msg to tell PK to ask another advocate to @remove them (or maybe make the @remove command support self removal)
                        }

                        // letting it fall through for the NpkSwitch because it will not change status and error properly.
                    }

                    //if (player.PkLevelModifier == 0) // wrong check but if PkTimestamp(? maybe different timestamp) + MINIMUM_TIME_SINCE_PK_FLOAT < Time.GetUnixTimestamp proceed else fail
                    //{
                    if ((player.PkLevelModifier ?? -1) != PkLevelModifier)
                    {
                        AllowedActivator = ObjectGuid.Invalid.Full;

                        var switchTimer = new ActionChain();
                        var turnToMotion = new UniversalMotion(MotionStance.NonCombat, Location, Guid);
                        turnToMotion.MovementTypes = MovementTypes.TurnToObject;
                        switchTimer.AddAction(this, () => player.CurrentLandblock?.EnqueueBroadcastMotion(player, turnToMotion));
                        switchTimer.AddDelaySeconds(1);
                        switchTimer.AddAction(player, () =>
                        {
                            if (UseTargetSuccessAnimation.HasValue)
                                CurrentLandblock?.EnqueueBroadcastMotion(this, new UniversalMotion(MotionStance.NonCombat, new MotionItem((MotionCommand)UseTargetSuccessAnimation)));
                            else
                                CurrentLandblock?.EnqueueBroadcastMotion(this, twitch);
                        });
                        if (UseTargetSuccessAnimation.HasValue)
                            switchTimer.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength((MotionCommand)UseTargetSuccessAnimation));
                        else
                            switchTimer.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.Twitch1));
                        switchTimer.AddAction(player, () =>
                        {
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.UseMessage), ChatMessageType.Broadcast));
                            player.PkLevelModifier = PkLevelModifier;

                            player.SendUseDoneEvent();

                            if (player.PkLevelModifier == 1)
                                player.PlayerKillerStatus = PlayerKillerStatus.PK;
                            else
                                player.PlayerKillerStatus = PlayerKillerStatus.NPK;

                            player.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(player, PropertyInt.PlayerKillerStatus, (int)player.PlayerKillerStatus));

                            Reset();
                        });
                        switchTimer.EnqueueChain();
                    }
                    else
                    {
                        if (UseTargetFailureAnimation.HasValue)
                            CurrentLandblock?.EnqueueBroadcastMotion(this, new UniversalMotion(MotionStance.NonCombat, new MotionItem((MotionCommand)UseTargetFailureAnimation)));
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.ActivationFailure), ChatMessageType.Broadcast));
                        player.SendUseDoneEvent();
                    }
                    //}
                }
                else
                {
                    // do nothing / in use error msg?
                    player.SendUseDoneEvent();
                }
            }
        }

        public void Reset()
        {
            AllowedActivator = null;
        }
    }
}

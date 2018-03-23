using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;

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
        }

        private static readonly UniversalMotion twitch = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Twitch1));

        private static readonly UniversalMotion twitch2 = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Twitch2));

        private static readonly UniversalMotion bowDeep = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.BowDeep));

        public uint? AllowedActivator
        {
            get => GetProperty(PropertyInstanceId.AllowedActivator);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.AllowedActivator); else SetProperty(PropertyInstanceId.AllowedActivator, value.Value); }
        }

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

        public uint? UseUserAnimation
        {
            get => GetProperty(PropertyDataId.UseUserAnimation);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.UseUserAnimation); else SetProperty(PropertyDataId.UseUserAnimation, value.Value); }
        }

        public uint? UseCreateItem
        {
            get => GetProperty(PropertyDataId.UseCreateItem);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.UseCreateItem); else SetProperty(PropertyDataId.UseCreateItem, value.Value); }
        }

        public override void ActOnUse(Player player)
        {
            if (!player.IsWithinUseRadiusOf(this))
                player.DoMoveTo(this);
            else
            {

                if (AllowedActivator == null)
                {
                    if ((player.PkLevelModifier ?? -1) != -1 || player.WeenieType == WeenieType.Admin || player.WeenieType == WeenieType.Sentinel) // PlayerKillers, Admins and Sentinels can't be Advocates.
                    {
                        //error msg here
                        if (UseTargetFailureAnimation.HasValue)
                            CurrentLandblock.EnqueueBroadcastMotion(this, new UniversalMotion(MotionStance.Standing, new MotionItem((MotionCommand)UseTargetFailureAnimation)));
                        else
                            CurrentLandblock.EnqueueBroadcastMotion(this, twitch2);

                        player.SendUseDoneEvent();
                        return;
                    }

                    if (!(player.AdvocateQuest ?? false))
                    {
                        AllowedActivator = ObjectGuid.Invalid.Full;

                        var faneTimer = new ActionChain();
                        var turnToMotion = new UniversalMotion(MotionStance.Standing, Location, Guid);
                        turnToMotion.MovementTypes = MovementTypes.TurnToObject;
                        faneTimer.AddAction(this, () => player.CurrentLandblock.EnqueueBroadcastMotion(player, turnToMotion));
                        faneTimer.AddDelaySeconds(1);
                        faneTimer.AddAction(player, () =>
                        {
                            if (UseUserAnimation.HasValue)
                                CurrentLandblock.EnqueueBroadcastMotion(player, new UniversalMotion(MotionStance.Standing, new MotionItem((MotionCommand)UseUserAnimation)));
                            else
                                CurrentLandblock.EnqueueBroadcastMotion(player, bowDeep);
                        });
                        faneTimer.AddAction(player, () =>
                        {
                            if (UseTargetSuccessAnimation.HasValue)
                                CurrentLandblock.EnqueueBroadcastMotion(this, new UniversalMotion(MotionStance.Standing, new MotionItem((MotionCommand)UseTargetSuccessAnimation)));
                            else
                                CurrentLandblock.EnqueueBroadcastMotion(this, twitch);
                        });
                        if (UseTargetSuccessAnimation.HasValue)
                            faneTimer.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength((MotionCommand)UseTargetSuccessAnimation));
                        else
                            faneTimer.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.Twitch1));
                        faneTimer.AddAction(player, () =>
                        {
                            player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.UseMessage), ChatMessageType.Broadcast));
                            player.AdvocateQuest = true;

                            if (UseCreateItem.HasValue)
                            {
                                var useCreateItem = WorldObjectFactory.CreateNewWorldObject(UseCreateItem.Value);

                                if (useCreateItem != null)
                                    player.TryCreateInInventoryWithNetworking(useCreateItem);
                            }

                            #region BestowCommandStuff // This stuff did not occur automatically IIRC, it was based on someone Advocate Level 2 or above issuing a bestow command. This is here temp likely
                            if (!player.AdvocateLevel.HasValue)
                                player.AdvocateLevel = 1;

                            player.IsAdvocate = true;

                            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, PropertyInt.AdvocateLevel, player.AdvocateLevel ?? 1));
                            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyBool(player, PropertyBool.IsAdvocate, player.IsAdvocate));

                            if (player.ChannelsActive.HasValue)
                                player.ChannelsActive |= Channel.Help | Channel.Abuse | Channel.Advocate1 | Channel.Advocate2 | Channel.Advocate3;
                            else
                                player.ChannelsActive = Channel.Help | Channel.Abuse | Channel.Advocate1 | Channel.Advocate2 | Channel.Advocate3;

                            if (player.ChannelsAllowed.HasValue)
                                player.ChannelsAllowed |= Channel.Help | Channel.Abuse | Channel.Advocate1 | Channel.Advocate2 | Channel.Advocate3 | Channel.TownChans;
                            else
                                player.ChannelsAllowed = Channel.Help | Channel.Abuse | Channel.Advocate1 | Channel.Advocate2 | Channel.Advocate3 | Channel.TownChans;

                            var useCreateBook = WorldObjectFactory.CreateNewWorldObject("bookadvocateinstructions");

                            if (useCreateBook != null)
                                player.TryCreateInInventoryWithNetworking(useCreateBook);

                            var useCreateAegis = WorldObjectFactory.CreateNewWorldObject($"shieldadvocate{player.AdvocateLevel ?? 1}");

                            if (useCreateAegis != null)
                                player.TryCreateInInventoryWithNetworking(useCreateAegis);
                            #endregion

                            player.SendUseDoneEvent();
                            
                            Reset();
                        });
                        faneTimer.EnqueueChain();
                    }
                    else
                    {
                        if (UseTargetFailureAnimation.HasValue)
                            CurrentLandblock.EnqueueBroadcastMotion(this, new UniversalMotion(MotionStance.Standing, new MotionItem((MotionCommand)UseTargetFailureAnimation)));
                        else
                            CurrentLandblock.EnqueueBroadcastMotion(this, twitch2);

                        player.SendUseDoneEvent();
                    }
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

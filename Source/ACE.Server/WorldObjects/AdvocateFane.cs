using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
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
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject worldObject)
        {
            var player = worldObject as Player;
            if (player == null) return;

            if (AllowedActivator != null)
            {
                // do nothing / in use error msg?
                return;
            }

            if (player.AdvocateQuest || player.PkLevelModifier != -1 || player.WeenieType == WeenieType.Admin || player.WeenieType == WeenieType.Sentinel) // PlayerKillers, Admins and Sentinels can't be Advocates.
            {
                var failMotion = UseTargetFailureAnimation != MotionCommand.Invalid ? UseTargetFailureAnimation : MotionCommand.Twitch2;
                EnqueueBroadcastMotion(new Motion(MotionStance.NonCombat, failMotion));
                return;
            }

            AllowedActivator = ObjectGuid.Invalid.Full;

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

            faneTimer.AddAction(player, () => Bestow(player));

            faneTimer.EnqueueChain();
        }

        public void Bestow(Player player)
        {
            player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.UseMessage), ChatMessageType.Broadcast));
            player.AdvocateQuest = true;

            if (UseCreateItem != null)
            {
                var useCreateItem = WorldObjectFactory.CreateNewWorldObject(UseCreateItem.Value);

                if (useCreateItem != null)
                    player.TryCreateInInventoryWithNetworking(useCreateItem);
            }

            // This stuff did not occur automatically IIRC, it was based on someone Advocate Level 2 or above issuing a bestow command. This is here temp likely
            if (!player.AdvocateLevel.HasValue)
                player.AdvocateLevel = 1;

            player.IsAdvocate = true;

            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, PropertyInt.AdvocateLevel, player.AdvocateLevel ?? 1));
            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyBool(player, PropertyBool.IsAdvocate, player.IsAdvocate));

            var advocateChannels = Channel.Help | Channel.Abuse | Channel.Advocate1 | Channel.Advocate2 | Channel.Advocate3;

            if (player.ChannelsActive == null)
                player.ChannelsActive = advocateChannels;
            else
                player.ChannelsActive |= advocateChannels;

            if (player.ChannelsAllowed == null)
                player.ChannelsAllowed = advocateChannels | Channel.TownChans;
            else
                player.ChannelsAllowed |= advocateChannels | Channel.TownChans;

            var useCreateBook = WorldObjectFactory.CreateNewWorldObject("bookadvocateinstructions");

            if (useCreateBook != null)
                player.TryCreateInInventoryWithNetworking(useCreateBook);

            var useCreateAegis = WorldObjectFactory.CreateNewWorldObject($"shieldadvocate{player.AdvocateLevel.Value}");

            if (useCreateAegis != null)
                player.TryCreateInInventoryWithNetworking(useCreateAegis);

            Reset();
        }

        public void Reset()
        {
            AllowedActivator = null;
        }
    }
}

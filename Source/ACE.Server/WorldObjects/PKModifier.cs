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
            CurrentMotionState = new Motion(MotionStance.Invalid);

            if (PkLevelModifier == -1)
                BaseDescriptionFlags |= ObjectDescriptionFlag.NpkSwitch;

            if (PkLevelModifier == 1)
                BaseDescriptionFlags |= ObjectDescriptionFlag.PkSwitch;
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject activator)
        {
            if (!(activator is Player player))
                return;

            if (AllowedActivator != null)
            {
                // do nothing / in use error msg?
                return;
            }

            if (player.IsAdvocate || player.AdvocateQuest || player.AdvocateState)
            {
                // Advocates cannot change their PK status
                if (PkLevelModifier == 1)
                    return; // maybe send error msg to tell PK to ask another advocate to @remove them (or maybe make the @remove command support self removal)

                // letting it fall through for the NpkSwitch because it will not change status and error properly.
            }

            //if (player.PkLevelModifier == 0) // wrong check but if PkTimestamp(? maybe different timestamp) + MINIMUM_TIME_SINCE_PK_FLOAT < Time.GetUnixTimestamp proceed else fail
            //{
            if (player.PkLevelModifier != PkLevelModifier)
            {
                AllowedActivator = ObjectGuid.Invalid.Full;

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
                    player.PkLevelModifier = PkLevelModifier;

                    if (player.PkLevelModifier == 1)
                        player.PlayerKillerStatus = PlayerKillerStatus.PK;
                    else
                        player.PlayerKillerStatus = PlayerKillerStatus.NPK;

                    player.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(player, PropertyInt.PlayerKillerStatus, (int)player.PlayerKillerStatus));

                    Reset();
                });

                actionChain.EnqueueChain();
            }
            else
            {
                if (UseTargetFailureAnimation != MotionCommand.Invalid)
                    EnqueueBroadcastMotion(new Motion(this, UseTargetFailureAnimation));

                player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.ActivationFailure), ChatMessageType.Broadcast));
            }
        }

        public void Reset()
        {
            AllowedActivator = null;
        }
    }
}

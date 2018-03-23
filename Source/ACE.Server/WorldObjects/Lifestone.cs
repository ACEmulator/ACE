using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    public class Lifestone : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Lifestone(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Lifestone(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            BaseDescriptionFlags |= ObjectDescriptionFlag.LifeStone;

            RadarColor = ACE.Entity.Enum.RadarColor.LifeStone;
        }

        private static readonly UniversalMotion sanctuary = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Sanctuary));

        public override void ActOnUse(Player player)
        {
            ActionChain sancTimer = new ActionChain();
            sancTimer.AddAction(player, () =>
            {
                CurrentLandblock.EnqueueBroadcastMotion(player, sanctuary);
                CurrentLandblock.EnqueueBroadcastSound(player, Sound.LifestoneOn, 1);
            });
            sancTimer.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<MotionTable>(player.MotionTableId).GetAnimationLength(MotionCommand.Sanctuary));
            sancTimer.AddAction(player, () =>
            {
                if (player.IsWithinUseRadiusOf(this))
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(GetProperty(PropertyString.UseMessage), ChatMessageType.Magic));
                    player.Sanctuary = player.Location;
                }
                // Unsure if there was a fail message...
                //else
                //{
                //    var serverMessage = "You wandered too far to attune with the Lifestone!";
                //    player.Session.Network.EnqueueSend(new GameMessageSystemChat(serverMessage, ChatMessageType.Magic));
                //}

                player.SendUseDoneEvent();
            });
            sancTimer.EnqueueChain();
        }
    }
}

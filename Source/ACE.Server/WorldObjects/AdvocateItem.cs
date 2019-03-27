using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameMessages.Messages;
using System.Collections.Generic;

namespace ACE.Server.WorldObjects
{
    public class AdvocateItem : GenericObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public AdvocateItem(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public AdvocateItem(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            //if (Biota.BiotaPropertiesEmote.Count == 0)
            //{
            //    Biota.BiotaPropertiesEmote.Add(new BiotaPropertiesEmote
            //    {
            //        ObjectId = Guid.Full,
            //        Category = (uint)EmoteCategory.Wield,
            //        Probability = 1,
            //        BiotaPropertiesEmoteAction = new List<BiotaPropertiesEmoteAction>
            //        {
            //            new BiotaPropertiesEmoteAction
            //            {
            //                EmoteId = uint.MaxValue,
            //                Order = 0,
            //                Type = (int)EmoteType.SetIntStat,
            //                Delay = 0,
            //                Extent = 1,
            //                Stat = (int)ACE.Entity.Enum.Properties.PropertyInt.RadarBlipColor,
            //                Amount = (int)ACE.Entity.Enum.RadarColor.Advocate
            //            }
            //        }
            //    });

            //    Biota.BiotaPropertiesEmote.Add(new BiotaPropertiesEmote
            //    {
            //        ObjectId = Guid.Full,
            //        Category = (uint)EmoteCategory.UnWield,
            //        Probability = 1,
            //        BiotaPropertiesEmoteAction = new List<BiotaPropertiesEmoteAction>
            //        {
            //            new BiotaPropertiesEmoteAction
            //            {
            //                EmoteId = uint.MaxValue,
            //                Order = 0,
            //                Type = (int)EmoteType.SetIntStat,
            //                Delay = 0,
            //                Extent = 1,
            //                Stat = (int)ACE.Entity.Enum.Properties.PropertyInt.RadarBlipColor,
            //                Amount = null
            //            }
            //        }
            //    });
            //}
        }

        public override void OnWield(Creature creature)
        {
            //var player = creature as Player;

            //player.UpdateProperty(player, ACE.Entity.Enum.Properties.PropertyInt.RadarBlipColor, (int)ACE.Entity.Enum.RadarColor.Advocate);
            //player.EnqueueBroadcast(new GameMessagePublicUpdatePropertyInt(player, (PropertyInt)emote.Stat, Convert.ToInt32(emote.Amount)));
            creature.RadarColor = ACE.Entity.Enum.RadarColor.Advocate;
            creature.EnqueueBroadcast(true, new GameMessagePublicUpdatePropertyInt(creature, PropertyInt.RadarBlipColor, (int)creature.RadarColor));
            creature.SetProperty(PropertyBool.AdvocateState, true);

            base.OnWield(creature);
        }

        public override void OnUnWield(Creature creature)
        {
            creature.RadarColor = null;
            creature.EnqueueBroadcast(true, new GameMessagePublicUpdatePropertyInt(creature, PropertyInt.RadarBlipColor, 0));
            creature.SetProperty(PropertyBool.AdvocateState, false);

            base.OnUnWield(creature);
        }
    }
}

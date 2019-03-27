using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
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
            if (Biota.BiotaPropertiesEmote.Count == 0)
            {
                Biota.BiotaPropertiesEmote.Add(new BiotaPropertiesEmote
                {
                    ObjectId = Guid.Full,
                    Category = (uint)EmoteCategory.Wield,
                    Probability = 1,
                    BiotaPropertiesEmoteAction = new List<BiotaPropertiesEmoteAction>
                    {
                        new BiotaPropertiesEmoteAction
                        {
                            EmoteId = uint.MaxValue,
                            Order = 0,
                            Type = (int)EmoteType.SetIntStat,
                            Delay = 0,
                            Extent = 1,
                            Stat = (int)ACE.Entity.Enum.Properties.PropertyInt.RadarBlipColor,
                            Amount = (int)ACE.Entity.Enum.RadarColor.Advocate
                        }
                    }
                });

                Biota.BiotaPropertiesEmote.Add(new BiotaPropertiesEmote
                {
                    ObjectId = Guid.Full,
                    Category = (uint)EmoteCategory.UnWield,
                    Probability = 1,
                    BiotaPropertiesEmoteAction = new List<BiotaPropertiesEmoteAction>
                    {
                        new BiotaPropertiesEmoteAction
                        {
                            EmoteId = uint.MaxValue,
                            Order = 0,
                            Type = (int)EmoteType.SetIntStat,
                            Delay = 0,
                            Extent = 1,
                            Stat = (int)ACE.Entity.Enum.Properties.PropertyInt.RadarBlipColor,
                            Amount = null
                        }
                    }
                });
            }
        }
    }
}

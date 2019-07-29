using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesEmote
    {
        public BiotaPropertiesEmote()
        {
            BiotaPropertiesEmoteAction = new HashSet<BiotaPropertiesEmoteAction>();
        }

        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public uint Category { get; set; }
        public float Probability { get; set; }
        public uint? WeenieClassId { get; set; }
        public uint? Style { get; set; }
        public uint? Substyle { get; set; }
        public string Quest { get; set; }
        public int? VendorType { get; set; }
        public float? MinHealth { get; set; }
        public float? MaxHealth { get; set; }

        public virtual Biota Object { get; set; }
        public virtual ICollection<BiotaPropertiesEmoteAction> BiotaPropertiesEmoteAction { get; set; }
    }
}

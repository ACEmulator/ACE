using System;
using System.Collections.Generic;

namespace ACE.Entity.Models
{
    public class PropertiesEmote
    {
        public uint Category { get; set; } // todo should this be a key in the Weenie/Biota dictionary?
        public float Probability { get; set; }
        public uint? WeenieClassId { get; set; }
        public uint? Style { get; set; }
        public uint? Substyle { get; set; }
        public string Quest { get; set; }
        public int? VendorType { get; set; }
        public float? MinHealth { get; set; }
        public float? MaxHealth { get; set; }

        public Weenie Object { get; set; }
        public IList<PropertiesEmoteAction> PropertiesEmoteAction { get; set; } = new List<PropertiesEmoteAction>();

        public PropertiesEmote Clone()
        {
            var result = new PropertiesEmote
            {
                Category = Category,
                Probability = Probability,
                WeenieClassId = WeenieClassId,
                Style = Style,
                Substyle = Substyle,
                Quest = Quest,
                VendorType = VendorType,
                MinHealth = MinHealth,
                MaxHealth = MaxHealth,
            };

            foreach (var action in PropertiesEmoteAction)
                result.PropertiesEmoteAction.Add(action.Clone());

            return result;
        }
    }
}

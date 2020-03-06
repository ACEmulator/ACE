using System;
using System.Collections.Generic;

using ACE.Entity.Enum;

namespace ACE.Entity.Models
{
    public class PropertiesEmote
    {
        public EmoteCategory Category { get; set; }
        public float Probability { get; set; }
        public uint? WeenieClassId { get; set; }
        public MotionStance? Style { get; set; }
        public MotionCommand? Substyle { get; set; }
        public string Quest { get; set; }
        public VendorType? VendorType { get; set; }
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

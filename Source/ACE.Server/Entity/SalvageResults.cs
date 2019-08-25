using System;
using System.Collections.Generic;
using System.Text;
using ACE.Entity.Enum;

namespace ACE.Server.Entity
{
    public class SalvageResults
    {
        public Dictionary<uint, SalvageMessage> Messages;

        public SalvageResults()
        {
            Messages = new Dictionary<uint, SalvageMessage>();
        }

        public SalvageMessage GetMessage(MaterialType materialType, Skill skill)
        {
            var key = (uint)materialType << 16 | (uint)skill;

            if (!Messages.TryGetValue(key, out var message))
            {
                message = new SalvageMessage(materialType, skill);
                Messages.Add(key, message);
            }
            return message;
        }

        public Dictionary<Skill, List<SalvageMessage>> GetMessages()
        {
            var skillGroups = new Dictionary<Skill, List<SalvageMessage>>();
            foreach (var kvp in Messages)
            {
                if (!skillGroups.TryGetValue(kvp.Value.Skill, out var messages))
                {
                    messages = new List<SalvageMessage>();
                    skillGroups.Add(kvp.Value.Skill, messages);
                }
                messages.Add(kvp.Value);
            }
            return skillGroups;
        }
    }

    public class SalvageMessage
    {
        // You obtain <amount> <material> (ws <workmanship>) using your knowledge of <skill>.
        // Your augmentation has given you a return bonus of <augMod>!

        public uint Amount;
        public MaterialType MaterialType;
        public float Workmanship;
        public int NumItemsInMaterial;
        public Skill Skill;

        public SalvageMessage(MaterialType materialType, Skill skill)
        {
            MaterialType = materialType;
            Skill = skill;
        }
    }
}

using System;

namespace ACE.Entity
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SkillCostAttribute : Attribute
    {
        public bool TrainsFree { get; set; } = false;

        public byte TrainingCost { get; set; }

        /// <summary>
        /// to be clear, this means a skill that can be specialized by normal means.  the exceptions
        /// will be things like Tinkering.
        /// </summary>
        public bool CanSpecialize { get; set; } = true;

        public byte SpecializationCost { get; set; }

        public SkillCostAttribute(byte trainingCost)
        {
            TrainingCost = trainingCost;
        }

        public SkillCostAttribute(byte trainingCost, byte specializationCost)
        {
            TrainingCost = trainingCost;
            SpecializationCost = specializationCost;
        }
    }
}

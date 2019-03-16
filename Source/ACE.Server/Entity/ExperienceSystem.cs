using System;
using ACE.Entity.Enum;

namespace ACE.Server.Entity
{
    public static class ExperienceSystem
    {
        public static ulong ItemLevelToTotalXP(int itemLevel, ulong baseXP, int maxLevel, ItemXpStyle xpScheme)
        {
            if (itemLevel < 1)
                return 0;

            if (itemLevel > maxLevel)
                itemLevel = maxLevel;

            if (itemLevel == 1)
                return baseXP;

            switch (xpScheme)
            {
                case ItemXpStyle.Fixed:
                    return (ulong)itemLevel * baseXP;

                case ItemXpStyle.ScalesWithLevel:
                default:
                    var levelXP = baseXP;
                    var totalXP = baseXP;

                    for (var i = itemLevel - 1; i > 0; i--)
                    {
                        levelXP *= 2;
                        totalXP += levelXP;
                    }

                    return totalXP;

                case ItemXpStyle.FixedPlusBase:

                    return (ulong)itemLevel * baseXP + baseXP;
            }
        }

        public static int ItemTotalXPToLevel(ulong gainedXP, ulong baseXP, int maxLevel, ItemXpStyle xpScheme)
        {
            var level = 0;

            switch (xpScheme)
            {
                case ItemXpStyle.Fixed:
                    level = (int)Math.Floor((double)gainedXP / baseXP);
                    break;

                case ItemXpStyle.ScalesWithLevel:

                    var levelXP = baseXP;
                    var remainXP = gainedXP;

                    while (remainXP >= levelXP)
                    {
                        level++;

                        remainXP -= levelXP;
                        levelXP *= 2;
                    }
                    break;

                case ItemXpStyle.FixedPlusBase:

                    if (gainedXP >= baseXP && gainedXP < baseXP * 3)
                        level = 1;
                    else
                        level = (int)Math.Floor((double)(gainedXP - baseXP) / baseXP);

                    break;
            }

            if (level > maxLevel)
                level = maxLevel;

            return level;
        }
    }
}

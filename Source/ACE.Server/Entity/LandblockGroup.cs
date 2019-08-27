using System;
using System.Collections;
using System.Collections.Generic;

using log4net;

namespace ACE.Server.Entity
{
    class LandblockGroup : IEnumerable<Landblock>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const int LandblockGroupMinSpacing = 10;
        private const int landblockGroupSpanRequiredBeforeSplitEligibility = LandblockGroupMinSpacing * 4;

        public bool IsDungeon { get; private set; }

        // Trying to split a group is a very costly operation, so we only do it periodically
        private readonly TimeSpan splitRetryPeriod = TimeSpan.FromMinutes(5);
        private DateTime nextSplitRetryTime = DateTime.MinValue;

        private readonly HashSet<Landblock> landblocks = new HashSet<Landblock>();

        private int xMin = int.MaxValue;
        private int xMax = int.MinValue;
        private int yMin = int.MaxValue;
        private int yMax = int.MinValue;

        private int xCenter;
        private int yCenter;

        private int width;
        private int height;

        public LandblockGroup()
        {
        }

        public LandblockGroup(Landblock landblock)
        {
            Add(landblock);
        }


        public int Count => landblocks.Count;

        public bool Contains(Landblock landblock)
        {
            return landblocks.Contains(landblock);
        }

        public bool Add(Landblock landblock)
        {
            if (landblocks.Count > 0)
            {
                if (IsDungeon)
                {
                    log.Error($"You cannot add a landblock ({landblock.Id}) to a LandblockGroup that represents a single Dungeon Landblock");
                    return false;
                }

                if (landblock.IsDungeon)
                {
                    log.Error($"You cannot add a dungeon landblock ({landblock.Id}) to an existing LandblockGroup");
                    return false;
                }
            }

            if (landblocks.Add(landblock))
            {
                if (landblocks.Count == 1)
                    IsDungeon = landblock.IsDungeon;

                if (landblock.Id.LandblockX < xMin) xMin = landblock.Id.LandblockX;
                if (landblock.Id.LandblockX > xMax) xMax = landblock.Id.LandblockX;
                if (landblock.Id.LandblockY < yMin) yMin = landblock.Id.LandblockY;
                if (landblock.Id.LandblockY > yMax) yMax = landblock.Id.LandblockY;

                xCenter = xMin + ((xMax - xMin) / 2);
                yCenter = yMin + ((yMax - yMin) / 2);

                width = xMax - xMin;
                height = yMax - yMin;

                return true;
            }

            return false;
        }

        public bool Remove(Landblock landblock)
        {
            if (landblocks.Remove(landblock))
            {
                // Empty landblock groups will be discarded immediately
                if (landblocks.Count == 0)
                    return true;

                // If this landblock is on the perimieter of the group, recalculate the boundaries (they may end up the same)
                if ((landblock.Id.LandblockX == xMin && landblock.Id.LandblockY == yMin) ||
                    (landblock.Id.LandblockX == xMax && landblock.Id.LandblockY == yMin) ||
                    (landblock.Id.LandblockX == xMin && landblock.Id.LandblockY == yMax) ||
                    (landblock.Id.LandblockX == xMax && landblock.Id.LandblockY == yMax))
                {
                    RecalculateBoundaries();
                }
                else
                {
                    // This landblock is not on a boundary. This landblock may be eligible for a split
                    if (width >= landblockGroupSpanRequiredBeforeSplitEligibility || height >= landblockGroupSpanRequiredBeforeSplitEligibility)
                    {
                        // Make sure this landblock is far enough away from any side
                        if (Math.Abs(xMin - landblock.Id.LandblockX) >= LandblockGroupMinSpacing ||
                            Math.Abs(xMax - landblock.Id.LandblockX) >= LandblockGroupMinSpacing ||
                            Math.Abs(yMin - landblock.Id.LandblockY) >= LandblockGroupMinSpacing ||
                            Math.Abs(yMax - landblock.Id.LandblockY) >= LandblockGroupMinSpacing)
                        {
                            if (nextSplitRetryTime == DateTime.MinValue)
                                nextSplitRetryTime = DateTime.UtcNow.Add(splitRetryPeriod);
                        }
                    }
                }

                return true;
            }

            return false;
        }

        public IEnumerator<Landblock> GetEnumerator()
        {
            return landblocks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        private void RecalculateBoundaries()
        {
            xMin = int.MaxValue;
            xMax = int.MinValue;
            yMin = int.MaxValue;
            yMax = int.MinValue;

            foreach (var existing in landblocks)
            {
                if (existing.Id.LandblockX < xMin) xMin = existing.Id.LandblockX;
                if (existing.Id.LandblockX > xMax) xMax = existing.Id.LandblockX;
                if (existing.Id.LandblockY < yMin) yMin = existing.Id.LandblockY;
                if (existing.Id.LandblockY > yMax) yMax = existing.Id.LandblockY;
            }

            xCenter = xMin + ((xMax - xMin) / 2);
            yCenter = yMin + ((yMax - yMin) / 2);

            width = xMax - xMin;
            height = yMax - yMin;
        }

        /// <summary>
        /// Will return null if no split was possible.<para />
        /// If a LandblockGroup is returned, you must use it. The results of the returned group will have been removed from this group, and this groups boundaries will have been recalculated.
        /// </summary>
        public LandblockGroup TrySplit()
        {
            var newLandblockGroup = new LandblockGroup();

            var remainingLandblocks = new List<Landblock>(landblocks);

            newLandblockGroup.Add(remainingLandblocks[remainingLandblocks.Count - 1]);
            remainingLandblocks.RemoveAt(remainingLandblocks.Count - 1);

            doAnotherPass:
            bool needsAnotherPass = false;

            for (int i = remainingLandblocks.Count - 1 ; i >= 0 ; i--)
            {
                if (newLandblockGroup.Distance(remainingLandblocks[i]) < LandblockGroupMinSpacing)
                {
                    newLandblockGroup.Add(remainingLandblocks[i]);
                    remainingLandblocks.RemoveAt(i);
                    needsAnotherPass = true;
                }
            }

            if (needsAnotherPass)
                goto doAnotherPass;

            nextSplitRetryTime = DateTime.MinValue;

            // If they're the same size, there's no split possible
            if (Count == newLandblockGroup.Count)
                return null;

            // Remove the split landblocks
            foreach (var landblock in newLandblockGroup)
                landblocks.Remove(landblock);

            RecalculateBoundaries();

            return newLandblockGroup;
        }

        /// <summary>
        /// Will return null if no split was possible.<para />
        /// If a LandblockGroup is returned, you must use it. The results of the returned group will have been removed from this group, and this groups boundaries will have been recalculated.
        /// </summary>
        public LandblockGroup TryThrottledSplit()
        {
            if (nextSplitRetryTime == DateTime.MinValue || nextSplitRetryTime > DateTime.UtcNow)
                return null;

            log.Debug($"CheckIfLandblockGroupsNeedRecalculating TryThrottledSplit(). GetHashCode(): {GetHashCode()}, Count: {Count}");

            return TrySplit();
        }


        /// <summary>
        /// This will calculate the spacing between landblock group boarders as described here:
        /// https://math.stackexchange.com/questions/2724537/finding-the-clear-spacing-distance-between-two-rectangles
        /// </summary>
        public int Distance(Landblock landblock)
        {
            return Math.Max(
                Math.Abs(xCenter - landblock.Id.LandblockX) - (width + 0) / 2,
                Math.Abs(yCenter - landblock.Id.LandblockY) - (height + 0) / 2);
        }

        /// <summary>
        /// This will calculate the spacing between landblock group boarders as described here:
        /// https://math.stackexchange.com/questions/2724537/finding-the-clear-spacing-distance-between-two-rectangles
        /// </summary>
        public int Distance(LandblockGroup landblockGroup)
        {
            return Math.Max(
                Math.Abs(xCenter - landblockGroup.xCenter) - (width + landblockGroup.width) / 2,
                Math.Abs(yCenter - landblockGroup.yCenter) - (height + landblockGroup.height) / 2);
        }
    }
}

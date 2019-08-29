using System;
using System.Collections;
using System.Collections.Generic;

using log4net;

namespace ACE.Server.Entity
{
    public class LandblockGroup : IEnumerable<Landblock>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const int LandblockGroupMinSpacing = 5;

        private const int landblockGroupSpanRequiredBeforeSplitEligibility = LandblockGroupMinSpacing * 4;

        private const int numberOfUniqueLandblocksRemovedBeforeSplitEligibility = 0; // LandblockGroupMinSpacing * LandblockGroupMinSpacing; TODO change value after testing

        public bool IsDungeon { get; private set; }

        /// <summary>
        /// The number of seconds that should elapse before the next time TrySplit is called
        /// </summary>
        public const double TrySplitTimeInterval = 0; // 300; TODO change value after testing

        public double NextTrySplitTime { get; private set; } = Timers.PortalYearTicks + TrySplitTimeInterval;

        private readonly HashSet<Landblock> landblocks = new HashSet<Landblock>();

        private readonly HashSet<uint> uniqueLandblockIdsRemoved = new HashSet<uint>();

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

                uniqueLandblockIdsRemoved.Add(landblock.Id.Raw);

                // If this landblock is on the perimieter of the group, recalculate the boundaries (they may end up the same)
                if (landblock.Id.LandblockX == xMin || landblock.Id.LandblockX == xMax ||
                    landblock.Id.LandblockY == yMin || landblock.Id.LandblockY == yMax)
                {
                    RecalculateBoundaries();
                }
                /*else
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
                            landblockHasBeenRemoved = true;
                        }
                    }
                }*/

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


        /*private static void AddLandblockToLandblockGroup(HashSet<Landblock> landblocksAdded, LandblockGroup workingGroup, Landblock workingLandblock)
        {
            // TODO: This is bugged because some adjacents might not be part of this landlock group yet

            if (landblocksAdded.Add(workingLandblock))
            {
                if (!workingGroup.Add(workingLandblock))
                    log.Error($"Failed to add landblock ({workingLandblock.Id}) to landblock group ({workingGroup})");

                foreach (var adjacent in workingLandblock.Adjacents)
                    AddLandblockToLandblockGroup(landblocksAdded, workingGroup, adjacent);
            }
        }*/

        /// <summary>
        /// Will return null if no split was possible.<para />
        /// If a result is returned, you must use it. The landblocks in the result will have been removed from this group.
        /// </summary>
        public LandblockGroup TrySplit()
        {
            if (IsDungeon || width < landblockGroupSpanRequiredBeforeSplitEligibility || height < landblockGroupSpanRequiredBeforeSplitEligibility || uniqueLandblockIdsRemoved.Count < numberOfUniqueLandblocksRemovedBeforeSplitEligibility)
                return null;

            /*var results = new List<LandblockGroup>();

            var landblocksAdded = new HashSet<Landblock>(landblocks.Count);

            foreach (var landblock in landblocks)
            {
                if (!landblocksAdded.Contains(landblock))
                {
                    var workingGroup = new LandblockGroup();

                    AddLandblockToLandblockGroup(landblocksAdded, workingGroup, landblock);

                    results.Add(workingGroup);
                }
            }

            landblockHasBeenRemoved = false;
            NextTrySplitTime = Timers.PortalYearTicks + TrySplitTimeInterval;

            if (results.Count <= 1)
                return null;

            // Go through the result landblock groups and see if any are close enough to be merged
            for (int i = results.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    var distance = results[i].Distance(results[j]);

                    if (distance <= LandblockGroupMinSpacing)
                    {
                        // We're close enough, copy i into j and remove i
                        foreach (var landblock in results[i])
                            results[j].Add(landblock);

                        results.RemoveAt(i);

                        break;
                    }
                }
            }

            if (results.Count <= 1)
                return null;

            return results;*/




            var newLandblockGroup = new LandblockGroup();

            var remainingLandblocks = new List<Landblock>(landblocks);

            newLandblockGroup.Add(remainingLandblocks[remainingLandblocks.Count - 1]);
            remainingLandblocks.RemoveAt(remainingLandblocks.Count - 1);

            doAnotherPass:
            bool needsAnotherPass = false;

            for (int i = remainingLandblocks.Count - 1; i >= 0; i--)
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

            NextTrySplitTime = Timers.PortalYearTicks + TrySplitTimeInterval;
            uniqueLandblockIdsRemoved.Clear();

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


        public override string ToString()
        {
            return $"x: {xMin} - {xMax}, y: {yMin} - {yMax}, w: {width}, h: {height}, Count: {Count}";
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

using log4net;

namespace ACE.Server.Entity
{
    /// <summary>
    /// The idea behind the landblock groups are that each group may contain multiple landblocks that must be ticked on the same thread, but, each group itself can be ticked on independent threads.
    ///
    /// Landblock groups describe their area using a rectangle.
    /// As landblocks are added (and removed) the rectangle that contains all the landblocks in the group is adjusted.
    ///
    /// Landblocks are added to groups based on a minimum distance to the landblock groups rectangle.
    ///
    /// In the event a landblock needs to be added and is within range of multiple groups, those groups will be combined into one.
    ///
    /// Periodically, landblock groups are checked to see if they can be split.
    /// When a landblock group is split, it can result in a new group that has an overlapping rectangle to the parent group.
    /// This is not a problem because all the landblocks within each group meet the minimum distance requirements.
    /// In the event that a new landblock is added and is close to both rectangles, the groups will then be merged again.
    ///
    /// Adding landblocks to groups is a very efficient process
    /// Removing landblocks from groups is a very efficient process
    /// Checking landblock groups for split potential does incur some overhead (~0.5 ms) which is why it's only done on intervals that are twice the Landblock.UnloadInterval.
    /// </summary>
    public class LandblockGroup : IEnumerable<Landblock>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const int LandblockGroupMinSpacing = 5;

        private const int landblockGroupSpanRequiredBeforeSplitEligibility = LandblockGroupMinSpacing * 4;

        private const int numberOfUniqueLandblocksRemovedBeforeSplitEligibility = LandblockGroupMinSpacing * LandblockGroupMinSpacing;

        public bool IsDungeon { get; private set; }

        public static readonly TimeSpan TrySplitInterval = Landblock.UnloadInterval * 2;

        public DateTime NextTrySplitTime { get; private set; } = DateTime.UtcNow.Add(TrySplitInterval);

        private readonly HashSet<Landblock> landblocks = new HashSet<Landblock>();

        private readonly HashSet<uint> uniqueLandblockIdsRemoved = new HashSet<uint>();

        private int xMin = int.MaxValue;
        private int xMax = int.MinValue;
        private int yMin = int.MaxValue;
        private int yMax = int.MinValue;

        private double xCenter;
        private double yCenter;

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
                    log.Error($"[LANDBLOCK GROUP] You cannot add a landblock ({landblock.Id}) to a LandblockGroup that represents a single Dungeon Landblock");
                    return false;
                }

                if (landblock.IsDungeon)
                {
                    log.Error($"[LANDBLOCK GROUP] You cannot add a dungeon landblock ({landblock.Id}) to an existing LandblockGroup");
                    return false;
                }
            }

            if (landblocks.Add(landblock))
            {
                landblock.CurrentLandblockGroup = this;

                if (landblocks.Count == 1)
                    IsDungeon = landblock.IsDungeon;

                if (landblock.Id.LandblockX < xMin) xMin = landblock.Id.LandblockX;
                if (landblock.Id.LandblockX > xMax) xMax = landblock.Id.LandblockX;
                if (landblock.Id.LandblockY < yMin) yMin = landblock.Id.LandblockY;
                if (landblock.Id.LandblockY > yMax) yMax = landblock.Id.LandblockY;

                xCenter = xMin + ((xMax - xMin) / 2.0);
                yCenter = yMin + ((yMax - yMin) / 2.0);

                width = (xMax - xMin) + 1;
                height = (yMax - yMin) + 1;

                return true;
            }

            return false;
        }

        public bool Remove(Landblock landblock)
        {
            if (landblocks.Remove(landblock))
            {
                landblock.CurrentLandblockGroup = null;

                // Empty landblock groups will be discarded immediately
                if (landblocks.Count == 0)
                    return true;

                uniqueLandblockIdsRemoved.Add(landblock.Id.Raw);

                // If this landblock is on the perimeter of the group, recalculate the boundaries (they may end up the same)
                if (landblock.Id.LandblockX == xMin || landblock.Id.LandblockX == xMax ||
                    landblock.Id.LandblockY == yMin || landblock.Id.LandblockY == yMax)
                {
                    RecalculateBoundaries();
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

            xCenter = xMin + ((xMax - xMin) / 2.0);
            yCenter = yMin + ((yMax - yMin) / 2.0);

            width = (xMax - xMin) + 1;
            height = (yMax - yMin) + 1;
        }


        private LandblockGroup DoTrySplit()
        {
            var landblockGroupSplitHelper = new LandblockGroupSplitHelper();

            var remainingLandblocks = new List<Landblock>(landblocks);

            landblockGroupSplitHelper.Add(remainingLandblocks[remainingLandblocks.Count - 1]);
            remainingLandblocks.RemoveAt(remainingLandblocks.Count - 1);

            doAnotherPass:
            bool needsAnotherPass = false;

            for (int i = remainingLandblocks.Count - 1; i >= 0; i--)
            {
                if (landblockGroupSplitHelper.BoundaryDistance(remainingLandblocks[i]) < LandblockGroupMinSpacing)
                {
                    landblockGroupSplitHelper.Add(remainingLandblocks[i]);
                    remainingLandblocks.RemoveAt(i);
                    needsAnotherPass = true;
                }
            }

            if (needsAnotherPass)
                goto doAnotherPass;

            // If they're the same size, there's no split possible
            if (Count == landblockGroupSplitHelper.Count)
                return null;

            // Split was a success
            var newLandblockGroup = new LandblockGroup();

            foreach (var landblock in landblockGroupSplitHelper)
            {
                // Remove the split landblocks. Do this manually, not through the public Remove() function
                landblocks.Remove(landblock);

                // Add them through the proper .Add() method to the new LandblockGroup
                newLandblockGroup.Add(landblock);
            }

            RecalculateBoundaries();

            // This can result in returning groups that overlap this ones boundary.
            // However, that isn't a problem for processing them on separate threads.
            // In the event a new landblock is added that is within range of both this block and any new block, they will be recombined at that point.

            return newLandblockGroup;
        }

        /// <summary>
        /// Will return null if no split was possible.<para />
        /// If a result is returned, you must use it. The landblocks in the result will have been removed from this group.
        /// </summary>
        public List<LandblockGroup> TrySplit()
        {
            if (IsDungeon)
                return null;

            var results = new List<LandblockGroup>();

            var newLandblockGroup = DoTrySplit();

            while (newLandblockGroup != null)
            {
                results.Add(newLandblockGroup);

                newLandblockGroup = DoTrySplit();
            }

            NextTrySplitTime = DateTime.UtcNow.Add(TrySplitInterval);
            uniqueLandblockIdsRemoved.Clear();

            return results;
        }

        /// <summary>
        /// Will return null if no split was possible.<para />
        /// If a result is returned, you must use it. The landblocks in the result will have been removed from this group.<para />
        /// This will only attempt the TrySplit() if all the conditions are met
        /// </summary>
        public List<LandblockGroup> TryThrottledSplit()
        {
            if (width < landblockGroupSpanRequiredBeforeSplitEligibility && height < landblockGroupSpanRequiredBeforeSplitEligibility)
                return null;

            if (uniqueLandblockIdsRemoved.Count < numberOfUniqueLandblocksRemovedBeforeSplitEligibility)
                return null;

            if (NextTrySplitTime > DateTime.UtcNow)
                return null;

            return TrySplit();
        }


        /// <summary>
        /// This will calculate the distance from the landblock group boarder.<para />
        /// -X = Inside the bounds, where -1 is the outer perimeter<para />
        ///  0 = Outside of the bounds but adjacent (touching)<para />
        /// +X = Has X landblocks between this and the bounds of the group<para />
        /// Distances are measured horizontally and vertically (not diagonally) pictured here: https://math.stackexchange.com/questions/2724537/finding-the-clear-spacing-distance-between-two-rectangles
        /// </summary>
        public int BoundaryDistance(Landblock landblock)
        {
            return (int)Math.Max(
                Math.Abs(xCenter - landblock.Id.LandblockX) - (width + 1) / 2.0,
                Math.Abs(yCenter - landblock.Id.LandblockY) - (height + 1) / 2.0);
        }

        /// <summary>
        /// This will calculate the distance between the landblock group boarders.<para />
        /// -X = Inside the bounds, where -1 is an overlapping outer perimeter<para />
        ///  0 = Outside of the bounds but adjacent (touching)<para />
        /// +X = Has X landblocks between this and the bounds of the group<para />
        /// Distances are measured horizontally and vertically (not diagonally) pictured here: https://math.stackexchange.com/questions/2724537/finding-the-clear-spacing-distance-between-two-rectangles
        /// </summary>
        public int BoundaryDistance(LandblockGroup landblockGroup)
        {
            return (int)Math.Max(
                Math.Abs(xCenter - landblockGroup.xCenter) - (width + landblockGroup.width) / 2.0,
                Math.Abs(yCenter - landblockGroup.yCenter) - (height + landblockGroup.height) / 2.0);
        }


        public override string ToString()
        {
            return $"x: 0x{xMin:X2} - 0x{xMax:X2}, y: 0x{yMin:X2} - 0x{yMax:X2}, w: {width.ToString().PadLeft(3)}, h: {height.ToString().PadLeft(3)}, Count: {Count.ToString().PadLeft(4)}";
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

using ACE.Common.Performance;

using log4net;

namespace ACE.Server.Entity
{
    /// <summary>
    /// The idea behind the landblock groups are that each group may contain multiple landblocks that must be ticked on the same thread, but, each group itself can be ticked on independent threads.
    ///
    /// Landblock groups describe their area using a rectangle.
    /// As landblocks are added (and removed) the rectangle that contains all the landblocks in the group is adjusted.
    ///
    /// While landblock groups describe their area using rectangles, groups can be ireggular shaped.
    /// Two groups can have overlapping rectangles as long as all of their landblocks are LandblockGroupMinSpacing apart from each other.
    /// 
    /// Landblocks are added to groups based on a minimum distance to all contained landblocks in the group.
    ///
    /// In the event a landblock needs to be added and is within LandblockGroupMinSpacing range of multiple groups, those groups will be combined into one.
    ///
    /// Periodically, landblock groups are checked to see if they can be split.
    /// When a landblock group is split, it can result in a new group that has an overlapping rectangle to the parent group.
    /// This is not a problem because all the landblocks within each group meet the minimum distance requirements.
    ///
    /// Adding landblocks to groups is a very efficient process
    /// Removing landblocks from groups is a very efficient process
    /// Checking landblock groups for split potential does incur some overhead (~0.5 ms) which is why it's only done once every Landblock.UnloadInterval.
    /// </summary>
    public class LandblockGroup : IEnumerable<Landblock>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public const int LandblockGroupMinSpacing = 4;

        public const int LandblockGroupMinSpacingWhenDormant = 3;

        public bool IsDungeon { get; private set; }

        public static readonly TimeSpan TrySplitInterval = Landblock.UnloadInterval;

        public DateTime NextTrySplitTime { get; private set; } = DateTime.UtcNow.Add(TrySplitInterval);

        private readonly HashSet<Landblock> landblocks = new HashSet<Landblock>();

        public int XMin { get; private set; } = int.MaxValue;
        public int XMax { get; private set; } = int.MinValue;
        public int YMin { get; private set; } = int.MaxValue;
        public int YMax { get; private set; } = int.MinValue;

        private int width;
        private int height;

        /// <summary>
        /// This is the historical tick times it took for this LandBlockGroup to process under LandblockManager.TickPhysics for the last ~10 s.
        /// This is used to help partition LandblockGroups for efficient multi-threaded distributed work.
        /// </summary>
        public readonly RollingAmountOverHitsTracker TickPhysicsTracker = new RollingAmountOverHitsTracker(500);

        /// <summary>
        /// This is the historical tick times it took for this LandBlockGroup to process under LandblockManager.TickMultiThreadedWork for the last ~10 s.
        /// This is used to help partition LandblockGroups for efficient multi-threaded distributed work.
        /// </summary>
        public readonly RollingAmountOverHitsTracker TickMultiThreadedWorkTracker = new RollingAmountOverHitsTracker(500);

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

                if (landblock.Id.LandblockX < XMin) XMin = landblock.Id.LandblockX;
                if (landblock.Id.LandblockX > XMax) XMax = landblock.Id.LandblockX;
                if (landblock.Id.LandblockY < YMin) YMin = landblock.Id.LandblockY;
                if (landblock.Id.LandblockY > YMax) YMax = landblock.Id.LandblockY;

                width = (XMax - XMin) + 1;
                height = (YMax - YMin) + 1;

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

                // If this landblock is on the perimeter of the group, recalculate the boundaries (they may end up the same)
                if (landblock.Id.LandblockX == XMin || landblock.Id.LandblockX == XMax ||
                    landblock.Id.LandblockY == YMin || landblock.Id.LandblockY == YMax)
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
            XMin = int.MaxValue;
            XMax = int.MinValue;
            YMin = int.MaxValue;
            YMax = int.MinValue;

            foreach (var existing in landblocks)
            {
                if (existing.Id.LandblockX < XMin) XMin = existing.Id.LandblockX;
                if (existing.Id.LandblockX > XMax) XMax = existing.Id.LandblockX;
                if (existing.Id.LandblockY < YMin) YMin = existing.Id.LandblockY;
                if (existing.Id.LandblockY > YMax) YMax = existing.Id.LandblockY;
            }

            width = (XMax - XMin) + 1;
            height = (YMax - YMin) + 1;
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
                if (landblockGroupSplitHelper.ShouldBeAddedToThisLandblockGroup(remainingLandblocks[i]))
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

            // If we have a very large landblock group that didn't split, we'll try to split it every 1 minute to help reduce server load
            if (results.Count == 0 && landblocks.Count >= 200)
                NextTrySplitTime = DateTime.UtcNow.AddMinutes(1);
            else
                NextTrySplitTime = DateTime.UtcNow.Add(TrySplitInterval);

            return results;
        }

        /// <summary>
        /// Will return null if no split was possible.<para />
        /// If a result is returned, you must use it. The landblocks in the result will have been removed from this group.<para />
        /// This will only attempt the TrySplit() if all the conditions are met
        /// </summary>
        public List<LandblockGroup> TryThrottledSplit()
        {
            if (NextTrySplitTime > DateTime.UtcNow)
                return null;

            return TrySplit();
        }


        public bool ShouldBeAddedToThisLandblockGroup(Landblock landblock)
        {
            foreach (var value in landblocks)
            {
                var distance = Math.Max(
                Math.Abs(value.Id.LandblockX - landblock.Id.LandblockX),
                Math.Abs(value.Id.LandblockY - landblock.Id.LandblockY));

                if (value.IsDormant || landblock.IsDormant)
                {
                    if (distance < LandblockGroup.LandblockGroupMinSpacingWhenDormant)
                        return true;
                }
                else
                {
                    if (distance < LandblockGroup.LandblockGroupMinSpacing)
                        return true;
                }
            }

            return false;
        }

        public override string ToString()
        {
            return $"x: 0x{XMin:X2} - 0x{XMax:X2}, y: 0x{YMin:X2} - 0x{YMax:X2}, w: {width.ToString().PadLeft(3)}, h: {height.ToString().PadLeft(3)}, Count: {Count.ToString().PadLeft(4)}";
        }
    }
}

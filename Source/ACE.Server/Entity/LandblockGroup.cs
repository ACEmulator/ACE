using System;
using System.Collections;
using System.Collections.Generic;

using log4net;

namespace ACE.Server.Entity
{
    class LandblockGroup : IEnumerable<Landblock>
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsDungeon { get; private set; }

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
            return landblocks.Remove(landblock);
        }

        public IEnumerator<Landblock> GetEnumerator()
        {
            return landblocks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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

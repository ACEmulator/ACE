using System;
using System.Collections;
using System.Collections.Generic;

namespace ACE.Server.Entity
{
    class LandblockGroupSplitHelper : IEnumerable<Landblock>
    {
        private readonly HashSet<Landblock> landblocks = new HashSet<Landblock>();

        private int xMin = int.MaxValue;
        private int xMax = int.MinValue;
        private int yMin = int.MaxValue;
        private int yMax = int.MinValue;

        private double xCenter;
        private double yCenter;

        private int width;
        private int height;

        public int Count => landblocks.Count;

        public void Add(Landblock landblock)
        {
            if (landblocks.Add(landblock))
            {
                if (landblock.Id.LandblockX < xMin) xMin = landblock.Id.LandblockX;
                if (landblock.Id.LandblockX > xMax) xMax = landblock.Id.LandblockX;
                if (landblock.Id.LandblockY < yMin) yMin = landblock.Id.LandblockY;
                if (landblock.Id.LandblockY > yMax) yMax = landblock.Id.LandblockY;

                xCenter = xMin + ((xMax - xMin) / 2.0);
                yCenter = yMin + ((yMax - yMin) / 2.0);

                width = (xMax - xMin) + 1;
                height = (yMax - yMin) + 1;
            }
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
    }
}

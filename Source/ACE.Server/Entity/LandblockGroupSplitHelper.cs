using System;
using System.Collections;
using System.Collections.Generic;

namespace ACE.Server.Entity
{
    class LandblockGroupSplitHelper : IEnumerable<Landblock>
    {
        private readonly HashSet<Landblock> landblocks = new HashSet<Landblock>();

        public int Count => landblocks.Count;

        public void Add(Landblock landblock)
        {
            landblocks.Add(landblock);
        }

        public IEnumerator<Landblock> GetEnumerator()
        {
            return landblocks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
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
    }
}

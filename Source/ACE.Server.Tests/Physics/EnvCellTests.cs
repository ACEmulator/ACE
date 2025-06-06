using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACE.Server.Physics.Common;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace ACE.Server.Tests.Physics
{
    [TestClass]
    public class EnvCellTests
    {
        [TestMethod]
        public void GetVisible_ReturnsNull_WhenNoVisibleCells()
        {
            uint cellId = 0xDEAD0001;
            uint landblockKey = cellId | 0xFFFF;

            var landblock = new Landblock();
            landblock.ID = cellId & 0xFFFF0000;
            landblock.LandCells = new ConcurrentDictionary<int, ObjCell>();

            var envCell = new EnvCell();
            envCell.ID = cellId;
            envCell.VisibleCells = new Dictionary<uint, EnvCell>();

            landblock.LandCells[(int)(cellId & 0xFFFF)] = envCell;
            LScape.Landblocks[landblockKey] = landblock;

            var visible = EnvCell.get_visible(cellId);

            Assert.IsNull(visible);

            LScape.Landblocks.TryRemove(landblockKey, out _);
        }
    }
}

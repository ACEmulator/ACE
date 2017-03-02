using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.DatLoader.Tests
{
    [TestClass]
    public class CellDatTests
    {
        [TestMethod]
        public void LoadCellDat_NoExceptions()
        {
            string celldat = @"C:\Turbine\client_cell_1.dat";
            CellDat dat = new CellDat(celldat);
            int count = dat.AllFiles.Where(f => f.BitFlags == 196608).Count();
            System.Diagnostics.Debug.WriteLine($"Count: {count}");

        }
    }
}

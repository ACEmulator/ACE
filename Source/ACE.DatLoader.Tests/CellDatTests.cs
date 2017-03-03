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
            int count = dat.AllFiles.Count();
            System.Diagnostics.Debug.WriteLine($"Cell.dat file count: {count}");
        }

        [TestMethod]
        public void LoadPortalDat_NoExceptions()
        {
            string portaldatfile = @"C:\Turbine\client_portal.dat";
            PortalDat dat = new PortalDat(portaldatfile);
            int count = dat.AllFiles.Count();
            System.Diagnostics.Debug.WriteLine($"Portal.dat file count: {count}");
        }
    }
}

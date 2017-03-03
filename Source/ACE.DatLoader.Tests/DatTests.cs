using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.DatLoader.Tests
{
    [TestClass]
    public class DatTests
    {
        [TestMethod]
        public void LoadCellDat_NoExceptions()
        {
            string celldat = @"C:\Turbine\client_cell_1.dat";
            DatDatabase dat = new DatDatabase(celldat);
            int count = dat.AllFiles.Count();
            Assert.AreEqual(805348, count);
        }

        [TestMethod]
        public void LoadPortalDat_NoExceptions()
        {
            string portaldatfile = @"C:\Turbine\client_portal.dat";
            DatDatabase dat = new DatDatabase(portaldatfile);
            int count = dat.AllFiles.Count();
            Assert.AreEqual(79694, count);
        }
    }
}

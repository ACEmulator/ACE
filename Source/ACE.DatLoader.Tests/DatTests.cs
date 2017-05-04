using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Collections.Generic;

namespace ACE.DatLoader.Tests
{
    [TestClass]
    public class DatTests
    {
        [TestMethod]
        public void LoadCellDat_NoExceptions()
        {
            string celldat = @"C:\Turbine\client_cell_1.dat";
            DatDatabase dat = new DatDatabase(celldat, DatDatabaseType.Cell);
            int count = dat.AllFiles.Count();
            Assert.AreEqual(805348, count);
        }

        [TestMethod]
        public void LoadPortalDat_NoExceptions()
        {
            string portaldatfile = @"C:\Turbine\client_portal.dat";
            DatDatabase dat = new DatDatabase(portaldatfile, DatDatabaseType.Portal);
            int count = dat.AllFiles.Count();
            Assert.AreEqual(79694, count);
        }

        // uncomment if you want to run this
        // [TestMethod]
        public void ExportPortalDatsWithTypeInfo()
        {
            string dat = @"C:\Turbine\client_portal.dat";
            string output = @"c:\Turbine\typed_portal_dat_export";
            PortalDatDatabase db = new PortalDatDatabase(dat);
            db.ExtractCategorizedContents(output);
        }

        // uncomment if you want to run this
        // [TestMethod]
        public void ExtractCellDatByLandblock()
        {
            string celldat = @"C:\Turbine\client_cell_1.dat";
            string output = @"c:\Turbine\cell_dat_export_by_landblock";
            CellDatDatabase db = new CellDatDatabase(celldat);
            db.ExtractLandblockContents(output);
        }
    }
}

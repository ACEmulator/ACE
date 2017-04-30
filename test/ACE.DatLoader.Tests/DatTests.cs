using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Xunit;
using ACE.DatLoader;

namespace ACE.DatLoader.Tests
{
    public class DatTests
    {
        [Fact]
        public void LoadCellDat_NoExceptions()
        {
            string celldat = @"C:\Turbine\client_cell_1.dat";
            DatDatabase dat = new DatDatabase(celldat, DatDatabaseType.Cell);
            int count = dat.AllFiles.Count();
            Assert.Equal(805348, count);
        }

        [Fact]
        public void LoadPortalDat_NoExceptions()
        {
            string portalDatFile = @"C:\Turbine\client_portal.dat";
            DatDatabase dat = new DatDatabase(portalDatFile, DatDatabaseType.Portal);
            int count = dat.AllFiles.Count();
            Assert.Equal(79694, count);
        }

        // uncomment if you want to run this
        // [Fact]
        public void ExportPortalDatsWithTypeInfo()
        {
            string dat = @"C:\Turbine\client_portal.dat";
            string output = @"c:\Turbine\typed_portal_dat_export";
            PortalDatDatabase db = new PortalDatDatabase(dat);
            db.ExtractCategorizedContents(output);
        }

        // uncomment if you want to run this
        // [Fact]
        public void ExtractCellDatByLandblock()
        {
            string celldat = @"C:\Turbine\client_cell_1.dat";
            string output = @"c:\Turbine\cell_dat_export_by_landblock";
            CellDatDatabase db = new CellDatDatabase(celldat);
            db.ExtractLandblockContents(output);
        }
    }
}

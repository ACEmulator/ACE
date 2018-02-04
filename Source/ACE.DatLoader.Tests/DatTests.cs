using System;
using System.IO;
using System.Linq;
using System.Reflection;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.DatLoader.Tests
{
    [TestClass]
    public class DatTests
    {
        private static string cellDatLocation = @"C:\Turbine\client_cell_1.dat";
        private static int expectedCellDatFileCount = 805003;

        private static string portalDatLocation = @"C:\Turbine\client_portal.dat";
        private static int expectedPortalDatFileCount = 79694; // Mag-nus.dat has 805003. Mogwais had 805348.


        [TestMethod]
        public void LoadCellDat_NoExceptions()
        {
            DatDatabase dat = new DatDatabase(cellDatLocation, DatDatabaseType.Cell);
            int count = dat.AllFiles.Count();
            //Assert.AreEqual(ExpectedCellDatFileCount, count);
            Assert.IsTrue(expectedCellDatFileCount <= count, $"Insufficient files parsed from .dat. Expected: >= {expectedCellDatFileCount}, Actual: {count}");
        }

        [TestMethod]
        public void LoadPortalDat_NoExceptions()
        {
            DatDatabase dat = new DatDatabase(portalDatLocation, DatDatabaseType.Portal);
            int count = dat.AllFiles.Count();
            //Assert.AreEqual(expectedPortalDatFileCount, count);
            Assert.IsTrue(expectedPortalDatFileCount <= count, $"Insufficient files parsed from .dat. Expected: >= {expectedPortalDatFileCount}, Actual: {count}");
        }


        [TestMethod]
        public void UnpackCellDatFiles_NoExceptions()
        {
            var assembly = typeof(DatDatabase).GetTypeInfo().Assembly;
            var types = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(DatFileTypeAttribute), false).Length > 0).ToList();

            if (types.Count == 0)
                throw new Exception("Failed to locate any types with DatFileTypeAttribute.");

            DatDatabase dat = new DatDatabase(cellDatLocation, DatDatabaseType.Cell);

            foreach (var kvp in dat.AllFiles)
            {
                var fileType = kvp.Value.GetFileType();

                if (fileType == null)
                    continue;
                //Assert.IsNotNull(fileType, $"Key: 0x{kvp.Key:X8}, ObjectID: 0x{kvp.Value.ObjectId:X8}, FileSize: {kvp.Value.FileSize}, BitFlags:, 0x{kvp.Value.BitFlags:X8}");

                if (fileType == DatFileType.Cell && (kvp.Key & 0xFFFF) >= 0xFFFE) continue; // These don't parse, I don't know why.

                var type = types
                    .SelectMany(m => m.GetCustomAttributes(typeof(DatFileTypeAttribute), false), (m, a) => new { m, a })
                    .Where(t => ((DatFileTypeAttribute)t.a).FileType == fileType)
                    .Select(t => t.m);

                var first = type.FirstOrDefault();

                if (first == null)
                    throw new Exception($"Failed to Unpack fileType: {fileType}");

                var obj = Activator.CreateInstance(first);

                var unpackable = obj as IUnpackable;

                if (unpackable == null)
                    throw new Exception($"Class for fileType: {fileType} does not implement IUnpackable.");

                var datReader = new DatReader(cellDatLocation, kvp.Value.FileOffset, kvp.Value.FileSize, dat.SectorSize);

                using (var memoryStream = new MemoryStream(datReader.Buffer))
                using (var reader = new BinaryReader(memoryStream))
                    unpackable.Unpack(reader);
            }
        }

        [TestMethod]
        public void UnpackPortalDatFiles_NoExceptions()
        {
            var assembly = typeof(DatDatabase).GetTypeInfo().Assembly;
            var types = assembly.GetTypes().Where(t => t.GetCustomAttributes(typeof(DatFileTypeAttribute), false).Length > 0).ToList();

            if (types.Count == 0)
                throw new Exception("Failed to locate any types with DatFileTypeAttribute.");

            DatDatabase dat = new DatDatabase(portalDatLocation, DatDatabaseType.Portal);

            foreach (var kvp in dat.AllFiles)
            {
                if (kvp.Key == 0xFFFF0001) // Not sure what this is, EOF record maybe?
                    continue;

                var fileType = kvp.Value.GetFileType();

                Assert.IsNotNull(fileType, $"Key: 0x{kvp.Key:X8}, ObjectID: 0x{kvp.Value.ObjectId:X8}, FileSize: {kvp.Value.FileSize}, BitFlags:, 0x{kvp.Value.BitFlags:X8}");

                // These file types aren't converted yet
                if (fileType == DatFileType.SurfaceTexture) continue;
                if (fileType == DatFileType.RenderSurface) continue;
                if (fileType == DatFileType.SecondaryAttributeTable) continue;
                if (fileType == DatFileType.SkillTable) continue;
                if (fileType == DatFileType.ChatPoseTable) continue;
                if (fileType == DatFileType.BadData) continue;
                if (fileType == DatFileType.TabooTable) continue;
                if (fileType == DatFileType.NameFilterTable) continue;
                if (fileType == DatFileType.QualityFilter) continue;
                if (fileType == DatFileType.STable) continue;
                if (fileType == DatFileType.EnumMapper) continue;
                if (fileType == DatFileType.String) continue;
                if (fileType == DatFileType.KeyMap) continue;
                if (fileType == DatFileType.RenderTexture) continue;
                if (fileType == DatFileType.RenderMaterial) continue;
                if (fileType == DatFileType.MaterialModifier) continue;
                if (fileType == DatFileType.MaterialInstance) continue;
                if (fileType == DatFileType.DidMapper) continue;
                if (fileType == DatFileType.ActionMap) continue;
                if (fileType == DatFileType.DualDidMapper) continue;
                if (fileType == DatFileType.Font) continue;
                if (fileType == DatFileType.MasterProperty) continue;
                if (fileType == DatFileType.DbProperties) continue;

                // These have bugs
                if (fileType == DatFileType.Animation && kvp.Value.ObjectId == 0x0300055b) continue; // This one hook seems corrupt

                var type = types
                    .SelectMany(m => m.GetCustomAttributes(typeof(DatFileTypeAttribute), false), (m, a) => new {m, a})
                    .Where(t => ((DatFileTypeAttribute) t.a).FileType == fileType)
                    .Select(t => t.m);

                var first = type.FirstOrDefault();

                if (first == null)
                    throw new Exception($"Failed to Unpack fileType: {fileType}");

                var obj = Activator.CreateInstance(first);

                var unpackable = obj as IUnpackable;

                if (unpackable == null)
                    throw new Exception($"Class for fileType: {fileType} does not implement IUnpackable.");

                var datReader = new DatReader(portalDatLocation, kvp.Value.FileOffset, kvp.Value.FileSize, dat.SectorSize);

                using (var memoryStream = new MemoryStream(datReader.Buffer))
                using (var reader = new BinaryReader(memoryStream))
                    unpackable.Unpack(reader);
            }
        }


        // uncomment if you want to run this
        // [TestMethod]
        public void ExtractCellDatByLandblock()
        {
            string output = @"c:\Turbine\cell_dat_export_by_landblock";
            CellDatDatabase db = new CellDatDatabase(cellDatLocation);
            db.ExtractLandblockContents(output);
        }

        // uncomment if you want to run this
        // [TestMethod]
        public void ExportPortalDatsWithTypeInfo()
        {
            string output = @"c:\Turbine\typed_portal_dat_export";
            PortalDatDatabase db = new PortalDatDatabase(portalDatLocation);
            db.ExtractCategorizedContents(output);
        }
    }
}

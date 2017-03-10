using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

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

        [TestMethod]
        public void ExtractCellDat()
        {
            string dat = @"C:\Turbine\client_cell_1.dat";
            string output = @"c:\Turbine\cell_dat_export";

            // Uncomment line below to extract Cell DAT file
            //BeginExtract(dat, output);
        }

        [TestMethod]
        public void ExtractPortalDat()
        {
            string dat = @"C:\Turbine\client_portal.dat";
            string output = @"c:\Turbine\portal_dat_export";

            // Uncomment line below to extract Portal DAT file
            //BeginExtract(dat, output);
        }

        private void BeginExtract(string filename, string outDir)
        {
            DatDatabase dat = new DatDatabase(filename);

            if (!Directory.Exists(outDir))
            {
                Directory.CreateDirectory(outDir);
            }

            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                ExtractDat(stream, dat.RootDirectory, outDir);
            }
        }

        private void ExtractDat(FileStream stream, DatDirectory directory, string path)
        {
            string hex = "00000000"; //+ directory.DatOffset.ToString("X");
            //string hex = "00000000" + directory.ToString("X");
            string thisFolder = Path.Combine(path, "0x" + hex.Substring(hex.Length - 8, 8));

            if (!Directory.Exists(thisFolder))
            {
                Directory.CreateDirectory(thisFolder);
            }

            foreach (var cellfile in directory.Files)
            {
                byte[] buffer = new byte[cellfile.FileSize];
                stream.Seek(cellfile.FileOffset, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);

                hex = cellfile.ObjectId.ToString("X");
                string thisFile = Path.Combine(thisFolder, new string('0', 8 - hex.Length) + cellfile.ObjectId.ToString("X"));
                File.WriteAllBytes(thisFile, buffer);
            }

            directory.Directories.ForEach(d => ExtractDat(stream, d, thisFolder));
        }
    }
}

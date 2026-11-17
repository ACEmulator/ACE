using System.IO;

using ACE.DatLoader.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.DatLoader.Tests
{
    [TestClass]
    public class PortalPolyTests
    {
        [TestMethod]
        [DataRow(32, 0)]
        [DataRow(267, 0)]
        [DataRow(0, 7)]
        public void UnpackReadsPolygonIdBeforePortalIndex(
            int polygonId, int portalIndex)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream,
                    System.Text.Encoding.UTF8, true))
                {
                    writer.Write((short)polygonId);
                    writer.Write((short)portalIndex);
                }

                stream.Position = 0;

                using (var reader = new BinaryReader(stream))
                {
                    var portalPoly = new PortalPoly();
                    portalPoly.Unpack(reader);

                    Assert.AreEqual((short)polygonId, portalPoly.PolygonId);
                    Assert.AreEqual((short)portalIndex, portalPoly.PortalIndex);
                    Assert.AreEqual(stream.Length, stream.Position);
                }
            }
        }
    }
}

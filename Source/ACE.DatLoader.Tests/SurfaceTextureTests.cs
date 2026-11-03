using System.IO;
using System.Linq;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.DatLoader.Tests
{
    /// <summary>
    /// Tests the SurfaceTexture header and texture-reference layout without
    /// depending on installed DAT archives or the DatManager singleton.
    /// </summary>
    [TestClass]
    public class SurfaceTextureTests
    {
        [TestMethod]
        public void Unpack_TwoTextureReferences_PreservesRetailLayout()
        {
            // The IDs mirror an observed HighRes/Portal pair. Header bytes are
            // constructed in-memory so the test can run without client DAT files.
            var data = BuildRecord(0x0500000C, 0, TextureType.Texture2D,
                0x06003B9D, 0x06003B9E);

            var result = UnpackCompletely<SurfaceTexture>(data);

            Assert.AreEqual(0x0500000Cu, result.Id);
            Assert.AreEqual(0u, result.DataCategory);
            Assert.AreEqual(TextureType.Texture2D, result.TextureType);
            CollectionAssert.AreEqual(new uint[] { 0x06003B9D, 0x06003B9E }, result.Textures.ToArray());
        }

        [TestMethod]
        public void Unpack_NonzeroDataCategory_RetainsUnsignedBits()
        {
            // A synthetic boundary case: retail SurfaceTexture categories
            // observed so far were all zero, but the field is a UInt32 on disk.
            var data = BuildRecord(0x05000001, 0xF0000001u, TextureType.Cube,
                0x06000001);

            var result = UnpackCompletely<SurfaceTexture>(data);

            Assert.AreEqual(0xF0000001u, result.DataCategory);
            Assert.AreEqual(TextureType.Cube, result.TextureType);
            CollectionAssert.AreEqual(new uint[] { 0x06000001 }, result.Textures.ToArray());
        }

        [TestMethod]
        public void Unpack_ZeroTextureReferences_ConsumesCompleteRecord()
        {
            var data = BuildRecord(0x05000002, 0, TextureType.Texture2D);

            var result = UnpackCompletely<SurfaceTexture>(data);

            Assert.AreEqual(0u, result.DataCategory);
            Assert.AreEqual(TextureType.Texture2D, result.TextureType);
            Assert.AreEqual(0, result.Textures.Count);
        }

        [TestMethod]
        public void Unpack_HeaderMatchesExistingRenderTextureLayout()
        {
            // RenderTexture is an independently implemented reader for the
            // same binary field layout. Compare their decoded values.
            var data = BuildRecord(0x05000003, 0x80000000u, TextureType.Movie2D,
                0x06000010, 0x06000011);

            var surface = UnpackCompletely<SurfaceTexture>(data);
            var render = UnpackCompletely<RenderTexture>(data);

            Assert.AreEqual(render.Id, surface.Id);
            Assert.AreEqual(render.DataCategory, surface.DataCategory);
            Assert.AreEqual(render.TextureType, surface.TextureType);
            CollectionAssert.AreEqual(render.Textures.ToArray(), surface.Textures.ToArray());
        }

        private static T UnpackCompletely<T>(byte[] bytes) where T : FileType, new()
        {
            using var stream = new MemoryStream(bytes, writable: false);
            using var reader = new BinaryReader(stream);
            var value = new T();
            value.Unpack(reader);
            Assert.AreEqual(stream.Length, stream.Position,
                "The parser must consume exactly the bytes in the record.");
            return value;
        }

        private static byte[] BuildRecord(uint id, uint category,
            TextureType textureType, params uint[] textureIds)
        {
            using var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                writer.Write(id);                   // UInt32 resource ID
                writer.Write(category);             // UInt32 DataCategory
                writer.Write((byte)textureType);    // Byte TextureType
                writer.Write(textureIds.Length);    // Int32 List<uint> count
                foreach (var textureId in textureIds)
                    writer.Write(textureId);
            }
            return stream.ToArray();
        }
    }
}

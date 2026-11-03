
using System.IO;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.DatLoader.Tests
{
    [TestClass]
    public class TextureDataCategoryTests
    {
        [TestMethod]
        public void Unpack_DataCategory_PreservesUnsignedValuesAndFollowingFields()
        {
            // Include observed category values and synthetic UInt32
            // boundary cases. The meanings of the values remain unknown.
            var categories = new uint[]
            {
                0u, 7u, 0x80000000u, uint.MaxValue
            };

            var pixels = new byte[]
            {
                1, 2, 3, 4, 5, 6, 7, 8
            };

            foreach (var category in categories)
            {
                var bytes = BuildRecord(
                    0x06000001u,
                    category,
                    2,
                    1,
                    SurfacePixelFormat.PFID_A8R8G8B8,
                    pixels);

                var texture = UnpackCompletely(bytes);

                Assert.AreEqual(0x06000001u, texture.Id);
                Assert.AreEqual(category, texture.DataCategory);
                Assert.AreEqual(2, texture.Width);
                Assert.AreEqual(1, texture.Height);
                Assert.AreEqual(SurfacePixelFormat.PFID_A8R8G8B8,
                    texture.Format);
                Assert.AreEqual(pixels.Length, texture.Length);
                CollectionAssert.AreEqual(pixels, texture.SourceData);
                Assert.IsNull(texture.DefaultPaletteId);
            }
        }

        [TestMethod]
        public void Unpack_PalettizedTexture_PreservesPaletteAndAlignment()
        {
            var pixels = new byte[] { 3, 7 };

            var bytes = BuildRecord(
                0x06000002u,
                10u,
                2,
                1,
                SurfacePixelFormat.PFID_P8,
                pixels,
                0x04000001u);

            var texture = UnpackCompletely(bytes);

            Assert.AreEqual(0x06000002u, texture.Id);
            Assert.AreEqual(10u, texture.DataCategory);
            Assert.AreEqual(2, texture.Width);
            Assert.AreEqual(1, texture.Height);
            Assert.AreEqual(SurfacePixelFormat.PFID_P8, texture.Format);
            Assert.AreEqual(2, texture.Length);
            CollectionAssert.AreEqual(pixels, texture.SourceData);
            Assert.AreEqual((uint?)0x04000001u,
                texture.DefaultPaletteId);
        }

        private static Texture UnpackCompletely(byte[] bytes)
        {
            using var stream = new MemoryStream(bytes, writable: false);
            using var reader = new BinaryReader(stream);

            var texture = new Texture();
            texture.Unpack(reader);

            Assert.AreEqual(stream.Length, stream.Position,
                "The parser must consume the complete texture record.");

            return texture;
        }

        private static byte[] BuildRecord(
            uint id,
            uint category,
            int width,
            int height,
            SurfacePixelFormat format,
            byte[] pixels,
            uint? paletteId = null)
        {
            using var stream = new MemoryStream();

            using (var writer = new BinaryWriter(
                stream, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                writer.Write(id);
                writer.Write(category);
                writer.Write(width);
                writer.Write(height);
                writer.Write((uint)format);
                writer.Write(pixels.Length);
                writer.Write(pixels);

                if (paletteId.HasValue)
                    writer.Write(paletteId.Value);
            }

            return stream.ToArray();
        }
    }
}

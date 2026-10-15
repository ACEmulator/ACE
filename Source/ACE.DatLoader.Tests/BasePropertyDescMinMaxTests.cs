using System;
using System.IO;
using System.Text;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ACE.DatLoader.Tests
{
    /// <summary>
    /// Regression tests for independently optional minimum and maximum values.
    /// The test records are synthetic and do not require installed DAT archives.
    /// </summary>
    [TestClass]
    public class BasePropertyDescMinMaxTests
    {
        /// <summary>
        /// All four combinations must parse the correct values and stop exactly
        /// at the end of the descriptor. The mixed cases prevent regression.
        /// </summary>
        [TestMethod]
        [DataRow(false, false)]
        [DataRow(false, true)]
        [DataRow(true, false)]
        [DataRow(true, true)]
        public void Unpack_IndependentMinMaxFlags_ConsumesExactlyOneDescriptor(bool hasMax, bool hasMin)
        {
            byte[] bytes = BuildDescriptor(hasMax, hasMin);
            using var stream = new MemoryStream(bytes, writable: false);
            using var reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true);

            var descriptor = new BasePropertyDesc();
            descriptor.Unpack(reader);

            Assert.AreEqual(hasMax, descriptor.hasMax, "Maximum-presence flag was not preserved.");
            Assert.AreEqual(hasMin, descriptor.hasMin, "Minimum-presence flag was not preserved.");
            if (hasMax)
                Assert.AreEqual(99, descriptor.max, "Maximum value decoded incorrectly.");
            if (hasMin)
                Assert.AreEqual(33, descriptor.min, "Minimum value decoded incorrectly.");

            Assert.AreEqual(stream.Length, stream.Position,
                "The parser must consume exactly the bytes belonging to the descriptor.");
        }

        /// <summary>
        /// Constructs the integer-valued BasePropertyDesc layout in the order
        /// consumed by BasePropertyDesc.Unpack. Each optional value is emitted
        /// only when its own flag is true.
        /// </summary>
        private static byte[] BuildDescriptor(bool hasMax, bool hasMin)
        {
            using var stream = new MemoryStream();
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
            {
                writer.Write((uint)1);                         // m_propertyName
                writer.Write((uint)BasePropertyType.Integer);  // m_propertyType
                writer.Write((uint)0);                         // m_propertyGroup
                writer.Write((uint)0);                         // m_propertyProvider
                writer.Write((uint)0);                         // m_data
                writer.Write(0);                               // Unknown int, unmodified
                writer.Write(false);                           // No default value

                writer.Write(hasMax);                          // Independent maximum flag
                if (hasMax) writer.Write(99);                  // Optional Int32 maximum

                writer.Write(hasMin);                          // Independent minimum flag
                if (hasMin) writer.Write(33);                  // Optional Int32 minimum

                writer.Write(1.5f);                            // Prediction timeout
                writer.Write((byte)0);                         // Inheritance type
                writer.Write((byte)0);                         // DAT file type
                writer.Write((byte)0);                         // Propagation type

                // Ten boolean property flags follow the value restrictions.
                for (int index = 0; index < 10; index++)
                    writer.Write(false);

                writer.Write((byte)0);                         // Empty available-properties table
            }

            return stream.ToArray();
        }
    }
}

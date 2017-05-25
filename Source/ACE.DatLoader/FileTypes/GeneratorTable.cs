using ACE.DatLoader.Entity;
using System.Collections.Generic;
using System.Linq;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// Class for reading the File 0x0E00000D from the portal.dat.
    /// Thanks alot Widgeon of Leafcull for his ACDataTools which helped understanding this structure.
    /// And thanks alot to Pea as well whos hard work surely helped in the creation of those Tools too.
    /// </summary>
    public static class GeneratorTable
    {
        public static Generator ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(0x0E00000D))
            {
                return (Generator)DatManager.PortalDat.FileCache[0x0E00000D];
            }
            else
            {
                var gen = new Generator();

                // Create the datReader for the proper file
                var datReader = DatManager.PortalDat.GetReaderForFile(0x0E00000D);

                gen.Id = datReader.ReadInt32();
                gen.Name = "0E00000D";
                gen.Count = 2;
                datReader.Offset = 16;

                var playDay = new Generator();
                var weenieObjects = new Generator();
                gen.Items.Add(playDay.GetNextGenerator(datReader)); // Parse and add PlayDay hierarchy
                gen.Items.Add(weenieObjects.GetNextGenerator(datReader)); // Parse and add WeenieObjects hierarchy

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[0x0E00000D] = gen;
                return gen;
            }
        }

        public static IEnumerable<Generator> ReadItems(this Generator root)
        {
            var nodes = new Stack<Generator>(new[] { root });
            while (nodes.Any())
            {
                var node = nodes.Pop();
                yield return node;
                foreach (var n in node.Items) nodes.Push(n);
            }
        }
    }
}

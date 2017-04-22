using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// Class for reading the File 0x0E00000D from the portal.dat.
    /// Thanks alot Widgeon of Leafcull for his ACDataTools which helped understanding this structure.
    /// And thanks alot to Pea as well whos hard work surely helped in the creation of those Tools too.
    /// </summary>
    public static class GeneratorTable
    {
        public static Generator ReadFromDat(DatReader datReader)
        {
            Generator gen = new Generator();

            gen.Id = datReader.ReadInt32();
            gen.Count = 2;
            datReader.Offset = 16;

            Generator playDay = new Generator();
            Generator weenieObjects = new Generator();
            gen.Items.Add(playDay.GetNextGenerator(datReader)); // Parse and add PlayDay hierarchy
            gen.Items.Add(weenieObjects.GetNextGenerator(datReader)); // Parse and add WeenieObjects hierarchy

            return gen;
        }

        public static IEnumerable<Generator> ReadItems(this Generator root)
        {
            var nodes = new Stack<Generator>(new[] { root });
            while (nodes.Any())
            {
                Generator node = nodes.Pop();
                yield return node;
                foreach (var n in node.Items) nodes.Push(n);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class Generator
    {
        public int Id { get; set; }

        public int Count { get; set; }

        public string Name { get; set; }

        public List<Generator> Items;

        public Generator()
        {
            Id = 0;
            Count = 0;
            Name = "";
            Items = new List<Generator>();
        }

        public Generator GetNextGenerator(DatReader datReader)
        {
            Name = datReader.ReadOString();
            datReader.AlignBoundary();
            Id = datReader.ReadInt32();
            Count = datReader.ReadInt32();
                
            // Console.WriteLine($"{Id:X8} {Count:X8} {Name}");

            for (var i = 0; i < Count; i++)
            {
                var child = new Generator();
                child = child.GetNextGenerator(datReader);
                Items.Add(child);
            }
            return this;
        }
    }
}

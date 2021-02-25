using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Enum;

namespace ACE.Server.Entity
{
    public class TinkerLog
    {
        public List<MaterialType> Tinkers;

        public TinkerLog(string csv)
        {
            Tinkers = new List<MaterialType>();

            if (csv == null) return;

            var vals = csv.Split(',');

            foreach (var val in vals)
            {
                if (!Enum.TryParse(val, true, out MaterialType materialType))
                {
                    Console.WriteLine($"Couldn't parse {val}");
                    continue;
                }
                Tinkers.Add(materialType);
            }
        }

        public int NumTinkers(MaterialType type)
        {
            return Tinkers.Count(i => i == type);
        }
    }
}

using System.Collections.Generic;
using System.Linq;

using ACE.Entity.Models;

namespace ACE.Server.Entity
{
    public class CreateListSet
    {
        public List<PropertiesCreateList> Items;

        public List<PropertiesCreateList> Trophies => Items.Where(i => i.WeenieClassId != 0).ToList();

        public List<PropertiesCreateList> None => Items.Where(i => i.WeenieClassId == 0).ToList();

        public float TotalProbability => Items.Sum(i => i.Shade);

        public float TrophyProbability => Trophies.Sum(i => i.Shade);

        public float NoneProbability => None.Sum(i => i.Shade);


        public CreateListSet()
        {
            Items = new List<PropertiesCreateList>();
        }

        public void Add(PropertiesCreateList item)
        {
            Items.Add(item);
        }
    }
}

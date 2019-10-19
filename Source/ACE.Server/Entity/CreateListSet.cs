using System.Collections.Generic;
using System.Linq;

using ACE.Database.Models.Shard;

namespace ACE.Server.Entity
{
    public class CreateListSet
    {
        public List<BiotaPropertiesCreateList> Items;

        public List<BiotaPropertiesCreateList> Trophies => Items.Where(i => i.WeenieClassId != 0).ToList();

        public List<BiotaPropertiesCreateList> None => Items.Where(i => i.WeenieClassId == 0).ToList();

        public float TotalProbability => Items.Sum(i => i.Shade);

        public float TrophyProbability => Trophies.Sum(i => i.Shade);

        public float NoneProbability => None.Sum(i => i.Shade);


        public CreateListSet()
        {
            Items = new List<BiotaPropertiesCreateList>();
        }

        public void Add(BiotaPropertiesCreateList item)
        {
            Items.Add(item);
        }
    }
}

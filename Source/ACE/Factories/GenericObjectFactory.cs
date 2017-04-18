using ACE.Entity;
using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Factories
{
    public class GenericObjectFactory
    {
        public static List<WorldObject> CreateWorldObjects(List<AceObject> sourceObjects)
        {
            List<WorldObject> results = new List<WorldObject>();

            foreach (var aceO in sourceObjects)
            {
                ObjectType ot = (ObjectType)aceO.TypeId;
                switch (ot)
                {
                    case ObjectType.LifeStone:
                        results.Add(new Lifestone(aceO));
                        break;
                    case ObjectType.Portal:
                        results.Add(new Portal(aceO));
                        break;
                }
            }

            return results;
        }
    }
}

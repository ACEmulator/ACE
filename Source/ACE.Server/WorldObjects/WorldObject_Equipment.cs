using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Entity.Enum;
using ACE.Server.Factories;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public List<WorldObject> GetCreateList(DestinationType type)
        {
            var items = new List<WorldObject>();

            foreach (var item in Biota.BiotaPropertiesCreateList.Where(x => x.DestinationType == (int)type))
            {
                var wo = WorldObjectFactory.CreateNewWorldObject(item.WeenieClassId);

                if (item.Palette > 0)
                    wo.PaletteTemplate = item.Palette;

                if (item.Shade > 0)
                    wo.Shade = item.Shade;

                if (item.StackSize > 0)
                    wo.StackSize = item.StackSize;

                items.Add(wo);
            }
            return items;
        }
    }
}

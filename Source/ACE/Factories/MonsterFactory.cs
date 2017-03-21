using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Factories
{
    public class MonsterFactory
    {
        public static WorldObject CreateMonster(uint templateId, Position newPosition)
        {
            Monster mo = new Monster("Drudge Sneaker", newPosition);

            return mo;
        }
    }
}

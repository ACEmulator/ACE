﻿using System;
using System.Collections;

namespace ACE.Managers
{
    /// <summary>
    /// Used to assign global guids and ensure they are unique to server.
    /// todo:// use and reuse .. keep track of who using what and release ids..
    /// </summary>
    public static class GuidManager
    {
        // thanks pea for good ranges!

        // players
        private const uint playerMin = 0x50000001;
        private const uint playerMax = 0x59999999;
        private static uint player   = 0x50000001;

        // admins
        private const uint adminMin = 0x80000001;
        private const uint adminMax = 0x89999999;
        private static uint admin   = 0x80000001;

        // private static Hashtable usedplayerguids;
        // private static Hashtable avaliableplayerguids;

        // Orginally found in CommonObjectFactory
        // private static uint nextObjectId = 0x80000001;

        // Npc / Doors / Portals / Etc
        private const uint staticObjectMin = 0x6000001;
        private const uint staticObjectMax = 0x6999999;
        private static uint staticObject   = 0x6000001;

        // Monsters /
        private const uint monsterMin     = 0x8000001;
        private const uint monsterMax     = 0x8999999;
        private static uint monsterObject = 0x8000001;

        // Items / Player Items
        private const uint itemMin = 0x9000001;
        private const uint itemMax = 0x9999999;
        private static uint item   = 0x9000001;

        /// <summary>
        /// Returns New Player Guid
        /// </summary>
        /// <returns></returns>
        public static uint NewPlayerGuid()
        {
            player++;
            return player;
        }

        /// <summary>
        /// Returns New Admin Guid
        /// </summary>
        /// <returns></returns>
        public static uint NewAdminGuid()
        {
            admin++;
            return admin;
        }

        /// <summary>
        /// Returns New Guid for NPCs, Doors, World Portals, etc
        /// </summary>
        /// <returns></returns>
        public static uint NewStaticObjectGuid()
        {
            staticObject++;
            return staticObject;
        }

        /// <summary>
        /// Returns New Guid for Monsters
        /// </summary>
        /// <returns></returns>
        public static uint NewMonsterGuid()
        {
            monsterObject++;
            return monsterObject;
        }

        /// <summary>
        /// Returns New Guid for Items / Player Items
        /// </summary>
        /// <returns></returns>
        public static uint NewItemGuid()
        {
            item++;
            return item;
        }
    }
}

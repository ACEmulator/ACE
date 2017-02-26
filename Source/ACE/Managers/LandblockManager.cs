using ACE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Managers
{
    public class LandblockManager
    {
        /*         The World        --        * Cartesian grid of 255x255 landblocks        * Landblocks are themselves a cartesian grid of 8x8 landcells        * Landcells are 24x24 units in size        * Landblock 0,0 is in the southwest corner of the map        * An entity is positioned in the world with a 32-bit number:          * Landblock is the top 16-bits:            * (grid_position >> 24) & 0xFF ==> Landblock X, moving West(0x00) to East(0xFF)            * (grid_position >> 16) & 0XFF ==> Landblock Y, moving South(0x00) to North(0xFF)          * Landcell is in the bottom 7-bits:            * ((landblock & 0x7F) - 1) ==> Landcell 0-63            * ((landcell >> 3) & 0x7)  ==> Landcell X, moving West(0x0) to East(0x7)            * (landcell & 0x7)         ==> Landcell Y, moving South(0x0) to North(0x7)          * (grid_position & 0x1000) ==> Denotes we are in a dungeon          * ...not sure if the other bits are used / useful...        * ...and with a float triple (x,y,z)          * Represents location within a landblock          * X moves from 0.f - 192.f from West to East          * Y moves from 0.f - 192.f from South to North         * ...and with a float quaternion (w,x,y,z)          * Represents orientation          * Players, creatures will almost always have 0. for x, y (no pitch / roll)        */

        // see also: http://acpedia.org/wiki/Landblock

        // reading cell.dat: https://bitbucket.org/skinnyt/accellexport/src/4aaf9517e09fc28658305dff5edc2a0354d40322/mapac.c?at=master&fileviewer=file-view-default

        private Landblock[,] landblockGrid;

        public LandblockManager()
        {
            landblockGrid = new Landblock[255, 255];
            
        }

        /// <summary>
        /// main game loop
        /// </summary>
        public void UseTime()
        {
        }

        public void AddPlayer(Player player)
        {
            var pos = player.Position;

            
        }
    }
}

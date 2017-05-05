using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    // TODO: refactor to use existing Position object
    public class Loc
    {
        public int Cell;
        public float X;
        public float Y;
        public float Z;
        public float QW;
        public float QX;
        public float QY;
        public float QZ;

        public Loc(int cell, float x, float y, float z, float qw, float qx, float qy, float qz)
        {
            Cell = cell;
            X = x;
            Y = y;
            Z = z;
            QW = qw;
            QX = qx;
            QY = qy;
            QZ = qz;
        }
    }
}

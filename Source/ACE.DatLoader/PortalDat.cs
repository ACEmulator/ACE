using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader
{
    public class PortalDat : DatDatabase
    {
        public PortalDat(string filePath) : base(filePath)
        {
        }

        public override int SectorSize
        {
            get { return 256 * sizeof(uint); }
        }
    }
}

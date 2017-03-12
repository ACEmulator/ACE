﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Factories
{
    public class CommonObjectFactory
    {
        private static uint nextObjectId = 0x00000001;

        public static uint NextObjectId
        {
            get { return nextObjectId++; }
        }
    }
}

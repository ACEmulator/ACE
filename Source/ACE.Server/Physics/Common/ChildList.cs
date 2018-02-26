using System;
using System.Collections.Generic;
using System.Text;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class ChildList
    {
        public int NumObjects;
        public List<PhysicsObj> Objects;
        public List<AFrame> Frames;
        public List<int> PartNumbers;
        public List<int> LocationIDs;

        public int FindChildIndex(PhysicsObj obj)
        {
            return -1;
        }

        public void AddChild(PhysicsObj obj, AFrame frame, int partIdx, int locationID)
        {

        }

        public void RemoveChild(PhysicsObj obj)
        {

        }
    }
}

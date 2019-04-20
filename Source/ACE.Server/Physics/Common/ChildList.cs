using System.Collections.Generic;
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

        public ChildList()
        {
            Objects = new List<PhysicsObj>();
            Frames = new List<AFrame>();
            PartNumbers = new List<int>();
            LocationIDs = new List<int>();
        }

        public int FindChildIndex(PhysicsObj obj)
        {
            for (var i = 0; i < NumObjects; i++)
            {
                var child = Objects[i];
                if (child.Equals(obj))
                    return i;
            }
            return -1;
        }

        public void AddChild(PhysicsObj obj, AFrame frame, int partIdx, int locationID)
        {
            Objects.Add(obj);
            Frames.Add(frame);
            PartNumbers.Add(partIdx);
            LocationIDs.Add(locationID);
            NumObjects++;
        }

        public bool RemoveChild(PhysicsObj obj)
        {
            var idx = FindChildIndex(obj);
            if (idx == -1) return false;
            Objects.RemoveAt(idx);
            Frames.RemoveAt(idx);
            PartNumbers.RemoveAt(idx);
            LocationIDs.RemoveAt(idx);
            return true;
        }
    }
}

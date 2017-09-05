using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class PhysicsScriptData
    {
        public double StartTime { get; set; }
        public AnimationHook Hook { get; set; }

        public static PhysicsScriptData Read(DatReader datReader)
        {
            PhysicsScriptData obj = new PhysicsScriptData();

            obj.StartTime = datReader.ReadDouble();
            obj.Hook = AnimationHook.Read(datReader);

            return obj;
        }
    }
}

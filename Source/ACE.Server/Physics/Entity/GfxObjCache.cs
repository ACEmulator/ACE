using System;
using System.Collections.Generic;
using ACE.Server.Physics.Collision;

namespace ACE.Server.Physics.Entity
{
    public static class GfxObjCache
    {
        public static Dictionary<uint, GfxObj> GfxObjs;

        static GfxObjCache()
        {
            GfxObjs = new Dictionary<uint, GfxObj>();
        }

        public static int Requests;
        public static int Hits;

        public static GfxObj Get(GfxObj gfxObj)
        {
            Requests++;

            //if (Requests % 100 == 0)
                //Console.WriteLine($"GfxObjCache: Requests={Requests}, Hits={Hits}");

            GfxObjs.TryGetValue(gfxObj.ID, out var result);
            if (result != null)
            {
                Hits++;
                return result;
            }

            // not cached, add it
            GfxObjs.Add(gfxObj.ID, gfxObj);
            return gfxObj;
        }

        public static GfxObj Get(DatLoader.FileTypes.GfxObj gfxObj)
        {
            return Get(new GfxObj(gfxObj));
        }
    }
}

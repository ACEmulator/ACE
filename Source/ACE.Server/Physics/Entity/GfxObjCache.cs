using System;
using System.Collections.Concurrent;

using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Entity
{
    public static class GfxObjCache
    {
        //public static readonly ConcurrentDictionary<uint, GfxObj> GfxObjs = new ConcurrentDictionary<uint, GfxObj>();
        public static readonly ConcurrentDictionary<uint, WeakReference<GfxObj>> GfxObjs = new ConcurrentDictionary<uint, WeakReference<GfxObj>>();

        public static int Requests;
        public static int Hits;

        public static int Count => GfxObjs.Count;

        public static void Clear()
        {
            GfxObjs.Clear();
        }

        public static GfxObj Get(uint gfxObjID)
        {
            Requests++;

            //if (Requests % 100 == 0)
            //Console.WriteLine($"GfxObjCache: Requests={Requests}, Hits={Hits}");

            if (GfxObjs.TryGetValue(gfxObjID, out var result))
            {
                if (result.TryGetTarget(out var target))
                {
                    Hits++;
                    return target;
                }
            }

            var _gfxObj = DBObj.GetGfxObj(gfxObjID);

            // not cached, add it
            var gfxObj = new GfxObj(_gfxObj);
            //gfxObj = GfxObjs.GetOrAdd(_gfxObj.Id, gfxObj);
            GfxObjs[_gfxObj.Id] = new WeakReference<GfxObj>(gfxObj);
            return gfxObj;
        }
    }
}

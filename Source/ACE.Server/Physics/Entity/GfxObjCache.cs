// Uncomment this if you want to use weak reference cache. It could save 10's of MB and might be useful on a micro-server
//#define USE_WEAK_REFERENCE_CACHE

using System;
using System.Collections.Concurrent;

using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Entity
{
    public static class GfxObjCache
    {
        #if !USE_WEAK_REFERENCE_CACHE
        public static readonly ConcurrentDictionary<uint, GfxObj> GfxObjs = new ConcurrentDictionary<uint, GfxObj>();
        #else
        public static readonly ConcurrentDictionary<uint, WeakReference<GfxObj>> GfxObjs = new ConcurrentDictionary<uint, WeakReference<GfxObj>>();
        #endif

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
                #if !USE_WEAK_REFERENCE_CACHE
                Hits++;
                return result;
                #else
                if (result.TryGetTarget(out var target))
                {
                    Hits++;
                    return target;
                }
                #endif
            }

            var _gfxObj = DBObj.GetGfxObj(gfxObjID);

            // not cached, add it
            var gfxObj = new GfxObj(_gfxObj);

            #if !USE_WEAK_REFERENCE_CACHE
            gfxObj = GfxObjs.GetOrAdd(_gfxObj.Id, gfxObj);
            #else
            GfxObjs[_gfxObj.Id] = new WeakReference<GfxObj>(gfxObj);
            #endif

            return gfxObj;
        }
    }
}

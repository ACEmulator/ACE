using System;
using System.Collections.Concurrent;

using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Entity
{
    public static class GfxObjCache
    {
        public static readonly ConcurrentDictionary<uint, GfxObj> GfxObjs = new ConcurrentDictionary<uint, GfxObj>();

        public static int Requests;
        public static int Hits;

        public static GfxObj Get(DatLoader.FileTypes.GfxObj _gfxObj)
        {
            Requests++;

            //if (Requests % 100 == 0)
            //Console.WriteLine($"GfxObjCache: Requests={Requests}, Hits={Hits}");

            if (GfxObjs.TryGetValue(_gfxObj.Id, out var result))
            {
                Hits++;
                return result;
            }

            // not cached, add it
            var gfxObj = new GfxObj(_gfxObj);
            gfxObj = GfxObjs.GetOrAdd(_gfxObj.Id, gfxObj);
            return gfxObj;
        }

        public static GfxObj Get(uint rootObjectID)
        {
            Requests++;

            //if (Requests % 100 == 0)
            //Console.WriteLine($"GfxObjCache: Requests={Requests}, Hits={Hits}");

            if (GfxObjs.TryGetValue(rootObjectID, out var result))
            {
                Hits++;
                return result;
            }

            var _gfxObj = DBObj.GetGfxObj(rootObjectID);

            // not cached, add it
            var gfxObj = new GfxObj(_gfxObj);
            gfxObj = GfxObjs.GetOrAdd(_gfxObj.Id, gfxObj);
            return gfxObj;
        }
    }
}

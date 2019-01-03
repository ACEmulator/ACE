using System;
using System.Collections.Generic;

using ACE.Adapter.GDLE.Models;
using ACE.Database.Models.World;

namespace ACE.Adapter.GDLE
{
    public static class GDLEConverter
    {
        public static bool TryConvert(Landblock input, out List<LandblockInstance> results, out List<LandblockInstanceLink> links)
        {
            try
            {
                results = new List<LandblockInstance>();
                links = new List<LandblockInstanceLink>();

                foreach (var value in input.value.weenies)
                {
                    var result = new LandblockInstance();

                    result.Guid = value.id;     // TODO!!! I think we need to scale these to fit ACE model
                    //result.Landblock = input.key; ACE uses a virtual column here of (result.ObjCellId >> 16)
                    result.WeenieClassId = value.wcid;

                    result.ObjCellId = value.pos.objcell_id;
                    result.OriginX = (float)value.pos.frame.origin.x;
                    result.OriginY = (float)value.pos.frame.origin.y;
                    result.OriginZ = (float)value.pos.frame.origin.z;
                    result.AnglesW = (float)value.pos.frame.angles.w;
                    result.AnglesX = (float)value.pos.frame.angles.x;
                    result.AnglesY = (float)value.pos.frame.angles.y;
                    result.AnglesZ = (float)value.pos.frame.angles.z;

                    results.Add(result);
                }

                if (input.value.links != null)
                {
                    foreach (var value in input.value.links)
                    {
                        var result = new LandblockInstanceLink();

                        result.ParentGuid = value.source;   // TODO!!! I'm not sure about the order of these.. is source the parent, or child?
                        result.ChildGuid = value.target;    // TODO!!! I'm not sure about the order of these.. is source the parent, or child?

                        links.Add(result);
                    }
                }

                return true;
            }
            catch
            {
                results = null;
                links = null;
                return false;
            }
        }


        public static bool TryConvert(Models.Event input, out Database.Models.World.Event result)
        {
            try
            {
                result = new Database.Models.World.Event();

                // result.Id // TODO!!! is this id'd by index? If so, the parent caller needs to set the id... or this function could take an argument that specifies the id.

                result.Name = input.key;

                result.StartTime = input.value.startTime;
                result.EndTime = input.value.endTime;
                result.State = input.value.eventState;

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }


        public static bool TryConvert(Models.Quest input, out Database.Models.World.Quest result)
        {
            try
            {
                result = new Database.Models.World.Quest();

                // result.Id // TODO!!! is this id'd by index? If so, the parent caller needs to set the id... or this function could take an argument that specifies the id.

                result.Name = input.key;

                result.MinDelta = (uint)input.value.mindelta; // TODO!!! Should we convert the ACE property to an int
                result.MaxSolves = input.value.maxsolves;
                result.Message = input.value.fullname;

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}

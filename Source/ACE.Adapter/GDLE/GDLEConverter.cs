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

                foreach (var value in input.Value.Weenies)
                {
                    var result = new LandblockInstance();

                    result.Guid = value.Id;     // TODO!!! I think we need to scale these to fit ACE model
                    //result.Landblock = input.key; ACE uses a virtual column here of (result.ObjCellId >> 16)
                    result.WeenieClassId = value.WCID;

                    result.ObjCellId = value.Position.ObjCellId;
                    result.OriginX = (float)value.Position.Frame.Origin.X;
                    result.OriginY = (float)value.Position.Frame.Origin.Y;
                    result.OriginZ = (float)value.Position.Frame.Origin.Z;
                    result.AnglesW = (float)value.Position.Frame.Angles.W;
                    result.AnglesX = (float)value.Position.Frame.Angles.X;
                    result.AnglesY = (float)value.Position.Frame.Angles.Y;
                    result.AnglesZ = (float)value.Position.Frame.Angles.Z;

                    results.Add(result);
                }

                if (input.Value.Links != null)
                {
                    foreach (var value in input.Value.Links)
                    {
                        var result = new LandblockInstanceLink();

                        result.ParentGuid = value.Source;   // TODO!!! I'm not sure about the order of these.. is source the parent, or child?
                        result.ChildGuid = value.Target;    // TODO!!! I'm not sure about the order of these.. is source the parent, or child?

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

                result.Name = input.Key;

                result.StartTime = input.Value.StartTime;
                result.EndTime = input.Value.EndTime;
                result.State = input.Value.EventState;

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

                result.Name = input.Key;

                result.MinDelta = (uint)input.Value.MinDelta; // TODO!!! Should we convert the ACE property to an int
                result.MaxSolves = input.Value.MaxSolves;
                result.Message = input.Value.FullName;

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

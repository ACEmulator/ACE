using System;
using System.Numerics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.Physics.Common
{
    public class Position: IEquatable<Position>
    {
        public uint ObjCellID;
        public AFrame Frame;

        public Position()
        {
            Init();
        }

        public Position(uint objCellID)
        {
            ObjCellID = objCellID;
            Init();
        }

        public Position(uint objCellID, AFrame frame)
        {
            ObjCellID = objCellID;
            Frame = frame;
        }

        public Position(Position p)
        {
            ObjCellID = p.ObjCellID;
            Frame = new AFrame(p.Frame);
        }

        public Position(ACE.Entity.Position p)
        {
            ObjCellID = p.Cell;
            Frame = new AFrame(p.Pos, p.Rotation);
        }

        public void Init()
        {
            Frame = new AFrame();
        }

        public Vector3 LocalToLocal(Position pos, Vector3 offset)
        {
            var cellOffset = pos.Frame.LocalToGlobal(offset);
            var blockOffset = LandDefs.GetBlockOffset(ObjCellID, pos.ObjCellID);

            return Frame.GlobalToLocal(blockOffset + cellOffset);
        }

        public Vector3 LocalToGlobal(Vector3 point)
        {
            return Frame.LocalToGlobal(point);
        }

        public Vector3 LocalToGlobal(Position pos, Vector3 point)
        {
            var cellOffset = pos.Frame.LocalToGlobal(point);
            var blockOffset = LandDefs.GetBlockOffset(ObjCellID, pos.ObjCellID);

            return blockOffset + cellOffset;
        }

        public Vector3 LocalToGlobalVec(Vector3 point)
        {
            return Frame.LocalToGlobalVec(point);
        }

        public float Distance(Position pos)
        {
            return GetOffset(pos).Length();
        }

        public float DistanceSquared(Position pos)
        {
            return GetOffset(pos).LengthSquared();
        }

        public static double CylinderDistance(float radius, float height, Position pos, float otherRadius, float otherHeight, Position otherPos)
        {
            var offset = pos.GetOffset(otherPos);
            var reach = offset.Length() - (radius + otherRadius);

            var diffZ = pos.Frame.Origin.Z <= otherPos.Frame.Origin.Z ? otherPos.Frame.Origin.Z - (pos.Frame.Origin.Z + height) :
                pos.Frame.Origin.Z - (otherPos.Frame.Origin.Z + otherHeight);

            if (diffZ > 0 && reach > 0)
                return Math.Sqrt(diffZ * diffZ + reach * reach);
            else if (diffZ < 0 && reach < 0)
                return -Math.Sqrt(diffZ * diffZ + reach * reach);
            else
                return reach;
        }

        public static float CylinderDistanceNoZ(float radius, Position pos, float otherRadius, Position otherPos)
        {
            var offset = pos.GetOffset(otherPos);
            return offset.Length() - (radius + otherRadius);
        }

        public int DetermineQuadrant(float height, Position position)
        {
            var hitLocation = LocalToLocal(position, Vector3.Zero);

            var quadrant = hitLocation.X < 0.0f ? 0x8 : 0x10;
            quadrant |= hitLocation.Y >= 0.0f ? 0x20 : 0x40;

            if (height * 0.333333333f > hitLocation.Z)
                quadrant |= 4;  // low
            else if (height * 0.66666667f > hitLocation.Z)
                quadrant |= 2;  // medium
            else
                quadrant |= 1;  // high

            return quadrant;
        }

        public Vector3 GlobalToLocalVec(Vector3 point)
        {
            return Frame.GlobalToLocalVec(point);
        }

        public Vector3 GetOffset(Position pos)
        {
            var blockOffset = LandDefs.GetBlockOffset(ObjCellID, pos.ObjCellID);
            var globalOffset = blockOffset + pos.Frame.Origin - Frame.Origin;

            return globalOffset;
        }

        public Vector3 GetOffset(Position pos, Vector3 rotation)
        {
            var blockOffset = LandDefs.GetBlockOffset(ObjCellID, pos.ObjCellID);
            return blockOffset + pos.LocalToGlobal(rotation) - Frame.Origin;
        }

        public Vector3 GetOrigin()
        {
            return Frame.Origin;
        }

        public AFrame Subtract(Position pos)
        {
            Frame.Subtract(pos.Frame);
            return Frame;
        }

        public void add_offset(Vector3 offset)
        {
            Frame.Origin += offset;
        }

        public void adjust_to_outside()
        {
            LandDefs.AdjustToOutside(this);
        }

        public float heading(Position position)
        {
            var dir = GetOffset(position);
            dir.Z = 0.0f;

            if (dir.NormalizeCheckSmall())
                return 0.0f;

            return (450.0f - ((float)Math.Atan2(dir.Y, dir.X)).ToDegrees()) % 360.0f;
        }

        public float heading_diff(Position position)
        {
            return heading(position) - Frame.get_heading();
        }

        public bool Equals(Position pos)
        {
            return ObjCellID == pos.ObjCellID && Frame.Equals(pos.Frame);
        }
    }
}

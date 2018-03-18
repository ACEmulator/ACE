using System;
using System.Numerics;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.Physics.Animation
{
    public class AFrame
    {
        public Vector3 Origin;
        public Quaternion Orientation;

        public AFrame()
        {
            Origin = Vector3.Zero;
            Orientation = Quaternion.Identity;
        }

        public AFrame(AFrame frame)
        {
            Origin = new Vector3(frame.Origin.X, frame.Origin.Y, frame.Origin.Z);
            Orientation = new Quaternion(frame.Orientation.X, frame.Orientation.Y, frame.Orientation.Z, frame.Orientation.W);
        }

        public AFrame(DatLoader.Entity.Frame frame)
        {
            Origin = new Vector3(frame.Origin.X, frame.Origin.Y, frame.Origin.Z);
            Orientation = new Quaternion(frame.Orientation.X, frame.Orientation.Y, frame.Orientation.Z, frame.Orientation.W);
        }

        public static AFrame Combine(AFrame a, AFrame b)
        {
            var frame = new AFrame();
            frame.Origin = a.Origin + b.Origin;
            frame.Orientation = Quaternion.Multiply(a.Orientation, b.Orientation);
            return frame;
        }

        public void Combine(AFrame a, AFrame b, Vector3 scale)
        {
            Origin += a.Origin + b.Origin;
            Orientation *= Quaternion.Multiply(a.Orientation, b.Orientation);
        }

        public Vector3 GlobalToLocal(Vector3 point)
        {
            return Origin + GlobalToLocalVec(point);
        }

        public Vector3 GlobalToLocalVec(Vector3 point)
        {
            return Vector3.Transform(point, Orientation);
        }

        public void InterpolateOrigin(AFrame from, AFrame to, float t)
        {
            Origin = Vector3.Lerp(from.Origin, to.Origin, t);
        }

        public void InterpolateRotation(AFrame from, AFrame to, float t)
        {
            Orientation = Quaternion.Lerp(from.Orientation, to.Orientation, t);
        }

        public bool IsEqual(AFrame frame)
        {
            // implement IEquatable
            return frame.Equals(this);  
        }

        public bool IsQuaternionEqual(AFrame frame)
        {
            return Orientation.Equals(frame.Orientation);
        }

        public bool IsValid()
        {
            return Origin.IsValid() && Orientation.IsValid();
        }

        public bool IsValidExceptForHeading()
        {
            return Origin.IsValid();
        }

        public Vector3 LocalToGlobal(Vector3 point)
        {
            return Origin + LocalToGlobalVec(point);
        }

        public Vector3 LocalToGlobalVec(Vector3 point)
        {
            var rotation = Matrix4x4.CreateFromQuaternion(Orientation);
            rotation = Matrix4x4.Transpose(rotation);

            return Vector3.Transform(point, rotation);   
        }

        public void GRotate(Vector3 rotation)
        {
            Orientation = Quaternion.CreateFromYawPitchRoll(rotation.Z, rotation.X, rotation.Y);
        }

        public void Rotate(Vector3 rotation)
        {
            var angles = Vector3.Transform(rotation, Orientation);
            GRotate(angles);
        }

        public void Rotate(Quaternion rotation)
        {
            Orientation = Quaternion.Multiply(rotation, Orientation);
        }

        public void Subtract(AFrame frame)
        {
            Origin -= Vector3.Transform(frame.Origin, frame.Orientation);
            Orientation = Quaternion.Multiply(Orientation, frame.Orientation);
        }

        public bool close_rotation(AFrame a, AFrame b)
        {
            var ao = a.Orientation;
            var bo = b.Orientation;

            return Math.Abs(ao.X - bo.X) < PhysicsGlobals.EPSILON &&
                   Math.Abs(ao.Y - bo.Y) < PhysicsGlobals.EPSILON &&
                   Math.Abs(ao.Z - bo.Z) < PhysicsGlobals.EPSILON &&
                   Math.Abs(ao.W - bo.W) < PhysicsGlobals.EPSILON;
        }

        public float get_heading()
        {
            var matrix = Matrix4x4.CreateFromQuaternion(Orientation);
            var heading = (float)Math.Atan2(matrix.M21, matrix.M22);
            return (450.0f - heading.ToDegrees()) % 360.0f;
        }

        public Vector3 get_vector_heading()
        {
            var matrix = Matrix4x4.CreateFromQuaternion(Orientation);

            var heading = new Vector3();
            heading.X = matrix.M21 + matrix.M31;
            heading.Y = matrix.M22 + matrix.M32;
            heading.Z = matrix.M23 + matrix.M33;
            return heading;
        }

        public void rotate_around_axis_to_vector(int axis, Vector3 dir)
        {
            // will implement when actually needed...
        }

        public void set_heading(float degrees)
        {
            var rads = degrees.ToRadians();

            var matrix = Matrix4x4.CreateFromQuaternion(Orientation);
            var heading = new Vector3((float)Math.Sin(rads), (float)Math.Cos(rads), matrix.M23 + matrix.M13);
            set_vector_heading(heading);
        }

        public void set_rotate(Quaternion orientation)
        {
            Orientation = orientation;
        }

        public void set_vector_heading(Vector3 heading)
        {
            // copy constructor?
            if (heading.NormalizeCheckSmall()) return;
            Orientation = Quaternion.CreateFromYawPitchRoll(heading.Z, heading.X, heading.Y);
        }
    }
}

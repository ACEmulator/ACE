using System;
using System.Numerics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Extensions;

namespace ACE.Server.Physics.Common
{
    public static class ObjectDesc
    {
        /// <summary>
        /// Displaces a scenery object into a pseudo-randomized location
        /// </summary>
        /// <param name="obj">The object description</param>
        /// <param name="ix">The global cell X-offset</param>
        /// <param name="iy">The global cell Y-offset</param>
        /// <param name="iq">The scene index of the object</param>
        /// <returns>The new location of the object</returns>
        public static Vector3 Displace(DatLoader.Entity.ObjectDesc obj, uint ix, uint iy, uint iq)
        {
            float x;
            float y;
            float z;

            var loc = obj.BaseLoc.Origin;

            if (obj.DisplaceX <= 0)
                x = loc.X;
            else
                x = (float)((1813693831 * iy - (iq + 45773) * (1360117743 * iy * ix + 1888038839) - 1109124029 * ix)
                    * 2.3283064e-10 * obj.DisplaceX + loc.X);

            if (obj.DisplaceY <= 0)
                y = loc.Y;
            else
                y = (float)((1813693831 * iy - (iq + 72719) * (1360117743 * iy * ix + 1888038839) - 1109124029 * ix)
                    * 2.3283064e-10 * obj.DisplaceY + loc.Y);

            z = loc.Z;

            var quadrant = (1813693831 * iy - ix * (1870387557 * iy + 1109124029) - 402451965) * 2.3283064e-10;

            if (quadrant >= 0.75) return new Vector3(y, -x, z);
            if (quadrant >= 0.5) return new Vector3(-x, -y, z);
            if (quadrant >= 0.25) return new Vector3(-y, x, z);

            return new Vector3(x, y, z);
        }
        
        /// <summary>
        /// Returns the scale for a scenery object
        /// </summary>
        /// <param name="obj">The object decription</param>
        /// <param name="x">The global cell X-offset</param>
        /// <param name="y">The global cell Y-offset</param>
        /// <param name="k">The scene index of the object</param>
        public static float ScaleObj(DatLoader.Entity.ObjectDesc obj, uint x, uint y, uint k)
        {
            var scale = 1.0f;

            var minScale = obj.MinScale;
            var maxScale = obj.MaxScale;

            if (minScale == maxScale)
                scale = maxScale;
            else
                scale = (float)(Math.Pow(maxScale / minScale,
                    (1813693831 * y - (k + 32593) * (1360117743 * y * x + 1888038839) - 1109124029 * x) * 2.3283064e-10) * minScale);

            return scale;
        }

        /// <summary>
        /// Returns the rotation for a scenery object
        /// </summary>
        public static AFrame RotateObj(DatLoader.Entity.ObjectDesc obj, uint x, uint y, uint k, Vector3 loc)
        {
            var frame = new AFrame(obj.BaseLoc);
            frame.Origin = loc;
            if (obj.MaxRotation > 0.0f)
            {
                var degrees = (float)((1813693831 * y - (k + 63127) * (1360117743 * y * x + 1888038839) - 1109124029 * x) * 2.3283064e-10 * obj.MaxRotation);
                frame.set_heading(degrees);
            }
            return frame;
        }

        /// <summary>
        /// Aligns an object to a plane
        /// </summary>
        public static AFrame ObjAlign(DatLoader.Entity.ObjectDesc obj, Plane plane, Vector3 loc)
        {
            var frame = new AFrame(obj.BaseLoc);
            frame.Origin = loc;
            var negNormal = -plane.Normal;
            var degrees = negNormal.get_heading();
            frame.set_heading(degrees);
            return frame;
        }

        /// <summary>
        /// Returns TRUE if floor slope is within bounds for this object
        /// </summary>
        public static bool CheckSlope(DatLoader.Entity.ObjectDesc obj, float z)
        {
            return z >= obj.MinSlope && z <= obj.MaxSlope;
        }
    }
}

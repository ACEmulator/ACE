using System.Numerics;

namespace ACE.Server.Physics.Util
{
    /// <summary>
    /// Defines the position adjustment for a dungeon
    /// given a bad position (usually the portal drop) from an outdated database
    /// and the matching good position from end-of-retail
    /// </summary>
    public class AdjustPosProfile
    {
        public Vector3 BadPosition;
        public Quaternion BadRotation;
        public Vector3 GoodPosition;
        public Quaternion GoodRotation;
    }
}

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
        public uint Landblock;
        public Vector3 BadPosition;
        public Quaternion BadRotation;
        public Vector3 GoodPosition;
        public Quaternion GoodRotation;

        public Matrix4x4? Transform;

        public AdjustPosProfile() { }

        public AdjustPosProfile(uint landblock)
        {
            Landblock = landblock;
        }

        public void BuildTransform()
        {
            var diff = GoodPosition - BadPosition;
            Transform = Matrix4x4.CreateTranslation(diff);
        }

        public Matrix4x4 GetTransform()
        {
            if (Transform == null)
                BuildTransform();

            return Transform.Value;
        }
    }
}

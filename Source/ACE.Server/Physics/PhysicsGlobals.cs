using System;
using System.Numerics;
using ACE.Entity.Enum;

namespace ACE.Server.Physics
{
    public class PhysicsGlobals
    {
        public const float EPSILON = 0.0002f;

        public const float EpsilonSq = EPSILON * EPSILON;

        public const float Gravity = -9.8f;

        public const float DefaultFriction = 0.95f;

        public const float DefaultElasticity = 0.05f;

        public const float DefaultTranslucency = 0.0f;

        public const float DefaultMass = 1.0f;

        public const float DefaultScale = 1.0f;

        public static readonly PhysicsState DefaultState =
            PhysicsState.EdgeSlide | PhysicsState.LightingOn | PhysicsState.Gravity | PhysicsState.ReportCollisions;

        public const float MaxElasticity = 0.1f;

        public const float MaxVelocity = 50.0f;

        public const float MaxVelocitySquared = MaxVelocity * MaxVelocity;

        public const float SmallVelocity = 0.25f;

        public const float SmallVelocitySquared = SmallVelocity * SmallVelocity;

        public const float MinQuantum = 1.0f / 30.0f;     // 0.0333... 30fps

        //public const float MaxQuantum = 0.2f;     // 5fps   // this is buggy with MoveToManager turning
        public const float MaxQuantum = 0.1f;     // 10fps

        public const float HugeQuantum = 2.0f;    // 0.5fps

        /// <summary>
        /// The walkable allowance when landing on the ground
        /// </summary>
        public const float LandingZ = 0.0871557f;

        public const float FloorZ = 0.66417414618662751f;

        public const float DummySphereRadius = 0.1f;

        public static readonly Sphere DummySphere = new Sphere(new Vector3(0, 0, DummySphereRadius), DummySphereRadius);

        public static readonly Sphere DefaultSortingSphere;

        public const float DefaultStepHeight = 0.01f;
    }
}

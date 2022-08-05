using System;
using System.Numerics;
using ACE.Entity.Enum;

namespace ACE.Server.Physics
{
    public class PhysicsGlobals
    {
        public static readonly float EPSILON = 0.0002f;

        public static readonly float EpsilonSq = EPSILON * EPSILON;

        public static readonly float Gravity = -9.8f;

        public static readonly float DefaultFriction = 0.95f;

        public static readonly float DefaultElasticity = 0.05f;

        public static readonly float DefaultTranslucency = 0.0f;

        public static readonly float DefaultMass = 1.0f;

        public static readonly float DefaultScale = 1.0f;

        public static readonly PhysicsState DefaultState =
            PhysicsState.EdgeSlide | PhysicsState.LightingOn | PhysicsState.Gravity | PhysicsState.ReportCollisions;

        public static readonly float MaxElasticity = 0.1f;

        public static readonly float MaxVelocity = 50.0f;

        public static readonly float MaxVelocitySquared = MaxVelocity * MaxVelocity;

        public static readonly float SmallVelocity = 0.25f;

        public static readonly float SmallVelocitySquared = SmallVelocity * SmallVelocity;

        public static readonly float MinQuantum = 1.0f / 30.0f;     // 0.0333... 30fps

        //public static readonly float MaxQuantum = 0.2f;     // 5fps   // this is buggy with MoveToManager turning
        public static readonly float MaxQuantum = 0.1f;     // 10fps

        public static readonly float HugeQuantum = 2.0f;    // 0.5fps

        /// <summary>
        /// The walkable allowance when landing on the ground
        /// </summary>
        public static readonly float LandingZ = 0.0871557f;

        public static readonly float FloorZ = 0.66417414618662751f;

        public static readonly float DummySphereRadius = 0.1f;

        public static readonly Sphere DummySphere = new Sphere(new Vector3(0, 0, DummySphereRadius), DummySphereRadius);

        public static readonly Sphere DefaultSortingSphere;

        public static readonly float DefaultStepHeight = 0.01f;
    }
}

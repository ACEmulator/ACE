namespace ACE.Server.Physics
{
    public class PhysicsGlobals
    {
        public static readonly float EPSILON = 0.00019999999f;

        public static readonly float Gravity = -9.8000002f;

        public static readonly float DefaultFriction = 0.94999999f;

        public static readonly float DefaultElasticity = 0.050000001f;

        public static readonly float DefaultTranslucency = 0.0f;

        public static readonly float DefaultMass = 1.0f;

        public static readonly float DefaultScale = 1.0f;

        public static readonly PhysicsState DefaultState =
            PhysicsState.EdgeSlide | PhysicsState.LightingOn | PhysicsState.Gravity | PhysicsState.ReportCollisions;

        /// <summary>
        /// The walkable allowance when landing on the ground
        /// </summary>
        public static readonly float LandingZ = 0.0871557f;

        public static readonly float DummySphereRadius = 0.1f;
    }
}

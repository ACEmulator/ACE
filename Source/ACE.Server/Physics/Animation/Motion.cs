namespace ACE.Server.Physics.Animation
{
    public class Motion
    {
        public uint ID;
        public float SpeedMod;

        public Motion() { }

        public Motion(uint id, float speedMod = 1.0f)
        {
            ID = id;
            SpeedMod = speedMod;
        }
    }
}

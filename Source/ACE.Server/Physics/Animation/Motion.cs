namespace ACE.Server.Physics.Animation
{
    public class Motion
    {
        public int ID;
        public float SpeedMod;

        public Motion() { }

        public Motion(int id, float speedMod = 1.0f)
        {
            ID = id;
            SpeedMod = speedMod;
        }
    }
}

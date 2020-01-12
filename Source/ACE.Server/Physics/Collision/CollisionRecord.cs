namespace ACE.Server.Physics.Collision
{
    public class CollisionRecord
    {
        public double TouchedTime;
        public bool Ethereal;

        public CollisionRecord(double touchedTime, bool ethereal)
        {
            TouchedTime = touchedTime;
            Ethereal = ethereal;
        }
    }
}

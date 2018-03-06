namespace ACE.Server.Physics.Combat
{
    public enum DetectionType
    {
        NoChangeDetection = 0x0,
        EnteredDetection = 0x1,
        LeftDetection = 0x2,
    };

    public class DetectionInfo
    {
        public int ObjectID;
        public DetectionType Status;

        public DetectionInfo() { }

        public DetectionInfo(int objectID, DetectionType status)
        {
            ObjectID = objectID;
            Status = status;
        }
    }
}

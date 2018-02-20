namespace ACE.Server.Physics.Combat
{
    public enum DetectionType
    {
        NoChangeDetection = 0x0,
        EnteredDetection = 0x1,
        LeftDetection = 0x2,
        FORCE_DetectionType_32_BIT = 0x7FFFFFFF,
    };

    public class DetectionInfo
    {
        public int ObjectID;
        public DetectionType ObjectStatus;
    }
}

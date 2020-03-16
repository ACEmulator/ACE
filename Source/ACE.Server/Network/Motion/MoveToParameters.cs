using System.IO;
using ACE.Entity.Enum;

namespace ACE.Server.Network.Structure
{
    public class MoveToParameters
    {
        public MovementParams MovementParameters;
        public float DistanceToObject;      // move within this distance to the object
        public float MinDistance;           // the minimum distance to move
        public float FailDistance;          // the distance at which the movement will fail
        public float Speed;                 // walk/run speed multiplier
        public float WalkRunThreshold;      // if below this distance, walk instead of run
        public float DesiredHeading;        // heading to turn to

        public MoveToParameters()
        {
            SetDefaults();
        }

        public void SetDefaults()
        {
            MovementParameters =
                MovementParams.CanWalk |
                MovementParams.CanRun |
                MovementParams.CanSideStep |
                MovementParams.CanWalkBackwards |
                MovementParams.MoveTowards |
                MovementParams.UseSpheres |
                MovementParams.SetHoldKey |
                MovementParams.ModifyRawState |
                MovementParams.ModifyInterpretedState |
                MovementParams.CancelMoveTo /*|
                MovementParams.StopCompletely*/;    // this should be default, as per acclient -- investigate

            MinDistance = 0.0f;
            FailDistance = float.MaxValue;
            Speed = 1.0f;
            WalkRunThreshold = 15.0f;
            DesiredHeading = 0.0f;
        }
    }

    public static class MoveToParametersExtensions
    {
        public static void Write(this BinaryWriter writer, MoveToParameters mvp)
        {
            writer.Write((uint)mvp.MovementParameters);
            writer.Write(mvp.DistanceToObject);
            writer.Write(mvp.MinDistance);
            writer.Write(mvp.FailDistance);
            writer.Write(mvp.Speed);
            writer.Write(mvp.WalkRunThreshold);
            writer.Write(mvp.DesiredHeading);
        }
    }
}

using log4net;
using ACE.Network.Enum;
using ACE.Network.Sequence;
using ACE.Network.Motion;
using System.Collections.Generic;

namespace ACE.Network.GameMessages.Messages
{
    using System;
    using System.Diagnostics.Eventing.Reader;
    using System.Runtime.Remoting.Messaging;

    using global::ACE.Entity;

    public class GameMessageUpdateMotion : GameMessage
    {
        public GameMessageUpdateMotion(WorldObject animationTarget, Session session, MotionState newState)
            : base(GameMessageOpcode.Motion, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(animationTarget.Guid); // Object_Id (uint)
            Writer.Write((ushort)session.Player.TotalLogins); // Instance_Timestamp
            Writer.Write(animationTarget.Sequences.GetNextSequence(SequenceType.ObjectMovement)); // Movement_Timestamp
            if (!newState.IsAutonomous)
                Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.ObjectServerControl)); // Server_Control_Timestamp
            else
                Writer.Write(animationTarget.Sequences.GetCurrentSequence(Sequence.SequenceType.ObjectServerControl)); // Server_Control_Timestamp

            ushort autonomous = newState.IsAutonomous ? (ushort)1 : (ushort)0;
            Writer.Write(autonomous); // autonomous flag - 1 or 0.   I think this is set if you have are holding the run key or some other autonomous movement
            var movementData = newState.GetPayload(animationTarget);
            Writer.Write(movementData);
            Writer.Align();
        }

        public GameMessageUpdateMotion(WorldObject animationTarget, MotionState newState) : base(GameMessageOpcode.Motion, GameMessageGroup.Group0A)
        {
            Writer.WriteGuid(animationTarget.Guid); // Object_Id (uint)
            Writer.Write(animationTarget.Sequences.GetCurrentSequence(SequenceType.ObjectInstance)); // Instance_Timestamp
            Writer.Write(animationTarget.Sequences.GetNextSequence(SequenceType.ObjectMovement)); // Movement_Timestamp
            if (!newState.IsAutonomous)
                Writer.Write(animationTarget.Sequences.GetNextSequence(Sequence.SequenceType.ObjectServerControl)); // Server_Control_Timestamp
            else
                Writer.Write(animationTarget.Sequences.GetCurrentSequence(Sequence.SequenceType.ObjectServerControl)); // Server_Control_Timestamp

            ushort autonomous;
            if (newState.IsAutonomous)
                autonomous = 1;
            else
                autonomous = 0;
            Writer.Write(autonomous); // autonomous flag - 1 or 0.   I think this is set if you have are holding the run key or some other autonomous movement
            var movementData = newState.GetPayload(animationTarget);
            Writer.Write(movementData);
            Writer.Align();
        }

        // TODO: Og II - Finish an overload for F74C 0x0006 - MoveToObject
        /// <summary>
        /// This method is used for server controled motion - such as move to object, turn to object etc.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="moveToTarget"></param>
        /// <param name="newState"></param>
        /// <param name="movementType"></param>
        /// <param name="runRate"></param>
        public GameMessageUpdateMotion(WorldObject player, WorldObject moveToTarget, ServerControlMotion newState, MovementTypes movementType, float runRate = 1.0f) : base(GameMessageOpcode.Motion, GameMessageGroup.Group0A)
        {           
            Writer.WriteGuid(player.Guid); // Object_Id (uint)
            Writer.Write(player.Sequences.GetCurrentSequence(SequenceType.ObjectInstance)); // Instance_Timestamp
            Writer.Write(player.Sequences.GetNextSequence(SequenceType.ObjectMovement)); // Movement_Timestamp

            // Looking at the pcaps in training_hall_main_section.pcap - the key to server control timestamp seems to be the movement type
            // TODO: Og II Reseach if it is all of the server control movement types - it may be all them them except MovementTypes.General

             if (movementType == MovementTypes.MoveToObject | movementType == MovementTypes.TurnToObject)
                Writer.Write(moveToTarget.Sequences.GetNextSequence(Sequence.SequenceType.ObjectServerControl)); // Server_Control_Timestamp
             else
                Writer.Write(moveToTarget.Sequences.GetCurrentSequence(Sequence.SequenceType.ObjectServerControl)); // Server_Control_Timestamp

            ushort autonomous;
            if (newState.IsAutonomous)
                autonomous = 1;
            else
                autonomous = 0;
            Writer.Write(autonomous);
                                   
            var movementData = newState.GetPayload(moveToTarget, 0.6f);   
            Writer.Write(movementData);
            Writer.Align();
        }
    }
}

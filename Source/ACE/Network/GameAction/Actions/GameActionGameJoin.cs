using System;
using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Factories;
using ACE.Managers;
using ACE.Network.GameMessages.Messages;
using ACE.Entity.Actions;
using ACE.Network.Motion;
using ACE.Network.Sequence;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionGameJoin
    {
        [GameAction(GameActionType.Join)]
        public static void Handle(ClientMessage message, Session session)
        {
            var gameId = message.Payload.ReadUInt32(); // object id of gameboard
            var whichTeam = message.Payload.ReadUInt32(); // expecting 0xFFFFFFFF here

            // var dobEvent = new GameMessages.Messages.GameMessageSystemChat($"Join request ack.", ChatMessageType.Broadcast);

            ////var msgJoinResponse = new GameEvent.Events.GameEventJoinGameResponse(session, gameId, -1);
            var msgJoinResponse = new GameEvent.Events.GameEventJoinGameResponse(session, gameId, 1);

            // var msgGameOver = new GameEvent.Events.GameEventGameOver(session, gameId, -2);
            ////var msgGameOver = new GameEvent.Events.GameEventGameOver(session, 0, -2);
            var msgGameOver = new GameEvent.Events.GameEventGameOver(session, gameId, 0);

            // session.Network.EnqueueSend(msgJoinResponse, msgGameOver);
            session.Network.EnqueueSend(msgJoinResponse);

            var drudgeRook1 = WorldObjectFactory.CreateNewWorldObject(14343);
            drudgeRook1.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 132.47f, 129.813f, 94.34682f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgeRook1).EnqueueChain();
            var drudgeRookPSCreate = new GameMessageScript(drudgeRook1.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgeRookPSCreate);

            var drudgeKnight1 = WorldObjectFactory.CreateNewWorldObject(14344);
            drudgeKnight1.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 133.47f, 129.813f, 94.34699f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgeKnight1).EnqueueChain();
            var drudgeKnightPSCreate = new GameMessageScript(drudgeKnight1.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgeKnightPSCreate);

            var drudgeBishop1 = WorldObjectFactory.CreateNewWorldObject(14345);
            drudgeBishop1.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 134.47f, 129.813f, 94.34717f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgeBishop1).EnqueueChain();
            var drudgeBishopPSCreate = new GameMessageScript(drudgeBishop1.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgeBishopPSCreate);

            var drudgeQueen = WorldObjectFactory.CreateNewWorldObject(14346);
            drudgeQueen.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 135.47f, 129.813f, 94.34735f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgeQueen).EnqueueChain();
            var drudgeQueenPSCreate = new GameMessageScript(drudgeQueen.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgeQueenPSCreate);

            var drudgeKing = WorldObjectFactory.CreateNewWorldObject(14347);
            drudgeKing.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 136.47f, 129.813f, 94.34752f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgeKing).EnqueueChain();
            var drudgeKingPSCreate = new GameMessageScript(drudgeKing.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgeKingPSCreate);

            var drudgeBishop2 = WorldObjectFactory.CreateNewWorldObject(14345);
            drudgeBishop2.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 137.47f, 129.813f, 94.34717f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgeBishop2).EnqueueChain();
            var drudgeBishopPSCreate2 = new GameMessageScript(drudgeBishop2.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgeBishopPSCreate2);

            var drudgeKnight2 = WorldObjectFactory.CreateNewWorldObject(14344);
            drudgeKnight2.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 138.47f, 129.813f, 94.34699f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgeKnight2).EnqueueChain();
            var drudgeKnightPSCreate2 = new GameMessageScript(drudgeKnight2.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgeKnightPSCreate2);

            var drudgeRook2 = WorldObjectFactory.CreateNewWorldObject(14343);
            drudgeRook2.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 139.47f, 129.813f, 94.34682f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgeRook2).EnqueueChain();
            var drudgeRookPSCreate2 = new GameMessageScript(drudgeRook2.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgeRookPSCreate2);

            var drudgePawn1 = WorldObjectFactory.CreateNewWorldObject(14342);
            drudgePawn1.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 132.47f, 130.813f, 94.34647f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgePawn1).EnqueueChain();
            var drudgePawn1PSCreate = new GameMessageScript(drudgePawn1.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgePawn1PSCreate);

            var drudgePawn2 = WorldObjectFactory.CreateNewWorldObject(14342);
            drudgePawn2.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 133.47f, 130.813f, 94.34647f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgePawn2).EnqueueChain();
            var drudgePawn2PSCreate = new GameMessageScript(drudgePawn2.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgePawn2PSCreate);

            var drudgePawn3 = WorldObjectFactory.CreateNewWorldObject(14342);
            drudgePawn3.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 134.47f, 130.813f, 94.34647f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgePawn3).EnqueueChain();
            var drudgePawn3PSCreate = new GameMessageScript(drudgePawn3.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgePawn3PSCreate);

            var drudgePawn4 = WorldObjectFactory.CreateNewWorldObject(14342);
            drudgePawn4.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 135.47f, 130.813f, 94.34647f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgePawn4).EnqueueChain();
            var drudgePawn4PSCreate = new GameMessageScript(drudgePawn4.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgePawn4PSCreate);

            var drudgePawn5 = WorldObjectFactory.CreateNewWorldObject(14342);
            drudgePawn5.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 136.47f, 130.813f, 94.34647f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgePawn5).EnqueueChain();
            var drudgePawn5PSCreate = new GameMessageScript(drudgePawn5.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgePawn5PSCreate);

            var drudgePawn6 = WorldObjectFactory.CreateNewWorldObject(14342);
            drudgePawn6.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 137.47f, 130.813f, 94.34647f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgePawn6).EnqueueChain();
            var drudgePawn6PSCreate = new GameMessageScript(drudgePawn6.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgePawn6PSCreate);

            var drudgePawn7 = WorldObjectFactory.CreateNewWorldObject(14342);
            drudgePawn7.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 138.47f, 130.813f, 94.34647f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgePawn7).EnqueueChain();
            var drudgePawn7PSCreate = new GameMessageScript(drudgePawn7.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgePawn7PSCreate);

            var drudgePawn8 = WorldObjectFactory.CreateNewWorldObject(14342);
            drudgePawn8.Location = new Position(Convert.ToUInt32("A9B2002E", 16), 139.47f, 130.813f, 94.34647f, 0, 0, 0, 1);
            LandblockManager.GetAddObjectChain(drudgePawn8).EnqueueChain();
            var drudgePawn8PSCreate = new GameMessageScript(drudgePawn8.Guid, PlayScript.Create, 1);
            session.Network.EnqueueSend(drudgePawn8PSCreate);

            UniversalMotion motionDeath = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));

            ActionChain autoCloseTimer = new ActionChain();
            autoCloseTimer.AddDelaySeconds(5);
            autoCloseTimer.AddAction(session.Player, () =>
            {
                var drudgeRook1Death = new GameMessageUpdateMotion(
                    drudgeRook1.Guid,
                    drudgeRook1.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgeRook1.Sequences, motionDeath);
                var drudgeKnight1Death = new GameMessageUpdateMotion(
                    drudgeKnight1.Guid,
                    drudgeKnight1.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgeKnight1.Sequences, motionDeath);
                var drudgeBishop1Death = new GameMessageUpdateMotion(
                    drudgeBishop1.Guid,
                    drudgeBishop1.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgeBishop1.Sequences, motionDeath);
                var drudgeQueenDeath = new GameMessageUpdateMotion(
                    drudgeQueen.Guid,
                    drudgeQueen.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgeQueen.Sequences, motionDeath);
                var drudgeKingDeath = new GameMessageUpdateMotion(
                    drudgeKing.Guid,
                    drudgeKing.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgeKing.Sequences, motionDeath);
                var drudgeBishop2Death = new GameMessageUpdateMotion(
                    drudgeBishop2.Guid,
                    drudgeBishop2.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgeBishop2.Sequences, motionDeath);
                var drudgeKnight2Death = new GameMessageUpdateMotion(
                    drudgeKnight2.Guid,
                    drudgeKnight2.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgeKnight2.Sequences, motionDeath);
                var drudgeRook2Death = new GameMessageUpdateMotion(
                    drudgeRook2.Guid,
                    drudgeRook2.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgeRook2.Sequences, motionDeath);
                var drudgePawn1Death = new GameMessageUpdateMotion(
                    drudgePawn1.Guid,
                    drudgePawn1.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgePawn1.Sequences, motionDeath);
                var drudgePawn2Death = new GameMessageUpdateMotion(
                    drudgePawn2.Guid,
                    drudgePawn2.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgePawn2.Sequences, motionDeath);
                var drudgePawn3Death = new GameMessageUpdateMotion(
                    drudgePawn3.Guid,
                    drudgePawn3.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgePawn3.Sequences, motionDeath);
                var drudgePawn4Death = new GameMessageUpdateMotion(
                    drudgePawn4.Guid,
                    drudgePawn4.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgePawn4.Sequences, motionDeath);
                var drudgePawn5Death = new GameMessageUpdateMotion(
                    drudgePawn5.Guid,
                    drudgePawn5.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgePawn5.Sequences, motionDeath);
                var drudgePawn6Death = new GameMessageUpdateMotion(
                    drudgePawn6.Guid,
                    drudgePawn6.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgePawn6.Sequences, motionDeath);
                var drudgePawn7Death = new GameMessageUpdateMotion(
                    drudgePawn7.Guid,
                    drudgePawn7.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgePawn7.Sequences, motionDeath);
                var drudgePawn8Death = new GameMessageUpdateMotion(
                    drudgePawn8.Guid,
                    drudgePawn8.Sequences.GetCurrentSequence(SequenceType.ObjectInstance),
                    drudgePawn8.Sequences, motionDeath);

                session.Network.EnqueueSend(msgGameOver);

                session.Network.EnqueueSend(
                    drudgeRook1Death, drudgeKnight1Death, drudgeBishop1Death, drudgeQueenDeath, drudgeKingDeath, drudgeBishop2Death, drudgeKnight2Death, drudgeRook2Death,
                    drudgePawn1Death, drudgePawn2Death, drudgePawn3Death, drudgePawn4Death, drudgePawn5Death, drudgePawn6Death, drudgePawn7Death, drudgePawn8Death);
            });
            autoCloseTimer.AddDelaySeconds(2);
            autoCloseTimer.AddAction(session.Player, () =>
            {
                byte[] msg = Convert.FromBase64String("Z2FtZXNkZWFkbG9s");
                var popupGDL = new GameEvent.Events.GameEventPopupString(session, System.Text.Encoding.UTF8.GetString(msg, 0, msg.Length));
                var msgGameOver2 = new GameEvent.Events.GameEventGameOver(session, 0, -2);
                session.Network.EnqueueSend(popupGDL, msgGameOver2);
            });
            autoCloseTimer.AddDelaySeconds(5);
            autoCloseTimer.AddAction(session.Player, () =>
            {
                LandblockManager.RemoveObject(drudgeRook1);
                LandblockManager.RemoveObject(drudgeKnight1);
                LandblockManager.RemoveObject(drudgeBishop1);
                LandblockManager.RemoveObject(drudgeQueen);
                LandblockManager.RemoveObject(drudgeKing);
                LandblockManager.RemoveObject(drudgeBishop2);
                LandblockManager.RemoveObject(drudgeKnight2);
                LandblockManager.RemoveObject(drudgeRook2);
                LandblockManager.RemoveObject(drudgePawn1);
                LandblockManager.RemoveObject(drudgePawn2);
                LandblockManager.RemoveObject(drudgePawn3);
                LandblockManager.RemoveObject(drudgePawn4);
                LandblockManager.RemoveObject(drudgePawn5);
                LandblockManager.RemoveObject(drudgePawn6);
                LandblockManager.RemoveObject(drudgePawn7);
                LandblockManager.RemoveObject(drudgePawn8);
            });
            autoCloseTimer.EnqueueChain();
        }
    }
}

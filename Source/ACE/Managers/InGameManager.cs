using ACE.Database;
using ACE.Entity;
using ACE.Network;
using System.Collections.Generic;
using System.Threading;
using System;
using ACE.Network.GameAction;

namespace ACE.InGameManager
{
    // impletements deisgn pattern Mediator
    public static class InGameManager
    {
        private static GameConcreteMediator mediator = new GameConcreteMediator();
        private static GamePlayers gameplayers;
        private static GameWorld gameworld;
        private static bool running = false;

        // pending player sessions trying to enter the world
        private static readonly object pendingplayersCacheLocker = new object();
        private static Queue<Session> pendingplayers = new Queue<Session>();

        // pending world objects waiting to enter game world from outsiders
        private static readonly object pendingobjectsCacheLocker = new object();
        private static Queue<WorldObject> pendingobjects = new Queue<WorldObject>();

        public static void Initialize()
        {
            // Create Colleague classes
            gameplayers = new GamePlayers(mediator);
            gameworld = new GameWorld(mediator);
            // physicsworld = new PhysicsWord(mediator);

            // Register Colleague classes with mediator
            mediator.RegisterGameWorld(gameworld);
            mediator.RegisterPlayers(gameplayers);

            // starts game loop.
            running = true;
            new Thread(UseTime).Start();
        }

        public async static void Register(WorldObject wo)
        {
            lock (pendingobjectsCacheLocker)
            {
                pendingobjects.Enqueue(wo);
            }
        }

        private static void UseTime()
        {
            while (running)
            {
                lock (pendingobjectsCacheLocker)
                {
                    if (pendingplayers.Count > 0)
                        mediator.PlayerEnterWorld(pendingplayers.Dequeue());
                }
                lock (pendingobjectsCacheLocker)
                {
                    if (pendingobjects.Count > 0)
                        mediator.Register(pendingobjects.Dequeue());
                }
                gameplayers.Tick();
                gameworld.Tick();
            }
        }

        public static async void PlayerEnterWorld(Session session)
        {
            var task = DatabaseManager.Character.LoadCharacter(session.Player.Guid.Low);
            task.Wait();
            Character c = task.Result;
            await session.Player.Load(c);
            lock (pendingplayersCacheLocker)
            {
                pendingplayers.Enqueue(session);
            }
        }

        // junk so i can compile while I work.
        internal static WorldObject ReadOnlyClone(ObjectGuid playerGuid)
        {
            throw new NotImplementedException();
        }
    }
}

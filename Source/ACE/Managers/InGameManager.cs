using ACE.Database;
using ACE.Entity;
using ACE.Network;
using System.Collections.Generic;
using System.Threading;

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
        private static readonly object objectCacheLocker = new object();
        private static Queue<Session> pendingplayers = new Queue<Session>();

        public static void Initialize()
        {
            //Create Colleague classes
            gameplayers = new GamePlayers(mediator);
            gameworld = new GameWorld(mediator);
            // physicsworld = new PhysicsWord(mediator);

            //Register Colleague classes with mediator
            mediator.RegisterGameWorld(gameworld);
            mediator.RegisterPlayers(gameplayers);

            // starts game loop.
            running = true;
            new Thread(UseTime).Start();
        }

        private static void UseTime()
        {
            while (running)
            {
                lock (objectCacheLocker)
                {
                    if (pendingplayers.Count > 0)
                        mediator.PlayerEnterWorld(pendingplayers.Dequeue());
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
            lock (objectCacheLocker)
            {
                pendingplayers.Enqueue(session);
            }
        }

    }
}

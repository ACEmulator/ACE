using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.LandManagers;
using ACE.Network;
using ACE.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ACE.Managers
{
    public static class OpenWorldManager
    {
        public static OpenWorld OpenWorld = new OpenWorld();

        public static void Initialize()
        {
            OpenWorld.StartUseTime();
        }

        public static async void PlayerEnterWorld(Session session)
        {
            var task = DatabaseManager.Character.LoadCharacter(session.Player.Guid.Low);
            task.Wait();
            Character c = task.Result;

            await session.Player.Load(c);
            OpenWorld.Register(session.Player);

            session.Network.EnqueueSend(new GameMessageSystemChat("Welcome to Asheron's Call", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat("  powered by ACEmulator  ", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat("", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat("For more information on commands supported by this server, type @acehelp", ChatMessageType.Broadcast));
        }
    }
}

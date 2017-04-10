using ACE.Entity.Enum;
using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity.Events
{
    public class BroadcastEventArgs
    {
        private BroadcastEventArgs()
        {
        }

        public static BroadcastEventArgs CreateAction(BroadcastAction actionType, WorldObject sender)
        {
            return new BroadcastEventArgs()
            {
                ActionType = actionType,
                Sender = sender
            };
        }

        public static BroadcastEventArgs CreateChatAction(WorldObject sender, ChatMessageArgs chatMessage)
        {
            return new BroadcastEventArgs()
            {
                ActionType = BroadcastAction.LocalChat,
                Sender = sender,
                ChatMessage = chatMessage
            };
        }

        public static BroadcastEventArgs CreateSoundAction(WorldObject sender, Sound sound)
        {
            return new BroadcastEventArgs()
            {
                ActionType = BroadcastAction.PlaySound,
                Sender = sender,
                Sound = sound
            };
        }

        public static BroadcastEventArgs CreateEffectAction(WorldObject sender, PlayScript effect)
        {
            return new BroadcastEventArgs()
            {
                ActionType = BroadcastAction.PlayParticleEffect,
                Sender = sender,
                Effect = effect
            };
        }

        public BroadcastAction ActionType { get; private set; }

        public WorldObject Sender { get; private set; }

        public ChatMessageArgs ChatMessage { get; private set; }

        public Sound Sound { get;  private set; }

        public PlayScript Effect { get; private set; }
    }
}

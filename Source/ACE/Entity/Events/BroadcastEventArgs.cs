using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameEvent;
using ACE.Network.GameMessages;
using ACE.Network.Motion;
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
                BroadcastType = actionType,
                Sender = sender
            };
        }

        public static BroadcastEventArgs CreateChatAction(WorldObject sender, ChatMessageArgs chatMessage)
        {
            return new BroadcastEventArgs()
            {
                BroadcastType = BroadcastAction.LocalChat,
                Sender = sender,
                ChatMessage = chatMessage
            };
        }

        public static BroadcastEventArgs CreateSoundAction(WorldObject sender, Sound sound)
        {
            return new BroadcastEventArgs()
            {
                BroadcastType = BroadcastAction.PlaySound,
                Sender = sender,
                Sound = sound
            };
        }

        public static BroadcastEventArgs CreateEffectAction(WorldObject sender, PlayScript effect)
        {
            return new BroadcastEventArgs()
            {
                BroadcastType = BroadcastAction.PlayParticleEffect,
                Sender = sender,
                Effect = effect
            };
        }

        public static BroadcastEventArgs CreateMovementEvent(WorldObject sender, GeneralMotion motion)
        {
            return new BroadcastEventArgs()
            {
                BroadcastType = BroadcastAction.MovementEvent,
                Sender = sender,
                Motion = motion,
            };
        }

        public static BroadcastEventArgs ActionEventBroadcast(WorldObject sender, OutboundEventArgs outboundEvent)
        {
            return new BroadcastEventArgs()
            {
                BroadcastType = outboundEvent.BroadcastType,
                Sender = sender,
                OutboundMessage = outboundEvent.Message,
            };
        }

        public GameMessage OutboundMessage { get; private set; }

        public BroadcastAction BroadcastType { get; private set; }

        public WorldObject Sender { get; private set; }

        public ChatMessageArgs ChatMessage { get; private set; }

        public Sound Sound { get;  private set; }

        public PlayScript Effect { get; private set; }

        public GeneralMotion Motion { get; private set; }
    }
}

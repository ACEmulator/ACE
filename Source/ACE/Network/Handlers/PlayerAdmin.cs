using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameAction;
using ACE.Network.GameEvent.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public partial class Player
    {
        [GameAction(GameActionType.AddChannel)]
        private void AddChannelAction(ClientMessage message)
        {
            var chatChannelID = (GroupChatType)message.Payload.ReadUInt32();
            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!IsAdmin && !IsArch && !IsPsr)
                return;

            // TODO: Subscribe to channel (chatChannelID) and save to db. Channel subscriptions are meant to persist between sessions.
        }

        [GameAction(GameActionType.IndexChannels)]
        private void IndexChannelsAction(ClientMessage message)
        {
            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!IsAdmin && !IsArch && !IsPsr)
                return;

            Session.EnqueueSend(new GameEventChannelIndex(Session));
        }

        [GameAction(GameActionType.RemoveChannel)]
        private void RemoveChannelAction(ClientMessage message)
        {
            var chatChannelID = (GroupChatType)message.Payload.ReadUInt32();
            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!IsAdmin && !IsArch && !IsPsr)
                return;

            // TODO: Unsubscribe from channel (chatChannelID) and save to db. Channel subscriptions are meant to persist between sessions.
        }

        [GameAction(GameActionType.ListChannels)]
        private void ListChannelsAction(ClientMessage message)
        {
            var chatChannelID = (GroupChatType)message.Payload.ReadUInt32();
            // Probably need some IsAdvocate and IsSentinel type thing going on here as well. leaving for now
            if (!IsAdmin && !IsArch && !IsPsr)
                return;

            Session.EnqueueSend(new GameEventChannelList(Session, chatChannelID));
        }

        [GameAction(GameActionType.AdvocateTeleport)]
        private void AdvocateTeleportAction(ClientMessage message)
        {
            var target = message.Payload.ReadString16L();
            var position = new Position(message.Payload);
            // this check is also done clientside, see: PlayerDesc::PlayerIsPSR
            if (!IsAdmin && !IsArch && !IsPsr)
                return;

            uint cell = position.LandblockId.Raw;
            uint cellX = (cell >> 3);

            // TODO: Wrap command in a check to confirm session.character IsAdvocate or higher access level

            // TODO: Maybe output to chat window coords teleported to.
            // ChatPacket.SendSystemMessage(session, $"Teleporting to: 0.0[N/S], 0.0[E/W]");
            ChatPacket.SendServerMessage(Session, "Teleporting...", ChatMessageType.Broadcast);

            // TODO: Lookup player
            Teleport(position);
        }
    }
}

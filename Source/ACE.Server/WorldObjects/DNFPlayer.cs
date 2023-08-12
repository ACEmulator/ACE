using System;
using System.Collections.Generic;
using System.Numerics;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Sequence;
using ACE.Server.Network.Structure;
using ACE.Server.Physics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;
using ACE.Server.WorldObjects.Managers;

using Character = ACE.Database.Models.Shard.Character;
using MotionTable = ACE.DatLoader.FileTypes.MotionTable;

namespace ACE.Server.WorldObjects
{
    public partial class Player : Creature, IPlayer
    {
        public void SendFrozenMessage()
        {
            var msg = "You are unable to perform any actions because you've been frozen or jailed by an Admin.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg,ChatMessageType.WorldBroadcast));
        }

        public void SendThawedMessage()
        {
            var msg = "You can now move and use abilities again. You were thawed by an Admin.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg,ChatMessageType.WorldBroadcast));
        }

        public void SendGemFrozenError()
        {
            var msg = "You are unable to use gems because you've been frozen or jailed by an Admin.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg,ChatMessageType.WorldBroadcast));
        }    

        public void SendSpellcastFrozenError()
        {
            var msg = "You are unable to cast spells because you've been frozen or jailed by an Admin.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg,ChatMessageType.WorldBroadcast));
        }

        public void SendMovementFrozenError()
        {
            var msg = "You are unable to move because you've been frozen by an Admin.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg,ChatMessageType.WorldBroadcast));
        }

        public void SendFrozenNotice()
        {
            var msg = "You are unable to move, use gems or cast spells because you've been frozen.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg,ChatMessageType.WorldBroadcast));
        }

        public void SendThawedNotice()
        {
            var msg = "Your ability to move, use gems and cast spells has been restored.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg,ChatMessageType.WorldBroadcast));
        }

        public void SendJailedMessage()
        {
            var msg = "You were jailed by an Admin.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg,ChatMessageType.WorldBroadcast));
        }

        public void SendPardonedMessage()
        {
            var msg = "You were pardoned by an Admin and can now roam freely once again.";
            Session.Network.EnqueueSend(new GameEventCommunicationTransientString(Session, msg), new GameMessageSystemChat(msg,ChatMessageType.WorldBroadcast));
        }
    }
}

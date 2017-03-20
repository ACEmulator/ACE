﻿
using ACE.Entity;
using ACE.Entity.Enum;
using System.Collections.Generic;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventChannelIndex : GameEventMessage
    {
        public GameEventChannelIndex(Session session) : base(GameEventType.ChannelIndex, GameMessageGroup.Group09, session)
        {
            WriteEventBody();
        }

        private void WriteEventBody()
        {

            // TODO: this probably could be done better but I'm not sure if there's a point, it's not like the client out of the box supports making up new channels

            // Admin
            // Abuse
            // Audit
            // Av1
            // Av2
            // Av3
            // Sentinel
            // Help

            // uint channelCount = 0;

            if (Session.Player.IsAdmin)
            {
                Writer.Write(8u);
                Writer.WriteString16L("Abuse");
                Writer.WriteString16L("Admin");
                Writer.WriteString16L("Audit");
                Writer.WriteString16L("Av1");
                Writer.WriteString16L("Av2");
                Writer.WriteString16L("Av3");
                Writer.WriteString16L("Sentinel");
                Writer.WriteString16L("Help");
            }

            if (Session.Player.IsArch || Session.Player.IsEnvoy || Session.Player.IsPsr)
            {
                Writer.Write(7u);
                Writer.WriteString16L("Abuse");
                Writer.WriteString16L("Audit");
                Writer.WriteString16L("Av1");
                Writer.WriteString16L("Av2");
                Writer.WriteString16L("Av3");
                Writer.WriteString16L("Sentinel");
                Writer.WriteString16L("Help");
            }

            // TODO: Needs an IsAdvocate thing to work like it should
            // if (Session.Player.IsAdvocate)
            // {
            //    Writer.Write(5u);
            //    Writer.WriteString16L("Abuse");
            //    Writer.WriteString16L("Av1");
            //    Writer.WriteString16L("Av2");
            //    Writer.WriteString16L("Av3");
            //    Writer.WriteString16L("Help");
            // }
        }
    }
}

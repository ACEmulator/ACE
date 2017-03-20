
using System;
using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.QueryAge)]
    public class GameActionQueryAge : GameActionPacket
    {
        private string target;

        public GameActionQueryAge(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            target = Fragment.Payload.ReadString16L();
        }

        public override void Handle()
        {

            DateTime playerDOB = new DateTime();
            playerDOB = playerDOB.AddSeconds(Session.Player.PropertiesInt[Entity.Enum.Properties.PropertyInt.Age]);
            TimeSpan tsAge = playerDOB - new DateTime();

            string age = "";

            if (tsAge.ToString("%d") != "0")
            {
                if (Convert.ToInt16(tsAge.ToString("%d")) > 0 && Convert.ToInt16(tsAge.ToString("%d")) <= 7)
                    age = age + tsAge.ToString("%d") + "d ";
                if (Convert.ToInt16(tsAge.ToString("%d")) > 7)
                {

                    int months  = 0;
                    int weeks   = 0;
                    int days    = 0;

                    for (int i = 0; i < tsAge.Days; i++)
                    {
                        days++;
                        if (days > 7)
                        {
                            weeks++;
                            days = 0;
                        }
                        if (weeks > 3)
                        {
                            months++;
                            weeks = 0;
                        }
                    }

                    if (months > 0)
                        age = age + months + "mo ";
                    if (weeks > 0)
                        age = age + weeks + "w ";
                    if (days > 0)
                        age = age + days + "d ";

                }
            }

            if (tsAge.ToString("%h") != "0")
                age = age + tsAge.ToString("%h") + "h ";

            if (tsAge.ToString("%m") != "0")
                age = age + tsAge.ToString("%m") + "m ";

            if (tsAge.ToString("%s") != "0")
                age = age + tsAge.ToString("%s") + "s";


            var ageEvent = new GameEvent.Events.GameEventQueryAgeResponse(Session, "", age);

            Session.Network.EnqueueSend(ageEvent);
        }
    }
}

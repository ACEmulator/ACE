using ACE.Common.Extensions;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameAction;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public partial class Player
    {
        [GameAction(GameActionType.TrainSkill)]
        private void TrainSkillAction(ClientMessage message)
        {
            var skill = (Skill)message.Payload.ReadUInt32();
            var creditsSpent = message.Payload.ReadUInt32();
            // train skills
            TrainSkill(skill, creditsSpent);
        }

        [GameAction(GameActionType.RaiseAbility)]
        private void RaiseAbilityAction(ClientMessage message)
        {
            Entity.Enum.Ability ability = Entity.Enum.Ability.None;
            var networkAbility = (Network.Enum.Ability)message.Payload.ReadUInt32();
            switch (networkAbility)
            {
                case Network.Enum.Ability.Strength:
                    ability = Entity.Enum.Ability.Strength;
                    break;
                case Network.Enum.Ability.Endurance:
                    ability = Entity.Enum.Ability.Endurance;
                    break;
                case Network.Enum.Ability.Coordination:
                    ability = Entity.Enum.Ability.Coordination;
                    break;
                case Network.Enum.Ability.Quickness:
                    ability = Entity.Enum.Ability.Quickness;
                    break;
                case Network.Enum.Ability.Focus:
                    ability = Entity.Enum.Ability.Focus;
                    break;
                case Network.Enum.Ability.Self:
                    ability = Entity.Enum.Ability.Self;
                    break;
                case Network.Enum.Ability.Undefined:
                    return;
            }
            var xpSpent = message.Payload.ReadUInt32();
            SpendXp(ability, xpSpent);
        }

        [GameAction(GameActionType.RaiseSkill)]
        private void RaiseSkillAction(ClientMessage message)
        {
            var skill = (Skill)message.Payload.ReadUInt32();
            var xpSpent = message.Payload.ReadUInt32();
            SpendXp(skill, xpSpent);
        }

        [GameAction(GameActionType.RaiseVital)]
        private void RaiseVitalAction(ClientMessage message)
        {
            var vital = (Vital)message.Payload.ReadUInt32();
            var xpSpent = message.Payload.ReadUInt32();
            Entity.Enum.Ability ability;

            switch (vital)
            {
                case Vital.MaxHealth:
                    ability = Entity.Enum.Ability.Health;
                    break;
                case Vital.MaxStamina:
                    ability = Entity.Enum.Ability.Stamina;
                    break;
                case Vital.MaxMana:
                    ability = Entity.Enum.Ability.Mana;
                    break;
                default:
                    ChatPacket.SendServerMessage(Session, $"Unable to Handle GameActionRaiseVital for vital {vital}", ChatMessageType.Broadcast);
                    return;
            }

            SpendXp(ability, xpSpent);
        }

        [GameAction(GameActionType.TitleSet)]
        private void TitleSetAction(ClientMessage message)
        {
            var title = message.Payload.ReadUInt32();
            SetTitle(title);
        }

        [GameAction(GameActionType.QueryBirth)]
        private void QueryBirthAction(ClientMessage message)
        {
            var target = message.Payload.ReadString16L();
            DateTime playerDOB = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            playerDOB = playerDOB.AddSeconds(PropertiesInt[Entity.Enum.Properties.PropertyInt.CreationTimestamp]).ToUniversalTime();

            var dobEvent = new GameMessageSystemChat($"You were born on {playerDOB.ToString("G")}.", ChatMessageType.Broadcast);

            Session.EnqueueSend(dobEvent);
        }

        [GameAction(GameActionType.QueryAge)]
        private void QueryAgeAction(ClientMessage message)
        {
            var target = message.Payload.ReadString16L();
            DateTime playerDOB = new DateTime();
            playerDOB = playerDOB.AddSeconds(PropertiesInt[Entity.Enum.Properties.PropertyInt.Age]);
            TimeSpan tsAge = playerDOB - new DateTime();

            string age = "";

            if (tsAge.ToString("%d") != "0")
            {
                if (Convert.ToInt16(tsAge.ToString("%d")) > 0 && Convert.ToInt16(tsAge.ToString("%d")) <= 7)
                    age = age + tsAge.ToString("%d") + "d ";
                if (Convert.ToInt16(tsAge.ToString("%d")) > 7)
                {
                    int months = 0;
                    int weeks = 0;
                    int days = 0;

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

            var ageEvent = new GameEventQueryAgeResponse(Session, "", age);

            Session.EnqueueSend(ageEvent);
        }
    }
}

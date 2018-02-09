using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Entity
{
    public class Food : WorldObject
    {
        public Food(AceObject aceObject)
            : base(aceObject)
        {
            Food = true;
            Attackable = true;
            
            SetObjectDescriptionBools();

            StackSize = (base.StackSize ?? 1);

            if (StackSize == null)
                StackSize = 1;

            Boost = (base.Boost ?? 0);

            if (BoostEnum == null)
                BoostEnum = 0;
        }

        public override void OnUse(Session session)
        {
            global::ACE.Server.Entity.Player.ConsumableBuffType buffType;

            if (Food)
            {
                switch (BoostEnum)
                {
                    case (int)global::ACE.Server.Entity.Player.ConsumableBuffType.Health:
                        buffType = global::ACE.Server.Entity.Player.ConsumableBuffType.Health;
                        break;
                    case (int)global::ACE.Server.Entity.Player.ConsumableBuffType.Mana:
                        buffType = global::ACE.Server.Entity.Player.ConsumableBuffType.Mana;
                        break;
                    default:
                        buffType = global::ACE.Server.Entity.Player.ConsumableBuffType.Stamina;
                        break;
                }
            }
            else
                buffType = global::ACE.Server.Entity.Player.ConsumableBuffType.Spell;

            session.Player.ApplyComsumable(Name, SolidOrLiquid(), buffType, (uint)Boost, SpellDID);

            session.Player.HandleActionRemoveItemFromInventory(Guid.Full, session.Player.Guid.Full, 1);

            var sendUseDoneEvent = new GameEventUseDone(session);
            session.Network.EnqueueSend(sendUseDoneEvent);
        }

        private Sound SolidOrLiquid()
        {
            if ((Name.ToLower().Contains("ale")) || (Name.ToLower().Contains("lager"))
                || (Name.ToLower().Contains("water")) || (Name.ToLower().Contains("milk"))
                || (Name.ToLower().Contains("potion")) || (Name.ToLower().Contains("elixir"))
                || (Name.ToLower().Contains("mead")) || (Name.ToLower().Contains("draught"))
                || (Name.ToLower().Contains("infusion")) || (Name.ToLower().Contains("tonic"))
                || (Name.ToLower().Contains("tincture")) || (Name.ToLower().Contains("philtre"))
                || (Name.ToLower().Contains("tincture")) || (Name.ToLower().Contains("philtre"))
                || (Name.ToLower().Contains("acidic rejuvenation")) || (Name.ToLower().Contains("tea"))
                || (Name.ToLower().Contains("saliva invigorator")))
                return Sound.Drink1;
            else
                return Sound.Eat1;
        }
    }
}

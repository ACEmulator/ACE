using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.Entity.WorldObjects
{
    public class Food : WorldObject
    {
        public Food(Weenie weenie) : base(weenie)
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
            Player.ConsumableBuffType buffType;

            if (Food)
            {
                switch (BoostEnum)
                {
                    case (int)WorldObjects.Player.ConsumableBuffType.Health:
                        buffType = WorldObjects.Player.ConsumableBuffType.Health;
                        break;
                    case (int)WorldObjects.Player.ConsumableBuffType.Mana:
                        buffType = WorldObjects.Player.ConsumableBuffType.Mana;
                        break;
                    default:
                        buffType = WorldObjects.Player.ConsumableBuffType.Stamina;
                        break;
                }
            }
            else
                buffType = WorldObjects.Player.ConsumableBuffType.Spell;

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
            return Sound.Eat1;
        }
    }
}

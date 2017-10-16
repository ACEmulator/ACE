using ACE.Database;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;

namespace ACE.Entity
{
    public class Food : WorldObject
    {
        public uint? SpellId
        {
            get { return AceObject.SpellDID ?? null; }
            set { AceObject.SpellDID = value; }
        }

        public uint? BoostEnum
        {
            get { return AceObject.BoostEnum ?? 0; }
            set { AceObject.BoostEnum = value; }
        }

        public Food(AceObject aceObject)
            : base(aceObject)
        {
            StackSize = (base.StackSize ?? 1);

            if (StackSize == null)
                StackSize = 1;

            Boost = (base.Boost ?? 0);

            if (BoostEnum == null)
                BoostEnum = 0;
        }

        public override void OnUse(Session session)
        {
            ushort origStackSize = (ushort)StackSize;

            Entity.Player.ConsumableBuffType buffType;

            if (Food == true)
            {
                switch (BoostEnum)
                {
                    case (uint)Entity.Player.ConsumableBuffType.Health:
                        buffType = Entity.Player.ConsumableBuffType.Health;
                        break;
                    case (uint)Entity.Player.ConsumableBuffType.Mana:
                        buffType = Entity.Player.ConsumableBuffType.Mana;
                        break;
                    default:
                        buffType = Entity.Player.ConsumableBuffType.Stamina;
                        break;
                }
            }
            else
                buffType = Entity.Player.ConsumableBuffType.Spell;

            session.Player.DoEatOrDrink(Name, FoodOrDrink(Name), buffType, (uint)Boost, SpellId);

            session.Player.HandleActionRemoveItemFromInventory(Guid.Full, session.Player.Guid.Full, 1);

            var sendUseDoneEvent = new GameEventUseDone(session);
            session.Network.EnqueueSend(sendUseDoneEvent);
        }

        private Sound FoodOrDrink(string name)
        {
            if ((name.ToLower().Contains("ale")) || (name.ToLower().Contains("lager")) || (name.ToLower().Contains("water")) || (name.ToLower().Contains("milk")))
                return Sound.Drink1;
            else
                return Sound.Eat1;
        }
    }
}

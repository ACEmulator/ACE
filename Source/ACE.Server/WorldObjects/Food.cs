using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;

namespace ACE.Server.WorldObjects
{
    public class Food : Stackable
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Food(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Food(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            // TODO we shouldn't be auto setting properties that come from our weenie by default

            BaseDescriptionFlags |= ObjectDescriptionFlag.Food;

            StackSize = StackSize ?? 1;
            Boost = Boost ?? 0;
            BoostEnum = BoostEnum ?? 0;
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item should be in the players possession.
        /// </summary>
        public override void ActOnUse(WorldObject activator)
        {
            var player = activator as Player;
            if (player == null) return;

            var buffType = Player.ConsumableBuffType.Stamina;

            if (BoostEnum != null)
            {
                switch (BoostEnum)
                {
                    case (int)Player.ConsumableBuffType.Health:
                        buffType = Player.ConsumableBuffType.Health;
                        break;
                    case (int)Player.ConsumableBuffType.Mana:
                        buffType = Player.ConsumableBuffType.Mana;
                        break;
                    case (int)Player.ConsumableBuffType.Spell:
                        buffType = Player.ConsumableBuffType.Spell;
                        break;
                }
            }
            player.ApplyConsumable(Name, GetSoundDid(), buffType, (uint)Boost, SpellDID);

            player.TryConsumeFromInventoryWithNetworking(this, 1);
        }

        private Sound GetSoundDid()
        {
            if (GetProperty(PropertyDataId.UseSound).HasValue)
                return (Sound)GetProperty(PropertyDataId.UseSound);

            return Sound.Eat1;
        }

        //private Sound SolidOrLiquid()
        //{
        //    if ((Name.ToLower().Contains("ale")) || (Name.ToLower().Contains("lager"))
        //        || (Name.ToLower().Contains("water")) || (Name.ToLower().Contains("milk"))
        //        || (Name.ToLower().Contains("potion")) || (Name.ToLower().Contains("elixir"))
        //        || (Name.ToLower().Contains("mead")) || (Name.ToLower().Contains("draught"))
        //        || (Name.ToLower().Contains("infusion")) || (Name.ToLower().Contains("tonic"))
        //        || (Name.ToLower().Contains("tincture")) || (Name.ToLower().Contains("philtre"))
        //        || (Name.ToLower().Contains("tincture")) || (Name.ToLower().Contains("philtre"))
        //        || (Name.ToLower().Contains("acidic rejuvenation")) || (Name.ToLower().Contains("tea"))
        //        || (Name.ToLower().Contains("saliva invigorator")))
        //        return Sound.Drink1;
        //    return Sound.Eat1;
        //}
    }
}

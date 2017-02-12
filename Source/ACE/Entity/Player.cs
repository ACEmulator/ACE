using ACE.Database;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameEvent;
using System.Collections.ObjectModel;

namespace ACE.Entity
{
    public class Player : WorldObject
    {
        public Session Session { get; }
        public bool InWorld { get; set; }

        public uint PortalIndex { get; set; } = 1u; // amount of times this character has left a portal this session
        
        private Character character;

        public bool IsAdmin
        {
            get { return character.IsAdmin; }
            set { character.IsAdmin = value; }
        }

        public bool IsEnvoy
        {
            get { return character.IsEnvoy; }
            set { character.IsEnvoy = value; }
        }

        public bool IsArch
        {
            get { return character.IsArch; }
            set { character.IsArch = value; }
        }

        public bool IsPsr
        {
            get { return character.IsPsr; }
            set { character.IsPsr = value; }
        }

        public ReadOnlyDictionary<PropertyBool, bool> PropertiesBool
        {
            get { return character.PropertiesBool; }
        }

        public ReadOnlyDictionary<PropertyDouble, double> PropertiesDouble
        {
            get { return character.PropertiesDouble; }
        }

        public ReadOnlyDictionary<PropertyInt, uint> PropertiesInt
        {
            get { return character.PropertiesInt; }
        }

        public ReadOnlyDictionary<PropertyInt64, ulong> PropertiesInt64
        {
            get { return character.PropertiesInt64; }
        }

        public ReadOnlyDictionary<PropertyString, string> PropertiesString
        {
            get { return character.PropertiesString; }
        }

        public ReadOnlyDictionary<Skill, CharacterSkill> Skills
        {
            get { return new ReadOnlyDictionary<Skill, CharacterSkill>(character.Skills); }
        }

        /// <summary>
        /// Accessor for strength.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Strength
        {
            get { return character.Strength; }
        }

        /// <summary>
        /// Accessor for Endurance.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Endurance
        {
            get { return character.Endurance; }
        }

        /// <summary>
        /// Accessor for Coordination.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Coordination
        {
            get { return character.Coordination; }
        }

        /// <summary>
        /// Accessor for Quickness.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Quickness
        {
            get { return character.Quickness; }
        }

        /// <summary>
        /// Accessor for Focus.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Focus
        {
            get { return character.Focus; }
        }

        /// <summary>
        /// Accessor for Self.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Self
        {
            get { return character.Self; }
        }

        /// <summary>
        /// Accessor for Health.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Health
        {
            get { return character.Health; }
        }

        /// <summary>
        /// Accessor for Stamina.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Stamina
        {
            get { return character.Stamina; }
        }

        /// <summary>
        /// Accessor for Mana.  Should be made into a readonly version if possible
        /// </summary>
        public CharacterAbility Mana
        {
            get { return character.Mana; }
        }

        public uint TotalLogins
        {
            get { return character.TotalLogins; }
            set { character.TotalLogins = value; }
        }

        public Player(Session session) : base(ObjectType.Creature, session.CharacterRequested.Guid)
        {
            Session           = session;
            DescriptionFlags |= ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Player | ObjectDescriptionFlag.Attackable;
            Name              = session.CharacterRequested.Name;

            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide, false);
        }
        
        public async void Load()
        {
            character = await DatabaseManager.Character.LoadCharacter(Guid.Low);
            Position  = character.Position;
            
            SendSelf();
        }

        public void GrantXp(ulong amount)
        {
            character.GrantXp(amount);
            var xpAvailUpdate = new GameEventPrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
            var xpTotalUpdate = new GameEventPrivateUpdatePropertyInt64(Session, PropertyInt64.TotalExperience, character.TotalExperience);

            xpAvailUpdate.Send();
            xpTotalUpdate.Send();
            ChatPacket.SendSystemMessage(Session, $"{amount} experience granted.");
        }

        public void SpendXp(Enum.Ability ability, uint amount)
        {
            uint baseValue = character.Abilities[ability].Base;
            uint result = character.Abilities[ability].SpendXp(amount);
            bool isSecondary = (ability == Enum.Ability.Health || ability == Enum.Ability.Stamina || ability == Enum.Ability.Mana);
            if (result > 0u)
            {
                uint ranks = character.Abilities[ability].Ranks;
                uint newValue = character.Abilities[ability].UnbuffedValue;
                var xpUpdate = new GameEventPrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
                GameEventPacket abilityUpdate;
                if (!isSecondary)
                {
                    abilityUpdate = new GameEventPrivateUpdateAbility(Session, ability, ranks, baseValue, result);
                }
                else
                {
                    abilityUpdate = new GameEventPrivateUpdateVital(Session, ability, ranks, baseValue, result, character.Abilities[ability].Current);
                }

                var soundEvent = new GameEventSound(Session, Network.Enum.Sound.AbilityIncrease, 1f);

                xpUpdate.Send();
                abilityUpdate.Send();
                soundEvent.Send();
                ChatPacket.SendSystemMessage(Session, $"Your base {ability} is now {newValue}!");
            }
            else
            {
                ChatPacket.SendSystemMessage(Session, $"Your attempt to raise {ability} has failed.");
            }
        }

        public void SpendXp(Skill skill, uint amount)
        {
            uint baseValue = 0;
            uint result = character.Skills[skill].SpendXp(amount);
            if (result > 0u)
            {
                uint ranks = character.Skills[skill].Ranks;
                uint newValue = character.Skills[skill].UnbuffedValue;
                var status = character.Skills[skill].Status;
                var xpUpdate = new GameEventPrivateUpdatePropertyInt64(Session, PropertyInt64.AvailableExperience, character.AvailableExperience);
                var ablityUpdate = new GameEventPrivateUpdateSkill(Session, skill, status, ranks, baseValue, result);
                var soundEvent = new GameEventSound(Session, Network.Enum.Sound.AbilityIncrease, 1f);

                xpUpdate.Send();
                ablityUpdate.Send();
                soundEvent.Send();
                ChatPacket.SendSystemMessage(Session, $"Your base {skill} is now {newValue}!");
            }
            else
            {
                ChatPacket.SendSystemMessage(Session, $"Your attempt to raise {skill} has failed.");
            }
        }
        
        private void SendSelf()
        {
            NetworkManager.SendPacket(ConnectionType.World, BuildObjectCreate(), Session);

            var playerCreate         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            var playerCreateFragment = new ServerPacketFragment(0x0A, FragmentOpcode.PlayerCreate);
            playerCreateFragment.Payload.WriteGuid(Guid);
            playerCreate.Fragments.Add(playerCreateFragment);

            NetworkManager.SendPacket(ConnectionType.World, playerCreate, Session);

            // TODO: gear and equip

            new GameEventPlayerDescription(Session).Send();
            new GameEventCharacterTitle(Session).Send();
        }
        
        public void SetPhysicsState(PhysicsState state, bool packet = true)
        {
            PhysicsState = state;

            if (packet)
            {
                var setState         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
                var setStateFragment = new ServerPacketFragment(0x0A, FragmentOpcode.SetState);
                setStateFragment.Payload.WriteGuid(Guid);
                setStateFragment.Payload.Write((uint)state);
                setStateFragment.Payload.Write((ushort)character.TotalLogins);
                setStateFragment.Payload.Write((ushort)++PortalIndex);
                setState.Fragments.Add(setStateFragment);

                // TODO: this should be broadcast
                NetworkManager.SendPacket(ConnectionType.World, setState, Session);
            }
        }

        public void Teleport(Position newPosition)
        {
            if (!InWorld)
                return;

            InWorld = false;
            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide);

            var playerTeleport         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            var playerTeleportFragment = new ServerPacketFragment(0x0A, FragmentOpcode.PlayerTeleport);
            playerTeleportFragment.Payload.Write(++TeleportIndex);
            playerTeleportFragment.Payload.Write(0u);
            playerTeleportFragment.Payload.Write(0u);
            playerTeleportFragment.Payload.Write((ushort)0);
            playerTeleport.Fragments.Add(playerTeleportFragment);

            NetworkManager.SendPacket(ConnectionType.World, playerTeleport, Session);

            // must be sent after the teleport packet
            UpdatePosition(newPosition);
        }
    }
}

using ACE.Database;
using ACE.Managers;
using ACE.Network;
using ACE.Network.GameEvent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace ACE.Entity
{
    public class Player : WorldObject
    {
        public Session Session { get; }
        public bool InWorld { get; set; }

        public uint LoginIndex { get; set; } = 1u;  // total amount of times the player has logged into this character
        public uint PortalIndex { get; set; } = 1u; // amount of times this character has left a portal this session

        public ReadOnlyDictionary<PropertyBool, bool> PropertiesBool => new ReadOnlyDictionary<PropertyBool, bool>(propertiesBool);
        public ReadOnlyDictionary<PropertyDouble, double> PropertiesDouble => new ReadOnlyDictionary<PropertyDouble, double>(propertiesDouble);
        public ReadOnlyDictionary<PropertyInt, uint> PropertiesInt => new ReadOnlyDictionary<PropertyInt, uint>(propertiesInt);
        public ReadOnlyDictionary<PropertyInt64, ulong> PropertiesInt64 => new ReadOnlyDictionary<PropertyInt64, ulong>(propertiesInt64);
        public ReadOnlyDictionary<PropertyString, string> PropertiesString => new ReadOnlyDictionary<PropertyString, string>(propertiesString);

        private Dictionary<PropertyBool, bool> propertiesBool = new Dictionary<PropertyBool, bool>();
        private Dictionary<PropertyDouble, double> propertiesDouble = new Dictionary<PropertyDouble, double>();
        private Dictionary<PropertyInt, uint> propertiesInt = new Dictionary<PropertyInt, uint>();
        private Dictionary<PropertyInt64, ulong> propertiesInt64 = new Dictionary<PropertyInt64, ulong>();
        private Dictionary<PropertyString, string> propertiesString = new Dictionary<PropertyString, string>();

        private Character character;

        public Player(Session session) : base(ObjectType.Creature, session.CharacterRequested.LowGuid, GuidType.Player)
        {
            Session           = session;
            DescriptionFlags |= ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Player | ObjectDescriptionFlag.Attackable;
            Name              = session.CharacterRequested.Name;

            SetPhysicsState(PhysicsState.IgnoreCollision | PhysicsState.Gravity | PhysicsState.Hidden | PhysicsState.EdgeSlide, false);
        }

        public Character Character { get { return character; } }

        public async void Load()
        {
            character = await DatabaseManager.Character.LoadCharacter(Guid.GetLow());
            Position = character.Position;

            // need to load the rest of the player information from DB in this async method, this is just temporary and is what is sent by the retail server
            SetPropertyInt(PropertyInt.Unknown384, 0);
            SetPropertyInt(PropertyInt.Unknown386, 0);
            SetPropertyInt(PropertyInt.Unknown387, 0);
            SetPropertyInt(PropertyInt.HealingBoostRating, 0);
            SetPropertyInt(PropertyInt.Unknown388, 0);
            SetPropertyInt(PropertyInt.Unknown389, 0);
            SetPropertyInt(PropertyInt.EncumbVal, 0);
            SetPropertyInt(PropertyInt.Unknown390, 0);
            SetPropertyInt(PropertyInt.AugmentationJackOfAllTrades, 1);
            SetPropertyInt(PropertyInt.PlayerKillerStatus, 2);
            SetPropertyInt(PropertyInt.ContainersCapacity, 20);
            SetPropertyInt(PropertyInt.WeaknessRating, 0);
            SetPropertyInt(PropertyInt.NetherOverTime, 0);
            SetPropertyInt(PropertyInt.NetherResistRating, 0);
            SetPropertyInt(PropertyInt.LumAugDamageRating, 0);
            SetPropertyInt(PropertyInt.LumAugDamageReductionRating, 0);
            SetPropertyInt(PropertyInt.LumAugCritDamageRating, 0);
            SetPropertyInt(PropertyInt.LumAugCritReductionRating, 0);
            SetPropertyInt(PropertyInt.LumAugSurgeChanceRating, 0);
            SetPropertyInt(PropertyInt.LumAugItemManaUsage, 0);
            SetPropertyInt(PropertyInt.LumAugItemManaGain, 0);
            SetPropertyInt(PropertyInt.CoinValue, 10000);
            SetPropertyInt(PropertyInt.LumAugHealingRating, 0);
            SetPropertyInt(PropertyInt.LumAugSkilledCraft, 0);
            SetPropertyInt(PropertyInt.LumAugSkilledSpec, 0);
            SetPropertyInt(PropertyInt.Level, 1);
            SetPropertyInt(PropertyInt.DotResistRating, 0);
            SetPropertyInt(PropertyInt.AllegianceRank, 0);
            SetPropertyInt(PropertyInt.LifeResistRating, 0);
            SetPropertyInt(PropertyInt.MeleeMastery, 1);
            SetPropertyInt(PropertyInt.CreationTimestamp, (uint)WorldManager.GetUnixTime());
            SetPropertyInt(PropertyInt.RangedMastery, 8);
            SetPropertyInt(PropertyInt.WeaponAuraDamage, 0);
            SetPropertyInt(PropertyInt.WeaponAuraSpeed, 0);
            SetPropertyInt(PropertyInt.LumAugAllSkills, 0);
            SetPropertyInt(PropertyInt.Gender, 1);
            SetPropertyInt(PropertyInt.GearDamage, 0);
            SetPropertyInt(PropertyInt.GearDamageResist, 0);
            SetPropertyInt(PropertyInt.DamageRating, 0);
            SetPropertyInt(PropertyInt.GearCrit, 0);
            SetPropertyInt(PropertyInt.DamageResistRating, 0);
            SetPropertyInt(PropertyInt.GearCritResist, 0);
            SetPropertyInt(PropertyInt.GearCritDamage, 0);
            SetPropertyInt(PropertyInt.GearCritDamageResist, 0);
            SetPropertyInt(PropertyInt.GearHealingBoost, 0);
            SetPropertyInt(PropertyInt.HealOverTime, 0);
            SetPropertyInt(PropertyInt.GearNetherResist, 0);
            SetPropertyInt(PropertyInt.CritRating, 0);
            SetPropertyInt(PropertyInt.GearLifeResist, 0);
            SetPropertyInt(PropertyInt.CritDamageRating, 0);
            SetPropertyInt(PropertyInt.GearMaxHealth, 0);
            SetPropertyInt(PropertyInt.CritResistRating, 0);
            SetPropertyInt(PropertyInt.CritDamageResistRating, 0);
            SetPropertyInt(PropertyInt.HeritageGroup, 3);
            SetPropertyInt(PropertyInt.Unknown381, 0);
            SetPropertyInt(PropertyInt.HealingResistRating, 0);
            SetPropertyInt(PropertyInt.Age, 0);
            SetPropertyInt(PropertyInt.Unknown382, 0);
            SetPropertyInt(PropertyInt.DamageOverTime, 0);
            SetPropertyInt(PropertyInt.Unknown383, 0);

            SetPropertyInt64(PropertyInt64.AvailableLuminance, 0);
            SetPropertyInt64(PropertyInt64.MaximumLuminance, 0);

            SetPropertyBool(PropertyBool.IsPsr, false);
            SetPropertyBool(PropertyBool.ActdReceivedItems, true);
            SetPropertyBool(PropertyBool.IsAdmin, character.IsAdmin);
            SetPropertyBool(PropertyBool.IsArch, false);
            SetPropertyBool(PropertyBool.IsSentinel, false);
            SetPropertyBool(PropertyBool.IsAdvocate, false);
            SetPropertyBool(PropertyBool.Account15Days, true);

            SetPropertyDouble(PropertyDouble.GlobalXpMod, 1);
            SetPropertyDouble(PropertyDouble.WeaponAuraOffense, 0);
            SetPropertyDouble(PropertyDouble.WeaponAuraDefense, 0);
            SetPropertyDouble(PropertyDouble.WeaponAuraElemental, 0);
            SetPropertyDouble(PropertyDouble.WeaponAuraManaConv, 1);
            SetPropertyDouble(PropertyDouble.ResistHealthDrain, 1);

            SetPropertyString(PropertyString.Name, Name);

            SendSelf();
        }

        private void SendSelf()
        {
            NetworkManager.SendPacket(ConnectionType.World, BuildObjectCreate(), Session);

            var playerCreate         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
            var playerCreateFragment = new ServerPacketFragment(0x0A, FragmentOpcode.PlayerCreate);
            playerCreateFragment.Payload.Write(Guid.Full);
            playerCreate.Fragments.Add(playerCreateFragment);

            NetworkManager.SendPacket(ConnectionType.World, playerCreate, Session);

            // TODO: gear and equip

            new GameEventPlayerDescription(Session).Send();
            new GameEventCharacterTitle(Session).Send();
        }

        // TODO: 4 packets, 2 for update (public and private) and 2 for removal for each property type
        public void SetPropertyBool(PropertyBool property, bool value, bool packet = false)
        {
            Debug.Assert(property < PropertyBool.Count);
            propertiesBool[property] = value;
        }

        public void SetPropertyDouble(PropertyDouble property, double value, bool packet = false)
        {
            Debug.Assert(property < PropertyDouble.Count);
            propertiesDouble[property] = value;
        }

        public void SetPropertyInt(PropertyInt property, uint value, bool packet = false)
        {
            Debug.Assert(property < PropertyInt.Count);
            propertiesInt[property] = value;
        }

        public void SetPropertyInt64(PropertyInt64 property, ulong value, bool packet = false)
        {
            Debug.Assert(property < PropertyInt64.Count);
            propertiesInt64[property] = value;
        }

        public void SetPropertyString(PropertyString property, string value, bool packet = false)
        {
            Debug.Assert(property < PropertyString.Count);
            propertiesString[property] = value;
        }

        public void SetPhysicsState(PhysicsState state, bool packet = true)
        {
            PhysicsState = state;

            if (packet)
            {
                var setState         = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum);
                var setStateFragment = new ServerPacketFragment(0x0A, FragmentOpcode.SetState);
                setStateFragment.Payload.Write(Guid.Full);
                setStateFragment.Payload.Write((uint)state);
                setStateFragment.Payload.Write((ushort)LoginIndex);
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

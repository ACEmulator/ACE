using System;
using System.Collections.Generic;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    /// <summary>
    /// Pet Devices are the essences used to summon creatures
    /// </summary>
    public class PetDevice : WorldObject
    {
        public int? GearDamage
        {
            get => GetProperty(PropertyInt.GearDamage);
            set { if (value.HasValue) SetProperty(PropertyInt.GearDamage, value.Value); else RemoveProperty(PropertyInt.GearDamage); }
        }

        public int? GearDamageResist
        {
            get => GetProperty(PropertyInt.GearDamageResist);
            set { if (value.HasValue) SetProperty(PropertyInt.GearDamageResist, value.Value); else RemoveProperty(PropertyInt.GearDamageResist); }
        }

        public int? GearCritDamage
        {
            get => GetProperty(PropertyInt.GearCritDamage);
            set { if (value.HasValue) SetProperty(PropertyInt.GearCritDamage, value.Value); else RemoveProperty(PropertyInt.GearCritDamage); }
        }

        public int? GearCritDamageResist
        {
            get => GetProperty(PropertyInt.GearCritDamageResist);
            set { if (value.HasValue) SetProperty(PropertyInt.GearCritDamageResist, value.Value); else RemoveProperty(PropertyInt.GearCritDamageResist); }
        }

        public int? GearCrit
        {
            get => GetProperty(PropertyInt.GearCrit);
            set { if (value.HasValue) SetProperty(PropertyInt.GearCrit, value.Value); else RemoveProperty(PropertyInt.GearCrit); }
        }

        public int? GearCritResist
        {
            get => GetProperty(PropertyInt.GearCritResist);
            set { if (value.HasValue) SetProperty(PropertyInt.GearCritResist, value.Value); else RemoveProperty(PropertyInt.GearCritResist); }
        }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public PetDevice(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();

            // todo: remove me when the data is fixed
            Structure = MaxStructure;
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public PetDevice(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public override void ActOnUse(WorldObject activator)
        {
            if (!(activator is Player player))
                return;

            // Good PCAP example of using a PetDevice to summon a pet:
            // Asherons-Call-packets-includes-3-towers\pkt_2017-1-30_1485823896_log.pcap lines 27837 - 27843

            if (!PetDeviceToPetMapping.TryGetValue(WeenieClassId, out var petData))
            {
                Console.WriteLine($"PetDevice.UseItem(): couldn't find a matching pet for essence wcid {WeenieClassId}");
                return;
            }

            var wcid = petData.Item1;
            var damageType = petData.Item2;

            if (SummonCreature(player, wcid, damageType))
            {
                // decrease remaining uses
                if (--Structure <= 0)
                    player.TryConsumeFromInventoryWithNetworking(this, 1);

                player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.Structure, Structure.Value));
            }
            else
            {
                // this would be a good place to send a friendly reminder to install the latest summoning updates from ACE-World-Patch
            }
        }

        public override ActivationResult CheckUseRequirements(WorldObject activator)
        {
            if (!(activator is Player player))
                return new ActivationResult(false);

            var baseRequirements = base.CheckUseRequirements(activator);
            if (!baseRequirements.Success)
                return baseRequirements;

            // cooldowns for gems and pet devices, anything else?

            // should this verification be in base CheckUseRequirements?
            if (!player.EnchantmentManager.CheckCooldown(CooldownId))
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "You have used this item too recently"));
                return new ActivationResult(false);
            }

            // TODO: limit non-golems to summoning mastery

            return new ActivationResult(true);
        }

        public bool SummonCreature(Player player, uint wcid, DamageType damageType)
        {
            // since we are instantiating regular creatures instead of actual CombatPet weenies atm,
            // bypassing CreateNewWorldObject() here...
            //var combatPet = WorldObjectFactory.CreateNewWorldObject(wcid) as CombatPet;
            var weenie = DatabaseManager.World.GetCachedWeenie(wcid);
            if (weenie == null)
            {
                Console.WriteLine($"Couldn't find pet wcid #{wcid}");
                return false;
            }
            player.EnchantmentManager.StartCooldown(this);

            var combatPet = new CombatPet(weenie, GuidManager.NewDynamicGuid());
            if (combatPet == null)
            {
                Console.WriteLine($"PetDevice.UseItem(): failed to create pet for wcid {wcid}");
                return false;
            }
            combatPet.Init(player, damageType, this);
            return true;
        }

        /// <summary>
        /// Maps an Essence to a WCID to be spawned
        /// </summary>
        public static Dictionary<uint, Tuple<uint, DamageType>> PetDeviceToPetMapping = new Dictionary<uint, Tuple<uint, DamageType>>()
        {
            // ============================ Geomancer ============================

            // ============ Golems ============

            { 48886, new Tuple<uint, DamageType>(200, DamageType.Bludgeon) }, // mud golem (15)
            { 48890, new Tuple<uint, DamageType>(202, DamageType.Bludgeon) }, // sandstone golem (30)
            { 48878, new Tuple<uint, DamageType>(194, DamageType.Bludgeon) }, // copper golem (50)
            { 48888, new Tuple<uint, DamageType>(14559, DamageType.Bludgeon) }, // oak golem (80)
            { 48882, new Tuple<uint, DamageType>(7096, DamageType.Bludgeon) }, // gold golem (100)
            { 48880, new Tuple<uint, DamageType>(7507, DamageType.Bludgeon) }, // coral golem (125)
            { 48884, new Tuple<uint, DamageType>(197, DamageType.Bludgeon) }, // iron golem (150)

            // ============================ Naturalist ============================

            // ============ Grievvers ============

            { 49366, new Tuple<uint, DamageType>(7978, DamageType.Acid) }, // acid grievver (50)
            { 49367, new Tuple<uint, DamageType>(7980, DamageType.Acid) }, // acid grievver (80)
            { 49368, new Tuple<uint, DamageType>(7981, DamageType.Acid) }, // acid grievver (100)
            { 49369, new Tuple<uint, DamageType>(7982, DamageType.Acid) }, // acid grievver (125)
            { 49370, new Tuple<uint, DamageType>(7983, DamageType.Acid) }, // acid grievver (150)
            { 49371, new Tuple<uint, DamageType>(8538, DamageType.Acid) }, // acid grievver (180)
            { 49372, new Tuple<uint, DamageType>(30756, DamageType.Acid) }, // caustic grievver (200)

            { 49380, new Tuple<uint, DamageType>(7978, DamageType.Fire) }, // fire grievver (50)
            { 49381, new Tuple<uint, DamageType>(7980, DamageType.Fire) }, // fire grievver (80)
            { 49382, new Tuple<uint, DamageType>(7981, DamageType.Fire) }, // fire grievver (100)
            { 49383, new Tuple<uint, DamageType>(7982, DamageType.Fire) }, // fire grievver (125)
            { 49384, new Tuple<uint, DamageType>(7983, DamageType.Fire) }, // fire grievver (150)
            { 49385, new Tuple<uint, DamageType>(8538, DamageType.Fire) }, // fire grievver (180)
            { 49386, new Tuple<uint, DamageType>(30756, DamageType.Fire) }, // scorched grievver (200)

            { 49387, new Tuple<uint, DamageType>(7978, DamageType.Cold) }, // frost grievver (50)
            { 49388, new Tuple<uint, DamageType>(7980, DamageType.Cold) }, // frost grievver (80)
            { 49389, new Tuple<uint, DamageType>(7981, DamageType.Cold) }, // frost grievver (100)
            { 49390, new Tuple<uint, DamageType>(7982, DamageType.Cold) }, // frost grievver (125)
            { 49391, new Tuple<uint, DamageType>(7983, DamageType.Cold) }, // frost grievver (150)
            { 49392, new Tuple<uint, DamageType>(8538, DamageType.Cold) }, // frost grievver (180)
            { 49365, new Tuple<uint, DamageType>(30756, DamageType.Cold) }, // arctic grievver (200)
            //{ -1, -1 }, // glacial grievver (200) ?

            { 49373, new Tuple<uint, DamageType>(7978, DamageType.Electric) }, // lightning grievver (50)
            { 49374, new Tuple<uint, DamageType>(7980, DamageType.Electric) }, // lightning grievver (80)
            { 49375, new Tuple<uint, DamageType>(7981, DamageType.Electric) }, // lightning grievver (100)
            { 49376, new Tuple<uint, DamageType>(7982, DamageType.Electric) }, // lightning grievver (125)
            { 49377, new Tuple<uint, DamageType>(7983, DamageType.Electric) }, // lightning grievver (150)
            { 49378, new Tuple<uint, DamageType>(8538, DamageType.Electric) }, // lightning grievver (180)
            { 49379, new Tuple<uint, DamageType>(30756, DamageType.Electric) }, // excited grievver (200)

            // ============ Moars ============

            { 49338, new Tuple<uint, DamageType>(22745, DamageType.Acid) }, // acid moar (50)
            { 49339, new Tuple<uint, DamageType>(24311, DamageType.Acid) }, // acid moar (80)
            { 49340, new Tuple<uint, DamageType>(22746, DamageType.Acid) }, // acid moar (100)
            { 49341, new Tuple<uint, DamageType>(22747, DamageType.Acid) }, // acid moar (125)
            { 49342, new Tuple<uint, DamageType>(24134, DamageType.Acid) }, // acid moar (150)
            { 49343, new Tuple<uint, DamageType>(25880, DamageType.Acid) }, // acid moar (180)
            { 49344, new Tuple<uint, DamageType>(25848, DamageType.Acid) }, // blistering moar (200)
            //{ 49344, 49114 },   // blistering moar (200)

            { 49352, new Tuple<uint, DamageType>(22745, DamageType.Fire) }, // fire moar (50)
            { 49353, new Tuple<uint, DamageType>(24311, DamageType.Fire) }, // fire moar (80)
            { 49354, new Tuple<uint, DamageType>(22746, DamageType.Fire) }, // fire moar (100)
            { 49355, new Tuple<uint, DamageType>(22747, DamageType.Fire) }, // fire moar (125)
            { 49356, new Tuple<uint, DamageType>(24134, DamageType.Fire) }, // fire moar (150)
            { 49357, new Tuple<uint, DamageType>(25880, DamageType.Fire) }, // fire moar (180)
            { 49358, new Tuple<uint, DamageType>(25848, DamageType.Fire) }, // volcanic moar (200)

            { 49359, new Tuple<uint, DamageType>(22745, DamageType.Cold) }, // frost moar (50)
            { 49360, new Tuple<uint, DamageType>(24311, DamageType.Cold) }, // frost moar (80)
            { 49361, new Tuple<uint, DamageType>(22746, DamageType.Cold) }, // frost moar (100)
            { 49362, new Tuple<uint, DamageType>(22747, DamageType.Cold) }, // frost moar (125)
            { 49363, new Tuple<uint, DamageType>(24134, DamageType.Cold) }, // frost moar (150)
            { 49364, new Tuple<uint, DamageType>(25880, DamageType.Cold) }, // frost moar (180)
            { 49337, new Tuple<uint, DamageType>(25848, DamageType.Cold) }, // freezing moar (200)

            { 49345, new Tuple<uint, DamageType>(22745, DamageType.Electric) }, // lightning moar (50)
            { 49346, new Tuple<uint, DamageType>(24311, DamageType.Electric) }, // lightning moar (80)
            { 49347, new Tuple<uint, DamageType>(22746, DamageType.Electric) }, // lightning moar (100)
            { 49348, new Tuple<uint, DamageType>(22747, DamageType.Electric) }, // lightning moar (125)
            { 49349, new Tuple<uint, DamageType>(24134, DamageType.Electric) }, // lightning moar (150)
            { 49350, new Tuple<uint, DamageType>(25880, DamageType.Electric) }, // lightning moar (180)
            { 49351, new Tuple<uint, DamageType>(25848, DamageType.Electric) }, // electrified moar (200)

            // ============ Phyntos Wasps ============

            { 49524, new Tuple<uint, DamageType>(217, DamageType.Acid) }, // acid phyntos wasp (50)
            { 49525, new Tuple<uint, DamageType>(28248, DamageType.Acid) }, // acid phyntos wasp (80)
            { 49526, new Tuple<uint, DamageType>(30904, DamageType.Acid) }, // acid phyntos wasp (100)
            { 49527, new Tuple<uint, DamageType>(28253, DamageType.Acid) }, // acid phyntos wasp (125)
            { 49528, new Tuple<uint, DamageType>(30757, DamageType.Acid) }, // acid phyntos wasp (150)
            { 49529, new Tuple<uint, DamageType>(28051, DamageType.Acid) }, // acid phyntos wasp (180)
            { 49530, new Tuple<uint, DamageType>(28052, DamageType.Acid) }, // acid phyntos swarm (200)

            { 49531, new Tuple<uint, DamageType>(217, DamageType.Fire) }, // fire phyntos wasp (50)
            { 49532, new Tuple<uint, DamageType>(28248, DamageType.Fire) }, // fire phyntos wasp (80)
            { 49533, new Tuple<uint, DamageType>(30904, DamageType.Fire) }, // fire phyntos wasp (100)
            { 49534, new Tuple<uint, DamageType>(28253, DamageType.Fire) }, // fire phyntos wasp (125)
            { 49535, new Tuple<uint, DamageType>(30757, DamageType.Fire) }, // fire phyntos wasp (150)
            { 49536, new Tuple<uint, DamageType>(28051, DamageType.Fire) }, // fire phyntos wasp (180)
            { 49537, new Tuple<uint, DamageType>(28052, DamageType.Fire) }, // fire phyntos swarm (200)

            { 49538, new Tuple<uint, DamageType>(217, DamageType.Cold) }, // frost phyntos wasp (50)
            { 49539, new Tuple<uint, DamageType>(28248, DamageType.Cold) }, // frost phyntos wasp (80)
            { 49540, new Tuple<uint, DamageType>(30904, DamageType.Cold) }, // frost phyntos wasp (100)
            { 49541, new Tuple<uint, DamageType>(28253, DamageType.Cold) }, // frost phyntos wasp (125)
            { 49542, new Tuple<uint, DamageType>(30757, DamageType.Cold) }, // frost phyntos wasp (150)
            { 49543, new Tuple<uint, DamageType>(28051, DamageType.Cold) }, // frost phyntos wasp (180)
            { 49544, new Tuple<uint, DamageType>(28052, DamageType.Cold) }, // frost phyntos swarm (200)

            { 49545, new Tuple<uint, DamageType>(217, DamageType.Electric) }, // lightning phyntos wasp (50)
            { 49546, new Tuple<uint, DamageType>(28248, DamageType.Electric) }, // lightning phyntos wasp (80)
            { 49547, new Tuple<uint, DamageType>(30904, DamageType.Electric) }, // lightning phyntos wasp (100)
            { 49548, new Tuple<uint, DamageType>(28253, DamageType.Electric) }, // lightning phyntos wasp (125)
            { 49549, new Tuple<uint, DamageType>(30757, DamageType.Electric) }, // lightning phyntos wasp (150)
            { 49550, new Tuple<uint, DamageType>(28051, DamageType.Electric) }, // lightning phyntos wasp (180)
            { 49551, new Tuple<uint, DamageType>(28052, DamageType.Electric) }, // lightning phyntos swarm (200)

            // ============================ Necromancer ============================

            // ============ Skeletons ============

            { 49213, new Tuple<uint, DamageType>(1762, DamageType.Acid) }, // acid skeleton minion (50)
            { 49214, new Tuple<uint, DamageType>(6773, DamageType.Acid) }, // acid skeleton minion (80)
            { 49215, new Tuple<uint, DamageType>(7178, DamageType.Acid) }, // acid skeleton minion (100)
            { 49216, new Tuple<uint, DamageType>(24313, DamageType.Acid) }, // acid skeleton bushi (125)
            { 49217, new Tuple<uint, DamageType>(25804, DamageType.Acid) }, // acid skeleton bushi (150)
            { 49218, new Tuple<uint, DamageType>(8593, DamageType.Acid) }, // acid skeleton bushi (180)
            { 49219, new Tuple<uint, DamageType>(22050, DamageType.Acid) }, // acid skeleton samurai (200)

            { 48942, new Tuple<uint, DamageType>(1762, DamageType.Fire) }, // fire skeleton minion (50)
            { 48943, new Tuple<uint, DamageType>(6773, DamageType.Fire) }, // fire skeleton minion (80)
            { 48944, new Tuple<uint, DamageType>(7178, DamageType.Fire) }, // fire skeleton minion (100)
            { 48945, new Tuple<uint, DamageType>(24313, DamageType.Fire) }, // fire skeleton bushi (125)
            { 48946, new Tuple<uint, DamageType>(25804, DamageType.Fire) }, // fire skeleton bushi (150)
            { 48947, new Tuple<uint, DamageType>(8593, DamageType.Fire) }, // fire skeleton bushi (180)
            { 48956, new Tuple<uint, DamageType>(22050, DamageType.Fire) }, // fire skeleton samurai (200)

            { 49227, new Tuple<uint, DamageType>(1762, DamageType.Cold) }, // frost skeleton minion (50)
            { 49228, new Tuple<uint, DamageType>(6773, DamageType.Cold) }, // frost skeleton minion (80)
            { 49229, new Tuple<uint, DamageType>(7178, DamageType.Cold) }, // frost skeleton minion (100)
            { 49230, new Tuple<uint, DamageType>(24313, DamageType.Cold) }, // frost skeleton bushi (125)
            { 49231, new Tuple<uint, DamageType>(25804, DamageType.Cold) }, // frost skeleton bushi (150)
            { 49232, new Tuple<uint, DamageType>(8593, DamageType.Cold) }, // frost skeleton bushi (180)
            { 49212, new Tuple<uint, DamageType>(22050, DamageType.Cold) }, // frost skeleton samurai (200)

            { 49220, new Tuple<uint, DamageType>(1762, DamageType.Electric) }, // lightning skeleton minion (50)
            { 49221, new Tuple<uint, DamageType>(6773, DamageType.Electric) }, // lightning skeleton minion (80)
            { 49222, new Tuple<uint, DamageType>(7178, DamageType.Electric) }, // lightning skeleton minion (100)
            { 49223, new Tuple<uint, DamageType>(24313, DamageType.Electric) }, // lightning skeleton bushi (125)
            { 49224, new Tuple<uint, DamageType>(25804, DamageType.Electric) }, // lightning skeleton bushi (150)
            { 49225, new Tuple<uint, DamageType>(8593, DamageType.Electric) }, // lightning skeleton bushi (180)
            { 49226, new Tuple<uint, DamageType>(22050, DamageType.Electric) }, // lightning skeleton samurai (200)

            // ============ Spectres ============

            { 49421, new Tuple<uint, DamageType>(8053, DamageType.Acid) }, // acid spectre (50)
            { 49422, new Tuple<uint, DamageType>(1757, DamageType.Acid) }, // acid spectre (80)
            { 49423, new Tuple<uint, DamageType>(30882, DamageType.Acid) }, // acid spectre (100)
            { 49424, new Tuple<uint, DamageType>(23564, DamageType.Acid) }, // acid spectre (125)
            { 49425, new Tuple<uint, DamageType>(6402, DamageType.Acid) }, // acid spectre (150)
            { 49426, new Tuple<uint, DamageType>(6403, DamageType.Acid) }, // acid spectre (180)
            { 49427, new Tuple<uint, DamageType>(8583, DamageType.Acid) }, // acid maiden (200)

            { 49435, new Tuple<uint, DamageType>(8053, DamageType.Fire) }, // fire spectre (50)
            { 49436, new Tuple<uint, DamageType>(1757, DamageType.Fire) }, // fire spectre (80)
            { 49437, new Tuple<uint, DamageType>(30882, DamageType.Fire) }, // fire spectre (100)
            { 49438, new Tuple<uint, DamageType>(23564, DamageType.Fire) }, // fire spectre (125)
            { 49439, new Tuple<uint, DamageType>(6402, DamageType.Fire) }, // fire spectre (150)
            { 49440, new Tuple<uint, DamageType>(6403, DamageType.Fire) }, // fire spectre (180)
            { 49441, new Tuple<uint, DamageType>(8583, DamageType.Fire) }, // fire maiden (200)

            { 49442, new Tuple<uint, DamageType>(8053, DamageType.Cold) }, // frost spectre (50)
            { 49443, new Tuple<uint, DamageType>(1757, DamageType.Cold) }, // frost spectre (80)
            { 49444, new Tuple<uint, DamageType>(30882, DamageType.Cold) }, // frost spectre (100)
            { 49445, new Tuple<uint, DamageType>(23564, DamageType.Cold) }, // frost spectre (125)
            { 49446, new Tuple<uint, DamageType>(6402, DamageType.Cold) }, // frost spectre (150)
            { 49447, new Tuple<uint, DamageType>(6403, DamageType.Cold) }, // frost spectre (180)
            { 49448, new Tuple<uint, DamageType>(8583, DamageType.Cold) }, // frost maiden (200)

            { 49428, new Tuple<uint, DamageType>(8053, DamageType.Electric) }, // lightning spectre (50)
            { 49429, new Tuple<uint, DamageType>(1757, DamageType.Electric) }, // lightning spectre (80)
            { 49430, new Tuple<uint, DamageType>(30882, DamageType.Electric) }, // lightning spectre (100)
            { 49431, new Tuple<uint, DamageType>(23564, DamageType.Electric) }, // lightning spectre (125)
            { 49432, new Tuple<uint, DamageType>(6402, DamageType.Electric) }, // lightning spectre (150)
            { 49433, new Tuple<uint, DamageType>(6403, DamageType.Electric) }, // lightning spectre (180)
            { 49434, new Tuple<uint, DamageType>(8583, DamageType.Electric) }, // lightning maiden (200)

            // ============ Zombies ============

            { 48972, new Tuple<uint, DamageType>(22114, DamageType.Acid) }, // acid zombie (50)
            { 49234, new Tuple<uint, DamageType>(7348, DamageType.Acid) }, // acid zombie (80)
            { 49235, new Tuple<uint, DamageType>(8673, DamageType.Acid) }, // acid zombie (100)
            { 49236, new Tuple<uint, DamageType>(4217, DamageType.Acid) }, // acid zombie (125)
            { 49237, new Tuple<uint, DamageType>(24320, DamageType.Acid) }, // acid zombie (150)
            { 49238, new Tuple<uint, DamageType>(24321, DamageType.Acid) }, // acid zombie (180)
            { 49239, new Tuple<uint, DamageType>(25347, DamageType.Acid) }, // blistered zombie (200)

            { 49247, new Tuple<uint, DamageType>(22114, DamageType.Fire) }, // fire zombie (50)
            { 49248, new Tuple<uint, DamageType>(7348, DamageType.Fire) }, // fire zombie (80)
            { 49249, new Tuple<uint, DamageType>(8673, DamageType.Fire) }, // fire zombie (100)
            { 49250, new Tuple<uint, DamageType>(4217, DamageType.Fire) }, // fire zombie (125)
            { 49251, new Tuple<uint, DamageType>(24320, DamageType.Fire) }, // fire zombie (150)
            { 49252, new Tuple<uint, DamageType>(24321, DamageType.Fire) }, // fire zombie (180)
            { 49253, new Tuple<uint, DamageType>(25347, DamageType.Fire) }, // charred zombie (200)

            { 49254, new Tuple<uint, DamageType>(22114, DamageType.Cold) }, // frost zombie (50)
            { 49255, new Tuple<uint, DamageType>(7348, DamageType.Cold) }, // frost zombie (80)
            { 49256, new Tuple<uint, DamageType>(8673, DamageType.Cold) }, // frost zombie (100)
            { 49257, new Tuple<uint, DamageType>(4217, DamageType.Cold) }, // frost zombie (125)
            { 49258, new Tuple<uint, DamageType>(24320, DamageType.Cold) }, // frost zombie (150)
            { 49259, new Tuple<uint, DamageType>(24321, DamageType.Cold) }, // frost zombie (180)
            { 49233, new Tuple<uint, DamageType>(25347, DamageType.Cold) }, // frigid zombie (200)

            { 49240, new Tuple<uint, DamageType>(22114, DamageType.Electric) }, // lightning zombie (50)
            { 49241, new Tuple<uint, DamageType>(7348, DamageType.Electric) }, // lightning zombie (80)
            { 49242, new Tuple<uint, DamageType>(8673, DamageType.Electric) }, // lightning zombie (100)
            { 49243, new Tuple<uint, DamageType>(4217, DamageType.Electric) }, // lightning zombie (125)
            { 49244, new Tuple<uint, DamageType>(24320, DamageType.Electric) }, // lightning zombie (150)
            { 49245, new Tuple<uint, DamageType>(24321, DamageType.Electric) }, // lightning zombie (180)
            { 49246, new Tuple<uint, DamageType>(25347, DamageType.Electric) }, // shocked zombie (200)

            // // ============================ Primalist ============================

            // ============ Elementals ============

            { 49261, new Tuple<uint, DamageType>(26692, DamageType.Acid) }, // acid elemental (50)
            { 49262, new Tuple<uint, DamageType>(7993, DamageType.Acid) }, // acid elemental (80)
            { 49263, new Tuple<uint, DamageType>(7994, DamageType.Acid) }, // acid elemental (100)
            { 49264, new Tuple<uint, DamageType>(12038, DamageType.Acid) }, // acid child (125)
            { 49265, new Tuple<uint, DamageType>(27715, DamageType.Acid) }, // acid child (150)
            { 49266, new Tuple<uint, DamageType>(27717, DamageType.Acid) }, // acid child (180)
            { 49267, new Tuple<uint, DamageType>(23568, DamageType.Acid) }, // caustic knight (200)

            { 48959, new Tuple<uint, DamageType>(26692, DamageType.Fire) }, // fire elemental (50)
            { 48961, new Tuple<uint, DamageType>(7993, DamageType.Fire) }, // fire elemental (80)
            { 48963, new Tuple<uint, DamageType>(7994, DamageType.Fire) }, // fire elemental (100)
            { 48965, new Tuple<uint, DamageType>(12038, DamageType.Fire) }, // fire child (125)
            { 48967, new Tuple<uint, DamageType>(27715, DamageType.Fire) }, // fire child (150)
            { 48969, new Tuple<uint, DamageType>(27717, DamageType.Fire) }, // fire child (180)
            { 48957, new Tuple<uint, DamageType>(23568, DamageType.Fire) }, // incendiary knight (200)
            //{ 0, new Tuple<uint, DamageType>(0, DamageType.Fire) }, // scorched knight (200) ?

            { 49275, new Tuple<uint, DamageType>(26692, DamageType.Cold) }, // frost elemental (50)
            { 49276, new Tuple<uint, DamageType>(7993, DamageType.Cold) }, // frost elemental (80)
            { 49277, new Tuple<uint, DamageType>(7994, DamageType.Cold) }, // frost elemental (100)
            { 49278, new Tuple<uint, DamageType>(12038, DamageType.Cold) }, // frost child (125)
            { 49279, new Tuple<uint, DamageType>(27715, DamageType.Cold) }, // frost child (150)
            { 49280, new Tuple<uint, DamageType>(27717, DamageType.Cold) }, // frost child (180)
            { 49260, new Tuple<uint, DamageType>(23568, DamageType.Cold) }, // glacial knight (200)

            { 49268, new Tuple<uint, DamageType>(26692, DamageType.Electric) }, // lightning elemental (50)
            { 49269, new Tuple<uint, DamageType>(7993, DamageType.Electric) }, // lightning elemental (80)
            { 49270, new Tuple<uint, DamageType>(7994, DamageType.Electric) }, // lightning elemental (100)
            { 49271, new Tuple<uint, DamageType>(12038, DamageType.Electric) }, // lightning child (125)
            { 49272, new Tuple<uint, DamageType>(27715, DamageType.Electric) }, // lightning child (150)
            { 49273, new Tuple<uint, DamageType>(27717, DamageType.Electric) }, // lightning child (180)
            { 49274, new Tuple<uint, DamageType>(23568, DamageType.Electric) }, // galvanic knight (200)

            // ============ K'naths ============

            { 49282, new Tuple<uint, DamageType>(1536, DamageType.Acid) }, // acid k'nath (50)
            { 49283, new Tuple<uint, DamageType>(2569, DamageType.Acid) }, // acid k'nath (80)
            { 49284, new Tuple<uint, DamageType>(2570, DamageType.Acid) }, // acid k'nath (100)
            { 49285, new Tuple<uint, DamageType>(2572, DamageType.Acid) }, // acid k'nath (125)
            { 49286, new Tuple<uint, DamageType>(29313, DamageType.Acid) }, // acid k'nath (150)
            { 49287, new Tuple<uint, DamageType>(2573, DamageType.Acid) }, // acid k'nath (180)
            { 49288, new Tuple<uint, DamageType>(23557, DamageType.Acid) }, // k'nath y'nda (200)

            { 49296, new Tuple<uint, DamageType>(1536, DamageType.Fire) }, // fire k'nath (50)
            { 49297, new Tuple<uint, DamageType>(2569, DamageType.Fire) }, // fire k'nath (80)
            { 49298, new Tuple<uint, DamageType>(2570, DamageType.Fire) }, // fire k'nath (100)
            { 49299, new Tuple<uint, DamageType>(2572, DamageType.Fire) }, // fire k'nath (125)
            { 49300, new Tuple<uint, DamageType>(29313, DamageType.Fire) }, // fire k'nath (150)
            { 49301, new Tuple<uint, DamageType>(2573, DamageType.Fire) }, // fire k'nath (180)
            { 49302, new Tuple<uint, DamageType>(23557, DamageType.Fire) }, // k'nath b'orret (200)

            { 49303, new Tuple<uint, DamageType>(1536, DamageType.Cold) }, // frost k'nath (50)
            { 49304, new Tuple<uint, DamageType>(2569, DamageType.Cold) }, // frost k'nath (80)
            { 49305, new Tuple<uint, DamageType>(2570, DamageType.Cold) }, // frost k'nath (100)
            { 49306, new Tuple<uint, DamageType>(2572, DamageType.Cold) }, // frost k'nath (125)
            { 49307, new Tuple<uint, DamageType>(29313, DamageType.Cold) }, // frost k'nath (150)
            { 49308, new Tuple<uint, DamageType>(2573, DamageType.Cold) }, // frost k'nath (180)
            { 49281, new Tuple<uint, DamageType>(23557, DamageType.Cold) }, // k'nath r'ajed (200)

            { 49289, new Tuple<uint, DamageType>(1536, DamageType.Electric) }, // lightning k'nath (50)
            { 49290, new Tuple<uint, DamageType>(2569, DamageType.Electric) }, // lightning k'nath (80)
            { 49291, new Tuple<uint, DamageType>(2570, DamageType.Electric) }, // lightning k'nath (100)
            { 49292, new Tuple<uint, DamageType>(2572, DamageType.Electric) }, // lightning k'nath (125)
            { 49293, new Tuple<uint, DamageType>(29313, DamageType.Electric) }, // lightning k'nath (150)
            { 49294, new Tuple<uint, DamageType>(2573, DamageType.Electric) }, // lightning k'nath (180)
            { 49295, new Tuple<uint, DamageType>(23557, DamageType.Electric) }, // k'nath t'soct (200)

            // ============ Wisps ============

            { 49310, new Tuple<uint, DamageType>(1989, DamageType.Acid) }, // acid wisp (50)
            { 49311, new Tuple<uint, DamageType>(5748, DamageType.Acid) }, // acid wisp (80)
            { 49312, new Tuple<uint, DamageType>(7126, DamageType.Acid) }, // acid wisp (100)
            { 49313, new Tuple<uint, DamageType>(21549, DamageType.Acid) }, // acid wisp (125)
            { 49314, new Tuple<uint, DamageType>(7125, DamageType.Acid) }, // acid wisp (150)
            { 49315, new Tuple<uint, DamageType>(7127, DamageType.Acid) }, // acid wisp (180)
            { 49316, new Tuple<uint, DamageType>(25667, DamageType.Acid) }, // corrosion wisp (200)

            { 49324, new Tuple<uint, DamageType>(1989, DamageType.Fire) }, // fire wisp (50)
            { 49325, new Tuple<uint, DamageType>(5748, DamageType.Fire) }, // fire wisp (80)
            { 49326, new Tuple<uint, DamageType>(7126, DamageType.Fire) }, // fire wisp (100)
            { 49327, new Tuple<uint, DamageType>(21549, DamageType.Fire) }, // fire wisp (125)
            { 49328, new Tuple<uint, DamageType>(7125, DamageType.Fire) }, // fire wisp (150)
            { 49329, new Tuple<uint, DamageType>(7127, DamageType.Fire) }, // fire wisp (180)
            { 49330, new Tuple<uint, DamageType>(25667, DamageType.Fire) }, // incendiary wisp (200)

            { 49331, new Tuple<uint, DamageType>(1989, DamageType.Cold) }, // frost wisp (50)
            { 49332, new Tuple<uint, DamageType>(5748, DamageType.Cold) }, // frost wisp (80)
            { 49333, new Tuple<uint, DamageType>(7126, DamageType.Cold) }, // frost wisp (100)
            { 49334, new Tuple<uint, DamageType>(21549, DamageType.Cold) }, // frost wisp (125)
            { 49335, new Tuple<uint, DamageType>(7125, DamageType.Cold) }, // frost wisp (150)
            { 49336, new Tuple<uint, DamageType>(7127, DamageType.Cold) }, // frost wisp (180)
            { 49309, new Tuple<uint, DamageType>(25667, DamageType.Cold) }, // blizzard wisp (200)

            { 49317, new Tuple<uint, DamageType>(1989, DamageType.Electric) }, // lightning wisp (50)
            { 49318, new Tuple<uint, DamageType>(5748, DamageType.Electric) }, // lightning wisp (80)
            { 49319, new Tuple<uint, DamageType>(7126, DamageType.Electric) }, // lightning wisp (100)
            { 49320, new Tuple<uint, DamageType>(21549, DamageType.Electric) }, // lightning wisp (125)
            { 49321, new Tuple<uint, DamageType>(7125, DamageType.Electric) }, // lightning wisp (150)
            { 49322, new Tuple<uint, DamageType>(7127, DamageType.Electric) }, // lightning wisp (180)
            { 49323, new Tuple<uint, DamageType>(25667, DamageType.Electric) }, // voltaic wisp (200)
        };
    }
}

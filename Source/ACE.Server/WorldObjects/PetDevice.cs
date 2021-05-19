using System;
using System.Collections.Generic;

using log4net;

using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
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
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public int? PetClass
        {
            get => GetProperty(PropertyInt.PetClass);
            set { if (value.HasValue) SetProperty(PropertyInt.PetClass, value.Value); else RemoveProperty(PropertyInt.PetClass); }
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

            if (PetClass == null)
            {
                log.Error($"{activator.Name}.ActOnUse({Name}) - PetClass is null for PetDevice {WeenieClassId}");
                return;
            }

            if (Structure == 0)
            {
                //player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, "You must refill the essence to use it again."));
                player.Session.Network.EnqueueSend(new GameMessageSystemChat("Your summoning device does not have enough charges to function!", ChatMessageType.Broadcast));
                return;
            }

            var wcid = (uint)PetClass;

            var result = SummonCreature(player, wcid);

            if (result == null || result.Value)
            {
                // CombatPet devices should always have structure
                if (Structure != null)
                {
                    // decrease remaining uses
                    Structure--;

                    player.Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyInt(this, PropertyInt.Structure, Structure.Value));
                }
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

            // verify summoning mastery
            if (SummoningMastery != null && player.SummoningMastery != SummoningMastery)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You must be a {SummoningMastery} to use the {Name}", ChatMessageType.Broadcast));
                return new ActivationResult(false);
            }

            // duplicating some of this verification logic here from Pet.Init()
            // since the PetDevice owner and the summoned Pet are separate objects w/ potentially different heartbeat offsets,
            // the cooldown can still expire before the CombatPet's lifespan
            // in this case, if the player tries to re-activate the PetDevice while the CombatPet is still in the world,
            // we want to return an error without re-activating the cooldown

            if (player.CurrentActivePet != null && player.CurrentActivePet is CombatPet)
            {
                if (PropertyManager.GetBool("pet_stow_replace").Item)
                {
                    // original ace
                    player.SendTransientError($"{player.CurrentActivePet.Name} is already active");
                    return new ActivationResult(false);
                }
                else
                {
                    // retail stow
                    var pet = WorldObjectFactory.CreateNewWorldObject((uint)PetClass) as Pet;

                    if (pet == null || !pet.IsPassivePet)
                    {
                        player.SendTransientError($"{player.CurrentActivePet.Name} is already active");
                        return new ActivationResult(false);
                    }
                }
            }
            return new ActivationResult(true);
        }

        public bool? SummonCreature(Player player, uint wcid)
        {
            var wo = WorldObjectFactory.CreateNewWorldObject(wcid);

            if (wo == null)
            {
                log.Error($"{player.Name}.SummonCreature({wcid}) - couldn't find wcid for PetDevice {WeenieClassId} - {WeenieClassName}");
                return false;
            }

            var pet = wo as Pet;

            if (pet == null)
            {
                log.Error($"{player.Name}.SummonCreature({wcid}) - PetDevice {WeenieClassId} - {WeenieClassName} tried to summon {wo.WeenieClassId} - {wo.WeenieClassName} of unknown type {wo.WeenieType}");
                return false;
            }
            var success = pet.Init(player, this);

            return success;
        }

        /// <summary>
        /// Maps an Essence to a WCID to be spawned
        /// </summary>
        public static Dictionary<uint, Tuple<uint, DamageType>> PetDeviceToPetMapping = new Dictionary<uint, Tuple<uint, DamageType>>()
        {
            // ============================ Geomancer ============================

            // ============ Golems ============

            { 48886, new Tuple<uint, DamageType>(48887, DamageType.Bludgeon) }, // mud golem (15)
            { 48890, new Tuple<uint, DamageType>(48891, DamageType.Bludgeon) }, // sandstone golem (30)
            { 48878, new Tuple<uint, DamageType>(48879, DamageType.Bludgeon) }, // copper golem (50)
            { 48888, new Tuple<uint, DamageType>(48889, DamageType.Bludgeon) }, // oak golem (80)
            { 48882, new Tuple<uint, DamageType>(48883, DamageType.Bludgeon) }, // gold golem (100)
            { 48880, new Tuple<uint, DamageType>(48881, DamageType.Bludgeon) }, // coral golem (125)
            { 48884, new Tuple<uint, DamageType>(48885, DamageType.Bludgeon) }, // iron golem (150)

            // ============================ Naturalist ============================

            // ============ Grievvers ============

            { 49366, new Tuple<uint, DamageType>(49052, DamageType.Acid) }, // acid grievver (50)
            { 49367, new Tuple<uint, DamageType>(49053, DamageType.Acid) }, // acid grievver (80)
            { 49368, new Tuple<uint, DamageType>(49054, DamageType.Acid) }, // acid grievver (100)
            { 49369, new Tuple<uint, DamageType>(49055, DamageType.Acid) }, // acid grievver (125)
            { 49370, new Tuple<uint, DamageType>(49056, DamageType.Acid) }, // acid grievver (150)
            { 49371, new Tuple<uint, DamageType>(49057, DamageType.Acid) }, // acid grievver (180)
            { 49372, new Tuple<uint, DamageType>(49058, DamageType.Acid) }, // caustic grievver (200)

            { 49380, new Tuple<uint, DamageType>(49066, DamageType.Fire) }, // fire grievver (50)
            { 49381, new Tuple<uint, DamageType>(49067, DamageType.Fire) }, // fire grievver (80)
            { 49382, new Tuple<uint, DamageType>(49068, DamageType.Fire) }, // fire grievver (100)
            { 49383, new Tuple<uint, DamageType>(49069, DamageType.Fire) }, // fire grievver (125)
            { 49384, new Tuple<uint, DamageType>(49070, DamageType.Fire) }, // fire grievver (150)
            { 49385, new Tuple<uint, DamageType>(49071, DamageType.Fire) }, // fire grievver (180)
            { 49386, new Tuple<uint, DamageType>(49072, DamageType.Fire) }, // scorched grievver (200)

            { 49387, new Tuple<uint, DamageType>(49073, DamageType.Cold) }, // frost grievver (50)
            { 49388, new Tuple<uint, DamageType>(49074, DamageType.Cold) }, // frost grievver (80)
            { 49389, new Tuple<uint, DamageType>(49075, DamageType.Cold) }, // frost grievver (100)
            { 49390, new Tuple<uint, DamageType>(49076, DamageType.Cold) }, // frost grievver (125)
            { 49391, new Tuple<uint, DamageType>(49077, DamageType.Cold) }, // frost grievver (150)
            { 49392, new Tuple<uint, DamageType>(49078, DamageType.Cold) }, // frost grievver (180)
            { 49365, new Tuple<uint, DamageType>(49051, DamageType.Cold) }, // arctic grievver (200)
            //{ -1, -1 }, // glacial grievver (200) ?

            { 49373, new Tuple<uint, DamageType>(49059, DamageType.Electric) }, // lightning grievver (50)
            { 49374, new Tuple<uint, DamageType>(49060, DamageType.Electric) }, // lightning grievver (80)
            { 49375, new Tuple<uint, DamageType>(49061, DamageType.Electric) }, // lightning grievver (100)
            { 49376, new Tuple<uint, DamageType>(49062, DamageType.Electric) }, // lightning grievver (125)
            { 49377, new Tuple<uint, DamageType>(49063, DamageType.Electric) }, // lightning grievver (150)
            { 49378, new Tuple<uint, DamageType>(49064, DamageType.Electric) }, // lightning grievver (180)
            { 49379, new Tuple<uint, DamageType>(49065, DamageType.Electric) }, // excited grievver (200)

            // ============ Moars ============

            { 49338, new Tuple<uint, DamageType>(49108, DamageType.Acid) }, // acid moar (50)
            { 49339, new Tuple<uint, DamageType>(49109, DamageType.Acid) }, // acid moar (80)
            { 49340, new Tuple<uint, DamageType>(49110, DamageType.Acid) }, // acid moar (100)
            { 49341, new Tuple<uint, DamageType>(49111, DamageType.Acid) }, // acid moar (125)
            { 49342, new Tuple<uint, DamageType>(49112, DamageType.Acid) }, // acid moar (150)
            { 49343, new Tuple<uint, DamageType>(49113, DamageType.Acid) }, // acid moar (180)
            { 49344, new Tuple<uint, DamageType>(49114, DamageType.Acid) }, // blistering moar (200)
            //{ 49344, 49114 },   // blistering moar (200)

            { 49352, new Tuple<uint, DamageType>(49122, DamageType.Fire) }, // fire moar (50)
            { 49353, new Tuple<uint, DamageType>(49123, DamageType.Fire) }, // fire moar (80)
            { 49354, new Tuple<uint, DamageType>(49124, DamageType.Fire) }, // fire moar (100)
            { 49355, new Tuple<uint, DamageType>(49125, DamageType.Fire) }, // fire moar (125)
            { 49356, new Tuple<uint, DamageType>(49126, DamageType.Fire) }, // fire moar (150)
            { 49357, new Tuple<uint, DamageType>(49127, DamageType.Fire) }, // fire moar (180)
            { 49358, new Tuple<uint, DamageType>(49128, DamageType.Fire) }, // volcanic moar (200)

            { 49359, new Tuple<uint, DamageType>(49129, DamageType.Cold) }, // frost moar (50)
            { 49360, new Tuple<uint, DamageType>(49130, DamageType.Cold) }, // frost moar (80)
            { 49361, new Tuple<uint, DamageType>(49131, DamageType.Cold) }, // frost moar (100)
            { 49362, new Tuple<uint, DamageType>(49132, DamageType.Cold) }, // frost moar (125)
            { 49363, new Tuple<uint, DamageType>(49133, DamageType.Cold) }, // frost moar (150)
            { 49364, new Tuple<uint, DamageType>(49134, DamageType.Cold) }, // frost moar (180)
            { 49337, new Tuple<uint, DamageType>(49107, DamageType.Cold) }, // freezing moar (200)

            { 49345, new Tuple<uint, DamageType>(49115, DamageType.Electric) }, // lightning moar (50)
            { 49346, new Tuple<uint, DamageType>(49116, DamageType.Electric) }, // lightning moar (80)
            { 49347, new Tuple<uint, DamageType>(49117, DamageType.Electric) }, // lightning moar (100)
            { 49348, new Tuple<uint, DamageType>(49118, DamageType.Electric) }, // lightning moar (125)
            { 49349, new Tuple<uint, DamageType>(49119, DamageType.Electric) }, // lightning moar (150)
            { 49350, new Tuple<uint, DamageType>(49120, DamageType.Electric) }, // lightning moar (180)
            { 49351, new Tuple<uint, DamageType>(49121, DamageType.Electric) }, // electrified moar (200)

            // ============ Phyntos Wasps ============

            { 49524, new Tuple<uint, DamageType>(49136, DamageType.Acid) }, // acid phyntos wasp (50)
            { 49525, new Tuple<uint, DamageType>(49137, DamageType.Acid) }, // acid phyntos wasp (80)
            { 49526, new Tuple<uint, DamageType>(49138, DamageType.Acid) }, // acid phyntos wasp (100)
            { 49527, new Tuple<uint, DamageType>(49139, DamageType.Acid) }, // acid phyntos wasp (125)
            { 49528, new Tuple<uint, DamageType>(49140, DamageType.Acid) }, // acid phyntos wasp (150)
            { 49529, new Tuple<uint, DamageType>(49141, DamageType.Acid) }, // acid phyntos wasp (180)
            { 49530, new Tuple<uint, DamageType>(49142, DamageType.Acid) }, // acid phyntos swarm (200)

            { 49531, new Tuple<uint, DamageType>(49143, DamageType.Fire) }, // fire phyntos wasp (50)
            { 49532, new Tuple<uint, DamageType>(49144, DamageType.Fire) }, // fire phyntos wasp (80)
            { 49533, new Tuple<uint, DamageType>(49145, DamageType.Fire) }, // fire phyntos wasp (100)
            { 49534, new Tuple<uint, DamageType>(49146, DamageType.Fire) }, // fire phyntos wasp (125)
            { 49535, new Tuple<uint, DamageType>(49147, DamageType.Fire) }, // fire phyntos wasp (150)
            { 49536, new Tuple<uint, DamageType>(49148, DamageType.Fire) }, // fire phyntos wasp (180)
            { 49537, new Tuple<uint, DamageType>(49149, DamageType.Fire) }, // fire phyntos swarm (200)

            { 49538, new Tuple<uint, DamageType>(49150, DamageType.Cold) }, // frost phyntos wasp (50)
            { 49539, new Tuple<uint, DamageType>(49151, DamageType.Cold) }, // frost phyntos wasp (80)
            { 49540, new Tuple<uint, DamageType>(49152, DamageType.Cold) }, // frost phyntos wasp (100)
            { 49541, new Tuple<uint, DamageType>(49153, DamageType.Cold) }, // frost phyntos wasp (125)
            { 49542, new Tuple<uint, DamageType>(49154, DamageType.Cold) }, // frost phyntos wasp (150)
            { 49543, new Tuple<uint, DamageType>(49155, DamageType.Cold) }, // frost phyntos wasp (180)
            { 49544, new Tuple<uint, DamageType>(49156, DamageType.Cold) }, // frost phyntos swarm (200)

            { 49545, new Tuple<uint, DamageType>(49157, DamageType.Electric) }, // lightning phyntos wasp (50)
            { 49546, new Tuple<uint, DamageType>(49158, DamageType.Electric) }, // lightning phyntos wasp (80)
            { 49547, new Tuple<uint, DamageType>(49159, DamageType.Electric) }, // lightning phyntos wasp (100)
            { 49548, new Tuple<uint, DamageType>(49160, DamageType.Electric) }, // lightning phyntos wasp (125)
            { 49549, new Tuple<uint, DamageType>(49161, DamageType.Electric) }, // lightning phyntos wasp (150)
            { 49550, new Tuple<uint, DamageType>(49162, DamageType.Electric) }, // lightning phyntos wasp (180)
            { 49551, new Tuple<uint, DamageType>(49135, DamageType.Electric) }, // lightning phyntos swarm (200)

            // ============================ Necromancer ============================

            // ============ Skeletons ============

            { 49213, new Tuple<uint, DamageType>(49164, DamageType.Acid) }, // acid skeleton minion (50)
            { 49214, new Tuple<uint, DamageType>(49165, DamageType.Acid) }, // acid skeleton minion (80)
            { 49215, new Tuple<uint, DamageType>(49166, DamageType.Acid) }, // acid skeleton minion (100)
            { 49216, new Tuple<uint, DamageType>(49167, DamageType.Acid) }, // acid skeleton bushi (125)
            { 49217, new Tuple<uint, DamageType>(49168, DamageType.Acid) }, // acid skeleton bushi (150)
            { 49218, new Tuple<uint, DamageType>(49169, DamageType.Acid) }, // acid skeleton bushi (180)
            { 49219, new Tuple<uint, DamageType>(49163, DamageType.Acid) }, // acid skeleton samurai (200)

            { 48942, new Tuple<uint, DamageType>(48943, DamageType.Fire) }, // fire skeleton minion (50)
            { 48944, new Tuple<uint, DamageType>(48950, DamageType.Fire) }, // fire skeleton minion (80)
            { 48945, new Tuple<uint, DamageType>(48951, DamageType.Fire) }, // fire skeleton minion (100)
            { 48946, new Tuple<uint, DamageType>(48952, DamageType.Fire) }, // fire skeleton bushi (125)
            { 48947, new Tuple<uint, DamageType>(48953, DamageType.Fire) }, // fire skeleton bushi (150)
            { 48948, new Tuple<uint, DamageType>(48949, DamageType.Fire) }, // fire skeleton bushi (180)
            { 48956, new Tuple<uint, DamageType>(48955, DamageType.Fire) }, // fire skeleton samurai (200)

            { 49227, new Tuple<uint, DamageType>(49178, DamageType.Cold) }, // frost skeleton minion (50)
            { 49228, new Tuple<uint, DamageType>(49179, DamageType.Cold) }, // frost skeleton minion (80)
            { 49229, new Tuple<uint, DamageType>(49180, DamageType.Cold) }, // frost skeleton minion (100)
            { 49230, new Tuple<uint, DamageType>(49181, DamageType.Cold) }, // frost skeleton bushi (125)
            { 49231, new Tuple<uint, DamageType>(49182, DamageType.Cold) }, // frost skeleton bushi (150)
            { 49232, new Tuple<uint, DamageType>(49183, DamageType.Cold) }, // frost skeleton bushi (180)
            { 49212, new Tuple<uint, DamageType>(49177, DamageType.Cold) }, // frost skeleton samurai (200)

            { 49220, new Tuple<uint, DamageType>(49171, DamageType.Electric) }, // lightning skeleton minion (50)
            { 49221, new Tuple<uint, DamageType>(49172, DamageType.Electric) }, // lightning skeleton minion (80)
            { 49222, new Tuple<uint, DamageType>(49173, DamageType.Electric) }, // lightning skeleton minion (100)
            { 49223, new Tuple<uint, DamageType>(49174, DamageType.Electric) }, // lightning skeleton bushi (125)
            { 49224, new Tuple<uint, DamageType>(49175, DamageType.Electric) }, // lightning skeleton bushi (150)
            { 49225, new Tuple<uint, DamageType>(49176, DamageType.Electric) }, // lightning skeleton bushi (180)
            { 49226, new Tuple<uint, DamageType>(49170, DamageType.Electric) }, // lightning skeleton samurai (200)

            // ============ Spectres ============

            { 49421, new Tuple<uint, DamageType>(49394, DamageType.Acid) }, // acid spectre (50)
            { 49422, new Tuple<uint, DamageType>(49395, DamageType.Acid) }, // acid spectre (80)
            { 49423, new Tuple<uint, DamageType>(49396, DamageType.Acid) }, // acid spectre (100)
            { 49424, new Tuple<uint, DamageType>(49397, DamageType.Acid) }, // acid spectre (125)
            { 49425, new Tuple<uint, DamageType>(49398, DamageType.Acid) }, // acid spectre (150)
            { 49426, new Tuple<uint, DamageType>(49399, DamageType.Acid) }, // acid spectre (180)
            { 49427, new Tuple<uint, DamageType>(49393, DamageType.Acid) }, // acid maiden (200)

            { 49435, new Tuple<uint, DamageType>(49408, DamageType.Fire) }, // fire spectre (50)
            { 49436, new Tuple<uint, DamageType>(49409, DamageType.Fire) }, // fire spectre (80)
            { 49437, new Tuple<uint, DamageType>(49410, DamageType.Fire) }, // fire spectre (100)
            { 49438, new Tuple<uint, DamageType>(49411, DamageType.Fire) }, // fire spectre (125)
            { 49439, new Tuple<uint, DamageType>(49412, DamageType.Fire) }, // fire spectre (150)
            { 49440, new Tuple<uint, DamageType>(49413, DamageType.Fire) }, // fire spectre (180)
            { 49441, new Tuple<uint, DamageType>(49407, DamageType.Fire) }, // fire maiden (200)

            { 49442, new Tuple<uint, DamageType>(49415, DamageType.Cold) }, // frost spectre (50)
            { 49443, new Tuple<uint, DamageType>(49416, DamageType.Cold) }, // frost spectre (80)
            { 49444, new Tuple<uint, DamageType>(49417, DamageType.Cold) }, // frost spectre (100)
            { 49445, new Tuple<uint, DamageType>(49418, DamageType.Cold) }, // frost spectre (125)
            { 49446, new Tuple<uint, DamageType>(49419, DamageType.Cold) }, // frost spectre (150)
            { 49447, new Tuple<uint, DamageType>(49420, DamageType.Cold) }, // frost spectre (180)
            { 49448, new Tuple<uint, DamageType>(49414, DamageType.Cold) }, // frost maiden (200)

            { 49428, new Tuple<uint, DamageType>(49401, DamageType.Electric) }, // lightning spectre (50)
            { 49429, new Tuple<uint, DamageType>(49402, DamageType.Electric) }, // lightning spectre (80)
            { 49430, new Tuple<uint, DamageType>(49403, DamageType.Electric) }, // lightning spectre (100)
            { 49431, new Tuple<uint, DamageType>(49404, DamageType.Electric) }, // lightning spectre (125)
            { 49432, new Tuple<uint, DamageType>(49405, DamageType.Electric) }, // lightning spectre (150)
            { 49433, new Tuple<uint, DamageType>(49406, DamageType.Electric) }, // lightning spectre (180)
            { 49434, new Tuple<uint, DamageType>(49400, DamageType.Electric) }, // lightning maiden (200)

            // ============ Zombies ============

            { 48972, new Tuple<uint, DamageType>(49000, DamageType.Acid) }, // acid zombie (50)
            { 49234, new Tuple<uint, DamageType>(49003, DamageType.Acid) }, // acid zombie (80)
            { 49235, new Tuple<uint, DamageType>(49004, DamageType.Acid) }, // acid zombie (100)
            { 49236, new Tuple<uint, DamageType>(49005, DamageType.Acid) }, // acid zombie (125)
            { 49237, new Tuple<uint, DamageType>(49006, DamageType.Acid) }, // acid zombie (150)
            { 49238, new Tuple<uint, DamageType>(49007, DamageType.Acid) }, // acid zombie (180)
            { 49239, new Tuple<uint, DamageType>(49008, DamageType.Acid) }, // blistered zombie (200)

            { 49247, new Tuple<uint, DamageType>(49016, DamageType.Fire) }, // fire zombie (50)
            { 49248, new Tuple<uint, DamageType>(49017, DamageType.Fire) }, // fire zombie (80)
            { 49249, new Tuple<uint, DamageType>(49018, DamageType.Fire) }, // fire zombie (100)
            { 49250, new Tuple<uint, DamageType>(49019, DamageType.Fire) }, // fire zombie (125)
            { 49251, new Tuple<uint, DamageType>(49020, DamageType.Fire) }, // fire zombie (150)
            { 49252, new Tuple<uint, DamageType>(49021, DamageType.Fire) }, // fire zombie (180)
            { 49253, new Tuple<uint, DamageType>(49022, DamageType.Fire) }, // charred zombie (200)

            { 49254, new Tuple<uint, DamageType>(49023, DamageType.Cold) }, // frost zombie (50)
            { 49255, new Tuple<uint, DamageType>(49024, DamageType.Cold) }, // frost zombie (80)
            { 49256, new Tuple<uint, DamageType>(49025, DamageType.Cold) }, // frost zombie (100)
            { 49257, new Tuple<uint, DamageType>(49026, DamageType.Cold) }, // frost zombie (125)
            { 49258, new Tuple<uint, DamageType>(49027, DamageType.Cold) }, // frost zombie (150)
            { 49259, new Tuple<uint, DamageType>(49028, DamageType.Cold) }, // frost zombie (180)
            { 49233, new Tuple<uint, DamageType>(49029, DamageType.Cold) }, // frigid zombie (200)

            { 49240, new Tuple<uint, DamageType>(49009, DamageType.Electric) }, // lightning zombie (50)
            { 49241, new Tuple<uint, DamageType>(49010, DamageType.Electric) }, // lightning zombie (80)
            { 49242, new Tuple<uint, DamageType>(49011, DamageType.Electric) }, // lightning zombie (100)
            { 49243, new Tuple<uint, DamageType>(49012, DamageType.Electric) }, // lightning zombie (125)
            { 49244, new Tuple<uint, DamageType>(49013, DamageType.Electric) }, // lightning zombie (150)
            { 49245, new Tuple<uint, DamageType>(49014, DamageType.Electric) }, // lightning zombie (180)
            { 49246, new Tuple<uint, DamageType>(49015, DamageType.Electric) }, // shocked zombie (200)

            // // ============================ Primalist ============================

            // ============ Elementals ============

            { 49261, new Tuple<uint, DamageType>(49031, DamageType.Acid) }, // acid elemental (50)
            { 49262, new Tuple<uint, DamageType>(49032, DamageType.Acid) }, // acid elemental (80)
            { 49263, new Tuple<uint, DamageType>(49033, DamageType.Acid) }, // acid elemental (100)
            { 49264, new Tuple<uint, DamageType>(49034, DamageType.Acid) }, // acid child (125)
            { 49265, new Tuple<uint, DamageType>(49035, DamageType.Acid) }, // acid child (150)
            { 49266, new Tuple<uint, DamageType>(49036, DamageType.Acid) }, // acid child (180)
            { 49267, new Tuple<uint, DamageType>(49030, DamageType.Acid) }, // caustic knight (200)

            { 48959, new Tuple<uint, DamageType>(48960, DamageType.Fire) }, // fire elemental (50)
            { 48961, new Tuple<uint, DamageType>(48962, DamageType.Fire) }, // fire elemental (80)
            { 48963, new Tuple<uint, DamageType>(48964, DamageType.Fire) }, // fire elemental (100)
            { 48965, new Tuple<uint, DamageType>(48966, DamageType.Fire) }, // fire child (125)
            { 48967, new Tuple<uint, DamageType>(48968, DamageType.Fire) }, // fire child (150)
            { 48969, new Tuple<uint, DamageType>(48970, DamageType.Fire) }, // fire child (180)
            { 48957, new Tuple<uint, DamageType>(48958, DamageType.Fire) }, // incendiary knight (200)
            //{ 0, new Tuple<uint, DamageType>(0, DamageType.Fire) }, // scorched knight (200) ?

            { 49275, new Tuple<uint, DamageType>(49045, DamageType.Cold) }, // frost elemental (50)
            { 49276, new Tuple<uint, DamageType>(49046, DamageType.Cold) }, // frost elemental (80)
            { 49277, new Tuple<uint, DamageType>(49047, DamageType.Cold) }, // frost elemental (100)
            { 49278, new Tuple<uint, DamageType>(49048, DamageType.Cold) }, // frost child (125)
            { 49279, new Tuple<uint, DamageType>(49049, DamageType.Cold) }, // frost child (150)
            { 49280, new Tuple<uint, DamageType>(49050, DamageType.Cold) }, // frost child (180)
            { 49260, new Tuple<uint, DamageType>(49044, DamageType.Cold) }, // glacial knight (200)

            { 49268, new Tuple<uint, DamageType>(49038, DamageType.Electric) }, // lightning elemental (50)
            { 49269, new Tuple<uint, DamageType>(49039, DamageType.Electric) }, // lightning elemental (80)
            { 49270, new Tuple<uint, DamageType>(49040, DamageType.Electric) }, // lightning elemental (100)
            { 49271, new Tuple<uint, DamageType>(49041, DamageType.Electric) }, // lightning child (125)
            { 49272, new Tuple<uint, DamageType>(49042, DamageType.Electric) }, // lightning child (150)
            { 49273, new Tuple<uint, DamageType>(49043, DamageType.Electric) }, // lightning child (180)
            { 49274, new Tuple<uint, DamageType>(49037, DamageType.Electric) }, // galvanic knight (200)

            // ============ K'naths ============

            { 49282, new Tuple<uint, DamageType>(49080, DamageType.Acid) }, // acid k'nath (50)
            { 49283, new Tuple<uint, DamageType>(49081, DamageType.Acid) }, // acid k'nath (80)
            { 49284, new Tuple<uint, DamageType>(49082, DamageType.Acid) }, // acid k'nath (100)
            { 49285, new Tuple<uint, DamageType>(49083, DamageType.Acid) }, // acid k'nath (125)
            { 49286, new Tuple<uint, DamageType>(49084, DamageType.Acid) }, // acid k'nath (150)
            { 49287, new Tuple<uint, DamageType>(49085, DamageType.Acid) }, // acid k'nath (180)
            { 49288, new Tuple<uint, DamageType>(49086, DamageType.Acid) }, // k'nath y'nda (200)

            { 49296, new Tuple<uint, DamageType>(49094, DamageType.Fire) }, // fire k'nath (50)
            { 49297, new Tuple<uint, DamageType>(49095, DamageType.Fire) }, // fire k'nath (80)
            { 49298, new Tuple<uint, DamageType>(49096, DamageType.Fire) }, // fire k'nath (100)
            { 49299, new Tuple<uint, DamageType>(49097, DamageType.Fire) }, // fire k'nath (125)
            { 49300, new Tuple<uint, DamageType>(49098, DamageType.Fire) }, // fire k'nath (150)
            { 49301, new Tuple<uint, DamageType>(49099, DamageType.Fire) }, // fire k'nath (180)
            { 49302, new Tuple<uint, DamageType>(49100, DamageType.Fire) }, // k'nath b'orret (200)

            { 49303, new Tuple<uint, DamageType>(49101, DamageType.Cold) }, // frost k'nath (50)
            { 49304, new Tuple<uint, DamageType>(49102, DamageType.Cold) }, // frost k'nath (80)
            { 49305, new Tuple<uint, DamageType>(49103, DamageType.Cold) }, // frost k'nath (100)
            { 49306, new Tuple<uint, DamageType>(49104, DamageType.Cold) }, // frost k'nath (125)
            { 49307, new Tuple<uint, DamageType>(49105, DamageType.Cold) }, // frost k'nath (150)
            { 49308, new Tuple<uint, DamageType>(49106, DamageType.Cold) }, // frost k'nath (180)
            { 49281, new Tuple<uint, DamageType>(49079, DamageType.Cold) }, // k'nath r'ajed (200)

            { 49289, new Tuple<uint, DamageType>(49087, DamageType.Electric) }, // lightning k'nath (50)
            { 49290, new Tuple<uint, DamageType>(49088, DamageType.Electric) }, // lightning k'nath (80)
            { 49291, new Tuple<uint, DamageType>(49089, DamageType.Electric) }, // lightning k'nath (100)
            { 49292, new Tuple<uint, DamageType>(49090, DamageType.Electric) }, // lightning k'nath (125)
            { 49293, new Tuple<uint, DamageType>(49091, DamageType.Electric) }, // lightning k'nath (150)
            { 49294, new Tuple<uint, DamageType>(49092, DamageType.Electric) }, // lightning k'nath (180)
            { 49295, new Tuple<uint, DamageType>(49093, DamageType.Electric) }, // k'nath t'soct (200)

            // ============ Wisps ============

            { 49310, new Tuple<uint, DamageType>(49185, DamageType.Acid) }, // acid wisp (50)
            { 49311, new Tuple<uint, DamageType>(49186, DamageType.Acid) }, // acid wisp (80)
            { 49312, new Tuple<uint, DamageType>(49187, DamageType.Acid) }, // acid wisp (100)
            { 49313, new Tuple<uint, DamageType>(49188, DamageType.Acid) }, // acid wisp (125)
            { 49314, new Tuple<uint, DamageType>(49189, DamageType.Acid) }, // acid wisp (150)
            { 49315, new Tuple<uint, DamageType>(49190, DamageType.Acid) }, // acid wisp (180)
            { 49316, new Tuple<uint, DamageType>(49191, DamageType.Acid) }, // corrosion wisp (200)

            { 49324, new Tuple<uint, DamageType>(49199, DamageType.Fire) }, // fire wisp (50)
            { 49325, new Tuple<uint, DamageType>(49200, DamageType.Fire) }, // fire wisp (80)
            { 49326, new Tuple<uint, DamageType>(49201, DamageType.Fire) }, // fire wisp (100)
            { 49327, new Tuple<uint, DamageType>(49202, DamageType.Fire) }, // fire wisp (125)
            { 49328, new Tuple<uint, DamageType>(49203, DamageType.Fire) }, // fire wisp (150)
            { 49329, new Tuple<uint, DamageType>(49204, DamageType.Fire) }, // fire wisp (180)
            { 49330, new Tuple<uint, DamageType>(49205, DamageType.Fire) }, // incendiary wisp (200)

            { 49331, new Tuple<uint, DamageType>(49206, DamageType.Cold) }, // frost wisp (50)
            { 49332, new Tuple<uint, DamageType>(49207, DamageType.Cold) }, // frost wisp (80)
            { 49333, new Tuple<uint, DamageType>(49208, DamageType.Cold) }, // frost wisp (100)
            { 49334, new Tuple<uint, DamageType>(49209, DamageType.Cold) }, // frost wisp (125)
            { 49335, new Tuple<uint, DamageType>(49210, DamageType.Cold) }, // frost wisp (150)
            { 49336, new Tuple<uint, DamageType>(49211, DamageType.Cold) }, // frost wisp (180)
            { 49309, new Tuple<uint, DamageType>(49184, DamageType.Cold) }, // blizzard wisp (200)

            { 49317, new Tuple<uint, DamageType>(49192, DamageType.Electric) }, // lightning wisp (50)
            { 49318, new Tuple<uint, DamageType>(49193, DamageType.Electric) }, // lightning wisp (80)
            { 49319, new Tuple<uint, DamageType>(49194, DamageType.Electric) }, // lightning wisp (100)
            { 49320, new Tuple<uint, DamageType>(49195, DamageType.Electric) }, // lightning wisp (125)
            { 49321, new Tuple<uint, DamageType>(49196, DamageType.Electric) }, // lightning wisp (150)
            { 49322, new Tuple<uint, DamageType>(49197, DamageType.Electric) }, // lightning wisp (180)
            { 49323, new Tuple<uint, DamageType>(49198, DamageType.Electric) }, // voltaic wisp (200)
        };

        /// <summary>
        /// Returns TRUE if wo is Encapsulated Spirit
        /// </summary>
        public static bool IsEncapsulatedSpirit(WorldObject wo)
        {
            return wo.WeenieClassId == 49485;
        }

        /// <summary>
        /// Applies an encapsulated spirit to a PetDevice
        /// </summary>
        public void Refill(Player player, CraftTool spirit)
        {
            // TODO: this should be moved to recipe system
            if (!IsEncapsulatedSpirit(spirit))
            {
                player.SendUseDoneEvent();
                return;
            }

            if (player.IsBusy)
            {
                player.SendUseDoneEvent(WeenieError.YoureTooBusy);
                return;
            }

            // verify use requirements
            var useError = VerifyUseRequirements(player, spirit, this);
            if (useError != WeenieError.None)
            {
                player.SendUseDoneEvent(useError);
                return;
            }

            player.IsBusy = true;

            var animTime = 0.0f;

            var actionChain = new ActionChain();

            // handle switching to peace mode
            if (player.CombatMode != CombatMode.NonCombat)
            {
                var stanceTime = player.SetCombatMode(CombatMode.NonCombat);
                actionChain.AddDelaySeconds(stanceTime);

                animTime += stanceTime;
            }

            // perform clapping motion
            animTime += player.EnqueueMotion(actionChain, MotionCommand.ClapHands);

            actionChain.AddAction(player, () =>
            {
                // re-verify
                var useError = VerifyUseRequirements(player, spirit, this);
                if (useError != WeenieError.None)
                {
                    player.SendUseDoneEvent(useError);
                    player.IsBusy = false;
                    return;
                }

                if (Structure == MaxStructure)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat("This essence is already full.", ChatMessageType.Broadcast));
                    player.SendUseDoneEvent(WeenieError.YouDoNotPassCraftingRequirements);
                    player.IsBusy = false;
                    return;
                }

                player.UpdateProperty(this, PropertyInt.Structure, MaxStructure);

                player.TryConsumeFromInventoryWithNetworking(spirit, 1);

                player.Session.Network.EnqueueSend(new GameMessageSystemChat("You add the spirit to the essence.", ChatMessageType.Broadcast));

                player.SendUseDoneEvent();

                player.IsBusy = false;
            });

            player.EnqueueMotion(actionChain, MotionCommand.Ready);

            actionChain.EnqueueChain();

            player.NextUseTime = DateTime.UtcNow.AddSeconds(animTime);
        }

        public static WeenieError VerifyUseRequirements(Player player, WorldObject source, WorldObject target)
        {
            // ensure target is summoning essence? source.TargetType is Misc

            // ensure both source and target are in player's inventory
            if (player.FindObject(source.Guid.Full, Player.SearchLocations.MyInventory) == null)
                return WeenieError.YouDoNotPassCraftingRequirements;

            if (player.FindObject(target.Guid.Full, Player.SearchLocations.MyInventory) == null)
                return WeenieError.YouDoNotPassCraftingRequirements;

            return WeenieError.None;
        }
    }
}

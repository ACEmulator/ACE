using System;
using System.Collections.Generic;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;
using log4net;

namespace ACE.Server.Entity
{
    public enum Sigil
    {
        /// <summary>
        /// Increased damage resistance rating
        /// </summary>
        Defense,

        /// <summary>
        /// Increased damage rating
        /// </summary>
        Destruction,

        /// <summary>
        /// Increased critical damage rating
        /// </summary>
        Fury,

        /// <summary>
        /// Increased healing rating
        /// </summary>
        Growth,

        /// <summary>
        /// Vital regeneration spells
        /// </summary>
        Vigor,
    }

    public enum AetheriaColor
    {
        Blue,
        Yellow,
        Red
    };

    public class Aetheria
    {
        // https://asheron.fandom.com/wiki/Aetheria

        public const uint AetheriaBlue = 42635;
        public const uint AetheriaRed = 42636;
        public const uint AetheriaYellow = 42637;

        public const uint AetheriaManaStone = 42645;

        public static Dictionary<AetheriaColor, Dictionary<Sigil, uint>> Icons;

        static Aetheria()
        {
            Icons = new Dictionary<AetheriaColor, Dictionary<Sigil, uint>>();

            Icons.Add(AetheriaColor.Blue,   new Dictionary<Sigil, uint>());
            Icons.Add(AetheriaColor.Yellow, new Dictionary<Sigil, uint>());
            Icons.Add(AetheriaColor.Red,    new Dictionary<Sigil, uint>());

            Icons[AetheriaColor.Blue].Add(Sigil.Defense,     0x06006BF2);
            Icons[AetheriaColor.Blue].Add(Sigil.Destruction, 0x06006BFE);
            Icons[AetheriaColor.Blue].Add(Sigil.Fury,        0x06006BFF);
            Icons[AetheriaColor.Blue].Add(Sigil.Growth,      0x06006C00);
            Icons[AetheriaColor.Blue].Add(Sigil.Vigor,       0x06006C01);

            Icons[AetheriaColor.Yellow].Add(Sigil.Defense,     0x06006C06);
            Icons[AetheriaColor.Yellow].Add(Sigil.Destruction, 0x06006C07);
            Icons[AetheriaColor.Yellow].Add(Sigil.Fury,        0x06006BF3);
            Icons[AetheriaColor.Yellow].Add(Sigil.Growth,      0x06006C08);
            Icons[AetheriaColor.Yellow].Add(Sigil.Vigor,       0x06006BFD);

            Icons[AetheriaColor.Red].Add(Sigil.Defense,     0x06006C02);
            Icons[AetheriaColor.Red].Add(Sigil.Destruction, 0x06006C03);
            Icons[AetheriaColor.Red].Add(Sigil.Fury,        0x06006C04);
            Icons[AetheriaColor.Red].Add(Sigil.Growth,      0x06006BF4);
            Icons[AetheriaColor.Red].Add(Sigil.Vigor,       0x06006C05);
        }


        public static bool IsAetheria(uint wcid)
        {
            return wcid == AetheriaBlue || wcid == AetheriaYellow || wcid == AetheriaRed;
        }

        public static AetheriaColor? GetColor(uint wcid)
        {
            switch (wcid)
            {
                case AetheriaBlue:
                    return AetheriaColor.Blue;
                case AetheriaYellow:
                    return AetheriaColor.Yellow;
                case AetheriaRed:
                    return AetheriaColor.Red;
                default:
                    return null;
            }
        }

        /// <summary>
        /// The player uses an aetheria mana stone on a piece of coalesced aetheria
        /// </summary>
        public static void UseObjectOnTarget(Player player, WorldObject source, WorldObject target)
        {
            //Console.WriteLine($"Aetheria.UseObjectOnTarget({player.Name}, {source.Name}, {target.Name})");

            if (player.IsBusy)
            {
                player.SendUseDoneEvent(WeenieError.YoureTooBusy);
                return;
            }

            // verify use requirements
            var useError = VerifyUseRequirements(player, source, target);
            if (useError != WeenieError.None)
            {
                player.SendUseDoneEvent(useError);
                return;
            }

            var animTime = 0.0f;

            var actionChain = new ActionChain();

            player.IsBusy = true;

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
                var useError = VerifyUseRequirements(player, source, target);
                if (useError != WeenieError.None)
                {
                    player.SendUseDoneEvent(useError);
                    return;
                }

                ActivateSigil(player, source, target);
            });

            player.EnqueueMotion(actionChain, MotionCommand.Ready);

            actionChain.AddAction(player, () => player.IsBusy = false);

            actionChain.EnqueueChain();

            player.NextUseTime = DateTime.UtcNow.AddSeconds(animTime);
        }

        public static WeenieError VerifyUseRequirements(Player player, WorldObject source, WorldObject target)
        {
            if (source == target)
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"You can't use the {source.Name} on itself."));
                return WeenieError.YouDoNotPassCraftingRequirements;
            }

            // ensure both source and target are in player's inventory
            if (player.FindObject(source.Guid.Full, Player.SearchLocations.MyInventory) == null)
                return WeenieError.YouDoNotPassCraftingRequirements;

            if (player.FindObject(target.Guid.Full, Player.SearchLocations.MyInventory) == null)
                return WeenieError.YouDoNotPassCraftingRequirements;

            if (source.WeenieClassId != AetheriaManaStone ||
                target.WeenieClassId != AetheriaBlue && target.WeenieClassId != AetheriaYellow && target.WeenieClassId != AetheriaRed)

                return WeenieError.YouDoNotPassCraftingRequirements;

            if (target.Name != "Coalesced Aetheria")
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"You can't use the {source.Name} on {target.Name} because the sigil is already visible."));
                return WeenieError.YouDoNotPassCraftingRequirements;
            }

            return WeenieError.None;
        }

        public static void ActivateSigil(Player player, WorldObject source, WorldObject target)
        {
            // rng select a sigil / spell set
            var randSigil = (Sigil)ThreadSafeRandom.Next(0, 4);

            var equipmentSet = SigilToEquipmentSet[randSigil];
            target.SetProperty(PropertyInt.EquipmentSetId, (int)equipmentSet);

            // change icon
            var color = GetColor(target.WeenieClassId).Value;
            var icon = Icons[color][randSigil];
            target.SetProperty(PropertyDataId.Icon, icon);

            // rng select a surge spell
            var surgeSpell = (SpellId)ThreadSafeRandom.Next(5204, 5208);

            //target.Biota.GetOrAddKnownSpell((int)surgeSpell, target.BiotaDatabaseLock, target.BiotaPropertySpells, out _);

            target.SetProperty(PropertyDataId.ProcSpell, (uint)surgeSpell);
            //target.SetProperty(PropertyFloat.ProcSpellRate, 0.05f);   // proc rate for aetheria?

            if (SurgeTargetSelf[surgeSpell])
                target.SetProperty(PropertyBool.ProcSpellSelfTargeted, true);

            // set equip mask
            target.SetProperty(PropertyInt.ValidLocations, (int)ColorToMask[color]);

            // level?
            player.Session.Network.EnqueueSend(new GameMessageSystemChat("A sigil rises to the surface as you bathe the aetheria in mana.", ChatMessageType.Broadcast));

            player.UpdateProperty(target, PropertyString.Name, "Aetheria");
            player.UpdateProperty(target, PropertyString.LongDesc, "This aetheria's sigil now shows on the surface.");
            player.Session.Network.EnqueueSend(new GameMessageUpdateObject(target));

            target.SaveBiotaToDatabase();

            player.SendUseDoneEvent();

            log.Debug($"[CRAFTING] {player.Name} revealed a {color} {randSigil} with a surge of {surgeSpell} on {target.Name}:0x{target.Guid}");
        }

        public static Dictionary<Sigil, EquipmentSet> SigilToEquipmentSet = new Dictionary<Sigil, EquipmentSet>()
        {
            { Sigil.Defense, EquipmentSet.AetheriaDefense },
            { Sigil.Destruction, EquipmentSet.AetheriaDestruction },
            { Sigil.Fury, EquipmentSet.AetheriaFury },
            { Sigil.Growth, EquipmentSet.AetheriaGrowth },
            { Sigil.Vigor, EquipmentSet.AetheriaVigor }
        };

        public static Dictionary<AetheriaColor, EquipMask> ColorToMask = new Dictionary<AetheriaColor, EquipMask>()
        {
            { AetheriaColor.Blue, EquipMask.SigilOne },
            { AetheriaColor.Yellow, EquipMask.SigilTwo },
            { AetheriaColor.Red, EquipMask.SigilThree },
        };

        public static Dictionary<SpellId, bool> SurgeTargetSelf = new Dictionary<SpellId, bool>()
        {
            { SpellId.AetheriaProcDamageBoost,     true },
            { SpellId.AetheriaProcDamageOverTime,  false },
            { SpellId.AetheriaProcDamageReduction, true },
            { SpellId.AetheriaProcHealDebuff,      false },
            { SpellId.AetheriaProcHealthOverTime,  true },
        };

        public static float CalcProcRate(WorldObject aetheria, Creature wielder)
        {
            // ~1% base rate per level?
            var procRate = (aetheria.ItemLevel ?? 0) * 0.01f;

            if (wielder is Player player)
            {
                // +0.1% per luminance aug?
                var augBonus = player.LumAugSurgeChanceRating * 0.001f;
                procRate += augBonus;
            }

            // The proc rates depend on the attack type. Magic is best, then missile is slightly lower, then Melee is slightly lower than missile.
            switch (wielder.CombatMode)
            {
                case CombatMode.Magic:
                    procRate *= 2.0f;
                    break;

                case CombatMode.Missile:
                    procRate *= 1.5f;
                    break;
            }
            // It is unconfirmed, but believed, that the act of being hit or attacked increases the chances of a surge triggering.
            return procRate;
        }

        /// <summary>
        /// Returns TRUE if wo is AetheriaManaStone
        /// </summary>
        public static bool IsAetheriaManaStone(WorldObject wo)
        {
            return wo.WeenieClassId == AetheriaManaStone;
        }

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    }
}

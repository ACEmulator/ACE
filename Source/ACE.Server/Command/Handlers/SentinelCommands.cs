using ACE.Database;
using ACE.DatLoader;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;
using System;
using System.Linq;

namespace ACE.Server.Command.Handlers
{
    public static class SentinelCommands
    {
        // cloak < on / off / player / creature >
        [CommandHandler("cloak", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 1,
            "Sets your cloaking state.",
            "< on / off / player / creature >\n" +
            "This command sets your current cloaking state\n" +
            "< on > You will be completely invisible to players.\n" +
            "< off > You will show up as a normal.\n" +
            "< player > You will appear as a player. (No + and a white radar dot.)\n" +
            "< creature > You will appear as a creature. (No + and an orange radar dot.)")]
        public static void HandleCloak(Session session, params string[] parameters)
        {
            // Please specify if you want cloaking on or off.usage: @cloak < on / off / player / creature >
            // This command sets your current cloaking state.
            // < on > You will be completely invisible to players.
            // < off > You will show up as a normal.
            // < player > You will appear as a player. (No + and a white radar dot.)
            // < creature > You will appear as a creature. (No + and an orange radar dot.)
            // @cloak - Sets your cloaking state.

            // TODO: investigate translucensy/visbility of other cloaked admins.

            switch (parameters?[0].ToLower())
            {
                case "off":
                    session.Player.Cloaked = false;
                    session.Player.Ethereal = false;
                    // session.Player.IgnoreCollisions = false;
                    session.Player.NoDraw = false;
                    // session.Player.ReportCollisions = true;
                    session.Player.EnqueueBroadcastPhysicsState();
                    session.Player.Translucency = null;
                    session.Player.Visibility = false;
                    session.Player.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.CloakStatus, (int)CloakStatus.Off);

                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageRemoveObject(session.Player));
                    //session.Player.CurrentLandblock.RemoveWorldObject(session.Player.Guid, false);
                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageCreateObject(session.Player));
                    //session.Player.CurrentLandblock.AddWorldObject(session.Player);

                    break;
                case "on":
                    session.Player.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.CloakStatus, (int)CloakStatus.On);
                    session.Player.Cloaked = true;
                    session.Player.Ethereal = true;
                    // session.Player.IgnoreCollisions = true;
                    session.Player.NoDraw = true;
                    // session.Player.ReportCollisions = false;
                    session.Player.EnqueueBroadcastPhysicsState();
                    session.Player.Visibility = true;
                    session.Player.Translucency = 0.5f;

                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageRemoveObject(session.Player));
                    //session.Player.CurrentLandblock.RemoveWorldObject(session.Player.Guid, false);
                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageCreateObject(session.Player));
                    //session.Network.EnqueueSend(new GameMessageCreateObject(session.Player));
                    //session.Player.CurrentLandblock.AddWorldObject(session.Player);

                    break;
                case "player":
                    session.Player.Cloaked = false;
                    session.Player.Ethereal = false;
                    // session.Player.IgnoreCollisions = false;
                    session.Player.NoDraw = false;
                    // session.Player.ReportCollisions = true;
                    session.Player.EnqueueBroadcastPhysicsState();
                    session.Player.Translucency = null;
                    session.Player.Visibility = false;
                    session.Player.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.CloakStatus, (int)CloakStatus.Player);

                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageRemoveObject(session.Player));
                    //session.Player.CurrentLandblock.RemoveWorldObject(session.Player.Guid, false);
                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageCreateObject(session.Player));
                    //session.Player.CurrentLandblock.AddWorldObject(session.Player);

                    break;
                case "creature":
                    session.Player.Cloaked = false;
                    session.Player.Ethereal = false;
                    // session.Player.IgnoreCollisions = false;
                    session.Player.NoDraw = false;
                    // session.Player.ReportCollisions = true;
                    session.Player.EnqueueBroadcastPhysicsState();
                    session.Player.Translucency = null;
                    session.Player.Visibility = false;
                    session.Player.SetProperty(ACE.Entity.Enum.Properties.PropertyInt.CloakStatus, (int)CloakStatus.Creature);

                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageRemoveObject(session.Player));
                    //session.Player.CurrentLandblock.RemoveWorldObject(session.Player.Guid, false);
                    //session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageCreateObject(session.Player));
                    //session.Player.CurrentLandblock.AddWorldObject(session.Player);

                    break;
                default:
                    session.Network.EnqueueSend(new GameMessageSystemChat("Please specify if you want cloaking on or off.", ChatMessageType.Broadcast));
                    break;
            }
        }

        // neversaydie [on/off]
        [CommandHandler("neversaydie", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Turn immortality on or off.",
            "[ on | off ]\n" + "Defaults to on.")]
        public static void HandleNeverSayDie(Session session, params string[] parameters)
        {
            // @neversaydie [on/off] - Turn immortality on or off. Defaults to on.

            string param;

            if (parameters.Length > 0)
                param = parameters[0];
            else
                param = "on";

            switch (param)
            {
                case "off":
                    session.Player.Invincible = false;
                    session.Network.EnqueueSend(new GameMessageSystemChat("You are once again mortal.", ChatMessageType.Broadcast));
                    break;
                case "on":
                default:
                    session.Player.Invincible = true;
                    session.Network.EnqueueSend(new GameMessageSystemChat("You are now immortal.", ChatMessageType.Broadcast));
                    break;
            }
        }

        // portal_bypass
        [CommandHandler("portal_bypass", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Toggles the ability to bypass portal restrictions",
            "")]
        public static void HandlePortalBypass(Session session, params string[] parameters)
        {
            // @portal_bypass - Toggles the ability to bypass portal restrictions.

            var param = session.Player.IgnorePortalRestrictions ?? false;

            switch (param)
            {
                case true:
                    session.Player.IgnorePortalRestrictions = false;
                    session.Network.EnqueueSend(new GameMessageSystemChat("You are once again bound by portal restrictions.", ChatMessageType.Broadcast));
                    break;
                case false:
                    session.Player.IgnorePortalRestrictions = true;
                    session.Network.EnqueueSend(new GameMessageSystemChat("You are no longer bound by portal restrictions.", ChatMessageType.Broadcast));
                    break;
            }
        }

        // buff [name]
        [CommandHandler("buff", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Buffs you (or a player) with all beneficial spells.",
            "[name]\n"
            + "This command buffs yourself (or the specified character).")]
        public static void HandleBuff(Session session, params string[] parameters)
        {
            Player targetPlayer = null;
            if (parameters.Length > 0)
            {
                var parameterBlob = parameters.Aggregate((a, b) => a + " " + b);
                var targetPlayerSession = WorldManager.FindByPlayerName(parameterBlob);
                if (targetPlayerSession == null)
                {
                    ChatPacket.SendServerMessage(session, $"Unable to find player {parameterBlob}", ChatMessageType.Broadcast);
                    return;
                }
                targetPlayer = targetPlayerSession.Player;
            }
            else
                targetPlayer = session.Player;
            string SelfOrOther = (targetPlayer != session.Player) ? "Other" : "Self";
            string maxSpellLevel = (DatabaseManager.World.GetCachedSpell((uint)Network.Enum.Spell.ArmorOther8) == null) ? "7" : "8";
            var tySpell = typeof(Network.Enum.Spell);
            foreach (var spell in Buffs)
            {
                uint spellID = (uint)Enum.Parse(tySpell, spell + SelfOrOther + maxSpellLevel);
                CastBuffOnPlayer(spellID, targetPlayer);
            }
        }

        private static string[] Buffs = new string[] {
#region spells
            // TODO: Item Aura buffs
            "Strength",
            "Invulnerability",
            "FireProtection",
            "Armor",
            "Rejuvenation",
            "Regeneration",
            "ManaRenewal",
            "Impregnability",
            "MagicResistance",
            "AxeMastery",
            "DaggerMastery",
            "MaceMastery",
            "SpearMastery",
            "StaffMastery",
            "SwordMastery",
            "UnarmedCombatMastery",
            "BowMastery",
            "CrossbowMastery",
            "AcidProtection",
            "ThrownWeaponMastery",
            "CreatureEnchantmentMastery",
            "ItemEnchantmentMastery",
            "LifeMagicMastery",
            "WarMagicMastery",
            "ManaMastery",
            "ArcaneEnlightenment",
            "ArmorExpertise",
            "ItemExpertise",
            "MagicItemExpertise",
            "WeaponExpertise",
            "MonsterAttunement",
            "PersonAttunement",
            "DeceptionMastery",
            "HealingMastery",
            "LeadershipMastery",
            "LockpickMastery",
            "Fealty",
            "JumpingMastery",
            "Sprint",
            "BludgeonProtection",
            "ColdProtection",
            "LightningProtection",
            "BladeProtection",
            "PiercingProtection",
            "Endurance",
            "Coordination",
            "Quickness",
            "Focus",
            "Willpower",
            "CookingMastery",
            "FletchingMastery",
            "AlchemyMastery",
            "VoidMagicMastery",
            "SummoningMastery"
#endregion
            };

        public static bool CastBuffOnPlayer(uint spellID, Player targetPlayer)
        {
            if (spellID < 1) throw new Exception("spell not found");
            var spellBase = DatManager.PortalDat.SpellTable.Spells[spellID]; if (spellBase == null) return false;
            var spell = DatabaseManager.World.GetCachedSpell(spellID); if (spell == null) return false; // the database doesn't yet have the spell
            var runEnchantment = new Enchantment(targetPlayer, spellID, (double)spell.Duration, 0, spell.StatModType, spell.StatModVal);
            var msgRunEnchantment = new GameEventMagicUpdateEnchantment(targetPlayer.Session, runEnchantment);
            targetPlayer.Session.Network.EnqueueSend(msgRunEnchantment);
            targetPlayer.CurrentLandblock.EnqueueBroadcast(targetPlayer.Location, new GameMessageScript(targetPlayer.Guid, (PlayScript)spellBase.TargetEffect, 1f));
            targetPlayer.EnchantmentManager.Add(runEnchantment, false);
            return true;
        }

        // run < on | off | toggle | check >
        [CommandHandler("run", AccessLevel.Sentinel, CommandHandlerFlag.RequiresWorld, 0,
            "Temporarily boosts your run skill.",
            "( on | off | toggle | check )\n"
            + "Boosts the run skill of the PSR so they can pursue the \"bad folks\". The enchantment will wear off after a while. This command defaults to toggle.")]
        public static void HandleRun(Session session, params string[] parameters)
        {
            // usage: @run on| off | toggle | check
            // Boosts the run skill of the PSR so they can pursue the "bad folks".The enchantment will wear off after a while.This command defualts to toggle.
            // @run - Temporarily boosts your run skill.

            string param;

            if (parameters.Length > 0)
                param = parameters[0];
            else
                param = "toggle";

            var spellID = (uint)Network.Enum.Spell.SentinelRun;
            var spellBase = DatManager.PortalDat.SpellTable.Spells[spellID];
            var spell = DatabaseManager.World.GetCachedSpell(spellID);

            switch (param)
            {
                case "toggle":
                    if (session.Player.EnchantmentManager.HasSpell(spellID))
                        goto case "off";
                    else
                        goto case "on";
                case "check":
                    session.Network.EnqueueSend(new GameMessageSystemChat($"Run speed boost is currently {(session.Player.EnchantmentManager.HasSpell(spellID) ? "ACTIVE" : "INACTIVE")}", ChatMessageType.Broadcast));
                    break;
                case "off":
                    var runBoost = session.Player.EnchantmentManager.GetSpell(spellID);
                    if (runBoost != null)
                        session.Player.EnchantmentManager.Remove(runBoost);
                    else
                        session.Network.EnqueueSend(new GameMessageSystemChat("Run speed boost is currently INACTIVE", ChatMessageType.Broadcast));
                    break;
                case "on":
                    var runEnchantment = new Enchantment(session.Player, spellID, (double)spell.Duration, 0, spell.StatModType, spell.StatModVal);
                    var msgRunEnchantment = new GameEventMagicUpdateEnchantment(session, runEnchantment);
                    session.Player.CurrentLandblock.EnqueueBroadcast(session.Player.Location, new GameMessageScript(session.Player.Guid, (PlayScript)spell.TargetEffect, 1f));
                    session.Player.EnchantmentManager.Add(runEnchantment, false);
                    session.Network.EnqueueSend(new GameMessageSystemChat("Run forrest, run!", ChatMessageType.Broadcast), msgRunEnchantment);
                    break;
            }
        }
    }
}

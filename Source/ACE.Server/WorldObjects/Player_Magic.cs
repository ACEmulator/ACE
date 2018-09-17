using ACE.Database;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public enum TargetCategory
        {
            UnDef,
            WorldObject,
            Wielded,
            Inventory,
            Self
        }

        /// <summary>
        /// Handles player targeted casting message
        /// </summary>
        public void HandleActionCastTargetedSpell(ObjectGuid guidTarget, uint spellId)
        {
            Player player = CurrentLandblock?.GetObject(Guid) as Player;
            TargetCategory targetCategory = TargetCategory.WorldObject;

            if (guidTarget == Guid)
                targetCategory = TargetCategory.Self;
            if (!(CurrentLandblock?.GetObject(guidTarget) is WorldObject target))
            {
                target = GetWieldedItem(guidTarget);
                if (target != null)
                    targetCategory = TargetCategory.Wielded;
                else
                {
                    target = CurrentLandblock?.GetWieldedObject(guidTarget);
                    if (target != null)
                        targetCategory = TargetCategory.Wielded;
                }
            }
            if (target == null)
            {
                target = GetInventoryItem(guidTarget);
                targetCategory = TargetCategory.Inventory;
            }
            if (target == null)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.TargetNotAcquired));
                targetCategory = TargetCategory.UnDef;
                return;
            }

            if (targetCategory != TargetCategory.WorldObject)
            {
                CreatePlayerSpell(target, targetCategory, spellId);
            }
            else
            {
                // turn if required
                var rotateTime = Rotate(target) - 0.25f;
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(rotateTime);

                actionChain.AddAction(this, () => CreatePlayerSpell(target, targetCategory, spellId));
                actionChain.EnqueueChain();
            }
        }

        /// <summary>
        /// Handles player untargeted casting message
        /// </summary>
        public void HandleActionMagicCastUnTargetedSpell(uint spellId)
        {
            CreatePlayerSpell(spellId);
        }

        /// <summary>
        ///  Learns spells in bulk, without notification, filtered by school and level
        /// </summary>
        public void LearnSpellsInBulk(uint magicSchool, uint spellLevel)
        {
            var spellTable = DatManager.PortalDat.SpellTable;
            Player player = CurrentLandblock?.GetObject(Guid) as Player;

            for (uint x = 0; x < PlayerSpellID.Length; x++)
            {
                if (spellTable.Spells.ContainsKey(PlayerSpellID[x]))
                {
                    SpellBase spell = spellTable.Spells[PlayerSpellID[x]];
                    if ((uint)spell.School == magicSchool)
                    {
                        if ((uint)CalculateSpellLevel(spell) == spellLevel)
                            player.LearnSpellWithNetworking(spell.MetaSpellId, false);
                    }
                }
            }
        }

        /// <summary>
        /// Method used for handling items casting spells on the player who is either equiping the item, or using a gem in posessions
        /// </summary>
        /// <param name="spellId">the spell id</param>
        /// <returns>FALSE - the spell was NOT created because the spell is invalid or not implemented yet.<para />TRUE - the spell was created or it is surpassed</returns>
        public bool CreateSingleSpell(uint spellId)
        {
            Player player = CurrentLandblock?.GetObject(Guid) as Player;
            if (player == null && ((this as Player) != null)) player = this as Player;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.MagicInvalidSpellType));
                return false;
            }

            SpellBase spell = spellTable.Spells[spellId];

            Spell spellStatMod = DatabaseManager.World.GetCachedSpell(spellId);
            if (spellStatMod == null)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                return false;
            }

            float scale = SpellAttributes(player.Session.Account, spellId, out float castingDelay, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            EnchantmentStatus enchantmentStatus = default(EnchantmentStatus);
            switch (spell.School)
            {
                case MagicSchool.CreatureEnchantment:
                    enchantmentStatus = CreatureMagic(player, spell, spellStatMod);
                    if (enchantmentStatus.message != null)
                        EnqueueBroadcast(new GameMessageScript(player.Guid, (PlayScript)spell.TargetEffect, scale));
                    break;
                case MagicSchool.LifeMagic:
                    LifeMagic(player, spell, spellStatMod, out uint damage, out bool critical, out enchantmentStatus);
                    if (enchantmentStatus.message != null)
                        EnqueueBroadcast(new GameMessageScript(player.Guid, (PlayScript)spell.TargetEffect, scale));
                    break;
                case MagicSchool.ItemEnchantment:
                    if ((spell.MetaSpellType == SpellType.PortalLink)
                        || (spell.MetaSpellType == SpellType.PortalRecall)
                        || (spell.MetaSpellType == SpellType.PortalSending)
                        || (spell.MetaSpellType == SpellType.PortalSummon))
                    {
                        PlayScript playScript;
                        if (spell.CasterEffect > 0)
                            playScript = (PlayScript)spell.CasterEffect;
                        else
                            playScript = (PlayScript)spell.TargetEffect;
                        EnqueueBroadcast(new GameMessageScript(player.Guid, playScript, scale));
                        enchantmentStatus = ItemMagic(player, spell, spellStatMod);
                    }
                    else
                    {
                        if ((spell.Category == (uint)SpellCategory.AttackModRaising)
                            || (spell.Category == (uint)SpellCategory.DamageRaising)
                            || (spell.Category == (uint)SpellCategory.DefenseModRaising)
                            || (spell.Category == (uint)SpellCategory.WeaponTimeRaising)
                            || (spell.Category == (uint)SpellCategory.AppraisalResistanceLowering)
                            || (spell.Category == (uint)SpellCategory.SpellDamageRaising))
                        {
                            enchantmentStatus = ItemMagic(player, spell, spellStatMod);
                        }

                        EnqueueBroadcast(new GameMessageScript(player.Guid, (PlayScript)spell.TargetEffect, scale));
                    }
                    break;

                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// Method used for handling items casting spells on the player who is either equiping the item, or using a gem in posessions
        /// </summary>
        /// <param name="guidItem">the GUID of the item casting the spell(s)</param>
        /// <param name="spellId">the spell id</param>
        /// <param name="suppressSpellChatText">prevent spell text from being sent to the player's chat windows (used for already affecting items during Player.EnterWorld)</param>
        /// <param name="ignoreRequirements">disregard item activation requirements (used for already affecting items during Player.EnterWorld)</param>
        /// <returns>FALSE - the spell was NOT created because the spell is invalid or not implemented yet, the item was not found, the item was not either wielded or a gem, or the player did not meet one or more item activation requirements. <para />TRUE - the spell was created or it is surpassed</returns>
        public bool CreateItemSpell(ObjectGuid guidItem, uint spellId, bool suppressSpellChatText = false, bool ignoreRequirements = false)
        {
            Player player = CurrentLandblock?.GetObject(Guid) as Player;
            if (player == null && ((this as Player) != null)) player = this as Player;
            WorldObject item = player.GetWieldedItem(guidItem);

            if (item == null)
            {
                item = player.GetInventoryItem(guidItem);
                if (item == null)
                    return false;
                if (item.WeenieType != WeenieType.Gem)
                    return false;
            }

            CreatureSkill arcaneLore = player.GetCreatureSkill(Skill.ArcaneLore);
            CreatureSkill meleeDefense = player.GetCreatureSkill(Skill.MeleeDefense);
            CreatureSkill missileDefense = player.GetCreatureSkill(Skill.MissileDefense);
            CreatureSkill magicDefense = player.GetCreatureSkill(Skill.MagicDefense);

            if (ignoreRequirements || arcaneLore.Current >= item.ItemDifficulty || item.ItemDifficulty == null)
            {
                if (!ignoreRequirements && (item.AppraisalItemSkill != 0 || item.AppraisalItemSkill != null))
                {
                    switch (item.AppraisalItemSkill)
                    {
                        case 6:
                            if (meleeDefense.Current < item.ItemSkillLevelLimit)
                                return false;
                            break;
                        case 7:
                            if (missileDefense.Current < item.ItemSkillLevelLimit)
                                return false;
                            break;
                        case 8:
                            if (magicDefense.Current < item.ItemSkillLevelLimit)
                                return false;
                            break;
                    }
                }

                SpellTable spellTable = DatManager.PortalDat.SpellTable;
                if (!spellTable.Spells.ContainsKey(spellId))
                {
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.MagicInvalidSpellType));
                    return false;
                }

                SpellBase spell = spellTable.Spells[spellId];

                Database.Models.World.Spell spellStatMod = DatabaseManager.World.GetCachedSpell(spellId);
                if (spellStatMod == null)
                {
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                    return false;
                }

                float scale = SpellAttributes(player.Session.Account, spellId, out float castingDelay, out MotionCommand windUpMotion, out MotionCommand spellGesture);

                EnchantmentStatus enchantmentStatus = default(EnchantmentStatus);
                bool created = false;
                switch (spell.School)
                {
                    case MagicSchool.CreatureEnchantment:
                        if (IsSpellHarmful(spell))
                            break;
                        enchantmentStatus = CreatureMagic(player, spell, spellStatMod, item);
                        created = true;
                        if (enchantmentStatus.message != null)
                        {
                            EnqueueBroadcast(new GameMessageScript(player.Guid, (PlayScript)spell.TargetEffect, scale));
                            if (!suppressSpellChatText)
                                player.Session.Network.EnqueueSend(enchantmentStatus.message);
                        }
                        break;
                    case MagicSchool.LifeMagic:
                        if (spell.MetaSpellType != SpellType.LifeProjectile)
                        {
                            if (IsSpellHarmful(spell))
                                break;
                        }
                        LifeMagic(player, spell, spellStatMod, out uint damage, out bool critical, out enchantmentStatus, item);
                        created = true;
                        if (enchantmentStatus.message != null)
                        {
                            EnqueueBroadcast(new GameMessageScript(player.Guid, (PlayScript)spell.TargetEffect, scale));
                            if (!suppressSpellChatText)
                                player.Session.Network.EnqueueSend(enchantmentStatus.message);
                        }
                        break;
                    case MagicSchool.ItemEnchantment:
                        if ((spell.MetaSpellType == SpellType.PortalLink)
                            || (spell.MetaSpellType == SpellType.PortalRecall)
                            || (spell.MetaSpellType == SpellType.PortalSending)
                            || (spell.MetaSpellType == SpellType.PortalSummon))
                        {
                            PlayScript playScript;
                            if (spell.CasterEffect > 0)
                                playScript = (PlayScript)spell.CasterEffect;
                            else
                                playScript = (PlayScript)spell.TargetEffect;
                            EnqueueBroadcast(new GameMessageScript(player.Guid, playScript, scale));
                            enchantmentStatus = ItemMagic(player, spell, spellStatMod, item);
                        }
                        else
                        {
                            if ((spell.Category == (uint)SpellCategory.AttackModRaising)
                                || (spell.Category == (uint)SpellCategory.DamageRaising)
                                || (spell.Category == (uint)SpellCategory.DefenseModRaising)
                                || (spell.Category == (uint)SpellCategory.WeaponTimeRaising)
                                || (spell.Category == (uint)SpellCategory.AppraisalResistanceLowering)
                                || (spell.Category == (uint)SpellCategory.SpellDamageRaising))
                            {
                                enchantmentStatus = ItemMagic(player, spell, spellStatMod, item);
                            }
                            else
                                enchantmentStatus = ItemMagic(item, spell, spellStatMod, item);

                            EnqueueBroadcast(new GameMessageScript(player.Guid, (PlayScript)spell.TargetEffect, scale));
                        }
                        created = true;
                        if (enchantmentStatus.message != null)
                        {
                            if (!suppressSpellChatText)
                                player.Session.Network.EnqueueSend(enchantmentStatus.message);
                        }
                        break;

                    default:
                        break;
                }
                return created;
            }
            return false;
        }

        /// <summary>
        /// Method for handling the removal of an item's spell from the Enchantment registry, silently
        /// </summary>
        /// <param name="guidItem"></param>
        /// <param name="spellId"></param>
        public void DispelItemSpell(ObjectGuid guidItem, uint spellId)
        {
            WorldObject item = GetWieldedItem(guidItem);

            if (item == null)
                return;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType: WeenieError.MagicInvalidSpellType));
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            if (spell.School == MagicSchool.ItemEnchantment)
            {
                if ((spell.Category == (uint)SpellCategory.AttackModRaising)
                    || (spell.Category == (uint)SpellCategory.DamageRaising)
                    || (spell.Category == (uint)SpellCategory.DefenseModRaising)
                    || (spell.Category == (uint)SpellCategory.WeaponTimeRaising)
                    || (spell.Category == (uint)SpellCategory.AppraisalResistanceLowering)
                    || (spell.Category == (uint)SpellCategory.SpellDamageRaising))
                {
                    // Retrieve enchantment on player and dispel it, if present
                    if (EnchantmentManager.HasSpell(spellId))
                        EnchantmentManager.Dispel(EnchantmentManager.GetSpell(spellId));
                }
                else
                {
                    // Retrieve enchantment on item and dispel it, if present
                    if (item.EnchantmentManager.HasSpell(spellId))
                        item.EnchantmentManager.Dispel(item.EnchantmentManager.GetSpell(spellId));
                }
            }
            else
            {
                // Retrieve enchantment on player and dispel it, if present
                if (EnchantmentManager.HasSpell(spellId))
                    EnchantmentManager.Dispel(EnchantmentManager.GetSpell(spellId));
            }
        }

        /// <summary>
        /// Method for handling the removal of an item's spell from the Enchantment registry
        /// </summary>
        /// <param name="guidItem"></param>
        /// <param name="spellId"></param>
        public void RemoveItemSpell(ObjectGuid guidItem, uint spellId)
        {
            WorldObject item = GetWieldedItem(guidItem);

            if (item == null)
                return;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType: WeenieError.MagicInvalidSpellType));
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            if (spell.School == MagicSchool.ItemEnchantment)
            {
                if ((spell.Category == (uint)SpellCategory.AttackModRaising)
                    || (spell.Category == (uint)SpellCategory.DamageRaising)
                    || (spell.Category == (uint)SpellCategory.DefenseModRaising)
                    || (spell.Category == (uint)SpellCategory.WeaponTimeRaising)
                    || (spell.Category == (uint)SpellCategory.AppraisalResistanceLowering)
                    || (spell.Category == (uint)SpellCategory.SpellDamageRaising))
                {
                    // Retrieve enchantment on player and remove it, if present
                    if (EnchantmentManager.HasSpell(spellId))
                        EnchantmentManager.Remove(EnchantmentManager.GetSpell(spellId));
                }
                else
                {
                    // Retrieve enchantment on item and remove it, if present
                    if (item.EnchantmentManager.HasSpell(spellId))
                        item.EnchantmentManager.Remove(item.EnchantmentManager.GetSpell(spellId));
                }
            }
            else
            {
                // Retrieve enchantment on player and remove it, if present
                if (EnchantmentManager.HasSpell(spellId))
                    EnchantmentManager.Remove(EnchantmentManager.GetSpell(spellId));
            }
        }

        private enum CastingPreCheckStatus
        {
            OutOfMana,
            OutOfOtherVital,
            CastFailed,
            InvalidPKStatus,
            Success
        }

        /// <summary>
        /// Method used for handling player targeted spell casts
        /// </summary>
        public void CreatePlayerSpell(WorldObject target, TargetCategory targetCategory, uint spellId)
        {
            CastingPreCheckStatus castingPreCheckStatus = CastingPreCheckStatus.CastFailed;

            Player player = CurrentLandblock?.GetObject(Guid) as Player;

            var creatureTarget = target as Creature;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.MagicInvalidSpellType));
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            if (IsInvalidTarget(spell, target))
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"{spell.Name} cannot be cast on {target.Name}."));
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.None));
                return;
            }

            Spell spellStatMod = DatabaseManager.World.GetCachedSpell(spellId);
            if (spellStatMod == null)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.MagicInvalidSpellType));
                return;
            }

            if (player.IsBusy == true)
            {
                player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YoureTooBusy));
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.None));
                return;
            }
            else
                player.IsBusy = true;

            uint targetEffect = spell.TargetEffect;

            // Grab player's skill level in the spell's Magic School
            var magicSkill = player.GetCreatureSkill(spell.School).Current;

            if (targetCategory == TargetCategory.WorldObject)
            {
                if (target.Guid != Guid)
                {
                    float distanceTo = Location.Distance2D(target.Location);

                    if (distanceTo > spell.BaseRangeConstant + magicSkill * spell.BaseRangeMod)
                    {
                        player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.None),
                            new GameMessageSystemChat($"Target is out of range!", ChatMessageType.Magic));
                        player.IsBusy = false;
                        return;
                    }
                }
            }

            float scale = SpellAttributes(player.Session.Account, spellId, out float castingDelay, out MotionCommand windUpMotion, out MotionCommand spellGesture);
            var formula = SpellTable.GetSpellFormula(spellTable, spellId, player.Session.Account);

            var difficulty = spell.Power;
            var difficultyMod = Math.Max(difficulty, 25);   // fix difficulty for level 1 spells?

            if (magicSkill > 0 && magicSkill >= (int)difficulty - 50)
            {
                var chance = 1.0f - SkillCheck.GetMagicSkillChance((int)magicSkill, (int)difficulty);
                var rng = Physics.Common.Random.RollDice(0.0f, 1.0f);
                if (chance < rng)
                    castingPreCheckStatus = CastingPreCheckStatus.Success;
                else
                    castingPreCheckStatus = CastingPreCheckStatus.CastFailed;
            }
            else
                castingPreCheckStatus = CastingPreCheckStatus.CastFailed;

            // Calculating mana usage
            #region
            uint manaUsed = CalculateManaUsage(player, spell, target);

            if (spell.MetaSpellType == SpellType.Transfer)
            {
                uint vitalChange, casterVitalChange;
                vitalChange = (uint)(player.GetCurrentCreatureVital((PropertyAttribute2nd)spellStatMod.Source) * spellStatMod.Proportion);
                if (spellStatMod.TransferCap != 0)
                {
                    if (vitalChange > spellStatMod.TransferCap)
                        vitalChange = (uint)spellStatMod.TransferCap;
                }
                casterVitalChange = (uint)(vitalChange * (1.0f - spellStatMod.LossPercent));
                vitalChange = (uint)(casterVitalChange / (1.0f - spellStatMod.LossPercent));

                if (spellStatMod.Source == (int)PropertyAttribute2nd.Mana && (vitalChange + 10 + manaUsed) > player.Mana.Current)
                    castingPreCheckStatus = CastingPreCheckStatus.OutOfMana;
                else if ((vitalChange + 10) > player.GetCurrentCreatureVital((PropertyAttribute2nd)spellStatMod.Source))
                    castingPreCheckStatus = CastingPreCheckStatus.OutOfOtherVital;
            }
            else if (manaUsed > player.Mana.Current)
                castingPreCheckStatus = CastingPreCheckStatus.OutOfMana;
            else
            {
                Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.ManaConversion), difficultyMod);
                player.UpdateVitalDelta(player.Mana, -(int)manaUsed);
            }
            #endregion

            var checkPKStatusVsTarget = CheckPKStatusVsTarget(player, (target as Player), spell);
            if (checkPKStatusVsTarget != null)
            {
                if (checkPKStatusVsTarget == false)
                    castingPreCheckStatus = CastingPreCheckStatus.InvalidPKStatus;
            }

            ActionChain spellChain = new ActionChain();

            uint fastCast = (spell.Bitfield >> 14) & 0x1u;
            if (fastCast != 1)
            {
                spellChain.AddAction(this, () =>
                {
                    var motionWindUp = new UniversalMotion(MotionStance.Magic);
                    motionWindUp.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Magic & 0xFFFF);
                    motionWindUp.MovementData.ForwardCommand = (uint)windUpMotion;
                    motionWindUp.MovementData.ForwardSpeed = 2;
                    EnqueueBroadcastMotion(motionWindUp);
                });
            }

            spellChain.AddAction(this, () =>
            {
                string spellWords = spell.SpellWords;
                if (spellWords != null)
                    EnqueueBroadcast(new GameMessageCreatureMessage(spellWords, Name, Guid.Full, ChatMessageType.Spellcasting));

                var motionCastSpell = new UniversalMotion(MotionStance.Magic);
                motionCastSpell.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Magic & 0xFFFF);
                motionCastSpell.MovementData.ForwardCommand = (uint)spellGesture;
                motionCastSpell.MovementData.ForwardSpeed = 2;
                EnqueueBroadcastMotion(motionCastSpell);
            });

            if (fastCast == 1)
            {
                spellChain.AddDelaySeconds(castingDelay * 0.26f);
            }
            else
                spellChain.AddDelaySeconds(castingDelay);

            switch (castingPreCheckStatus)
            {
                case CastingPreCheckStatus.Success:
                    spellChain.AddAction(this, () =>
                    {
                        bool targetDeath;
                        EnchantmentStatus enchantmentStatus = default(EnchantmentStatus);

                        switch (spell.School)
                        {
                            case MagicSchool.WarMagic:
                                WarMagic(target, spell, spellStatMod);
                                break;
                            case MagicSchool.VoidMagic:
                                VoidMagic(target, spell, spellStatMod);
                                break;
                            case MagicSchool.CreatureEnchantment:

                                if (player != null && !(target is Player))
                                    player.OnAttackMonster(creatureTarget);

                                if (IsSpellHarmful(spell))
                                {
                                    var resisted = ResistSpell(target, spell);
                                    if (resisted == true)
                                        break;
                                    if (resisted == null)
                                    {
                                        log.Error("Something went wrong with the Magic resistance check");
                                        break;
                                    }
                                }

                                EnqueueBroadcast(new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, scale));
                                enchantmentStatus = CreatureMagic(target, spell, spellStatMod);
                                if (enchantmentStatus.message != null)
                                    player.Session.Network.EnqueueSend(enchantmentStatus.message);

                                if (IsSpellHarmful(spell))
                                    Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.CreatureEnchantment), (target as Creature).GetCreatureSkill(Skill.MagicDefense).Current);
                                else
                                    Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.CreatureEnchantment), difficultyMod);

                                break;

                            case MagicSchool.LifeMagic:

                                if (player != null && !(target is Player))
                                    player.OnAttackMonster(creatureTarget);

                                if (spell.MetaSpellType != SpellType.LifeProjectile)
                                {
                                    if (IsSpellHarmful(spell))
                                    {
                                        var resisted = ResistSpell(target, spell);
                                        if (resisted == true)
                                            break;
                                        if (resisted == null)
                                        {
                                            log.Error("Something went wrong with the Magic resistance check");
                                            break;
                                        }
                                    }
                                }

                                EnqueueBroadcast(new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, scale));
                                targetDeath = LifeMagic(target, spell, spellStatMod, out uint damage, out bool critical, out enchantmentStatus);

                                if (spell.MetaSpellType != SpellType.LifeProjectile)
                                {
                                    if (IsSpellHarmful(spell))
                                        Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.LifeMagic), (target as Creature).GetCreatureSkill(Skill.MagicDefense).Current);
                                    else
                                        Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.LifeMagic), difficultyMod);
                                }

                                if (targetDeath == true)
                                {
                                    creatureTarget.Die();

                                    Strings.DeathMessages.TryGetValue(DamageType.Base, out var messages);
                                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(string.Format(messages[0], target.Name), ChatMessageType.Broadcast));

                                    if ((creatureTarget as Player) == null)
                                        player.EarnXP((long)target.XpOverride, true);
                                }
                                else
                                {
                                    if (enchantmentStatus.message != null)
                                        player.Session.Network.EnqueueSend(enchantmentStatus.message);
                                }
                                break;

                            case MagicSchool.ItemEnchantment:
                                if (((spell.Category >= (ushort)SpellCategory.ArmorValueRaising) && (spell.Category <= (ushort)SpellCategory.AcidicResistanceLowering)) == false)
                                {
                                    // Non-impen/bane spells
                                    enchantmentStatus = ItemMagic(target, spell, spellStatMod);
                                    if (target.Guid == Guid)
                                        EnqueueBroadcast(new GameMessageScript(Guid, (PlayScript)spell.CasterEffect, scale));
                                    else
                                    {
                                        if (spell.MetaSpellType == SpellType.PortalLink)
                                            EnqueueBroadcast(new GameMessageScript(Guid, (PlayScript)spell.CasterEffect, scale));
                                        else
                                            EnqueueBroadcast(new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, scale));
                                    }
                                    if (enchantmentStatus.message != null)
                                        player.Session.Network.EnqueueSend(enchantmentStatus.message);
                                }
                                else
                                {
                                    if ((target as Player) == null)
                                    {
                                        // Individual impen/bane WeenieType.Clothing target
                                        enchantmentStatus = ItemMagic(target, spell, spellStatMod);
                                        if (target.Guid == Guid)
                                            EnqueueBroadcast(new GameMessageScript(Guid, (PlayScript)spell.CasterEffect, scale));
                                        else
                                            EnqueueBroadcast(new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, scale));
                                        if (enchantmentStatus.message != null)
                                            player.Session.Network.EnqueueSend(enchantmentStatus.message);
                                    }
                                    else
                                    {
                                        // Impen/bane targeted at a player
                                        var items = (target as Player).GetAllWieldedItems();
                                        foreach (var item in items)
                                        {
                                            if (item.WeenieType == WeenieType.Clothing)
                                            {
                                                enchantmentStatus = ItemMagic(item, spell, spellStatMod);
                                                EnqueueBroadcast(new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, scale));
                                                if (enchantmentStatus.message != null)
                                                    player.Session.Network.EnqueueSend(enchantmentStatus.message);
                                            }
                                        }
                                    }
                                }
                                Proficiency.OnSuccessUse(player, player.GetCreatureSkill(Skill.ItemEnchantment), difficultyMod);
                                break;
                            default:
                                break;
                        }
                    });
                    break;
                case CastingPreCheckStatus.InvalidPKStatus:
                    spellChain.AddAction(this, () =>
                    {
                        switch (spell.School)
                        {
                            case MagicSchool.WarMagic:
                                WarMagic(target, spell, spellStatMod);
                                break;
                            case MagicSchool.VoidMagic:
                                VoidMagic(target, spell, spellStatMod);
                                break;
                            case MagicSchool.ItemEnchantment:
                                // Do nothing
                                break;
                            default:
                                EnqueueBroadcast(new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, scale));
                                break;
                        }
                    });
                    break;
                default:
                    spellChain.AddAction(this, () => EnqueueBroadcast(new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Fizzle, 0.5f)));
                    break;
            }

            spellChain.AddAction(this, () =>
            {
                var motionReturnToCastStance = new UniversalMotion(MotionStance.Magic);
                motionReturnToCastStance.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Magic & 0xFFFF);
                motionReturnToCastStance.MovementData.ForwardCommand = (uint)MotionCommand.Invalid;
                EnqueueBroadcastMotion(motionReturnToCastStance);
            });

            switch (castingPreCheckStatus)
            {
                case CastingPreCheckStatus.InvalidPKStatus:
                    if (spell.School == MagicSchool.LifeMagic || spell.School == MagicSchool.CreatureEnchantment || spell.School == MagicSchool.ItemEnchantment)
                        spellChain.AddAction(this, () => player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.InvalidPkStatus)));
                    else
                        spellChain.AddAction(this, () => player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.None)));
                    break;
                case CastingPreCheckStatus.OutOfMana:
                    spellChain.AddAction(this, () => player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.YouDontHaveEnoughManaToCast)));
                    break;
                default:
                    spellChain.AddAction(this, () => player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, WeenieError.None)));
                    break;
            }

            spellChain.AddDelaySeconds(1.0f);
            spellChain.AddAction(this, () => player.IsBusy = false);
            spellChain.EnqueueChain();

            return;
        }

        /// <summary>
        /// Method used for handling player untargeted spell casts
        /// </summary>
        public void CreatePlayerSpell(uint spellId)
        {
            CastingPreCheckStatus castingPreCheckStatus = CastingPreCheckStatus.CastFailed;

            Player player = CurrentLandblock?.GetObject(Guid) as Player;

            if (IsBusy == true)
            {
                Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType: WeenieError.YoureTooBusy));
                return;
            }
            else
                IsBusy = true;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType: WeenieError.MagicInvalidSpellType));
                IsBusy = false;
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            Spell spellStatMod = DatabaseManager.World.GetCachedSpell(spellId);
            if (spellStatMod == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType: WeenieError.MagicInvalidSpellType));
                IsBusy = false;
                return;
            }

            // Grab player's skill level in the spell's Magic School
            var magicSkill = GetCreatureSkill(spell.School).Current;

            float scale = SpellAttributes(Session.Account, spellId, out float castingDelay, out MotionCommand windUpMotion, out MotionCommand spellGesture);
            var formula = SpellTable.GetSpellFormula(spellTable, spellId, Session.Account);

            if ((Physics.Common.Random.RollDice(0.0f, 1.0f) > (1.0f - SkillCheck.GetMagicSkillChance((int)magicSkill, (int)spell.Power)))
                && (magicSkill >= (int)spell.Power - 50) && (magicSkill > 0))
                castingPreCheckStatus = CastingPreCheckStatus.Success;
            else
                castingPreCheckStatus = CastingPreCheckStatus.CastFailed;

            // Calculating mana usage
            #region
            uint manaUsed = CalculateManaUsage(player, spell);

            if (spell.MetaSpellType == SpellType.Transfer)
            {
                uint vitalChange, casterVitalChange;
                vitalChange = (uint)(GetCurrentCreatureVital((PropertyAttribute2nd)spellStatMod.Source) * spellStatMod.Proportion);
                if (spellStatMod.TransferCap != 0)
                {
                    if (vitalChange > spellStatMod.TransferCap)
                        vitalChange = (uint)spellStatMod.TransferCap;
                }
                casterVitalChange = (uint)(vitalChange * (1.0f - spellStatMod.LossPercent));
                vitalChange = (uint)(casterVitalChange / (1.0f - spellStatMod.LossPercent));

                if (spellStatMod.Source == (int)PropertyAttribute2nd.Mana && (vitalChange + 10 + manaUsed) > Mana.Current)
                    castingPreCheckStatus = CastingPreCheckStatus.OutOfMana;
                else if ((vitalChange + 10) > GetCurrentCreatureVital((PropertyAttribute2nd)spellStatMod.Source))
                    castingPreCheckStatus = CastingPreCheckStatus.OutOfOtherVital;
            }
            else if (manaUsed > Mana.Current)
                castingPreCheckStatus = CastingPreCheckStatus.OutOfMana;
            else
                player.UpdateVital(Mana, Mana.Current - manaUsed);
            #endregion

            ActionChain spellChain = new ActionChain();

            uint fastCast = (spell.Bitfield >> 14) & 0x1u;
            if (fastCast != 1)
            {
                spellChain.AddAction(this, () =>
                {
                    var motionWindUp = new UniversalMotion(MotionStance.Magic);
                    motionWindUp.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Magic & 0xFFFF);
                    motionWindUp.MovementData.ForwardCommand = (uint)windUpMotion;
                    motionWindUp.MovementData.ForwardSpeed = 2;
                    EnqueueBroadcastMotion(motionWindUp);
                });
            }

            spellChain.AddAction(this, () =>
            {
                string spellWords = spell.SpellWords;
                if (spellWords != null)
                    EnqueueBroadcast(new GameMessageCreatureMessage(spellWords, Name, Guid.Full, ChatMessageType.Magic));

                var motionCastSpell = new UniversalMotion(MotionStance.Magic);
                motionCastSpell.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Magic & 0xFFFF);
                motionCastSpell.MovementData.ForwardCommand = (uint)spellGesture;
                motionCastSpell.MovementData.ForwardSpeed = 2;
                EnqueueBroadcastMotion(motionCastSpell);
            });

            if (fastCast == 1)
            {
                spellChain.AddDelaySeconds(castingDelay * 0.26f);
            }
            else
                spellChain.AddDelaySeconds(castingDelay);

            switch (castingPreCheckStatus)
            {
                case CastingPreCheckStatus.Success:
                    // TODO - Add other untargeted spells below
                    spellChain.AddAction(this, () =>
                    {
                        switch (spell.School)
                        {
                            case MagicSchool.WarMagic:
                                WarMagic(spell, spellStatMod);
                                break;
                            default:
                                Session.Network.EnqueueSend(new GameMessageSystemChat("Untargeted SpellID " + spellId + " not yet implemented!", ChatMessageType.System));
                                break;
                        }
                    });
                    break;
                default:
                    spellChain.AddAction(this, () => EnqueueBroadcast(new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Fizzle, 0.5f)));
                    break;
            }

            spellChain.AddAction(this, () =>
            {
                var motionReturnToCastStance = new UniversalMotion(MotionStance.Magic);
                motionReturnToCastStance.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Magic & 0xFFFF);
                motionReturnToCastStance.MovementData.ForwardCommand = (uint)MotionCommand.Invalid;
                EnqueueBroadcastMotion(motionReturnToCastStance);
            });

            switch (castingPreCheckStatus)
            {
                case CastingPreCheckStatus.OutOfMana:
                    spellChain.AddAction(this, () => player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.YouDontHaveEnoughManaToCast)));
                    break;
                default:
                    spellChain.AddAction(this, () => player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.None)));
                    break;
            }

            spellChain.AddDelaySeconds(1.0f);
            spellChain.AddAction(this, () => player.IsBusy = false);
            spellChain.EnqueueChain();

            return;
        }

        public void CreateSentinelBuffPlayers(IEnumerable<Player> players, bool self = false)
        {
            if (!(Session.AccessLevel >= AccessLevel.Sentinel)) return;

            var SelfOrOther = self ? "Self" : "Other";
            var maxSpellLevel = (DatabaseManager.World.GetCachedSpell((uint)Network.Enum.Spell.ArmorOther8) == null) ? "7" : "8";
            var tySpell = typeof(Network.Enum.Spell);
            List<BuffMessage> buffMessages = new List<BuffMessage>();
            // prepare messages
            List<string> buffsNotImplementedYet = new List<string>();
            foreach (var spell in Buffs)
            {
                var spellNamPrefix = spell;
                bool isBane = false;
                if (spellNamPrefix.StartsWith("@"))
                {
                    isBane = true;
                    spellNamPrefix = spellNamPrefix.Substring(1);
                }
                string fullSpellEnumName = spellNamPrefix + ((isBane) ? string.Empty : SelfOrOther) + maxSpellLevel;
                string fullSpellEnumNameAlt = spellNamPrefix + ((isBane) ? string.Empty : ((SelfOrOther == "Self") ? "Other" : "Self")) + maxSpellLevel;
                uint spellID = (uint)Enum.Parse(tySpell, fullSpellEnumName);
                var buffMsg = BuildBuffMessage(spellID);

                if (buffMsg == null)
                {
                    spellID = (uint)Enum.Parse(tySpell, fullSpellEnumNameAlt);
                    buffMsg = BuildBuffMessage(spellID);
                }

                if (buffMsg != null)
                {
                    buffMsg.Bane = isBane;
                    buffMessages.Add(buffMsg);
                }
                else
                {
                    buffsNotImplementedYet.Add(fullSpellEnumName);
                }
            }
            // buff each player
            players.ToList().ForEach(targetPlayer =>
            {
                if (buffMessages.Any(k => !k.Bane))
                {
                    // bake player into the messages
                    buffMessages.Where(k => !k.Bane).ToList().ForEach(k => k.SetTargetPlayer(targetPlayer));
                    // update client-side enchantments
                    targetPlayer.Session.Network.EnqueueSend(buffMessages.Where(k => !k.Bane).Select(k => k.SessionMessage).ToArray());
                    // run client-side effect scripts, omitting duplicates
                    targetPlayer.EnqueueBroadcast(buffMessages.Where(k => !k.Bane).ToList().GroupBy(m => m.SpellBase.TargetEffect).Select(a => a.First().LandblockMessage).ToArray());
                    // update server-side enchantments

                    var buffsForPlayer = buffMessages.Where(k => !k.Bane).ToList().Select(k => k.Enchantment);

                    var lifeBuffsForPlayer = buffsForPlayer.Where(k => ((MagicSchool)k.Spell.School) == MagicSchool.LifeMagic).ToList();
                    var critterBuffsForPlayer = buffsForPlayer.Where(k => ((MagicSchool)k.Spell.School) == MagicSchool.CreatureEnchantment).ToList();
                    var itemBuffsForPlayer = buffsForPlayer.Where(k => ((MagicSchool)k.Spell.School) == MagicSchool.ItemEnchantment).ToList();

                    bool crit = false;
                    uint dmg = 0;
                    EnchantmentStatus ec;
                    lifeBuffsForPlayer.ForEach(spl =>
                    {
                        bool casted = targetPlayer.LifeMagic(targetPlayer, spl.SpellBase, spl.Spell, out dmg, out crit, out ec, this);
                    });
                    critterBuffsForPlayer.ForEach(spl =>
                    {
                        ec = targetPlayer.CreatureMagic(targetPlayer, spl.SpellBase, spl.Spell, this);
                    });
                    itemBuffsForPlayer.ForEach(spl =>
                    {
                        ec = targetPlayer.ItemMagic(targetPlayer, spl.SpellBase, spl.Spell, this);
                    });
                }
                if (buffMessages.Any(k => k.Bane))
                {
                    // Impen/bane
                    var items = (targetPlayer as Player).GetAllWieldedItems();
                    var itembuffs = buffMessages.Where(k => k.Bane).ToList();
                    foreach (var itemBuff in itembuffs)
                    {
                        foreach (var item in items)
                        {
                            if (item.WeenieType == WeenieType.Clothing || item.IsShield)
                            {
                                itemBuff.SetLandblockMessage(item.Guid);
                                var enchantmentStatus = targetPlayer.ItemMagic(item, itemBuff.SpellBase, itemBuff.Spell, this);
                                targetPlayer?.EnqueueBroadcast(itemBuff.LandblockMessage);
                            }
                        }
                    }
                }
            });
        }
        private static string[] Buffs = new string[] {
#region spells
            // @ indicates impenetrability or a bane
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
            "SummoningMastery",
            "SwiftKiller",
            "Defender",
            "BloodDrinker",
            "HeartSeeker",
            "TrueValue", // aura of mystic's blessing
            "SpiritDrinker",
            "@Impenetrability",
            "@PiercingBane",
            "@BludgeonBane",
            "@BladeBane",
            "@AcidBane",
            "@FlameBane",
            "@FrostBane",
            "@LightningBane",
#endregion
            };

        private class BuffMessage
        {
            public bool Bane { get; set; } = false;
            public GameEventMagicUpdateEnchantment SessionMessage { get; set; } = null;
            public GameMessageScript LandblockMessage { get; set; } = null;
            public SpellBase SpellBase { get; set; } = null;
            public Spell Spell { get; set; } = null;
            public Enchantment Enchantment { get; set; } = null;
            public void SetTargetPlayer(Player p)
            {
                Enchantment.Target = p;
                SessionMessage = new GameEventMagicUpdateEnchantment(p.Session, Enchantment);
                SetLandblockMessage(p.Guid);
            }
            public void SetLandblockMessage(ObjectGuid target)
            {
                LandblockMessage = new GameMessageScript(target, (PlayScript)SpellBase.TargetEffect, 1f);
            }
        }

        private static BuffMessage BuildBuffMessage(uint spellID)
        {
            BuffMessage buff = new BuffMessage();
            if (spellID < 1) throw new Exception("spell not found");
            buff.SpellBase = DatManager.PortalDat.SpellTable.Spells[spellID]; if (buff.SpellBase == null) return null; // the portal data doesn't have the spell, throw here instead?
            buff.Spell = DatabaseManager.World.GetCachedSpell(spellID); if (buff.Spell == null) return null; // the database doesn't have the spell
            buff.Enchantment = new Enchantment(null, null, spellID, (double)buff.Spell.Duration, 1, buff.Spell.StatModType, buff.Spell.StatModVal);
            return buff;
        }
    }
}

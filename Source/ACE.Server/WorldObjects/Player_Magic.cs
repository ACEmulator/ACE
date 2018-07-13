using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.Motion;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Database;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        private enum TargetCategory
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
            WorldObject target = CurrentLandblock?.GetObject(guidTarget) as WorldObject;
            TargetCategory targetCategory = TargetCategory.WorldObject;

            if (guidTarget == Guid)
                targetCategory = TargetCategory.Self;
            if (target == null)
            {
                target = GetWieldedItem(guidTarget);
                targetCategory = TargetCategory.Wielded;
            }
            if (target == null)
            {
                target = GetInventoryItem(guidTarget);
                targetCategory = TargetCategory.Inventory;
            }
            if (target == null)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.TargetNotAcquired));
                targetCategory = TargetCategory.UnDef;
                return;
            }

            if (targetCategory != TargetCategory.WorldObject)
            {
                CreatePlayerSpell(guidTarget, spellId);
            }
            else
            {
                // turn if required
                var rotateTime = Rotate(target) - 0.25f;
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(rotateTime);

                actionChain.AddAction(this, () => CreatePlayerSpell(guidTarget, spellId));
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
                        if ((uint)CalculateSpellLevel(spell.Power) == spellLevel)
                            player.LearnSpellWithNetworking(spell.MetaSpellId, false);
                    }
                }
            }
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
                            CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(player.Guid, (PlayScript)spell.TargetEffect, scale));
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
                            CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(player.Guid, (PlayScript)spell.TargetEffect, scale));
                            if (!suppressSpellChatText)
                                player.Session.Network.EnqueueSend(enchantmentStatus.message);
                        }
                        break;
                    case MagicSchool.ItemEnchantment:
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
                        created = true;
                        if (enchantmentStatus.message != null)
                        {
                            CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(player.Guid, (PlayScript)spell.TargetEffect, scale));
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

            // Retrieve enchantment
            if (EnchantmentManager.HasSpell(spellId))
                EnchantmentManager.Dispel(EnchantmentManager.GetSpell(spellId));
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

            // Retrieve enchantment
            if (EnchantmentManager.HasSpell(spellId))
                EnchantmentManager.Remove(EnchantmentManager.GetSpell(spellId));
        }

        /// <summary>
        /// Method used for handling player targeted spell casts
        /// </summary>
        public void CreatePlayerSpell(ObjectGuid guidTarget, uint spellId)
        {
            Player player = CurrentLandblock?.GetObject(Guid) as Player;
            WorldObject target = CurrentLandblock?.GetObject(guidTarget);
            TargetCategory targetCategory = TargetCategory.WorldObject;

            if (target == null)
            {
                target = GetWieldedItem(guidTarget);
                targetCategory = TargetCategory.Wielded;
            }
            if (target == null)
            {
                target = GetInventoryItem(guidTarget);
                targetCategory = TargetCategory.Inventory;
            }
            if (target == null)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.TargetNotAcquired));
                targetCategory = TargetCategory.UnDef;
                return;
            }
            var creatureTarget = target as Creature;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.MagicInvalidSpellType));
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            if (IsInvalidTarget(spell, target))
            {
                player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"{spell.Name} cannot be cast on {target.Name}."));
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.None));
                return;
            }

            Database.Models.World.Spell spellStatMod = DatabaseManager.World.GetCachedSpell(spellId);
            if (spellStatMod == null)
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.MagicInvalidSpellType));
                return;
            }

            if (player.IsBusy == true)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.YoureTooBusy));
                return;
            }
            else
                player.IsBusy = true;

            uint targetEffect = spell.TargetEffect;

            // Grab player's skill level in the spell's Magic School
            var magicSkill = player.GetCreatureSkill(spell.School).Current;

            if (targetCategory == TargetCategory.WorldObject)
            {
                if (guidTarget != Guid)
                {
                    float distanceTo = Location.Distance2D(target.Location);

                    if (distanceTo > spell.BaseRangeConstant + magicSkill * spell.BaseRangeMod)
                    {
                        player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.MagicTargetOutOfRange),
                            new GameMessageSystemChat($"{target.Name} is out of range!", ChatMessageType.Magic));
                        player.IsBusy = false;
                        return;
                    }
                }
            }

            bool isSpellHarmful = IsSpellHarmful(spell);
            if (isSpellHarmful)
            {
                // Ensure that a non-PK cannot cast harmful spells on another player
                if ((target.WeenieClassId == 1) && (player.PlayerKillerStatus == ACE.Entity.Enum.PlayerKillerStatus.NPK))
                {
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.InvalidPkStatus));
                    player.IsBusy = false;
                    return;
                }

                // Ensure that a harmful spell isn't being cast on another player that doesn't have the same PK status
                if ((target.WeenieClassId == 1) && (player.PlayerKillerStatus != ACE.Entity.Enum.PlayerKillerStatus.NPK))
                {
                    if (player.PlayerKillerStatus != target.PlayerKillerStatus)
                    {
                        player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.InvalidPkStatus));
                        player.IsBusy = false;
                        return;
                    }
                }
            }

            float scale = SpellAttributes(player.Session.Account, spellId, out float castingDelay, out MotionCommand windUpMotion, out MotionCommand spellGesture);
            var formula = SpellTable.GetSpellFormula(spellTable, spellId, player.Session.Account);

            bool spellCastSuccess = false || ((Physics.Common.Random.RollDice(0.0f, 1.0f) > (1.0f - SkillCheck.GetMagicSkillChance((int)magicSkill, (int)spell.Power)))
                && (magicSkill >= (int)spell.Power - 50) && (magicSkill > 0));

            // Calculating mana usage
            #region
            CreatureSkill mc = player.GetCreatureSkill(Skill.ManaConversion);
            double z = mc.Current;
            double baseManaPercent = 1;
            if (z > spell.Power)
            {
                baseManaPercent = spell.Power / z;
            }
            double preCost;
            uint manaUsed;
            if ((int)Math.Floor(baseManaPercent) == 1)
            {
                preCost = spell.BaseMana;
                manaUsed = (uint)preCost;
            }
            else
            {
                preCost = spell.BaseMana * baseManaPercent;
                if (preCost < 1)
                    preCost = 1;
                manaUsed = (uint)Physics.Common.Random.RollDice(1, (int)preCost);
            }
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
                {
                    ActionChain resourceCheckChain = new ActionChain();

                    resourceCheckChain.AddAction(this, () =>
                    {
                        CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Fizzle, 0.5f));
                    });

                    resourceCheckChain.AddDelaySeconds(2.0f);
                    resourceCheckChain.AddAction(this, () =>
                    {
                        player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.YouDontHaveEnoughManaToCast));
                        player.IsBusy = false;
                    });

                    resourceCheckChain.EnqueueChain();

                    return;
                }
                else if ((vitalChange + 10) > player.GetCurrentCreatureVital((PropertyAttribute2nd)spellStatMod.Source))
                {
                    ActionChain resourceCheckChain = new ActionChain();

                    resourceCheckChain.AddAction(this, () =>
                    {
                        CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Fizzle, 0.5f));
                    });

                    resourceCheckChain.AddDelaySeconds(2.0f);
                    resourceCheckChain.AddAction(this, () => player.IsBusy = false);
                    resourceCheckChain.EnqueueChain();

                    return;
                }
            }
            else if (manaUsed > player.Mana.Current)
            {
                ActionChain resourceCheckChain = new ActionChain();

                resourceCheckChain.AddAction(this, () =>
                {
                    CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Fizzle, 0.5f));
                });

                resourceCheckChain.AddDelaySeconds(2.0f);
                resourceCheckChain.AddAction(this, () =>
                {
                    player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.YouDontHaveEnoughManaToCast));
                    player.IsBusy = false;
                });

                resourceCheckChain.EnqueueChain();

                return;
            }
            else
                player.UpdateVital(player.Mana, player.Mana.Current - manaUsed);
            #endregion

            ActionChain spellChain = new ActionChain();

            uint fastCast = (spell.Bitfield >> 14) & 0x1u;
            if (fastCast != 1)
            {
                spellChain.AddAction(this, () =>
                {
                    var motionWindUp = new UniversalMotion(MotionStance.Spellcasting);
                    motionWindUp.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Spellcasting & 0xFFFF);
                    motionWindUp.MovementData.ForwardCommand = (uint)windUpMotion;
                    motionWindUp.MovementData.ForwardSpeed = 2;
                    DoMotion(motionWindUp);
                });
            }


            spellChain.AddAction(this, () =>
            {
                string spellWords = SpellComponentsTable.GetSpellWords(DatManager.PortalDat.SpellComponentsTable, formula);
                if (spellWords != null)
                    CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageCreatureMessage(spellWords, Name, Guid.Full, ChatMessageType.Magic));
            });

            spellChain.AddAction(this, () =>
            {
                var motionCastSpell = new UniversalMotion(MotionStance.Spellcasting);
                motionCastSpell.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Spellcasting & 0xFFFF);
                motionCastSpell.MovementData.ForwardCommand = (uint)spellGesture;
                motionCastSpell.MovementData.ForwardSpeed = 2;
                DoMotion(motionCastSpell);
            });

            if (fastCast == 1)
            {
                spellChain.AddDelaySeconds(castingDelay * 0.26f);
            }
            else
                spellChain.AddDelaySeconds(castingDelay);

            if (spellCastSuccess == false)
                spellChain.AddAction(this, () => CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Fizzle, 0.5f)));
            else
            {
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
                                // Retrieve player's skill level in the Magic School
                                var playerMagicSkill = player.GetCreatureSkill(spell.School).Current;

                                // Retrieve target's Magic Defense Skill
                                Creature creature = (Creature)target;
                                var targetMagicDefenseSkill = creature.GetCreatureSkill(Skill.MagicDefense).Current;

                                if (MagicDefenseCheck(playerMagicSkill, targetMagicDefenseSkill))
                                {
                                    CurrentLandblock?.EnqueueBroadcastSound(player, Sound.ResistSpell);
                                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{creature.Name} resists {spell.Name}", ChatMessageType.Magic));
                                    break;
                                }
                            }
                            CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, scale));
                            enchantmentStatus = CreatureMagic(target, spell, spellStatMod);
                            if (enchantmentStatus.message != null)
                                player.Session.Network.EnqueueSend(enchantmentStatus.message);
                            break;

                        case MagicSchool.LifeMagic:

                            if (player != null && !(target is Player))
                                player.OnAttackMonster(creatureTarget);

                            if (spell.MetaSpellType != SpellType.LifeProjectile)
                            {
                                if (IsSpellHarmful(spell))
                                {
                                    // Retrieve player's skill level in the Magic School
                                    var playerMagicSkill = player.GetCreatureSkill(spell.School).Current;

                                    // Retrieve target's Magic Defense Skill
                                    Creature creature = (Creature)target;
                                    var targetMagicDefenseSkill = creature.GetCreatureSkill(Skill.MagicDefense).Current;

                                    if (MagicDefenseCheck(playerMagicSkill, targetMagicDefenseSkill))
                                    {
                                        CurrentLandblock?.EnqueueBroadcastSound(player, Sound.ResistSpell);
                                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{creature.Name} resists {spell.Name}", ChatMessageType.Magic));
                                        break;
                                    }
                                }
                            }

                            CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, scale));
                            targetDeath = LifeMagic(target, spell, spellStatMod, out uint damage, out bool critical, out enchantmentStatus);

                            AttackList.Add(new AttackDamage(player, damage, critical));

                            if (enchantmentStatus.message != null)
                                player.Session.Network.EnqueueSend(enchantmentStatus.message);
                            if (targetDeath == true)
                            {
                                creatureTarget.UpdateVital(creatureTarget.Health, 0);
                                creatureTarget.OnDeath();
                                creatureTarget.Die();

                                Strings.DeathMessages.TryGetValue(DamageType.Base, out var messages);
                                player.Session.Network.EnqueueSend(new GameMessageSystemChat(string.Format(messages[0], target.Name), ChatMessageType.Broadcast));

                                var topDamager = AttackDamage.GetTopDamager(AttackList);
                                if (topDamager != null)
                                    creatureTarget.Killer = topDamager.Guid.Full;

                                if ((creatureTarget as Player) == null)
                                    player.EarnXP((long)target.XpOverride);
                            }
                            break;
                        case MagicSchool.ItemEnchantment:
                            enchantmentStatus = ItemMagic(target, spell, spellStatMod);
                            if (guidTarget == Guid)
                                CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(Guid, (PlayScript)spell.CasterEffect, scale));
                            else
                                CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, scale));
                            if (enchantmentStatus.message != null)
                                player.Session.Network.EnqueueSend(enchantmentStatus.message);
                            break;
                        default:
                            break;
                    }
                });
            }

            spellChain.AddAction(this, () =>
            {
                var motionReturnToCastStance = new UniversalMotion(MotionStance.Spellcasting);
                motionReturnToCastStance.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Spellcasting & 0xFFFF);
                motionReturnToCastStance.MovementData.ForwardCommand = (uint)MotionCommand.Invalid;
                DoMotion(motionReturnToCastStance);
            });

            spellChain.AddDelaySeconds(1.0f);

            spellChain.AddAction(this, () =>
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.None));
                player.IsBusy = false;
            });

            spellChain.EnqueueChain();

            return;
        }

        /// <summary>
        /// Method used for handling player untargeted spell casts
        /// </summary>
        public void CreatePlayerSpell(uint spellId)
        {
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

            Database.Models.World.Spell spellStatMod = DatabaseManager.World.GetCachedSpell(spellId);
            if (spellStatMod == null)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType: WeenieError.MagicInvalidSpellType));
                IsBusy = false;
                return;
            }

            // Grab player's skill level in the spell's Magic School
            var magicSkill = GetCreatureSkill(spell.School);

            float scale = SpellAttributes(Session.Account, spellId, out float castingDelay, out MotionCommand windUpMotion, out MotionCommand spellGesture);
            var formula = SpellTable.GetSpellFormula(spellTable, spellId, Session.Account);

            bool spellCastSuccess = false || ((Physics.Common.Random.RollDice(0.0f, 1.0f) > (1.0f - SkillCheck.GetMagicSkillChance((int)magicSkill.Current, (int)spell.Power)))
                && (magicSkill.Current >= (int)spell.Power - 50) && (magicSkill.Current > 0));

            // Calculating mana usage
            #region
            if (spellCastSuccess == true)
            {
                CreatureSkill mc = GetCreatureSkill(Skill.ManaConversion);
                double z = mc.Current;
                double baseManaPercent = 1;
                if (z > spell.Power)
                {
                    baseManaPercent = spell.Power / z;
                }
                double preCost;
                uint manaUsed;
                if ((int)Math.Floor(baseManaPercent) == 1)
                {
                    preCost = spell.BaseMana;
                    manaUsed = (uint)preCost;
                }
                else
                {
                    preCost = spell.BaseMana * baseManaPercent;
                    if (preCost < 1)
                        preCost = 1;
                    manaUsed = (uint)Physics.Common.Random.RollDice(1, (int)preCost);
                }
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
                    {
                        ActionChain resourceCheckChain = new ActionChain();

                        resourceCheckChain.AddAction(this, () =>
                        {
                            CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Fizzle, 0.5f));
                        });

                        resourceCheckChain.AddDelaySeconds(2.0f);
                        resourceCheckChain.AddAction(this, () =>
                        {
                            Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType: WeenieError.YouDontHaveEnoughManaToCast));
                            IsBusy = false;
                        });

                        resourceCheckChain.EnqueueChain();

                        return;
                    }

                    if ((vitalChange + 10) > GetCurrentCreatureVital((PropertyAttribute2nd)spellStatMod.Source))
                    {
                        ActionChain resourceCheckChain = new ActionChain();

                        resourceCheckChain.AddAction(this, () =>
                        {
                            CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Fizzle, 0.5f));
                        });

                        resourceCheckChain.AddDelaySeconds(2.0f);
                        resourceCheckChain.AddAction(this, () => IsBusy = false);
                        resourceCheckChain.EnqueueChain();

                        return;
                    }
                }
                else if (manaUsed > Mana.Current)
                {
                    ActionChain resourceCheckChain = new ActionChain();

                    resourceCheckChain.AddAction(this, () =>
                    {
                        CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Fizzle, 0.5f));
                    });

                    resourceCheckChain.AddDelaySeconds(2.0f);
                    resourceCheckChain.AddAction(this, () =>
                    {
                        Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType: WeenieError.YouDontHaveEnoughManaToCast));
                        IsBusy = false;
                    });

                    resourceCheckChain.EnqueueChain();

                    return;
                }
                else
                    UpdateVital(Mana, Mana.Current - manaUsed);
            }
            #endregion

            ActionChain spellChain = new ActionChain();

            uint fastCast = (spell.Bitfield >> 14) & 0x1u;
            if (fastCast != 1)
            {
                spellChain.AddAction(this, () =>
                {
                    var motionWindUp = new UniversalMotion(MotionStance.Spellcasting);
                    motionWindUp.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Spellcasting & 0xFFFF);
                    motionWindUp.MovementData.ForwardCommand = (uint)windUpMotion;
                    motionWindUp.MovementData.ForwardSpeed = 2;
                    DoMotion(motionWindUp);
                });
            }

            spellChain.AddAction(this, () =>
            {
                CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageCreatureMessage(SpellComponentsTable.GetSpellWords(DatManager.PortalDat.SpellComponentsTable,
                    formula), Name, Guid.Full, ChatMessageType.Magic));
            });

            spellChain.AddAction(this, () =>
            {
                var motionCastSpell = new UniversalMotion(MotionStance.Spellcasting);
                motionCastSpell.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Spellcasting & 0xFFFF);
                motionCastSpell.MovementData.ForwardCommand = (uint)spellGesture;
                motionCastSpell.MovementData.ForwardSpeed = 2;
                DoMotion(motionCastSpell);
            });

            if (fastCast == 1)
            {
                spellChain.AddDelaySeconds(castingDelay * 0.26f);
            }
            else
                spellChain.AddDelaySeconds(castingDelay);

            if (spellCastSuccess == false)
                spellChain.AddAction(this, () => CurrentLandblock?.EnqueueBroadcast(Location, new GameMessageScript(Guid, ACE.Entity.Enum.PlayScript.Fizzle, 0.5f)));
            else
            {
                // TODO - Successful spell casting code goes here for untargeted spells to replace line below
                Session.Network.EnqueueSend(new GameMessageSystemChat("Targeted SpellID " + spellId + " not yet implemented!", ChatMessageType.System));
            }

            spellChain.AddAction(this, () =>
            {
                var motionReturnToCastStance = new UniversalMotion(MotionStance.Spellcasting);
                motionReturnToCastStance.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Spellcasting & 0xFFFF);
                motionReturnToCastStance.MovementData.ForwardCommand = (uint)MotionCommand.Invalid;
                DoMotion(motionReturnToCastStance);
            });

            spellChain.AddDelaySeconds(1.0f);

            spellChain.AddAction(this, () =>
            {
                Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType: WeenieError.None));
                IsBusy = false;
            });

            spellChain.EnqueueChain();

            return;
        }
    }
}

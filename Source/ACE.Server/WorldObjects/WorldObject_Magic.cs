using System;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        // TODO: Used to pass the scale of a spell to CalcuateDamage; to be removed once the StatMod properties are added
        private float spellScaling;

        private enum SpellLevel
        {
            One     = 1,
            Two     = 50,
            Three   = 100,
            Four    = 150,
            Five    = 200,
            Six     = 250,
            Seven   = 300,
            Eight   = 350
        }

        private SpellLevel CalculateSpellLevel(uint spellPower)
        {
            if (spellPower < 50)
                return SpellLevel.One;
            else if (spellPower < 100)
                return SpellLevel.Two;
            else if (spellPower < 150)
                return SpellLevel.Three;
            else if (spellPower < 200)
                return SpellLevel.Four;
            else if (spellPower < 250)
                return SpellLevel.Five;
            else if (spellPower < 300)
                return SpellLevel.Six;
            else if (spellPower < 350)
                return SpellLevel.Seven;
            else return SpellLevel.Eight;
        }

        /// <summary>
        /// Method used for handling player targeted spell casts
        /// </summary>
        private uint CalculateDamage(uint spellId)
        {
            Random rng = new Random();
            // TODO: Replace with damage values obtained from the StatMod properties, when added to the World DB
            uint damage = (uint)rng.Next((int)(50 * spellScaling), (int)(100 * spellScaling));

            return damage;
        }

        /// <summary>
        /// Method used for the scaling, windup motion, and spell gestures for spell casts
        /// </summary>
        private float SpellAttributes(uint spellId, out MotionCommand windUpMotion, out MotionCommand spellGesture)
        {
            float scale;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                windUpMotion = MotionCommand.Invalid;
                spellGesture = MotionCommand.Invalid;
                return -1.0f;
            }

            SpellBase spell = spellTable.Spells[spellId];

            switch(spell.Bitfield)
            {
                case 0x00000006:
                    // Enchant Other Helpful
                    spellGesture = MotionCommand.MagicSelfHeart;
                    break;
                case 0x0000400c:
                case 0x0000408e:
                    // Enchant Self Helpful
                    spellGesture = MotionCommand.MagicSelfHead;
                    break;
                case 0x00000018:
                    // Enchant Self Harmful
                    spellGesture = MotionCommand.MagicHarm;
                    break;
                case 0x00000004:
                    if (spell.School == (MagicSchool)0x00000002)
                    {
                        // Life heal other
                        spellGesture = MotionCommand.MagicHeal;
                    }
                    else
                    {
                        // item spell self armor
                        spellGesture = MotionCommand.MagicEnchantItem;
                    }
                    break;
                case 0x00000013:
                    // life harm other
                    spellGesture = MotionCommand.MagicHarm;
                    break;
                case 0x0000000c:
                    // Life heal self
                    spellGesture = MotionCommand.MagicHeal;
                    break;
                default:
                    if (spell.MetaSpellType == SpellType.Projectile)
                    {
                        // War and Void projectile cast animation
                        spellGesture = MotionCommand.MagicThrowMissile;
                    }
                    else
                    {
                        // Default cast gesture
                        spellGesture = MotionCommand.MagicSelfHead;
                    }
                    break;
            }

            ////Determine scale of the spell effects and windup animation
            SpellLevel spellLevel = CalculateSpellLevel(spell.Power);
            switch (spellLevel)
            {
                case SpellLevel.One:
                    scale = 0.1f;
                    windUpMotion = MotionCommand.MagicPowerUp01;
                    break;
                case SpellLevel.Two:
                    scale = 0.2f;
                    windUpMotion = MotionCommand.MagicPowerUp02;
                    break;
                case SpellLevel.Three:
                    scale = 0.4f;
                    windUpMotion = MotionCommand.MagicPowerUp03;
                    break;
                case SpellLevel.Four:
                    scale = 0.5f;
                    windUpMotion = MotionCommand.MagicPowerUp04;
                    break;
                case SpellLevel.Five:
                    scale = 0.6f;
                    windUpMotion = MotionCommand.MagicPowerUp05;
                    break;
                case SpellLevel.Six:
                    scale = 1.0f;
                    windUpMotion = MotionCommand.MagicPowerUp06;
                    break;
                default:
                    scale = 1.0f;
                    windUpMotion = MotionCommand.MagicPowerUp10Purple;
                    break;
            }

            spellScaling = scale;
            return scale;
        }

        /// <summary>
        /// Method used for handling player targeted spell casts
        /// </summary>
        public void CreatePlayerSpell(ObjectGuid guidTarget, uint spellId)
        {
            Player player = CurrentLandblock.GetObject(Guid) as Player;
            WorldObject target = CurrentLandblock.GetObject(guidTarget);

            if (player.IsBusy == true)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.YoureTooBusy));
                return;
            }
            else
                player.IsBusy = true;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.MagicInvalidSpellType));
                player.IsBusy = false;
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            uint targetEffect = spell.TargetEffect;

#if DEBUG
            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell bitfield is 0x{spell.Bitfield.ToString("X")}", ChatMessageType.System));
            player.Session.Network.EnqueueSend(new GameMessageSystemChat($"{spell.Name} spell power is {spell.Power}", ChatMessageType.System));
            player.Session.Network.EnqueueSend(
                new GameMessageSystemChat($"{spell.Name} spell range is {spell.BaseRangeConstant + (spell.BaseRangeConstant * spell.BaseRangeMod)} yards", ChatMessageType.System));
#endif

            if (guidTarget == null)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.YourSpellTargetIsMissing));
                player.IsBusy = false;
                return;
            }

            if (target == null)
                target = player.GetWieldedItem(guidTarget);
            else
            {
                if (guidTarget != Guid)
                {
                    float distanceTo = Location.DistanceTo(target.Location);

                    if (distanceTo > (spell.BaseRangeConstant + (spell.BaseRangeConstant * spell.BaseRangeMod)))
                    {
                        player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.MagicTargetOutOfRange),
                            new GameMessageSystemChat($"{target.Name} is out of range!", ChatMessageType.Magic));
                        player.IsBusy = false;
                        return;
                    }
                }
            }

            if (spell.School == MagicSchool.WarMagic || spell.School == MagicSchool.VoidMagic)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.None),
                    new GameMessageSystemChat($"{spell.Name} spell not implemented, yet!", ChatMessageType.System));
                player.IsBusy = false;
                return;
            }

            float scale = SpellAttributes(spellId, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            ActionChain spellChain = new ActionChain();

            spellChain.AddAction(this, () =>
            {
                var motionWindUp = new UniversalMotion(MotionStance.Spellcasting);
                motionWindUp.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Spellcasting & 0xFFFF);
                motionWindUp.MovementData.ForwardCommand = (uint)windUpMotion;
                motionWindUp.MovementData.ForwardSpeed = 2;
                DoMotion(motionWindUp);
            });

            spellChain.AddAction(this, () =>
            {
                var formula = SpellTable.GetSpellFormula(spellTable, spellId, player.Session.Account);
                CurrentLandblock.EnqueueBroadcast(Location, new GameMessageCreatureMessage(SpellComponentsTable.GetSpellWords(DatManager.PortalDat.SpellComponentsTable,
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

            spellChain.AddDelaySeconds(1.7422216f);

            spellChain.AddAction(this, () =>
            {
                if (guidTarget == Guid)
                {
                    if (spell.Name.Contains("Portal") || spell.Name.Contains("Lifestone"))
                    {
                        CurrentLandblock.EnqueueBroadcast(Location, new GameMessageScript(Guid, (PlayScript)spell.CasterEffect, scale));

                        switch(spellId)
                        {
                            case 2645: // Portal Recall
                                if (!player.TeleToPosition(PositionType.LastPortal))
                                {
                                    // You must link to a portal to recall it!
                                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToRecall));
                                }
                                break;
                            case 1635: // Lifestone Recall
                                if (!player.TeleToPosition(PositionType.LinkedLifestone))
                                {
                                    // You must link to a lifestone to recall it!
                                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToLifestoneToRecall));
                                }
                                break;
                            case 48: // Primary Portal Recall
                                if (!player.TeleToPosition(PositionType.LinkedPortalOne))
                                {
                                    // You must link to a portal to recall it!
                                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToRecall));
                                }
                                break;
                            case 2647: // Secondary Portal Recall
                                if (!player.TeleToPosition(PositionType.LinkedPortalTwo))
                                {
                                    // You must link to a portal to recall it!
                                    player.Session.Network.EnqueueSend(new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToRecall));
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    else
                        CurrentLandblock.EnqueueBroadcast(Location, new GameMessageScript(Guid, (PlayScript)spell.TargetEffect, scale));
                }
                else
                {
                    switch(spell.School)
                    {
                        case MagicSchool.WarMagic:
                            // TODO
                            break;
                        case MagicSchool.VoidMagic:
                            // TODO
                            break;
                        default:
                            CurrentLandblock.EnqueueBroadcast(Location, new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, scale));
                            if (spell.Bitfield == 0x00000013)
                            {
                                // TODO: To be changed with the implementation of StatMod
								if (spell.Name.Contains("Harm Other") || spell.Name.Contains("Drain Health Other"))
                                {
                                    uint dmg;
                                    int newMonsterHealth;
                                    Creature monster = (Creature)target;
                                    if (spell.Name.Contains("Harm Other"))
                                        dmg = CalculateDamage(spellId);
                                    else
                                        dmg = (uint)(monster.Health.Current * 0.25);
                                    newMonsterHealth = (int)(monster.Health.Current - dmg);
                                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You drain {dmg} of health from {monster.Name}", ChatMessageType.Magic));
                                    if (newMonsterHealth <= 0)
                                    {
                                        monster.Health.Current = 0;
                                        monster.DoOnKill(player.Session);
                                    }
                                    else
                                        monster.Health.Current = (uint)newMonsterHealth;
                                }
                            }
                            break;
                    }
                }
            });

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
            Player player = CurrentLandblock.GetObject(Guid) as Player;

            if (player.IsBusy == true)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.YoureTooBusy));
                return;
            }
            else
                player.IsBusy = true;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.YourSpellTargetIsMissing));
                player.IsBusy = false;
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            float scale = SpellAttributes(spellId, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            player.Session.Network.EnqueueSend(new GameMessageSystemChat("Targeted SpellID " + spellId + " not yet implemented!", ChatMessageType.System));
            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session, errorType: WeenieError.None));
            player.IsBusy = false;

            return;
        }

        /// <summary>
        /// Method used for handling creature untargeted spell casts
        /// </summary>
        public void CreateCreatureSpell(ObjectGuid guidTarget, uint spellId)
        {
            Creature creature = CurrentLandblock.GetObject(Guid) as Creature;

            if (creature.IsBusy == true)
                return;
            else
                creature.IsBusy = true;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                creature.IsBusy = false;
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            float scale = SpellAttributes(spellId, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            creature.IsBusy = false;
            return;
        }

        /// <summary>
        /// Method used for handling creature untargeted spell casts
        /// </summary>
        public void CreateCreatureSpell(uint spellId)
        {
            Creature creature = CurrentLandblock.GetObject(Guid) as Creature;

            if (creature.IsBusy == true)
                return;
            else
                creature.IsBusy = true;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                creature.IsBusy = false;
                return;
            }

            SpellBase spell = spellTable.Spells[spellId];

            float scale = SpellAttributes(spellId, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            creature.IsBusy = false;
            return;
        }
    }
}


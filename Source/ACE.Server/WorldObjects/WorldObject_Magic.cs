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
        public enum CastResult
        {
            SpellTargetInvalid,
            SpellNotImplemented,
            InvalidSpell,
            BusyCasting,
            SpellCastCompleted
        }

        private float spellAttributes(uint spellId, out MotionCommand windUpMotion, out MotionCommand spellGesture)
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
            switch(spell.Power)
            {
                case 1:
                    scale = 0.1f;
                    windUpMotion = MotionCommand.MagicPowerUp01;
                    break;
                case 50:
                    scale = 0.2f;
                    windUpMotion = MotionCommand.MagicPowerUp02;
                    break;
                case 100:
                    scale = 0.4f;
                    windUpMotion = MotionCommand.MagicPowerUp03;
                    break;
                case 150:
                    scale = 0.5f;
                    windUpMotion = MotionCommand.MagicPowerUp04;
                    break;
                case 200:
                    scale = 0.6f;
                    windUpMotion = MotionCommand.MagicPowerUp05;
                    break;
                case 250:
                    scale = 1.0f;
                    windUpMotion = MotionCommand.MagicPowerUp06;
                    break;
                default:
                    scale = 1.0f;
                    windUpMotion = MotionCommand.MagicPowerUp10Purple;
                    break;
            }

            return scale;
        }

        /// <summary>
        /// Method used for handling player targeted spell casts
        /// </summary>
        public CastResult CreatePlayerSpell(ObjectGuid guidTarget, uint spellId)
        {
            Player player = CurrentLandblock.GetObject(Guid) as Player;
            WorldObject target = CurrentLandblock.GetObject(guidTarget);

            if (player.BusyCasting == true)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
                return CastResult.BusyCasting;
            }
            else
                player.BusyCasting = true;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
                player.BusyCasting = false;
                return CastResult.InvalidSpell;
            }

            SpellBase spell = spellTable.Spells[spellId];

            uint targetEffect = spell.TargetEffect;

            if (guidTarget == null)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
                player.BusyCasting = false;
                return CastResult.SpellTargetInvalid;
            }

            if (target == null)
                target = player.GetWieldedItem(guidTarget);

            if (spell.School == MagicSchool.WarMagic || spell.School == MagicSchool.VoidMagic)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
                player.BusyCasting = false;
                return CastResult.SpellNotImplemented;
            }

            float scale = spellAttributes(spellId, out MotionCommand windUpMotion, out MotionCommand spellGesture);

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
                CurrentLandblock.EnqueueBroadcast(Location, new GameMessageCreatureMessage(SpellComponentsTable.GetSpellWords(DatManager.PortalDat.SpellComponentsTable, formula), Name, Guid.Full, ChatMessageType.Magic));
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
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
                player.BusyCasting = false;
            });

            spellChain.EnqueueChain();

            return CastResult.SpellCastCompleted;
        }

        /// <summary>
        /// Method used for handling player untargeted spell casts
        /// </summary>
        public CastResult CreatePlayerSpell(uint spellId)
        {
            Player player = CurrentLandblock.GetObject(Guid) as Player;

            if (player.BusyCasting == true)
            {
                player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
                return CastResult.BusyCasting;
            }
            else
                player.BusyCasting = true;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                player.BusyCasting = false;
                return CastResult.InvalidSpell;
            }

            SpellBase spell = spellTable.Spells[spellId];

            float scale = spellAttributes(spellId, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            player.Session.Network.EnqueueSend(new GameEventUseDone(player.Session));
            player.BusyCasting = false;
            return CastResult.SpellNotImplemented;
        }

        /// <summary>
        /// Method used for handling creature untargeted spell casts
        /// </summary>
        public CastResult CreateCreatureSpell(ObjectGuid guidTarget, uint spellId)
        {
            Creature creature = CurrentLandblock.GetObject(Guid) as Creature;

            if (creature.BusyCasting == true)
                return CastResult.BusyCasting;
            else
                creature.BusyCasting = true;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                creature.BusyCasting = false;
                return CastResult.InvalidSpell;
            }

            SpellBase spell = spellTable.Spells[spellId];

            float scale = spellAttributes(spellId, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            creature.BusyCasting = false;
            return CastResult.SpellNotImplemented;
        }

        /// <summary>
        /// Method used for handling creature untargeted spell casts
        /// </summary>
        public CastResult CreateCreatureSpell(uint spellId)
        {
            Creature creature = CurrentLandblock.GetObject(Guid) as Creature;

            if (creature.BusyCasting == true)
                return CastResult.BusyCasting;
            else
                creature.BusyCasting = true;

            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
            {
                creature.BusyCasting = false;
                return CastResult.InvalidSpell;
            }

            SpellBase spell = spellTable.Spells[spellId];

            float scale = spellAttributes(spellId, out MotionCommand windUpMotion, out MotionCommand spellGesture);

            creature.BusyCasting = false;
            return CastResult.SpellNotImplemented;
        }
    }
}


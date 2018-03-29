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
            ItemMagicSpellSpellNotImplemented,
            CreatureMagicSpellSpellNotImplemented,
            LifeMagicSpellSpellNotImplemented,
            WarMagicSpellSpellNotImplemented,
            VoidMagicSpellSpellNotImplemented,
            InvalidSpell,
            SpellCastCompleted
        }

        /// <summary>
        /// Method used for handling targeted spell casts
        /// </summary>
        public CastResult CreateSpell(ObjectGuid guidTarget, uint spellId)
        {
            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
                return CastResult.InvalidSpell;

            SpellBase spell = spellTable.Spells[spellId];

            uint targetEffect = spell.TargetEffect;

            if (guidTarget == null)
                return CastResult.SpellTargetInvalid;

            WorldObject target = CurrentLandblock.GetObject(guidTarget);

            if (target == null)
            {
                Player player = CurrentLandblock.GetObject(Guid) as Player;

                target = player.GetWieldedItem(guidTarget);
            }

            if (spell.School == MagicSchool.WarMagic)
                return CastResult.WarMagicSpellSpellNotImplemented;

            if (spell.School == MagicSchool.VoidMagic)
                return CastResult.VoidMagicSpellSpellNotImplemented;

            float scale;
            MotionCommand windUpMotion;
            MotionCommand spellGesture;

            if (spell.Bitfield == 0x00000006)
            {
                // Enchant Other Helpful
                spellGesture = MotionCommand.MagicSelfHeart;
            }
            else if (spell.Bitfield == 0x0000400c || spell.Bitfield == 0x0000408e)
            {
                // Enchant Self Helpful
                spellGesture = MotionCommand.MagicSelfHead;
            }
            else if (spell.Bitfield == 0x00000018)
            {
                // Enchant Self Harmful
                spellGesture = MotionCommand.MagicHarm;
            }
            else if (spell.Bitfield == 0x00000013)
            {
                // Enchant Other Harmful
                spellGesture = MotionCommand.MagicHarm;
            }
            else if (spell.Bitfield == 0x00000004 && spell.School == (MagicSchool)0x00000002)
            {
                // Life heal other
                spellGesture = MotionCommand.MagicHeal;
            }
            else if (spell.Bitfield == 0x00000013)
            {
                // life harm other
                spellGesture = MotionCommand.MagicHarm;
            }
            else if (spell.Bitfield == 0x0000000c)
            {
                // Life heal self
                spellGesture = MotionCommand.MagicHeal;
            }
            else if (spell.Bitfield == 0x00000004)
            {
                // item spell self armor
                spellGesture = MotionCommand.MagicEnchantItem;
            }
            else if (spell.MetaSpellType == SpellType.Projectile)
            {
                // War and Void projectile cast animation
                spellGesture = MotionCommand.MagicThrowMissile;
            }
            else
            {
                // generic cast
                spellGesture = MotionCommand.MagicSelfHead;
            }

            ////Determine scale of the spell effects
            if (spell.Power == 1)
            {
                scale = 0.1f;
                windUpMotion = MotionCommand.MagicPowerUp01;
            }
            else if (spell.Power == 50)
            {
                scale = 0.2f;
                windUpMotion = MotionCommand.MagicPowerUp02;
            }
            else if (spell.Power == 100)
            {
                scale = 0.4f;
                windUpMotion = MotionCommand.MagicPowerUp03;
            }
            else if (spell.Power == 150)
            {
                scale = 0.5f;
                windUpMotion = MotionCommand.MagicPowerUp04;
            }
            else if (spell.Power == 200)
            {
                scale = 0.6f;
                windUpMotion = MotionCommand.MagicPowerUp05;
            }
            else if (spell.Power == 260)
            {
                scale = 1.0f;
                windUpMotion = MotionCommand.MagicPowerUp06;
            }
            else
            {
                scale = 1.0f;
                windUpMotion = MotionCommand.MagicPowerUp10Purple;
            }
            
            ActionChain spellChain = new ActionChain();

            spellChain.AddAction(this, () =>
            {
                var motionWindUp = new UniversalMotion(MotionStance.Spellcasting);
                motionWindUp.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Spellcasting & 0xFFFF);
                motionWindUp.MovementData.ForwardCommand = (uint)windUpMotion;
                motionWindUp.MovementData.ForwardSpeed = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(windUpMotion);
                DoMotion(motionWindUp);
            });

            spellChain.AddAction(this, () =>
            {
                Player player = CurrentLandblock.GetObject(Guid) as Player;
                SpellComponentsTable comps = DatManager.PortalDat.SpellComponentsTable;
                var formula = SpellTable.GetSpellFormula(spellTable, spellId, player.Session.Account);

                string spellWordsMessage = $"{Name} says, \"{SpellComponentsTable.GetSpellWords(comps, formula)}\"";
                CurrentLandblock.EnqueueBroadcastSystemChat(this, spellWordsMessage, ChatMessageType.Magic);
            });

            spellChain.AddAction(this, () =>
            {
                var motionCastSpell = new UniversalMotion(MotionStance.Spellcasting);
                motionCastSpell.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Spellcasting & 0xFFFF);
                motionCastSpell.MovementData.ForwardCommand = (uint)spellGesture;
                motionCastSpell.MovementData.ForwardSpeed = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(spellGesture);
                DoMotion(motionCastSpell);
            });

            spellChain.AddDelaySeconds((DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.CastSpell))*1.5f);

            spellChain.AddAction(this, () =>
            {
                if ((ObjectGuid)guidTarget == Guid)
                {
                    if (spell.Name.Contains("Portal") || spell.Name.Contains("Lifestone"))
                    {
                        Player player = CurrentLandblock.GetObject(Guid) as Player;
                        CurrentLandblock.EnqueueBroadcast(Location, new GameMessageScript(Guid, (PlayScript)spell.CasterEffect, scale));

                        switch(spellId)
                        {
                            case 2645: // Portal Recall
                                if (!player.TeleToPosition(PositionType.LastPortal))
                                {
                                    // You must link to a portal to recall it!
                                    var portalRecallMessage = new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToRecall);
                                    player.Session.Network.EnqueueSend(portalRecallMessage);
                                }
                                break;
                            case 1635: // Lifestone Recall
                                if (!player.TeleToPosition(PositionType.LinkedLifestone))
                                {
                                    // You must link to a lifestone to recall it!
                                    var portalRecallMessage = new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToLifestoneToRecall);
                                    player.Session.Network.EnqueueSend(portalRecallMessage);
                                }
                                break;
                            case 48: // Primary Portal Recall
                                if (!player.TeleToPosition(PositionType.LinkedPortalOne))
                                {
                                    // You must link to a portal to recall it!
                                    var portalRecallMessage = new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToRecall);
                                    player.Session.Network.EnqueueSend(portalRecallMessage);
                                }
                                break;
                            case 2647: // Secondary Portal Recall
                                if (!player.TeleToPosition(PositionType.LinkedPortalTwo))
                                {
                                    // You must link to a portal to recall it!
                                    var portalRecallMessage = new GameEventWeenieError(player.Session, WeenieError.YouMustLinkToPortalToRecall);
                                    player.Session.Network.EnqueueSend(portalRecallMessage);
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
                    if (spell.School == MagicSchool.WarMagic)
                    {
                        // TODO
                    }
                    else if (spell.School == MagicSchool.VoidMagic)
                    {
                        // TODO
                    }
                    else
                    {
                        CurrentLandblock.EnqueueBroadcast(Location, new GameMessageScript(target.Guid, (PlayScript)spell.TargetEffect, scale));
                    }
                }
            });

            spellChain.AddAction(this, () =>
            {
                var motionReturnToCastStance = new UniversalMotion(MotionStance.Spellcasting);
                motionReturnToCastStance.MovementData.CurrentStyle = (ushort)((uint)MotionStance.Spellcasting & 0xFFFF);
                motionReturnToCastStance.MovementData.ForwardCommand = (uint)0;
                motionReturnToCastStance.MovementData.ForwardSpeed = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(0);
                DoMotion(motionReturnToCastStance);
            });

            spellChain.EnqueueChain();

            return CastResult.SpellCastCompleted;
        }

        /// <summary>
        /// Method used for handling untargeted spell casts
        /// </summary>
        public CastResult CreateSpell(uint spellId)
        {
            SpellTable spellTable = DatManager.PortalDat.SpellTable;
            if (!spellTable.Spells.ContainsKey(spellId))
                return CastResult.InvalidSpell;

            SpellBase spell = spellTable.Spells[spellId];

            return CastResult.SpellNotImplemented;
        }
    }
}

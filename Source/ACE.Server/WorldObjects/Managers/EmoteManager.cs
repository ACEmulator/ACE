using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.DatLoader;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Factories.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

using log4net;

using Position = ACE.Entity.Position;
using Spell = ACE.Server.Entity.Spell;

namespace ACE.Server.WorldObjects.Managers
{
    public class EmoteManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WorldObject WorldObject => _proxy ?? _worldObject;

        private WorldObject _worldObject;
        private WorldObject _proxy;

        /// <summary>
        /// Returns TRUE if this WorldObject is currently busy processing other emotes
        /// </summary>
        public bool IsBusy { get; set; }
        public int Nested { get; set; }

        public bool Debug = false;

        public EmoteManager(WorldObject worldObject)
        {
            _worldObject = worldObject;
        }

        /// <summary>
        /// Executes an emote
        /// </summary>
        /// <param name="emoteSet">The parent set of this emote</param>
        /// <param name="emote">The emote to execute</param>
        /// <param name="targetObject">A target object, usually player</param>
        /// <param name="actionChain">Only used for passing to further sets</param>
        public float ExecuteEmote(PropertiesEmote emoteSet, PropertiesEmoteAction emote, WorldObject targetObject = null)
        {
            var player = targetObject as Player;
            var creature = WorldObject as Creature;
            var targetCreature = targetObject as Creature;

            var delay = 0.0f;
            var emoteType = (EmoteType)emote.Type;

            //if (Debug)
                //Console.WriteLine($"{WorldObject.Name}.ExecuteEmote({emoteType})");

            var text = emote.Message;

            switch ((EmoteType)emote.Type)
            {
                case EmoteType.Act:
                    // short for 'acting' text
                    var message = Replace(text, WorldObject, targetObject, emoteSet.Quest);
                    WorldObject.EnqueueBroadcast(new GameMessageSystemChat(message, ChatMessageType.Broadcast), 30.0f);
                    break;

                case EmoteType.Activate:

                    if (WorldObject.ActivationTarget > 0)
                    {
                        // ActOnUse delay?
                        var activationTarget = WorldObject.CurrentLandblock?.GetObject(WorldObject.ActivationTarget);
                        activationTarget?.OnActivate(WorldObject);
                    }
                    else if (WorldObject.GeneratorId.HasValue && WorldObject.GeneratorId > 0) // Fallback to linked generator
                    {
                        var linkedGenerator = WorldObject.CurrentLandblock?.GetObject(WorldObject.GeneratorId ?? 0);
                        linkedGenerator?.OnActivate(WorldObject);
                    }
                    break;

                case EmoteType.AddCharacterTitle:

                    // emoteAction.Stat == null for all EmoteType.AddCharacterTitle entries in current db?
                    if (player != null && emote.Amount != 0)
                        player.AddTitle((CharacterTitle)emote.Amount);
                    break;

                case EmoteType.AddContract:

                    // Contracts werent in emote table for 16py, guessing that Stat was used to hold id for contract.
                    if (player != null && emote.Stat.HasValue && emote.Stat.Value > 0)
                        player.ContractManager.Add(emote.Stat.Value);
                    break;

                case EmoteType.AdminSpam:

                    text = Replace(emote.Message, WorldObject, targetObject, emoteSet.Quest);

                    PlayerManager.BroadcastToChannelFromEmote(Channel.Admin, text);
                    break;

                case EmoteType.AwardLevelProportionalSkillXP:

                    var min = emote.Min64 ?? emote.Min ?? 0;
                    var max = emote.Max64 ?? emote.Max ?? 0;

                    if (player != null)
                        player.GrantLevelProportionalSkillXP((Skill)emote.Stat, emote.Percent ?? 0, min, max);
                    break;

                case EmoteType.AwardLevelProportionalXP:

                    bool shareXP = emote.Display ?? false;
                    min = emote.Min64 ?? emote.Min ?? 0;
                    max = emote.Max64 ?? emote.Max ?? 0;

                    if (player != null)
                        player.GrantLevelProportionalXp(emote.Percent ?? 0, min, max, shareXP);
                    break;

                case EmoteType.AwardLuminance:

                    if (player != null)
                        player.EarnLuminance(emote.HeroXP64 ?? 0, XpType.Quest, ShareType.None);

                    break;

                case EmoteType.AwardNoShareXP:

                    if (player != null)
                        player.EarnXP(emote.Amount64 ?? emote.Amount ?? 0, XpType.Quest, ShareType.None);

                    break;

                case EmoteType.AwardSkillPoints:

                    if (player != null)
                        player.AwardSkillPoints((Skill)emote.Stat, (uint)emote.Amount);
                    break;

                case EmoteType.AwardSkillXP:

                    if (player != null)
                        player.AwardSkillXP((Skill)emote.Stat, (uint)emote.Amount, true);
                    break;

                case EmoteType.AwardTrainingCredits:

                    if (player != null)
                        player.AddSkillCredits(emote.Amount ?? 0, false);
                    break;

                case EmoteType.AwardXP:

                    if (player != null)
                    {
                        var amt = emote.Amount64 ?? emote.Amount ?? 0;
                        if (amt > 0)
                        {
                            player.EarnXP(amt, XpType.Quest, ShareType.All);
                        }
                        else if (amt < 0)
                        {
                            player.SpendXP(-amt);
                        }
                    }
                    break;

                case EmoteType.BLog:

                    text = Replace(emote.Message, WorldObject, targetObject, emoteSet.Quest);

                    log.Info($"0x{WorldObject.Guid}:{WorldObject.Name}({WorldObject.WeenieClassId}).EmoteManager.BLog - {text}");
                    break;

                case EmoteType.CastSpell:

                    if (WorldObject != null)
                    {
                        var spell = new Spell((uint)emote.SpellId);
                        if (spell.NotFound)
                        {
                            log.Error($"{WorldObject.Name} ({WorldObject.Guid}) EmoteManager.CastSpell - unknown spell {emote.SpellId}");
                            break;
                        }

                        var spellTarget = GetSpellTarget(spell, targetObject);

                        var preCastTime = creature.PreCastMotion(spellTarget);

                        delay = preCastTime + creature.GetPostCastTime(spell);

                        var castChain = new ActionChain();
                        castChain.AddDelaySeconds(preCastTime);
                        castChain.AddAction(creature, () =>
                        {
                            creature.TryCastSpell(spell, spellTarget, creature);
                            creature.PostCastMotion();
                        });
                        castChain.EnqueueChain();
                    }
                    break;

                case EmoteType.CastSpellInstant:

                    if (WorldObject != null)
                    {
                        var spell = new Spell((uint)emote.SpellId);

                        if (!spell.NotFound)
                        {
                            var spellTarget = GetSpellTarget(spell, targetObject);

                            WorldObject.TryCastSpell(spell, spellTarget, WorldObject);
                        }
                    }
                    break;

                case EmoteType.CloseMe:

                    // animation delay?
                    if (WorldObject is Container container)
                        container.Close(null);
                    else if (WorldObject is Door closeDoor)
                        closeDoor.Close();

                    break;

                case EmoteType.CreateTreasure:

                    if (player != null)
                    {
                        var treasureTier = emote.WealthRating ?? 1;

                        var treasureType = (TreasureItemCategory?)emote.TreasureType ?? TreasureItemCategory.Undef;

                        var treasureClass = (TreasureItemType_Orig?)emote.TreasureClass ?? TreasureItemType_Orig.Undef;

                        // Create a dummy treasure profile for passing emote values
                        var profile = new Database.Models.World.TreasureDeath
                        {
                            Tier = treasureTier,
                            //TreasureType = (uint)treasureType,
                            LootQualityMod = 0,
                            ItemChance = 100,
                            ItemMinAmount = 1,
                            ItemMaxAmount = 1,
                            //ItemTreasureTypeSelectionChances = (int)treasureClass,
                            MagicItemChance = 100,
                            MagicItemMinAmount = 1,
                            MagicItemMaxAmount = 1,
                            //MagicItemTreasureTypeSelectionChances = (int)treasureClass,
                            MundaneItemChance = 100,
                            MundaneItemMinAmount = 1,
                            MundaneItemMaxAmount = 1,
                            //MundaneItemTypeSelectionChances = (int)treasureClass,
                            UnknownChances = 21
                        };

                        var treasure = LootGenerationFactory.CreateRandomLootObjects_New(profile, treasureType, treasureClass);
                        if (treasure != null)
                        {
                            player.TryCreateForGive(WorldObject, treasure);
                        }
                    }
                    break;

                /* decrements a PropertyInt stat by some amount */
                case EmoteType.DecrementIntStat:

                    // only used by 1 emote in 16PY - check for lower bounds?
                    if (targetObject != null && emote.Stat != null)
                    {
                        var intProperty = (PropertyInt)emote.Stat;
                        var current = targetObject.GetProperty(intProperty) ?? 0;
                        current -= emote.Amount ?? 1;
                        targetObject.SetProperty(intProperty, current);

                        if (player != null)
                            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, intProperty, current));
                    }
                    break;

                case EmoteType.DecrementMyQuest:
                case EmoteType.DecrementQuest:

                    var questTarget = GetQuestTarget((EmoteType)emote.Type, targetCreature, creature);

                    if (questTarget != null)
                        questTarget.QuestManager.Decrement(emote.Message, emote.Amount ?? 1);

                    break;

                case EmoteType.DeleteSelf:

                    if (player != null)
                    {
                        var wo = player.FindObject(WorldObject.Guid.Full, Player.SearchLocations.Everywhere, out _, out Container rootOwner, out bool wasEquipped);

                        WorldObject.DeleteObject(rootOwner);
                    }
                    else
                        WorldObject.DeleteObject();

                    break;

                case EmoteType.DirectBroadcast:

                    text = Replace(emote.Message, WorldObject, targetObject, emoteSet.Quest);

                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Broadcast));

                    break;

                case EmoteType.Enlightenment:

                    if (player != null)
                    {
                        Enlightenment.HandleEnlightenment(WorldObject, player);
                    }

                    break;

                case EmoteType.EraseMyQuest:
                case EmoteType.EraseQuest:

                    questTarget = GetQuestTarget((EmoteType)emote.Type, targetCreature, creature);

                    if (questTarget != null)
                        questTarget.QuestManager.Erase(emote.Message);

                    break;

                case EmoteType.FellowBroadcast:

                    if (player != null)
                    {
                        var fellowship = player.Fellowship;

                        if (fellowship != null)
                        {
                            text = Replace(emote.Message, WorldObject, player, emoteSet.Quest);

                            fellowship.BroadcastToFellow(text);
                        }
                    }
                    break;

                case EmoteType.Generate:

                    if (WorldObject.IsGenerator)
                        WorldObject.Generator_Regeneration();
                    break;

                case EmoteType.Give:

                    bool success = false;

                    var stackSize = emote.StackSize ?? 1;

                    if (player != null && emote.WeenieClassId != null)
                        player.GiveFromEmote(WorldObject, emote.WeenieClassId ?? 0, stackSize > 0 ? stackSize : 1);

                    break;

                /* redirects to the GotoSet category for this action */
                case EmoteType.Goto:

                    // TODO: revisit if nested chains need to back-propagate timers
                    var gotoSet = GetEmoteSet(EmoteCategory.GotoSet, emote.Message);
                    ExecuteEmoteSet(gotoSet, targetObject, true);
                    break;

                /* increments a PropertyInt stat by some amount */
                case EmoteType.IncrementIntStat:

                    if (targetObject != null && emote.Stat != null)
                    {
                        var intProperty = (PropertyInt)emote.Stat;
                        var current = targetObject.GetProperty(intProperty) ?? 0;
                        current += emote.Amount ?? 1;
                        targetObject.SetProperty(intProperty, current);

                        if (player != null)
                            player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(player, intProperty, current));
                    }
                    break;

                case EmoteType.IncrementMyQuest:
                case EmoteType.IncrementQuest:

                    questTarget = GetQuestTarget((EmoteType)emote.Type, targetCreature, creature);

                    if (questTarget != null)
                        questTarget.QuestManager.Increment(emote.Message, emote.Amount ?? 1);

                    break;

                case EmoteType.InflictVitaePenalty:
                    if (player != null)
                        player.InflictVitaePenalty(emote.Amount ?? 5);
                    break;

                case EmoteType.InqAttributeStat:

                    if (targetCreature != null)
                    {
                        var attr = targetCreature.Attributes[(PropertyAttribute)emote.Stat];

                        if (attr == null && HasValidTestNoQuality(emote.Message))
                        {
                            ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                        }
                        else
                        {
                            success = attr != null && attr.Current >= (emote.Min ?? int.MinValue) && attr.Current <= (emote.Max ?? int.MaxValue);

                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqBoolStat:

                    if (targetObject != null)
                    {
                        var stat = targetObject.GetProperty((PropertyBool)emote.Stat);

                        if (stat == null && HasValidTestNoQuality(emote.Message))
                        {
                            ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                        }
                        else
                        {
                            success = stat ?? false;

                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqContractsFull:

                    ExecuteEmoteSet(player != null && player.ContractManager.IsFull ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    break;

                case EmoteType.InqEvent:

                    var started = EventManager.IsEventStarted(emote.Message, WorldObject, targetObject);
                    ExecuteEmoteSet(started ? EmoteCategory.EventSuccess : EmoteCategory.EventFailure, emote.Message, targetObject, true);
                    break;

                case EmoteType.InqFellowNum:

                    // unused in PY16 - ensure # of fellows between min-max?
                    ExecuteEmoteSet(player != null && player.Fellowship != null ? EmoteCategory.TestSuccess : EmoteCategory.TestNoFellow, emote.Message, targetObject, true);
                    break;

                case EmoteType.InqFellowQuest:

                    if (player != null)
                    {
                        if (player.Fellowship != null)
                        {
                            var hasQuest = player.Fellowship.QuestManager.HasQuest(emote.Message);
                            var canSolve = player.Fellowship.QuestManager.CanSolve(emote.Message);

                            // verify: QuestSuccess = player has quest, and their last completed time + quest minDelta <= currentTime
                            success = hasQuest && !canSolve;

                            ExecuteEmoteSet(success ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                        }
                        else
                            ExecuteEmoteSet(EmoteCategory.QuestNoFellow, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqFloatStat:

                    if (targetObject != null)
                    {
                        var stat = targetObject.GetProperty((PropertyFloat)emote.Stat);

                        if (stat == null && HasValidTestNoQuality(emote.Message))
                            ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                        else
                        {
                            stat ??= 0.0f;
                            success = stat >= (emote.MinDbl ?? double.MinValue) && stat <= (emote.MaxDbl ?? double.MaxValue);
                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqInt64Stat:

                    if (targetObject != null)
                    {
                        var stat = targetObject.GetProperty((PropertyInt64)emote.Stat);

                        if (stat == null && HasValidTestNoQuality(emote.Message))
                            ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                        else
                        {
                            stat ??= 0;
                            success = stat >= (emote.Min64 ?? long.MinValue) && stat <= (emote.Max64 ?? long.MaxValue);
                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqIntStat:

                    if (targetObject != null)
                    {
                        var stat = targetObject.GetProperty((PropertyInt)emote.Stat);

                        if (stat == null && HasValidTestNoQuality(emote.Message))
                            ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                        else
                        {
                            stat ??= 0;
                            success = stat >= (emote.Min ?? int.MinValue) && stat <= (emote.Max ?? int.MaxValue);
                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqNumCharacterTitles:

                    //if (player != null)
                    //InqCategory(player.NumCharacterTitles != 0 ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote);
                    break;

                case EmoteType.InqOwnsItems:

                    if (player != null)
					{
                        var numRequired = emote.StackSize ?? 1;

                        var items = player.GetInventoryItemsOfWCID(emote.WeenieClassId ?? 0);
                        items.AddRange(player.GetEquippedObjectsOfWCID(emote.WeenieClassId ?? 0));
                        var numItems = items.Sum(i => i.StackSize ?? 1);

                        success = numItems >= numRequired;

                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqPackSpace:

                    if (player != null)
                    {
                        var numRequired = emote.Amount ?? 1;

                        success = false;
                        if (numRequired > 10000) // Since emote was not in 16py and we have just the two fields to go on, I will assume you could "mask" the value to pick between free Item Capacity space or free Container Capacity space
                        {
                            var freeSpace = player.GetFreeContainerSlots();

                            success = freeSpace >= (numRequired - 10000);
                        }
                        else
                        {
                            var freeSpace = player.GetFreeInventorySlots(false); // assuming this was only for main pack. makes things easier at this point.

                            success = freeSpace >= numRequired;
                        }

                        ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqMyQuest:
                case EmoteType.InqQuest:

                    questTarget = GetQuestTarget((EmoteType)emote.Type, targetCreature, creature);

                    if (questTarget != null)
                    {
                        var hasQuest = questTarget.QuestManager.HasQuest(emote.Message);
                        var canSolve = questTarget.QuestManager.CanSolve(emote.Message);

                        //  verify: QuestSuccess = player has quest, but their quest timer is currently still on cooldown
                        success = hasQuest && !canSolve;

                        ExecuteEmoteSet(success ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                    }

                    break;

                case EmoteType.InqMyQuestBitsOff:
                case EmoteType.InqQuestBitsOff:

                    questTarget = GetQuestTarget((EmoteType)emote.Type, targetCreature, creature);

                    if (questTarget != null)
                    {
                        var hasNoQuestBits = questTarget.QuestManager.HasNoQuestBits(emote.Message, emote.Amount ?? 0);

                        ExecuteEmoteSet(hasNoQuestBits ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                    }

                    break;

                case EmoteType.InqMyQuestBitsOn:
                case EmoteType.InqQuestBitsOn:

                    questTarget = GetQuestTarget((EmoteType)emote.Type, targetCreature, creature);

                    if (questTarget != null)
                    {
                        var hasQuestBits = questTarget.QuestManager.HasQuestBits(emote.Message, emote.Amount ?? 0);

                        ExecuteEmoteSet(hasQuestBits ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                    }

                    break;

                case EmoteType.InqMyQuestSolves:
                case EmoteType.InqQuestSolves:

                    questTarget = GetQuestTarget((EmoteType)emote.Type, targetCreature, creature);

                    if (questTarget != null)
                    {
                        var questSolves = questTarget.QuestManager.HasQuestSolves(emote.Message, emote.Min, emote.Max);

                        ExecuteEmoteSet(questSolves ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.InqRawAttributeStat:

                    if (targetCreature != null)
                    {
                        var attr = targetCreature.Attributes[(PropertyAttribute)emote.Stat];

                        if (attr == null && HasValidTestNoQuality(emote.Message))
                        {
                            ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                        }
                        else
                        {
                            success = attr != null && attr.Base >= (emote.Min ?? int.MinValue) && attr.Base <= (emote.Max ?? int.MaxValue);

                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqRawSecondaryAttributeStat:

                    if (targetCreature != null)
                    {
                        var vital = targetCreature.Vitals[(PropertyAttribute2nd)emote.Stat];

                        if (vital == null && HasValidTestNoQuality(emote.Message))
                        {
                            ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                        }
                        else
                        {
                            success = vital != null && vital.Base >= (emote.Min ?? int.MinValue) && vital.Base <= (emote.Max ?? int.MaxValue);

                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqRawSkillStat:

                    if (targetCreature != null)
                    {
                        var skill = targetCreature.GetCreatureSkill((Skill)emote.Stat);

                        if (skill == null && HasValidTestNoQuality(emote.Message))
                        {
                            ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                        }
                        else
                        {
                            success = skill != null && skill.Base >= (emote.Min ?? int.MinValue) && skill.Base <= (emote.Max ?? int.MaxValue);

                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqSecondaryAttributeStat:

                    if (targetCreature != null)
                    {
                        var vital = targetCreature.Vitals[(PropertyAttribute2nd)emote.Stat];

                        if (vital == null && HasValidTestNoQuality(emote.Message))
                        {
                            ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                        }
                        else
                        {
                            success = vital != null && vital.Current >= (emote.Min ?? int.MinValue) && vital.Current <= (emote.Max ?? int.MaxValue);

                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqSkillSpecialized:

                    if (targetCreature != null)
                    {
                        var skill = targetCreature.GetCreatureSkill((Skill)emote.Stat);

                        if (skill == null && HasValidTestNoQuality(emote.Message))
                        {
                            ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                        }
                        else
                        {
                            success = skill != null && skill.AdvancementClass == SkillAdvancementClass.Specialized;

                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqSkillStat:

                    if (targetCreature != null)
                    {
                        var skill = targetCreature.GetCreatureSkill((Skill)emote.Stat);

                        if (skill == null && HasValidTestNoQuality(emote.Message))
                        {
                            ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                        }
                        else
                        {
                            success = skill != null && skill.Current >= (emote.Min ?? int.MinValue) && skill.Current <= (emote.Max ?? int.MaxValue);

                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqSkillTrained:

                    if (targetCreature != null)
                    {
                        var skill = targetCreature.GetCreatureSkill((Skill)emote.Stat);

                        if (skill == null && HasValidTestNoQuality(emote.Message))
                        {
                            ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                        }
                        else
                        {
                            success = skill != null && skill.AdvancementClass >= SkillAdvancementClass.Trained;

                            ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                        }
                    }
                    break;

                case EmoteType.InqStringStat:

                    if (targetCreature != null)
                    {
                        if (Enum.TryParse(emote.TestString, true, out PropertyString propStr))
                        {
                            var stat = targetCreature.GetProperty(propStr);

                            if (stat == null && HasValidTestNoQuality(emote.Message))
                            {
                                ExecuteEmoteSet(EmoteCategory.TestNoQuality, emote.Message, targetObject, true);
                            }
                            else
                            {
                                success = stat != null && stat.Equals(emote.Message);

                                ExecuteEmoteSet(success ? EmoteCategory.TestSuccess : EmoteCategory.TestFailure, emote.Message, targetObject, true);
                            }
                        }
                    }
                    break;

                case EmoteType.InqYesNo:

                    if (player != null)
                    {
                        player.ConfirmationManager.EnqueueSend(new Confirmation_YesNo(WorldObject.Guid, player.Guid, emote.Message), emote.TestString);
                    }
                    break;

                case EmoteType.Invalid:
                    break;

                case EmoteType.KillSelf:

                    if (creature != null)
                        creature.Smite(creature);
                    break;

                case EmoteType.LocalBroadcast:

                    message = Replace(emote.Message, WorldObject, targetObject, emoteSet.Quest);
                    WorldObject.EnqueueBroadcast(new GameMessageSystemChat(message, ChatMessageType.Broadcast));
                    break;

                case EmoteType.LocalSignal:

                    if (WorldObject != null)
                    {
                        if (WorldObject.CurrentLandblock != null)
                            WorldObject.CurrentLandblock.EmitSignal(WorldObject, emote.Message);
                    }
                    break;

                case EmoteType.LockFellow:

                    if (player != null && player.Fellowship != null)
                        player.HandleActionFellowshipChangeLock(true, emoteSet.Quest);

                    break;

                /* plays an animation on the target object (usually the player) */
                case EmoteType.ForceMotion:

                    var motionCommand = MotionCommandHelper.GetMotion(emote.Motion.Value);
                    var motion = new Motion(targetObject, motionCommand, emote.Extent);
                    targetObject.EnqueueBroadcastMotion(motion);
                    break;

                /* plays an animation on the source object */
                case EmoteType.Motion:

                    var debugMotion = false;

                    if (Debug)
                        Console.Write($".{(MotionCommand)emote.Motion}");

                    // If the landblock is dormant, there are no players in range
                    if (WorldObject.CurrentLandblock?.IsDormant ?? false)
                        break;

                    // are there players within emote range?
                    if (!WorldObject.PlayersInRange(ClientMaxAnimRange))
                        break;

                    if (WorldObject.PhysicsObj != null && WorldObject.PhysicsObj.IsMovingTo())
                        break;

                    if (WorldObject == null || WorldObject.CurrentMotionState == null) break;

                    // TODO: REFACTOR ME
                    if (emoteSet.Category != EmoteCategory.Vendor && emoteSet.Style != null)
                    {
                        var startingMotion = new Motion((MotionStance)emoteSet.Style, (MotionCommand)emoteSet.Substyle);
                        motion = new Motion((MotionStance)emoteSet.Style, (MotionCommand)emote.Motion, emote.Extent);

                        if (WorldObject.CurrentMotionState.Stance != startingMotion.Stance)
                        {
                            if (WorldObject.CurrentMotionState.Stance == MotionStance.Invalid)
                            {
                                if (debugMotion)
                                    Console.WriteLine($"{WorldObject.Name} running starting motion {(MotionStance)emoteSet.Style}, {(MotionCommand)emoteSet.Substyle}");

                                delay = WorldObject.ExecuteMotion(startingMotion);
                            }
                        }
                        else
                        {
                            if (WorldObject.CurrentMotionState.MotionState.ForwardCommand == startingMotion.MotionState.ForwardCommand
                                    && startingMotion.Stance == MotionStance.NonCombat)     // enforce non-combat here?
                            {
                                if (debugMotion)
                                    Console.WriteLine($"{WorldObject.Name} running motion {(MotionStance)emoteSet.Style}, {(MotionCommand)emote.Motion}");

                                float? maxRange = ClientMaxAnimRange;
                                if (MotionQueue.Contains((MotionCommand)emote.Motion))
                                    maxRange = null;

                                var motionTable = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(WorldObject.MotionTableId);
                                var animLength = motionTable.GetAnimationLength(WorldObject.CurrentMotionState.Stance, (MotionCommand)emote.Motion, MotionCommand.Ready);

                                delay = WorldObject.ExecuteMotion(motion, true, maxRange);

                                var motionChain = new ActionChain();
                                motionChain.AddDelaySeconds(animLength);
                                motionChain.AddAction(WorldObject, () =>
                                {
                                    // FIXME: better cycle handling
                                    var cmd = WorldObject.CurrentMotionState.MotionState.ForwardCommand;
                                    if (cmd != MotionCommand.Dead && cmd != MotionCommand.Sleeping && cmd != MotionCommand.Sitting && !cmd.ToString().EndsWith("State"))
                                    {
                                        if (debugMotion)
                                            Console.WriteLine($"{WorldObject.Name} running starting motion again {(MotionStance)emoteSet.Style}, {(MotionCommand)emoteSet.Substyle}");

                                        WorldObject.ExecuteMotion(startingMotion);
                                    }
                                });
                                motionChain.EnqueueChain();

                                if (debugMotion)
                                    Console.WriteLine($"{WorldObject.Name} appending time to existing chain: " + animLength);
                            }
                        }
                    }
                    else
                    {
                        // vendor / other motions
                        var startingMotion = new Motion(MotionStance.NonCombat, MotionCommand.Ready);
                        var motionTable = DatManager.PortalDat.ReadFromDat<DatLoader.FileTypes.MotionTable>(WorldObject.MotionTableId);
                        var animLength = motionTable.GetAnimationLength(WorldObject.CurrentMotionState.Stance, (MotionCommand)emote.Motion, MotionCommand.Ready);

                        motion = new Motion(MotionStance.NonCombat, (MotionCommand)emote.Motion, emote.Extent);

                        if (debugMotion)
                            Console.WriteLine($"{WorldObject.Name} running motion (block 2) {MotionStance.NonCombat}, {(MotionCommand)(emote.Motion ?? 0)}");

                        delay = WorldObject.ExecuteMotion(motion);

                        var motionChain = new ActionChain();
                        motionChain.AddDelaySeconds(animLength);
                        motionChain.AddAction(WorldObject, () => WorldObject.ExecuteMotion(startingMotion, false));

                        motionChain.EnqueueChain();
                    }

                    break;

                /* move to position relative to home */
                case EmoteType.Move:

                    if (creature != null)
                    {
                        // If the landblock is dormant, there are no players in range
                        if (WorldObject.CurrentLandblock?.IsDormant ?? false)
                            break;

                        // are there players within emote range?
                        if (!WorldObject.PlayersInRange(ClientMaxAnimRange))
                            break;

                        var newPos = new Position(creature.Home);
                        newPos.Pos += new Vector3(emote.OriginX ?? 0, emote.OriginY ?? 0, emote.OriginZ ?? 0);      // uses relative position

                        // ensure valid quaternion - all 0s for example can lock up physics engine
                        if (emote.AnglesX != null && emote.AnglesY != null && emote.AnglesZ != null && emote.AnglesW != null &&
                           (emote.AnglesX != 0    || emote.AnglesY != 0    || emote.AnglesZ != 0    || emote.AnglesW != 0) )
                        {
                            // also relative, or absolute?
                            newPos.Rotation *= new Quaternion(emote.AnglesX.Value, emote.AnglesY.Value, emote.AnglesZ.Value, emote.AnglesW.Value);  
                        }

                        if (Debug)
                            Console.WriteLine(newPos.ToLOCString());

                        // get new cell
                        newPos.LandblockId = new LandblockId(PositionExtensions.GetCell(newPos));

                        // TODO: handle delay for this?
                        creature.MoveTo(newPos, creature.GetRunRate(), true, null, emote.Extent);
                    }
                    break;

                case EmoteType.MoveHome:

                    // TODO: call MoveToManager on server, handle delay for this?
                    if (creature != null && creature.Home != null)
                    {
                        // are we already at home origin?
                        if (creature.Location.Pos.Equals(creature.Home.Pos))
                        {
                            // just turnto if required?
                            if (Debug)
                                Console.Write($" - already at home origin, checking rotation");

                            if (!creature.Location.Rotation.Equals(creature.Home.Rotation))
                            {
                                if (Debug)
                                    Console.Write($" - turning to");
                                delay = creature.TurnTo(creature.Home);
                            }
                            else if (Debug)
                                Console.Write($" - already at home rotation, doing nothing");
                        }
                        else
                        {
                            if (Debug)
                                Console.Write($" - {creature.Home.ToLOCString()}");

                            // how to get delay with this, callback required?
                            creature.MoveTo(creature.Home, creature.GetRunRate(), true, null, emote.Extent);
                        }
                    }
                    break;

                case EmoteType.MoveToPos:

                    if (creature != null)
                    {
                        var currentPos = creature.Location;

                        var newPos = new Position();
                        newPos.LandblockId = new LandblockId(currentPos.LandblockId.Raw);
                        newPos.Pos = new Vector3(emote.OriginX ?? currentPos.Pos.X, emote.OriginY ?? currentPos.Pos.Y, emote.OriginZ ?? currentPos.Pos.Z);

                        if (emote.AnglesX == null || emote.AnglesY == null || emote.AnglesZ == null || emote.AnglesW == null)
                            newPos.Rotation = new Quaternion(currentPos.Rotation.X, currentPos.Rotation.Y, currentPos.Rotation.Z, currentPos.Rotation.W);
                        else
                            newPos.Rotation = new Quaternion(emote.AnglesX ?? 0, emote.AnglesY ?? 0, emote.AnglesZ ?? 0, emote.AnglesW ?? 1);

                        if (emote.ObjCellId != null)
                            newPos.LandblockId = new LandblockId(emote.ObjCellId.Value);

                        // TODO: handle delay for this?
                        creature.MoveTo(newPos, creature.GetRunRate(), true, null, emote.Extent);
                    }
                    break;

                case EmoteType.OpenMe:

                    if (WorldObject is Container openContainer)
                        openContainer.Open(null);
                    else if (WorldObject is Door openDoor)
                        openDoor.Open();

                    break;

                case EmoteType.PetCastSpellOnOwner:

                    if (creature is Pet passivePet && passivePet.P_PetOwner != null)
                    {
                        var spell = new Spell((uint)emote.SpellId);
                        passivePet.TryCastSpell(spell, passivePet.P_PetOwner);
                    }
                    break;

                case EmoteType.PhysScript:

                    WorldObject.PlayParticleEffect((PlayScript)emote.PScript, WorldObject.Guid, emote.Extent);
                    break;

                case EmoteType.PopUp:

                    if (player != null)
                        player.Session.Network.EnqueueSend(new GameEventPopupString(player.Session, emote.Message));
                    break;

                case EmoteType.RemoveContract:

                    if (player != null && emote.Stat.HasValue && emote.Stat.Value > 0)
                        player.HandleActionAbandonContract((uint)emote.Stat);
                    break;

                case EmoteType.RemoveVitaePenalty:

                    if (player != null)
                        player.EnchantmentManager.RemoveVitae();

                    break;

                case EmoteType.ResetHomePosition:

                    if (WorldObject.Location != null)
                        WorldObject.Home = new Position(WorldObject.Location);
                    break;

                case EmoteType.Say:

                    if (Debug)
                        Console.Write($" - {emote.Message}");

                    message = Replace(emote.Message, WorldObject, targetObject, emoteSet.Quest);
                    if (emote.Extent > 0)
                        WorldObject.EnqueueBroadcast(new GameMessageHearRangedSpeech(message, WorldObject.Name, WorldObject.Guid.Full, emote.Extent, ChatMessageType.Emote), WorldObject.LocalBroadcastRange);
                    else
                        WorldObject.EnqueueBroadcast(new GameMessageHearSpeech(message, WorldObject.Name, WorldObject.Guid.Full, ChatMessageType.Emote), WorldObject.LocalBroadcastRange);
                    break;

                case EmoteType.SetAltRacialSkills:
                    break;

                case EmoteType.SetBoolStat:

                    if (player != null)
                    {
                        player.UpdateProperty(player, (PropertyBool)emote.Stat, emote.Amount == 0 ? false : true);
                        player.EnqueueBroadcast(false, new GameMessagePublicUpdatePropertyBool(player, (PropertyBool)emote.Stat, emote.Amount == 0 ? false : true));
                    }
                    break;

                case EmoteType.SetEyePalette:
                    //if (creature != null)
                    //    creature.EyesPaletteDID = (uint)emote.Display;
                    break;

                case EmoteType.SetEyeTexture:
                    //if (creature != null)
                    //    creature.EyesTextureDID = (uint)emote.Display;
                    break;

                case EmoteType.SetFloatStat:

                    if (player != null)
                    {
                        player.UpdateProperty(player, (PropertyFloat)emote.Stat, emote.Percent);
                        player.EnqueueBroadcast(false, new GameMessagePublicUpdatePropertyFloat(player, (PropertyFloat)emote.Stat, Convert.ToDouble(emote.Percent)));
                    }
                    break;

                case EmoteType.SetHeadObject:
                    //if (creature != null)
                    //    creature.HeadObjectDID = (uint)emote.Display;
                    break;

                case EmoteType.SetHeadPalette:
                    break;

                case EmoteType.SetInt64Stat:

                    if (player != null)
                    {
                        player.UpdateProperty(player, (PropertyInt64)emote.Stat, emote.Amount64);
                        player.EnqueueBroadcast(false, new GameMessagePublicUpdatePropertyInt64(player, (PropertyInt64)emote.Stat, Convert.ToInt64(emote.Amount64)));
                    }
                    break;

                case EmoteType.SetIntStat:

                    if (player != null)
                    {
                        player.UpdateProperty(player, (PropertyInt)emote.Stat, emote.Amount);
                        player.EnqueueBroadcast(false, new GameMessagePublicUpdatePropertyInt(player, (PropertyInt)emote.Stat, Convert.ToInt32(emote.Amount)));
                    }
                    break;

                case EmoteType.SetMouthPalette:
                    break;

                case EmoteType.SetMouthTexture:
                    //if (creature != null)
                    //    creature.MouthTextureDID = (uint)emote.Display;
                    break;

                case EmoteType.SetNosePalette:
                    break;

                case EmoteType.SetNoseTexture:
                    //if (creature != null)
                    //    creature.NoseTextureDID = (uint)emote.Display;
                    break;

                case EmoteType.SetMyQuestBitsOff:
                case EmoteType.SetQuestBitsOff:

                    questTarget = GetQuestTarget((EmoteType)emote.Type, targetCreature, creature);

                    if (questTarget != null && emote.Message != null && emote.Amount != null)
                        questTarget.QuestManager.SetQuestBits(emote.Message, (int)emote.Amount, false);

                    break;

                case EmoteType.SetMyQuestBitsOn:
                case EmoteType.SetQuestBitsOn:

                    questTarget = GetQuestTarget((EmoteType)emote.Type, targetCreature, creature);

                    if (questTarget != null && emote.Message != null && emote.Amount != null)
                        questTarget.QuestManager.SetQuestBits(emote.Message, (int)emote.Amount);

                    break;

                case EmoteType.SetMyQuestCompletions:
                case EmoteType.SetQuestCompletions:

                    questTarget = GetQuestTarget((EmoteType)emote.Type, targetCreature, creature);

                    if (questTarget != null && emote.Amount != null)
                        questTarget.QuestManager.SetQuestCompletions(emote.Message, (int)emote.Amount);

                    break;

                case EmoteType.SetSanctuaryPosition:

                    if (player != null)
                        player.SetPosition(PositionType.Sanctuary, new Position(emote.ObjCellId.Value, emote.OriginX.Value, emote.OriginY.Value, emote.OriginZ.Value, emote.AnglesX.Value, emote.AnglesY.Value, emote.AnglesZ.Value, emote.AnglesW.Value));
                    break;

                case EmoteType.Sound:

                    WorldObject.EnqueueBroadcast(new GameMessageSound(WorldObject.Guid, (Sound)emote.Sound, 1.0f));
                    break;

                case EmoteType.SpendLuminance:

                    if (player != null)
                        player.SpendLuminance(emote.HeroXP64 ?? 0);
                    break;

                case EmoteType.StampFellowQuest:

                    if (player != null)
                    {
                        if (player.Fellowship != null)
                        {
                            var questName = emote.Message;

                            player.Fellowship.QuestManager.Stamp(emote.Message);
                        }
                    }
                    break;

                case EmoteType.StampMyQuest:
                case EmoteType.StampQuest:

                    questTarget = GetQuestTarget((EmoteType)emote.Type, targetCreature, creature);

                    if (questTarget != null)
                    {
                        var questName = emote.Message;

                        if (questName.EndsWith("@#kt", StringComparison.Ordinal))
                            log.Warn($"0x{WorldObject.Guid}:{WorldObject.Name} ({WorldObject.WeenieClassId}).EmoteManager.ExecuteEmote: EmoteType.StampQuest({questName}) is a depreciated kill task method.");

                        questTarget.QuestManager.Stamp(emote.Message);
                    }
                    break;

                case EmoteType.StartBarber:

                    if (player != null)
                        player.StartBarber();
                    break;

                case EmoteType.StartEvent:

                    EventManager.StartEvent(emote.Message, WorldObject, targetObject);
                    break;

                case EmoteType.StopEvent:

                    EventManager.StopEvent(emote.Message, WorldObject, targetObject);
                    break;

                case EmoteType.TakeItems:

                    if (player != null)
                    {
                        var weenieItemToTake = emote.WeenieClassId ?? 0;
                        var amountToTake = emote.StackSize ?? 1;

                        if (weenieItemToTake == 0)
                        {
                            log.Warn($"EmoteManager.Execute: 0x{WorldObject.Guid} {WorldObject.Name} ({WorldObject.WeenieClassId}) EmoteType.TakeItems has invalid emote.WeenieClassId: {weenieItemToTake}");
                            break;
                        }

                        if (amountToTake < -1 || amountToTake == 0)
                        {
                            log.Warn($"EmoteManager.Execute: 0x{WorldObject.Guid} {WorldObject.Name} ({WorldObject.WeenieClassId}) EmoteType.TakeItems has invalid emote.StackSize: {amountToTake}");
                            break;
                        }

                        if ((player.GetNumInventoryItemsOfWCID(weenieItemToTake) > 0 && player.TryConsumeFromInventoryWithNetworking(weenieItemToTake, amountToTake == -1 ? int.MaxValue : amountToTake))
                            || (player.GetNumEquippedObjectsOfWCID(weenieItemToTake) > 0 && player.TryConsumeFromEquippedObjectsWithNetworking(weenieItemToTake, amountToTake == -1 ? int.MaxValue : amountToTake)))
                        {
                            var itemTaken = DatabaseManager.World.GetCachedWeenie(weenieItemToTake);
                            if (itemTaken != null)
                            {
                                var amount = amountToTake == -1 ? "all" : amountToTake.ToString();

                                var msg = $"You hand over {amount} of your {itemTaken.GetPluralName()}.";

                                player.Session.Network.EnqueueSend(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                            }
                        }
                    }
                    break;

                case EmoteType.TeachSpell:

                    if (player != null)
                        player.LearnSpellWithNetworking((uint)emote.SpellId);
                    break;

                case EmoteType.TeleportSelf:

                    //if (WorldObject is Player)
                    //(WorldObject as Player).Teleport(emote.Position);
                    break;

                case EmoteType.TeleportTarget:

                    if (player != null)
                    {
                        if (emote.ObjCellId.HasValue && emote.OriginX.HasValue && emote.OriginY.HasValue && emote.OriginZ.HasValue && emote.AnglesX.HasValue && emote.AnglesY.HasValue && emote.AnglesZ.HasValue && emote.AnglesW.HasValue)
                        {
                            var destination = new Position(emote.ObjCellId.Value, emote.OriginX.Value, emote.OriginY.Value, emote.OriginZ.Value, emote.AnglesX.Value, emote.AnglesY.Value, emote.AnglesZ.Value, emote.AnglesW.Value);

                            WorldObject.AdjustDungeon(destination);
                            WorldManager.ThreadSafeTeleport(player, destination);
                        }
                    }
                    break;

                case EmoteType.Tell:

                    if (player != null)
                    {
                        message = Replace(emote.Message, WorldObject, player, emoteSet.Quest);
                        player.Session.Network.EnqueueSend(new GameEventTell(WorldObject, message, player, ChatMessageType.Tell));
                    }
                    break;

                case EmoteType.TellFellow:

                    if (player != null)
                    {
                        var fellowship = player.Fellowship;
                        if (fellowship != null)
                        {
                            text = Replace(emote.Message, WorldObject, player, emoteSet.Quest);

                            fellowship.TellFellow(WorldObject, text);
                        }
                    }
                    break;

                case EmoteType.TextDirect:

                    if (player != null)
                    {
                        message = Replace(emote.Message, WorldObject, player, emoteSet.Quest);
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat(message, ChatMessageType.Broadcast));
                    }
                    break;

                case EmoteType.Turn:

                    if (creature != null)
                    {
                        // turn to heading
                        var rotation = new Quaternion(emote.AnglesX ?? 0, emote.AnglesY ?? 0, emote.AnglesZ ?? 0, emote.AnglesW ?? 1);
                        var newPos = new Position(creature.Location);
                        newPos.Rotation = rotation;

                        var rotateTime = creature.TurnTo(newPos);
                        delay = rotateTime;
                    }
                    break;

                case EmoteType.TurnToTarget:

                    if (creature != null && targetCreature != null)
                        delay = creature.Rotate(targetCreature);

                    break;

                case EmoteType.UntrainSkill:

                    if (player != null)
                        player.ResetSkill((Skill)emote.Stat);
                    break;

                case EmoteType.UpdateFellowQuest:

                    if (player != null)
                    {
                        if (player.Fellowship != null)
                        {
                            var questName = emote.Message;

                            var hasQuest = player.Fellowship.QuestManager.HasQuest(questName);

                            if (!hasQuest)
                            {
                                // add new quest
                                player.Fellowship.QuestManager.Update(questName);
                                hasQuest = player.Fellowship.QuestManager.HasQuest(questName);
                                ExecuteEmoteSet(hasQuest ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                            }
                            else
                            {
                                // update existing quest
                                var canSolve = player.Fellowship.QuestManager.CanSolve(questName);
                                if (canSolve)
                                    player.Fellowship.QuestManager.Stamp(questName);
                                ExecuteEmoteSet(canSolve ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                            }
                        }
                        else
                            ExecuteEmoteSet(EmoteCategory.QuestNoFellow, emote.Message, targetObject, true);
                    }
                    break;

                case EmoteType.UpdateMyQuest:
                case EmoteType.UpdateQuest:

                    questTarget = GetQuestTarget((EmoteType)emote.Type, targetCreature, creature);

                    if (questTarget != null)
                    {
                        var questName = emote.Message;

                        var hasQuest = questTarget.QuestManager.HasQuest(questName);

                        if (!hasQuest)
                        {
                            // add new quest
                            questTarget.QuestManager.Update(questName);
                            hasQuest = questTarget.QuestManager.HasQuest(questName);
                            ExecuteEmoteSet(hasQuest ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                        }
                        else
                        {
                            // update existing quest
                            var canSolve = questTarget.QuestManager.CanSolve(questName);
                            if (canSolve)
                                questTarget.QuestManager.Stamp(questName);
                            ExecuteEmoteSet(canSolve ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, emote.Message, targetObject, true);
                        }
                    }

                    break;

                case EmoteType.WorldBroadcast:

                    message = Replace(text, WorldObject, targetObject, emoteSet.Quest);

                    PlayerManager.BroadcastToAll(new GameMessageSystemChat(message, ChatMessageType.WorldBroadcast));

                    PlayerManager.LogBroadcastChat(Channel.AllBroadcast, WorldObject, message);

                    break;

                default:
                    log.Debug($"EmoteManager.Execute - Encountered Unhandled EmoteType {(EmoteType)emote.Type} for {WorldObject.Name} ({WorldObject.WeenieClassId})");
                    break;
            }

            return delay;
        }

        /// <summary>
        /// Selects an emote set based on category, and optional: quest, vendor, rng
        /// </summary>
        public PropertiesEmote GetEmoteSet(EmoteCategory category, string questName = null, VendorType? vendorType = null, uint? wcid = null, bool useRNG = true)
        {
            //if (Debug) Console.WriteLine($"{WorldObject.Name}.EmoteManager.GetEmoteSet({category}, {questName}, {vendorType}, {wcid}, {useRNG})");

            if (_worldObject.Biota.PropertiesEmote == null)
                return null;

            // always pull emoteSet from _worldObject
            var emoteSet = _worldObject.Biota.PropertiesEmote.Where(e => e.Category == category);

            // optional criteria
            if ((category == EmoteCategory.HearChat || category == EmoteCategory.ReceiveTalkDirect) && questName != null)
                emoteSet = emoteSet.Where(e => e.Quest != null && e.Quest.Equals(questName, StringComparison.OrdinalIgnoreCase) || e.Quest == null);
            else if (questName != null)
                emoteSet = emoteSet.Where(e => e.Quest != null && e.Quest.Equals(questName, StringComparison.OrdinalIgnoreCase));
            if (vendorType != null)
                emoteSet = emoteSet.Where(e => e.VendorType != null && e.VendorType.Value == vendorType);
            if (wcid != null)
                emoteSet = emoteSet.Where(e => e.WeenieClassId == wcid.Value);

            if (category == EmoteCategory.HeartBeat)
            {
                WorldObject.GetCurrentMotionState(out MotionStance currentStance, out MotionCommand currentMotion);

                emoteSet = emoteSet.Where(e => e.Style == null || e.Style == currentStance);
                emoteSet = emoteSet.Where(e => e.Substyle == null || e.Substyle == currentMotion);
            }

            if (category == EmoteCategory.WoundedTaunt)
            {
                if (_worldObject is Creature creature)
                    emoteSet = emoteSet.Where(e => creature.Health.Percent >= e.MinHealth && creature.Health.Percent <= e.MaxHealth);
            }

            if (useRNG)
            {
                var rng = ThreadSafeRandom.Next(0.0f, 1.0f);
                emoteSet = emoteSet.Where(e => e.Probability > rng).OrderBy(e => e.Probability);
                //emoteSet = emoteSet.Where(e => e.Probability >= rng);
            }

            return emoteSet.FirstOrDefault();
        }

        /// <summary>
        /// Convenience wrapper between GetEmoteSet and ExecututeEmoteSet
        /// </summary>
        public void ExecuteEmoteSet(EmoteCategory category, string quest = null, WorldObject targetObject = null, bool nested = false)
        {
            //if (Debug) Console.WriteLine($"{WorldObject.Name}.EmoteManager.ExecuteEmoteSet({category}, {quest}, {targetObject}, {nested})");

            var emoteSet = GetEmoteSet(category, quest);

            if (emoteSet == null) return;

            // TODO: revisit if nested chains need to propagate timers
            ExecuteEmoteSet(emoteSet, targetObject, nested);
        }

        /// <summary>
        /// Executes a set of emotes to run with delays
        /// </summary>
        /// <param name="emoteSet">A list of emotes to execute</param>
        /// <param name="targetObject">An optional target, usually player</param>
        /// <param name="actionChain">For adding delays between emotes</param>
        public bool ExecuteEmoteSet(PropertiesEmote emoteSet, WorldObject targetObject = null, bool nested = false)
        {
            //if (Debug) Console.WriteLine($"{WorldObject.Name}.EmoteManager.ExecuteEmoteSet({emoteSet}, {targetObject}, {nested})");

            // detect busy state
            // TODO: maybe eventually we should consider having categories that can be queued?
            // there are some categories that shouldn't be queued, like heartbeats...
            if (IsBusy && !nested) return false;

            // start action chain
            Nested++;
            Enqueue(emoteSet, targetObject);

            return true;
        }

        public void Enqueue(PropertiesEmote emoteSet, WorldObject targetObject, int emoteIdx = 0, float delay = 0.0f)
        {
            //if (Debug) Console.WriteLine($"{WorldObject.Name}.EmoteManager.Enqueue({emoteSet}, {targetObject}, {emoteIdx}, {delay})");

            if (emoteSet == null)
            {
                Nested--;
                return;
            }

            IsBusy = true;

            var emote = emoteSet.PropertiesEmoteAction.ElementAt(emoteIdx);

            if (delay + emote.Delay > 0)
            {
                var actionChain = new ActionChain();

                if (Debug)
                    actionChain.AddAction(WorldObject, () => Console.Write($"{emote.Delay} - "));

                // delay = post-delay from actual time of previous emote
                // emote.Delay = pre-delay for current emote
                actionChain.AddDelaySeconds(delay + emote.Delay);

                actionChain.AddAction(WorldObject, () => DoEnqueue(emoteSet, targetObject, emoteIdx, emote));
                actionChain.EnqueueChain();
            }
            else
            {
                DoEnqueue(emoteSet, targetObject, emoteIdx, emote);
            }
        }

        /// <summary>
        /// This should only be called by Enqueue
        /// </summary>
        private void DoEnqueue(PropertiesEmote emoteSet, WorldObject targetObject, int emoteIdx, PropertiesEmoteAction emote)
        {
            if (Debug)
                Console.Write($"{(EmoteType)emote.Type}");

            var nextDelay = ExecuteEmote(emoteSet, emote, targetObject);

            if (Debug)
                Console.WriteLine($" - { nextDelay}");

            if (emoteIdx < emoteSet.PropertiesEmoteAction.Count - 1)
                Enqueue(emoteSet, targetObject, emoteIdx + 1, nextDelay);
            else
            {
                if (nextDelay > 0)
                {
                    var delayChain = new ActionChain();
                    delayChain.AddDelaySeconds(nextDelay);
                    delayChain.AddAction(WorldObject, () =>
                    {
                        Nested--;

                        if (Nested == 0)
                            IsBusy = false;
                    });
                    delayChain.EnqueueChain();
                }
                else
                {
                    Nested--;

                    if (Nested == 0)
                        IsBusy = false;
                }
            }
        }

        public bool HasValidTestNoQuality(string testName) => GetEmoteSet(EmoteCategory.TestNoQuality, testName) != null;

        /// <summary>
        /// The maximum animation range of the client
        /// Motions broadcast outside of this range will be automatically queued by client
        /// </summary>
        public static float ClientMaxAnimRange = 96.0f;     // verify: same indoors?

        /// <summary>
        /// The client automatically queues animations that are broadcast outside of 96.0f range
        /// Normally we exclude these emotes from being broadcast outside this range,
        /// but for certain emotes (like monsters going to sleep) we want to always broadcast / enqueue
        /// </summary>
        public static HashSet<MotionCommand> MotionQueue = new HashSet<MotionCommand>()
        {
            MotionCommand.Sleeping
        };

        public void DoVendorEmote(VendorType vendorType, WorldObject target)
        {
            var vendorSet = GetEmoteSet(EmoteCategory.Vendor, null, vendorType);
            var heartbeatSet = GetEmoteSet(EmoteCategory.Vendor, null, VendorType.Heartbeat);

            ExecuteEmoteSet(vendorSet, target);
            ExecuteEmoteSet(heartbeatSet, target, true);
        }

        public IEnumerable<PropertiesEmote> Emotes(EmoteCategory emoteCategory)
        {
            return WorldObject.Biota.PropertiesEmote.Where(x => x.Category == emoteCategory);
        }

        public string Replace(string message, WorldObject source, WorldObject target, string quest)
        {
            var result = message;

            var sourceName = source != null ? source.Name : "";
            var targetName = target != null ? target.Name : "";

            result = result.Replace("%n", sourceName);
            result = result.Replace("%mn", sourceName);
            result = result.Replace("%s", targetName);
            result = result.Replace("%tn", targetName);

            var sourceLevel = source != null ? $"{source.Level ?? 0}" : "";
            var targetLevel = target != null ? $"{target.Level ?? 0}" : "";
            result = result.Replace("%ml", sourceLevel);
            result = result.Replace("%tl", targetLevel);

            //var sourceTemplate = source != null ? source.GetProperty(PropertyString.Title) : "";
            //var targetTemplate = source != null ? target.GetProperty(PropertyString.Title) : "";
            var sourceTemplate = source != null ? source.GetProperty(PropertyString.Template) : "";
            var targetTemplate = target != null ? target.GetProperty(PropertyString.Template) : "";
            result = result.Replace("%mt", sourceTemplate);
            result = result.Replace("%tt", targetTemplate);

            var sourceHeritage = source != null ? source.HeritageGroupName : "";
            var targetHeritage = target != null ? target.HeritageGroupName : "";
            result = result.Replace("%mh", sourceHeritage);
            result = result.Replace("%th", targetHeritage);

            //result = result.Replace("%mf", $"{source.GetProperty(PropertyString.Fellowship)}");
            //result = result.Replace("%tf", $"{target.GetProperty(PropertyString.Fellowship)}");

            //result = result.Replace("%l", $"{???}"); // level?
            //result = result.Replace("%pk", $"{???}"); // pk status?
            //result = result.Replace("%a", $"{???}"); // allegiance?
            //result = result.Replace("%p", $"{???}"); // patron?

            // Find quest in standard or LSD custom usage for %tqt and %CDtime
            var embeddedQuestName = result.Contains("@") ? message.Split("@")[0] : null;
            var questName = !string.IsNullOrWhiteSpace(embeddedQuestName) ? embeddedQuestName : quest;

            // LSD custom tqt usage
            result = result.Replace($"{questName}@%tqt", "You may complete this quest again in %tqt.", StringComparison.OrdinalIgnoreCase);

            // LSD custom CDtime variable
            if (result.Contains("%CDtime"))
                result = result.Replace($"{questName}@", "", StringComparison.OrdinalIgnoreCase);

            if (target is Player targetPlayer)
            {
                result = result.Replace("%tqt", !string.IsNullOrWhiteSpace(quest) ? targetPlayer.QuestManager.GetNextSolveTime(questName).GetFriendlyString() : "");
                
                result = result.Replace("%CDtime", !string.IsNullOrWhiteSpace(quest) ? targetPlayer.QuestManager.GetNextSolveTime(questName).GetFriendlyString() : "");

                result = result.Replace("%tf", $"{(targetPlayer.Fellowship != null ? targetPlayer.Fellowship.FellowshipName : "")}");

                result = result.Replace("%fqt", !string.IsNullOrWhiteSpace(quest) && targetPlayer.Fellowship != null ? targetPlayer.Fellowship.QuestManager.GetNextSolveTime(questName).GetFriendlyString() : "");

                result = result.Replace("%tqm", !string.IsNullOrWhiteSpace(quest) ? targetPlayer.QuestManager.GetMaxSolves(questName).ToString() : "");

                result = result.Replace("%tqc", !string.IsNullOrWhiteSpace(quest) ? targetPlayer.QuestManager.GetCurrentSolves(questName).ToString() : "");
            }

            if (source is Creature sourceCreature)
            {
                result = result.Replace("%mqt", !string.IsNullOrWhiteSpace(quest) ? sourceCreature.QuestManager.GetNextSolveTime(questName).GetFriendlyString() : "");

                result = result.Replace("%mxqt", !string.IsNullOrWhiteSpace(quest) ? sourceCreature.QuestManager.GetNextSolveTime(questName).GetFriendlyLongString() : "");

                //result = result.Replace("%CDtime", !string.IsNullOrWhiteSpace(quest) ? sourceCreature.QuestManager.GetNextSolveTime(questName).GetFriendlyString() : "");

                result = result.Replace("%mqc", !string.IsNullOrWhiteSpace(quest) ? sourceCreature.QuestManager.GetCurrentSolves(questName).ToString() : "");
            }

            return result;
        }

        /// <summary>
        /// Returns the creature target for quest emotes
        /// </summary>
        public static Creature GetQuestTarget(EmoteType emote, Creature target, Creature self)
        {
            switch (emote)
            {
                // MyQuest always targets self
                case EmoteType.DecrementMyQuest:
                case EmoteType.EraseMyQuest:
                case EmoteType.IncrementMyQuest:
                case EmoteType.InqMyQuest:
                case EmoteType.InqMyQuestBitsOff:
                case EmoteType.InqMyQuestBitsOn:
                case EmoteType.InqMyQuestSolves:
                case EmoteType.SetMyQuestBitsOff:
                case EmoteType.SetMyQuestBitsOn:
                case EmoteType.SetMyQuestCompletions:
                case EmoteType.StampMyQuest:
                case EmoteType.UpdateMyQuest:

                    return self;

                default:

                    return target ?? self;
            }
        }

        private WorldObject GetSpellTarget(Spell spell, WorldObject target)
        {
            var targetSelf = spell.Flags.HasFlag(SpellFlags.SelfTargeted);
            var untargeted = spell.NonComponentTargetType == ItemType.None;

            var spellTarget = target;
            if (untargeted)
                spellTarget = null;
            else if (targetSelf)
                spellTarget = WorldObject;

            return spellTarget;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void HeartBeat()
        {
            // player didn't do idle emotes in retail?
            if (WorldObject is Player)
                return;

            if (WorldObject is Creature creature && creature.IsAwake)
                return;

            ExecuteEmoteSet(EmoteCategory.HeartBeat);
        }

        public void OnUse(Creature activator)
        {
            ExecuteEmoteSet(EmoteCategory.Use, null, activator);
        }

        public void OnPortal(Creature activator)
        {
            IsBusy = false;

            ExecuteEmoteSet(EmoteCategory.Portal, null, activator);
        }

        public void OnActivation(Creature activator)
        {
            ExecuteEmoteSet(EmoteCategory.Activation, null, activator);
        }

        public void OnGeneration()
        {
            ExecuteEmoteSet(EmoteCategory.Generation, null, null);
        }

        public void OnWield(Creature wielder)
        {
            ExecuteEmoteSet(EmoteCategory.Wield, null, wielder);
        }

        public void OnUnwield(Creature wielder)
        {
            ExecuteEmoteSet(EmoteCategory.UnWield, null, wielder);
        }

        public void OnPickup(Creature initiator)
        {
            ExecuteEmoteSet(EmoteCategory.PickUp, null, initiator);
        }

        public void OnDrop(Creature dropper)
        {
            ExecuteEmoteSet(EmoteCategory.Drop, null, dropper);
        }

        /// <summary>
        /// Called when an idle mob becomes alerted by a player
        /// and initially wakes up
        /// </summary>
        public void OnWakeUp(Creature target)
        {
            ExecuteEmoteSet(EmoteCategory.Scream, null, target);
        }

        /// <summary>
        /// Called when a monster switches targets
        /// </summary>
        public void OnNewEnemy(WorldObject newEnemy)
        {
            ExecuteEmoteSet(EmoteCategory.NewEnemy, null, newEnemy);
        }

        /// <summary>
        /// Called when a monster completes an attack
        /// </summary>
        public void OnAttack(WorldObject target)
        {
            ExecuteEmoteSet(EmoteCategory.Taunt, null, target);
        }

        public void OnDamage(Creature attacker)
        {
            ExecuteEmoteSet(EmoteCategory.WoundedTaunt, null, attacker);
        }

        public void OnReceiveCritical(Creature attacker)
        {
            ExecuteEmoteSet(EmoteCategory.ReceiveCritical, null, attacker);
        }

        public void OnResistSpell(Creature attacker)
        {
            ExecuteEmoteSet(EmoteCategory.ResistSpell, null, attacker);
        }

        public void OnDeath(DamageHistoryInfo lastDamagerInfo)
        {
            IsBusy = false;

            var lastDamager = lastDamagerInfo?.TryGetPetOwnerOrAttacker();

            ExecuteEmoteSet(EmoteCategory.Death, null, lastDamager);
        }

        /// <summary>
        /// Called when a monster kills a player
        /// </summary>
        public void OnKill(Player player)
        {
            ExecuteEmoteSet(EmoteCategory.KillTaunt, null, player);
        }

        /// <summary>
        /// Called when player interacts with item that has a Quest string
        /// </summary>
        public void OnQuest(Creature initiator)
        {
            var questName = WorldObject.Quest;

            var hasQuest = initiator.QuestManager.HasQuest(questName);

            if (!hasQuest)
            {
                // add new quest
                initiator.QuestManager.Update(questName);
                hasQuest = initiator.QuestManager.HasQuest(questName);
                ExecuteEmoteSet(hasQuest ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, questName, initiator);
            }
            else
            {
                // update existing quest
                var canSolve = initiator.QuestManager.CanSolve(questName);
                if (canSolve)
                    initiator.QuestManager.Stamp(questName);
                ExecuteEmoteSet(canSolve ? EmoteCategory.QuestSuccess : EmoteCategory.QuestFailure, questName, initiator);
            }
        }

        /// <summary>
        /// Called when this NPC receives a direct text message from a player
        /// </summary>
        public void OnTalkDirect(Player player, string message)
        {
            ExecuteEmoteSet(EmoteCategory.ReceiveTalkDirect, message, player);
        }

        /// <summary>
        /// Called when this NPC receives a local signal from a player
        /// </summary>
        public void OnLocalSignal(WorldObject emitter, string message)
        {
            ExecuteEmoteSet(EmoteCategory.ReceiveLocalSignal, message, emitter);
        }

        /// <summary>
        /// Called when monster exceeds the maximum distance from home position
        /// </summary>
        public void OnHomeSick(WorldObject attackTarget)
        {
            ExecuteEmoteSet(EmoteCategory.Homesick, null, attackTarget);
        }

        /// <summary>
        /// Called when this NPC hears local chat from a player
        /// </summary>
        public void OnHearChat(Player player, string message)
        {
            ExecuteEmoteSet(EmoteCategory.HearChat, message, player);
        }

        //public bool HasAntennas => WorldObject.Biota.BiotaPropertiesEmote.Count(x => x.Category == (int)EmoteCategory.ReceiveLocalSignal) > 0;

        /// <summary>
        /// Call this function when WorldObject is being used via a proxy object, e.g.: Hooker on a Hook
        /// </summary>
        public void SetProxy(WorldObject worldObject)
        {
            _proxy = worldObject;
        }

        /// <summary>
        /// Called when this object is removed from the proxy object (Hooker is picked up from Hook)
        /// </summary>
        public void ClearProxy()
        {
            _proxy = null;
        }
    }
}

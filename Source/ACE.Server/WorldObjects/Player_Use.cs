using System;
using System.Collections.Generic;
using System.Threading;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// This is set by HandleActionUseItem
        /// </summary>
        private ObjectGuid lastUsedContainerId;


        private int moveToChainCounter;

        private int GetNextMoveToChainNumber()
        {
            return Interlocked.Increment(ref moveToChainCounter);
        }

        private void StopExistingMoveToChains()
        {
            Interlocked.Increment(ref moveToChainCounter);
        }

        private ActionChain CreateMoveToChain(ObjectGuid target, float distance, out int thisMoveToChainNumber)
        {
            thisMoveToChainNumber = GetNextMoveToChainNumber();

            ActionChain moveToChain = new ActionChain();

            moveToChain.AddAction(this, () =>
            {
                var targetObject = CurrentLandblock.GetObject(target);

                if (targetObject == null)
                {
                    // Is the item we're trying to move to in the container we have open?
                    var lastUsedContainer = CurrentLandblock.GetObject(lastUsedContainerId) as Container;

                    if (lastUsedContainer != null)
                    {
                        if (lastUsedContainer.Inventory.ContainsKey(target))
                            targetObject = lastUsedContainer;
                        else
                        {
                            // could be a child container of this container
                            log.Error("Player_Use CreateMoveToChain container inception not finished");
                            return;
                        }
                    }
                }

                if (targetObject == null)
                {
                    log.Error("Player_Use CreateMoveToChain targetObject null");
                    return;
                }

                if (targetObject.Location == null)
                {
                    log.Error("Player_Use CreateMoveToChain targetObject.Location null");
                    return;
                }

                if (targetObject.WeenieType == WeenieType.Portal)
                    OnAutonomousMove(targetObject.Location, Sequences, MovementTypes.MoveToPosition, target);
                else
                    OnAutonomousMove(targetObject.Location, Sequences, MovementTypes.MoveToObject, target);
            });

            // poll for arrival every .1 seconds
            ActionChain moveToBody = new ActionChain();
            moveToBody.AddDelaySeconds(.1);

            var thisMoveToChainNumberCopy = thisMoveToChainNumber;

            moveToChain.AddLoop(this, () =>
            {
                if (thisMoveToChainNumberCopy != moveToChainCounter)
                    return false;

                // Break loop if CurrentLandblock == null (we portaled or logged out)
                if (CurrentLandblock == null)
                    return false;

                // Are we within use radius?
                bool ret = !CurrentLandblock.WithinUseRadius(this, target, out var valid);

                // If one of the items isn't on a landblock
                if (!valid)
                    ret = false;

                return ret;
            }, moveToBody);

            return moveToChain;
        }

        private ActionChain CreateMoveToChain(WorldObject target, float distance, out int thisMoveToChainNumber)
        {
            thisMoveToChainNumber = GetNextMoveToChainNumber();

            ActionChain moveToChain = new ActionChain();

            moveToChain.AddAction(this, () =>
            {
                if (target.Location == null)
                {
                    log.Error("Player_Use CreateMoveToChain targetObject.Location null");
                    return;
                }

                if (target.WeenieType == WeenieType.Portal)
                    OnAutonomousMove(target.Location, Sequences, MovementTypes.MoveToPosition, target.Guid);
                else
                    OnAutonomousMove(target.Location, Sequences, MovementTypes.MoveToObject, target.Guid);
            });

            // poll for arrival every .1 seconds
            ActionChain moveToBody = new ActionChain();
            moveToBody.AddDelaySeconds(.1);

            var thisMoveToChainNumberCopy = thisMoveToChainNumber;

            moveToChain.AddLoop(this, () =>
            {
                if (thisMoveToChainNumberCopy != moveToChainCounter)
                    return false;

                // Break loop if CurrentLandblock == null (we portaled or logged out)
                if (CurrentLandblock == null)
                    return false;

                // Are we within use radius?
                bool ret = !CurrentLandblock.WithinUseRadius(this, target.Guid, out var valid);

                // If one of the items isn't on a landblock
                if (!valid)
                    ret = false;

                return ret;
            }, moveToBody);

            return moveToChain;
        }


        public void SendUseDoneEvent(WeenieError errorType = WeenieError.None)
        {
            Session.Network.EnqueueSend(new GameEventUseDone(Session, errorType));
        }


        // ===============================
        // Game Action Handlers - Use Item
        // ===============================
        // These are raised by client actions
        
        
        /// Table data was pulled from Endy's Tinkering Calculator
        static Dictionary<int, int> SalvageMods = new Dictionary<int, int>()
        {
             {
              10,
              20
            },
            {
              66,
              11
            },
            {
              13,
              20
            },
            {
              53,
              11
            },
            {
              14,
              20
            },
            {
              15,
              20
            },
            {
              16,
              20
            },
            {
              17,
              25
            },
            {
              57,
              11
            },
            {
              58,
              11
            },
            {
              18,
              25
            },
            {
              1,
              11
            },
            {
              19,
              25
            },
            {
              59,
              15
            },
            {
              73,
              12
            },
            {
              21,
              20
            },
            {
              22,
              20
            },
            {
              60,
              10
            },
            {
              67,
              11
            },
            {
              23,
              12
            },
            {
              25,
              25
            },
            {
              26,
              20
            },
            {
              61,
              12
            },
            {
              27,
              20
            },
            {
              29,
              25
            },
            {
              4,
              11
            },
            {
              74,
              12
            },
            {
              30,
              25
            },
            {
              68,
              11
            },
            {
              31,
              11
            },
            {
              75,
              10
            },
            {
              33,
              11
            },
            {
              34,
              20
            },
            {
              76,
              11
            },
            {
              2,
              12
            },
            {
              55,
             11
            },
            {
              35,
              20
            },
            {
              36,
              25
            },
            {
              37,
              25
            },
            {
              5,
              12
            },
            {
              63,
              15
            },
            {
              64,
              12
            },
            {
              41,
              20
            },
            {
              77,
              12
            },
            {
              7,
              11
            },
            {
              47,
              20
            },
            {
              8,
              11
            },
            {
              49,
              20
            },
            {
              50,
              20
            },
            {
                ///armatures
              80,
              25
            },
            {
                ///armatures
              81,
              25
            },
            {
                ///armatures
              83,
              25
            }
          };

        static Dictionary<int, int> imbueEffect = new Dictionary<int, int>()
        {
             {
                ////Cold
              13,
              128
            },
            {
                ///Pierce
              15,
              16
            },
            {
                ///Black Opal
                16,
                1
            },
            {
                ///Acid, just a guess
                21,
                64
            },
            {
               ///Fire Opal
               22,
               2
            },
            {
                ///Imperial Topaz
                26,
                32
            },
            {
                ///Jet
                27,
                256
            },
            {
                ///Red Garnet
                35,
                512
            },
            {
                ///Sunstone
                41,
                4
            },
            {
                ///White Sapphire
                47,
                8
            }
        };

        static Dictionary<int, int> iconUnderlay = new Dictionary<int, int>()
        {
            ///These are as accurate as I could find. One may be inaccurate, but this will be easy to change.
             {
                ////Cold
              13,
              0x06003353
            },
            {
                ///Pierce
              15,
              0x06003358
            },
            {
                ///Black Opal
                16,
                0x06003357
            },
            {
                ///Acid, just a guess
                21,
                0x06003355
            },
            {
               ///Fire Opal
               22,
               0x06003357
            },
            {
                ///Imperial Topaz
                26,
                0x0600335c
            },
            {
                ///Jet
                27,
                0x06003354
            },
            {
                ///Red Garnet
                35,
                0x06003359
            },
            {
                ///Sunstone
                41,
                0x06003356
            },
            {
                ///White Sapphire
                47,
                0x0600335a
            }
        };

        public void HandleActionUseWithTarget(ObjectGuid sourceObjectId, ObjectGuid targetObjectId)
        {
            StopExistingMoveToChains();

            new ActionChain(this, () =>
            {
            var invSource = GetInventoryItem(sourceObjectId);
            var invTarget = GetInventoryItem(targetObjectId);

            var worldTarget = (invTarget == null) ? CurrentLandblock.GetObject(targetObjectId) : null;

            if (invTarget != null)
            {
                double[] diffs = { 1, 1.1, 1.3, 1.6, 2.0, 2.5, 3.0, 3.5, 4.0, 4.5 };
                double AttemptDiff = diffs[invTarget.GetProperty(PropertyInt.NumTimesTinkered) ?? 0 + 1];
                double SalvageMod = SalvageMods[(int)invSource.MaterialType];
                double Multiple;
                if (invSource.Workmanship < invTarget.Workmanship)
                {
                    Multiple = 1;
                }
                else
                {
                    Multiple = 2;
                }
                double itemWorkmanship = invTarget.Workmanship ?? 1.0;
                double salvageWorkmanship = invSource.Workmanship ?? 1.0;
                double buffedWeaponSkill = (double)GetCreatureSkill(Skill.WeaponTinkering).Current;
                double buffedArmorSkill = (double)GetCreatureSkill(Skill.ArmorTinkering).Current;
                double buffedItemSkill = (double)GetCreatureSkill(Skill.ItemTinkering).Current;
                double buffedMagicItemSkill = (double)GetCreatureSkill(Skill.MagicItemTinkering).Current;

                // inventory on inventory, we can do this now
                if ((invSource.MaterialType == Material.Aquamarine || invSource.MaterialType == Material.Sunstone || invSource.MaterialType == Material.RedGarnet || invSource.MaterialType == Material.WhiteSapphire || invSource.MaterialType == Material.Jet || invSource.MaterialType == Material.BlackGarnet || invSource.MaterialType == Material.FireOpal || invSource.MaterialType == Material.BlackOpal  || invSource.MaterialType == Material.ImperialTopaz || invSource.MaterialType == Material.Emerald) && invSource.MaxStructure != null )
                {
                        string imbueName = invTarget.GetProperty(PropertyString.ImbuerName) ?? "";
                        int timesTinkered = invTarget.GetProperty(PropertyInt.NumTimesTinkered) ?? 0;
                    if (invSource.MaxStructure != 100 || timesTinkered > 9 || imbueName.Length > 0)
                    {
                            Console.WriteLine("Imbue name length = " + imbueName.Length);
                            Console.WriteLine("Num times tinkered  = " + timesTinkered);
                            SendUseDoneEvent();
                    }
                    else
                    {
                        if (GetCharacterOptions1(CharacterOptions1.UseCraftingChangeOfSuccessDialog))
                        {
                            ///this is after I understand the best way to handle the user input box
                        }
                        else
                        {
                            double tinkDiff = Math.Floor(((5 * SalvageMod) + (2 * itemWorkmanship * SalvageMod) - (salvageWorkmanship * Multiple * SalvageMod / 5)) * AttemptDiff);
                                double chance = Math.Round((1 - (1 / (1 + Math.Exp(0.03 * (buffedWeaponSkill - tinkDiff))))) * 100.0);
                                ///Augmentations are not in yet, so not worrying about Charmed Smith modifier chance.
                                double imbueChance = chance * .33;
                                Random r = new Random();
                                double roll = (double)r.Next(10000) / 100.00;
                                if (roll <= imbueChance)
                                {
                                    var clapAnimation = new MotionItem(MotionCommand.ClapHands, 1.0f);

                                    var motion = new UniversalMotion(this.CurrentMotionState.Stance, clapAnimation);
                                    motion.MovementData.CurrentStyle = (uint)this.CurrentMotionState.Stance;

                                    var actionChain = new ActionChain();
                                    actionChain.AddAction(this, () => this.DoMotion(motion));
                                    actionChain.AddDelaySeconds(1);
                                    actionChain.EnqueueChain();
                                    Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyDataID(invTarget, PropertyDataId.IconUnderlay, (uint)iconUnderlay[(int)invSource.MaterialType]));
                                    Session.Network.EnqueueSend(new GameMessagePublicUpdatePropertyString(invTarget, PropertyString.ImbuerName, this.Name));
                                    ///UpdateIconDID (May not need to do it)
                                    int tinked = invTarget.GetProperty(PropertyInt.NumTimesTinkered) ?? 0;
                                    invTarget.SetProperty(PropertyDataId.IconUnderlay, (uint)iconUnderlay[(int)invSource.MaterialType]);
                                    invTarget.SetProperty(PropertyString.TinkerName, this.Name);
                                    invTarget.SetProperty(PropertyString.ImbuerName, this.Name);
                                    invTarget.SetProperty(PropertyInt.ImbuedEffect, imbueEffect[(int)invSource.MaterialType]);
                                    invTarget.SetProperty(PropertyInt.NumTimesTinkered, tinked + 1);
                                    string text = this.Name + " successfully applies the " + invSource.Name + " (workmanship " + invSource.Workmanship + ") to the " + invTarget.Name + ".";
                                    Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Craft));
                                    Session.Network.EnqueueSend(new GameMessageUpdateObject(invTarget));
                                    SendUseDoneEvent();

                                }
                                else
                                {
                                    string text = this.Name + " fails to apply the " + invSource.Name + " (workmanship " + invSource.Workmanship + ") to the " + invTarget.Name + ". The target is destroyed.";
                                    TryRemoveFromInventoryWithNetworking(invSource);
                                    TryRemoveFromInventoryWithNetworking(invTarget);
                                    Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Craft));
                                    SendUseDoneEvent();

                                }
                         }
                     }
                    }
                    else if ((invSource.MaterialType == Material.Steel || invSource.MaterialType == Material.Brass || invSource.MaterialType == Material.Alabaster || invSource.MaterialType == Material.ArmoredilloHide ||
                             invSource.MaterialType == Material.Bronze || invSource.MaterialType == Material.Ceramic || invSource.MaterialType == Material.Copper || invSource.MaterialType == Material.Ebony ||
                             invSource.MaterialType == Material.Gold || invSource.MaterialType == Material.Granite || invSource.MaterialType == Material.GreenGarnet || invSource.MaterialType == Material.Iron ||
                             invSource.MaterialType == Material.Ivory || invSource.MaterialType == Material.Leather || invSource.MaterialType == Material.Linen || invSource.MaterialType == Material.Mahogany ||
                             invSource.MaterialType == Material.Marble || invSource.MaterialType == Material.Moonstone || invSource.MaterialType == Material.Oak || invSource.MaterialType == Material.Opal ||
                             invSource.MaterialType == Material.Pine || invSource.MaterialType == Material.Porcelain || invSource.MaterialType == Material.ReedSharkHide || invSource.MaterialType == Material.Sandstone ||
                             invSource.MaterialType == Material.Satin || invSource.MaterialType == Material.Silk || invSource.MaterialType == Material.Silver || invSource.MaterialType == Material.Teak ||
                             invSource.MaterialType == Material.Velvet || invSource.MaterialType == Material.Wool) && invSource.MaxStructure != null)
                    {
                        if (invSource.MaxStructure != 100 || invTarget.GetProperty(PropertyInt.NumTimesTinkered) > 9)
                        {
                            SendUseDoneEvent();
                        }
                        else
                        {
                            if (GetCharacterOptions1(CharacterOptions1.UseCraftingChangeOfSuccessDialog))
                            {
                                ///this is after I understand the best way to handle the user input box
                            }
                            else
                            {
                                ///This formula was obtained from Endy's Tinkering Calculator
                                double tinkDiff = Math.Floor(((5 * SalvageMod) + (2 * itemWorkmanship * SalvageMod) - (salvageWorkmanship * Multiple * SalvageMod / 5)) * AttemptDiff);
                                double chance = Math.Round((1 - (1 / (1 + Math.Exp(0.03 * (buffedWeaponSkill - tinkDiff))))) * 100.0);
                                ///Augmentations are not in yet, so not worrying about Charmed Smith modifier chance.
                                Random r = new Random();
                                var clapAnimation = new MotionItem(MotionCommand.ClapHands, 1.0f);

                                var motion = new UniversalMotion(this.CurrentMotionState.Stance, clapAnimation);
                                motion.MovementData.CurrentStyle = (uint)this.CurrentMotionState.Stance;

                                var actionChain = new ActionChain();
                                actionChain.AddAction(this, () => this.DoMotion(motion));
                                actionChain.AddDelaySeconds(1);
                                actionChain.AddAction(this, () =>
                                {
                                    this.Session.Network.EnqueueSend(new GameEventUseDone(this.Session, WeenieError.None));
                                });
                                actionChain.EnqueueChain();
                                double roll = (double)r.Next(10000) / 100.00;
                                if (roll <= chance)
                                {
                                    int tinked = invTarget.GetProperty(PropertyInt.NumTimesTinkered) ?? 0;
                                    invTarget.SetProperty(PropertyString.TinkerName, this.Name);
                                    invTarget.SetProperty(PropertyInt.NumTimesTinkered, tinked + 1);
                                    switch(invSource.MaterialType)
                                    {
                                        case Material.Steel:
                                            int prevAL = invTarget.GetProperty(PropertyInt.ArmorLevel) ?? 0;
                                            invTarget.SetProperty(PropertyInt.ArmorLevel, prevAL + 20);
                                            break;
                                        case Material.Brass:
                                            double prevMeleeBonus = invTarget.GetProperty(PropertyFloat.WeaponDefense) ?? 0;
                                            invTarget.SetProperty(PropertyFloat.WeaponDefense, prevMeleeBonus + .01);
                                            break;
                                        case Material.Alabaster:
                                            double prevPierceProt = invTarget.GetProperty(PropertyFloat.ArmorModVsPierce) ?? 0;
                                            invTarget.SetProperty(PropertyFloat.ArmorModVsPierce, prevPierceProt + .2);
                                            break;
                                        case Material.ArmoredilloHide:
                                            double prevAcidProt = invTarget.GetProperty(PropertyFloat.ArmorModVsAcid) ?? 0;
                                            invTarget.SetProperty(PropertyFloat.ArmorModVsAcid, prevAcidProt + .4);
                                            break;
                                        case Material.Bronze:
                                            double prevSlashProt = invTarget.GetProperty(PropertyFloat.ArmorModVsSlash) ?? 0;
                                            invTarget.SetProperty(PropertyFloat.ArmorModVsSlash, prevSlashProt + .2);
                                            break;
                                        case Material.Ceramic:
                                            double prevFireProt = invTarget.GetProperty(PropertyFloat.ArmorModVsFire) ?? 0;
                                            invTarget.SetProperty(PropertyFloat.ArmorModVsFire, prevFireProt + .4);
                                            break;
                                        case Material.Copper:
                                            ///Changes Missile D to Melee D
                                            ///Not currently on items, so this will wait.
                                            break;
                                        case Material.Ebony:
                                            ///Changes Missile D to Melee D
                                            ///Not currently on items, so this will wait.
                                            break;
                                        case Material.Gold:
                                            int prevValue= invTarget.GetProperty(PropertyInt.Value) ?? 0;
                                            int value = (int)(prevValue + (prevValue * .25));
                                            invTarget.SetProperty(PropertyInt.Value, value);
                                            break;
                                        case Material.Granite:
                                            double prevmeleeVar = invTarget.GetProperty(PropertyFloat.DamageVariance) ?? 0;
                                            double meleeVar = prevmeleeVar - (prevmeleeVar * .2);
                                            invTarget.SetProperty(PropertyFloat.DamageVariance, meleeVar);
                                            break;
                                        case Material.GreenGarnet:
                                            double prevAttackP = invTarget.GetProperty(PropertyFloat.ElementalDamageMod) ?? 0;
                                            invTarget.SetProperty(PropertyFloat.ElementalDamageMod, prevAttackP + .01);
                                            break;
                                        case Material.Iron:
                                            int prevAttack = invTarget.GetProperty(PropertyInt.Damage) ?? 0;
                                            invTarget.SetProperty(PropertyInt.Damage, (prevAttack + 1));
                                            break;
                                        case Material.Ivory:
                                            invTarget.SetProperty(PropertyInt.Attuned, 0);
                                            break;
                                        case Material.Leather:
                                            invTarget.SetProperty(PropertyInt.Bonded, 1);
                                            break;
                                        case Material.Linen:
                                            int prevBurden = invTarget.GetProperty(PropertyInt.EncumbranceVal) ?? 0;
                                            int encumb = (int)(prevBurden - (prevBurden * .25));
                                            invTarget.SetProperty(PropertyInt.EncumbranceVal, encumb);
                                            break;
                                        case Material.Mahogany:
                                            double preDMod = invTarget.GetProperty(PropertyFloat.DamageMod) ?? 0;
                                           double DMod = (preDMod + .04);
                                            invTarget.SetProperty(PropertyFloat.DamageMod, DMod);
                                            break;
                                        case Material.Marble:
                                            double prevBludgeProt = invTarget.GetProperty(PropertyFloat.ArmorModVsBludgeon) ?? 0;
                                            invTarget.SetProperty(PropertyFloat.ArmorModVsBludgeon, prevBludgeProt + .2);
                                            break;
                                        case Material.Moonstone:
                                            int prevMana = invTarget.GetProperty(PropertyInt.ItemMaxMana) ?? 0;
                                            invTarget.SetProperty(PropertyInt.ItemMaxMana, prevMana + 500);
                                            break;
                                        case Material.Oak:
                                            int prevSpeed = invTarget.GetProperty(PropertyInt.WeaponTime) ?? 0;
                                            int speed = prevSpeed > 0 ? prevSpeed : 0;
                                            invTarget.SetProperty(PropertyInt.WeaponTime, speed);
                                            break;
                                        case Material.Opal:
                                            double prevManaC = invTarget.GetProperty(PropertyFloat.ManaRate) ?? 0;
                                            invTarget.SetProperty(PropertyFloat.ManaRate, prevManaC - (prevManaC * .01));
                                            break;
                                        case Material.Pine:
                                            int prevValue2 = invTarget.GetProperty(PropertyInt.Value) ?? 0;
                                            int value2 = (int)(prevValue2 - (prevValue2 * .25));
                                            invTarget.SetProperty(PropertyInt.Value, value2);
                                            break;
                                        case Material.Porcelain:
                                            ///Change Heritage to Sho
                                            ///Not currently implemented in loot
                                            break;
                                        case Material.ReedSharkHide:
                                            double prevLightProt = invTarget.GetProperty(PropertyFloat.ArmorModVsElectric) ?? 0;
                                            invTarget.SetProperty(PropertyFloat.ArmorModVsElectric, prevLightProt + .4);
                                            break;
                                        case Material.Sandstone:
                                            invTarget.SetProperty(PropertyInt.Bonded, 0);
                                            break;
                                        case Material.Satin:
                                            ///Change Heritage to Viamontian
                                            ///Not currently implemented in loot
                                            break;
                                        case Material.Silk:
                                            invTarget.SetProperty(PropertyInt.WieldDifficulty4, 0);
                                            ///Maybe this is the rank diff? will revisit after a bit of testing
                                            break;
                                        case Material.Silver:
                                            ///Change Melee Req to Missile
                                            ///Not currently implemented in loot
                                            break;
                                        case Material.Teak:
                                            ///Change Heritage to Aluvian
                                            ///Not currently implemented in loot
                                            break;
                                        case Material.Velvet:
                                            double prevOff = invTarget.GetProperty(PropertyFloat.WeaponOffense) ?? 0;
                                            invTarget.SetProperty(PropertyFloat.WeaponOffense, prevOff + .01);
                                            break;
                                        case Material.Wool:
                                            double prevColdProt = invTarget.GetProperty(PropertyFloat.ArmorModVsCold) ?? 0;
                                            invTarget.SetProperty(PropertyFloat.ArmorModVsCold, prevColdProt + .4);
                                            break;


                                    }
                                    string text = this.Name + " successfully applies the " + invSource.Name + " (workmanship " + invSource.Workmanship + ") to the " + invTarget.Name + ".";
                                    TryRemoveFromInventoryWithNetworking(invSource);
                                    Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Craft));
                                    Session.Network.EnqueueSend(new GameMessageUpdateObject(invTarget));
                                    Session.Network.EnqueueSend(new GameEventQueryItemManaResponse(this.Session, invSource.Guid.Full, 0, 0));

                                }
                                else
                                {
                                    string text = this.Name + " fails to apply the " + invSource.Name + " (workmanship " + invSource.Workmanship + ") to the " + invTarget.Name + ". The target is destroyed.";
                                    TryRemoveFromInventoryWithNetworking(invSource);
                                    TryRemoveFromInventoryWithNetworking(invTarget);
                                    Session.Network.EnqueueSend(new GameMessageSystemChat(text, ChatMessageType.Craft));
                                    Session.Network.EnqueueSend(new GameEventQueryItemManaResponse(this.Session, invSource.Guid.Full, 0, 0));
                                }
                            }
                        }
                    }
                    else
                    {
                        RecipeManager.UseObjectOnTarget(this, invSource, invTarget);
                    }
                }
                else if (invSource.WeenieType == WeenieType.Healer)
                {
                    if (!(worldTarget is Player))
                        return;

                    var healer = invSource as Healer;
                    healer.HandleActionUseOnTarget(this, worldTarget as Player);
                }
                else if (invSource.WeenieType == WeenieType.Key)
                {
                    var key = invSource as Key;
                    key.HandleActionUseOnTarget(this, worldTarget);
                }
                else if (invSource.WeenieType == WeenieType.Lockpick && worldTarget is Lock)
                {
                    var lp = invSource as Lockpick;
                    lp.HandleActionUseOnTarget(this, worldTarget);
                }
                else if (targetObjectId == Guid)
                {
                    // using something on ourselves
                    RecipeManager.UseObjectOnTarget(this, invSource, this);
                }
                else
                {
                    RecipeManager.UseObjectOnTarget(this, invSource, worldTarget);
                }
            }).EnqueueChain();
        }

        public void HandleActionUseItem(ObjectGuid usedItemId)
        {
            StopExistingMoveToChains();

            var actionChain = new ActionChain();

            actionChain.AddAction(this, () =>
            {
                // Search our inventory first
                var item = GetInventoryItem(usedItemId);

                if (item != null)
                    item.UseItem(this, actionChain);
                else
                {
                    // Search the world second
                    item = CurrentLandblock.GetObject(usedItemId);

                    if (item == null)
                    {
                        Session.Network.EnqueueSend(new GameEventUseDone(Session)); // todo add an argument that indicates the item was not found
                        return;
                    }

                    if (item is Container)
                        lastUsedContainerId = usedItemId;

                    if (!IsWithinUseRadiusOf(item))
                    {
                        var moveToChain = CreateMoveToChain(item, item.UseRadiusSquared, out var thisMoveToChainNumber);

                        actionChain.AddChain(moveToChain);
                        actionChain.AddDelaySeconds(0.50);

                        // Make sure that after we've executed our MoveToChain, and waited our delay, we're still within use radius.
                        actionChain.AddAction(this, () =>
                        {
                            if (IsWithinUseRadiusOf(item) && thisMoveToChainNumber == moveToChainCounter)
                                actionChain.AddAction(item, () => item.ActOnUse(this));
                            else
                                // Action is cancelled
                                Session.Network.EnqueueSend(new GameEventUseDone(Session));
                        });
                    }
                    else
                        actionChain.AddAction(item, () => item.ActOnUse(this));
                }
            });

            actionChain.EnqueueChain();
        }
    }
}

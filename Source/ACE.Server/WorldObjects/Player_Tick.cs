using System;
using System.Linq;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        private readonly ActionQueue actionQueue = new ActionQueue();

        private int initialAge;
        private DateTime initialAgeTime;

        private const double ageUpdateInterval = 7;
        private double nextAgeUpdateTime;

        public void Player_Tick(double currentUnixTime)
        {
            actionQueue.RunActions();

            if (nextAgeUpdateTime <= currentUnixTime)
            {
                nextAgeUpdateTime = currentUnixTime + ageUpdateInterval;

                if (initialAgeTime == DateTime.MinValue)
                {
                    initialAge = Age ?? 1;
                    initialAgeTime = DateTime.UtcNow;
                }

                Age = initialAge + (int)(DateTime.UtcNow - initialAgeTime).TotalSeconds;

                // In retail, this is sent every 7 seconds. If you adjust ageUpdateInterval from 7, you'll need to re-add logic to send this every 7s (if you want to match retail)
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.Age, Age ?? 1));
            }

            if (FellowVitalUpdate && Fellowship != null)
            {
                Fellowship.OnVitalUpdate(this);
                FellowVitalUpdate = false;
            }
        }

        /// <summary>
        /// Called every ~5 seconds for Players
        /// </summary>
        public override void Heartbeat(double currentUnixTime)
        {
            NotifyLandblocks();

            ManaConsumersTick();

            HandleTargetVitals();

            LifestoneProtectionTick();

            PK_DeathTick();

            GagsTick();

            PhysicsObj.ObjMaint.DestroyObjects();

            // Check if we're due for our periodic SavePlayer
            if (LastRequestedDatabaseSave == DateTime.MinValue)
                LastRequestedDatabaseSave = DateTime.UtcNow;

            if (LastRequestedDatabaseSave + PlayerSaveInterval <= DateTime.UtcNow)
                SavePlayerToDatabase();

            base.Heartbeat(currentUnixTime);
        }

        private bool gagNoticeSent = false;

        public void GagsTick()
        {
            if (IsGagged)
            {
                if (!gagNoticeSent)
                {
                    SendGagNotice();
                    gagNoticeSent = true;
                }

                // check for gag expiration, if expired, remove gag.
                GagDuration -= CachedHeartbeatInterval;

                if (GagDuration <= 0)
                {
                    IsGagged = false;
                    GagTimestamp = 0;
                    GagDuration = 0;
                    SaveBiotaToDatabase();
                    SendUngagNotice();
                    gagNoticeSent = false;
                }
            }
        }

        /// <summary>
        /// Prepare new action to run on this player
        /// </summary>
        public override void EnqueueAction(IAction action)
        {
            actionQueue.EnqueueAction(action);
        }

        /// <summary>
        /// Called every ~5 secs for equipped mana consuming items
        /// </summary>
        public void ManaConsumersTick()
        {
            if (!EquippedObjectsLoaded)
                return;

            var EquippedManaConsumers = EquippedObjects.Where(k =>
                (k.Value.IsAffecting ?? false) &&
                //k.Value.ManaRate.HasValue &&
                k.Value.ItemMaxMana.HasValue &&
                k.Value.ItemCurMana.HasValue &&
                k.Value.ItemCurMana.Value > 0).ToList();

            foreach (var k in EquippedManaConsumers)
            {
                var item = k.Value;

                // this was a bug in lootgen until 7/11/2019, mostly for clothing/armor/shields
                // tons of existing items on servers are in this bugged state, where they aren't ticking mana.
                // this retroactively fixes them when equipped
                // items such as Impious Staff are excluded from this via IsAffecting

                if (item.ManaRate == null)
                {
                    item.ManaRate = LootGenerationFactory.GetManaRate(item);
                    log.Warn($"{Name}.ManaConsumersTick(): {k.Value.Name} ({k.Value.Guid}) fixed missing ManaRate");
                }

                var rate = item.ManaRate.Value;

                if (LumAugItemManaUsage != 0)
                    rate *= GetNegativeRatingMod(LumAugItemManaUsage);

                if (!item.ItemManaConsumptionTimestamp.HasValue) item.ItemManaConsumptionTimestamp = DateTime.Now;
                DateTime mostRecentBurn = item.ItemManaConsumptionTimestamp.Value;

                var timePerBurn = -1 / rate;

                var secondsSinceLastBurn = (DateTime.Now - mostRecentBurn).TotalSeconds;

                var delta = secondsSinceLastBurn / timePerBurn;

                var deltaChopped = (int)Math.Floor(delta);
                var deltaExtra = delta - deltaChopped;

                if (deltaChopped <= 0)
                    continue;

                var timeToAdd = (int)Math.Floor(deltaChopped * timePerBurn);
                item.ItemManaConsumptionTimestamp = mostRecentBurn + new TimeSpan(0, 0, timeToAdd);
                var manaToBurn = Math.Min(item.ItemCurMana.Value, deltaChopped);
                deltaChopped = Math.Clamp(deltaChopped, 0, 10);
                item.ItemCurMana -= deltaChopped;

                if (item.ItemCurMana < 1 || item.ItemCurMana == null)
                {
                    item.IsAffecting = false;
                    var msg = new GameMessageSystemChat($"Your {item.Name} is out of Mana.", ChatMessageType.Magic);
                    var sound = new GameMessageSound(Guid, Sound.ItemManaDepleted);
                    Session.Network.EnqueueSend(msg, sound);
                    if (item.WielderId != null)
                    {
                        if (item.Biota.BiotaPropertiesSpellBook != null)
                        {
                            // unsure if these messages / sounds were ever sent in retail,
                            // or if it just purged the enchantments invisibly
                            // doing a delay here to prevent 'SpellExpired' sounds from overlapping with 'ItemManaDepleted'
                            var actionChain = new ActionChain();
                            actionChain.AddDelaySeconds(2.0f);
                            actionChain.AddAction(this, () =>
                            {
                                for (int i = 0; i < item.Biota.BiotaPropertiesSpellBook.Count; i++)
                                {
                                    RemoveItemSpell(item, (uint)item.Biota.BiotaPropertiesSpellBook.ElementAt(i).Spell);
                                }
                            });
                            actionChain.EnqueueChain();
                        }
                    }
                }
                else
                {
                    // get time until empty
                    var secondsUntilEmpty = ((item.ItemCurMana - deltaExtra) * timePerBurn);
                    if (secondsUntilEmpty <= 120 && (!item.ItemManaDepletionMessageTimestamp.HasValue || (DateTime.Now - item.ItemManaDepletionMessageTimestamp.Value).TotalSeconds > 120))
                    {
                        item.ItemManaDepletionMessageTimestamp = DateTime.Now;
                        Session.Network.EnqueueSend(new GameMessageSystemChat($"Your {item.Name} is low on Mana.", ChatMessageType.Magic));
                    }
                }
            }
        }
    }
}

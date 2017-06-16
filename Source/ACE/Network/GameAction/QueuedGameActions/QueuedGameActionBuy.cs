using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Factories;
using ACE.InGameManager;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.QueuedGameActions
{
    public class QueuedGameActionBuy : QueuedGameAction
    {
        public QueuedGameActionBuy(uint objectId, List<ItemProfile> profileItems)
        {
            ObjectId = objectId;
            ProfileItems = profileItems;
        }

        protected override void Handle(GameMediator mediator, Player player)
        {
            // todo: lots, need vendor list, money checks, etc.

            var money = new GameMessagePrivateUpdatePropertyInt(player.Session, PropertyInt.CoinValue, 4000);
            var sound = new GameMessageSound(player.Guid, Sound.PickUpItem, 1);
            var sendUseDoneEvent = new GameEventUseDone(player.Session);
            player.Session.Network.EnqueueSend(money, sound, sendUseDoneEvent);

            // send updated vendor inventory.
            player.Session.Network.EnqueueSend(new GameEventApproachVendor(player.Session, this.ObjectId));

            // this is just some testing code for now.
            foreach (ItemProfile item in this.ProfileItems)
            {
                // todo: something with vendor id and profile list... iid list from vendor dbs.
                // todo: something with amounts..

                if (item.Iid == 5)
                {
                    while (item.Amount > 0)
                    {
                        item.Amount--;
                        WorldObject loot = LootGenerationFactory.CreateTestWorldObject(5090);
                        LootGenerationFactory.AddToContainer(loot, player);
                        player.TrackObject(loot);
                    }
                    var rudecomment = "Who do you think you are, Johny Apple Seed ?";
                    var buyrudemsg = new GameMessageSystemChat(rudecomment, ChatMessageType.Tell);
                    player.Session.Network.EnqueueSend(buyrudemsg);
                }
                else if (item.Iid == 10)
                {
                    while (item.Amount > 0)
                    {
                        item.Amount--;
                        WorldObject loot = LootGenerationFactory.CreateTestWorldObject(30537);
                        LootGenerationFactory.AddToContainer(loot, player);
                        player.TrackObject(loot);
                    }
                    var rudecomment = "That smells awful, Enjoy eating it!";
                    var buyrudemsg = new GameMessageSystemChat(rudecomment, ChatMessageType.Tell);
                    player.Session.Network.EnqueueSend(buyrudemsg);
                }
            }
        }
    }
}

using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameEvent.Events;

namespace ACE.Entity
{
    public class Gem : WorldObject
    {
        public uint? UseCreateContractId
        {
            get { return AceObject.UseCreateContractId; }
            set { AceObject.UseCreateContractId = value; }
        }

        public Gem(AceObject aceObject)
            : base(aceObject)
        {
        }

        public override void OnUse(Session session)
        {
            if (UseCreateContractId == null) return;
            ContractTracker contractTracker = new ContractTracker((uint)UseCreateContractId)
            {
                Stage                = 0,
                TimeWhenDone         = 0,
                TimeWhenRepeats      = 0,
                DeleteContract       = 0,
                SetAsDisplayContract = 1
            };

            if (!session.Player.TrackedContracts.ContainsKey((uint)UseCreateContractId))
            {
                new ActionChain(this, () =>
                {
                    session.Player.TrackedContracts.Add((uint)UseCreateContractId, contractTracker);

                    GameEventSendClientContractTracker contractMsg =
                        new GameEventSendClientContractTracker(session, contractTracker);
                    session.Network.EnqueueSend(contractMsg);
                    ChatPacket.SendServerMessage(session,
                        "You just added " + contractTracker.ContractDetails.ContractName, ChatMessageType.Broadcast);
                    session.Player.SendUseDoneEvent();
                }).EnqueueChain();

                // Ok this was not known to us, so we used the contract - now remove it from inventory.
                // HandleActionRemoveItemFromInventory is has it's own action chain.
                session.Player.HandleActionRemoveItemFromInventory(Guid.Full, (uint)ContainerId, 1);
            }
            else
                ChatPacket.SendServerMessage(session, "You already have this quest tracked: " + contractTracker.ContractDetails.ContractName, ChatMessageType.Broadcast);
        }
    }
}

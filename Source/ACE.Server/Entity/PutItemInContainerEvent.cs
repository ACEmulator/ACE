using System;

namespace ACE.Server.Entity
{
    // This handles a peculiar sequence sent by the client in certain scenarios
    // The client will double-send 0x19 PutItemInContainer for the same object
    // (swapping dual wield weapons, swapping ammo types in combat)
    public class PutItemInContainerEvent
    {
        public uint ItemGuid;
        public uint ContainerGuid;
        public int Placement;

        public DateTime Timestamp;

        public static TimeSpan Threshold = TimeSpan.FromSeconds(0.5f);

        public PutItemInContainerEvent(uint itemGuid, uint containerGuid, int placement)
        {
            ItemGuid = itemGuid;
            ContainerGuid = containerGuid;
            Placement = placement;

            Timestamp = DateTime.UtcNow;
        }

        public bool IsDoubleSend(PutItemInContainerEvent data)
        {
            return ItemGuid == data.ItemGuid && ContainerGuid == data.ContainerGuid && Placement == data.Placement &&
                DateTime.UtcNow - Timestamp < Threshold && Timestamp - data.Timestamp < Threshold;
        }
    }
}

using ACE.Server.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public bool AddContract(uint contractID)
        {
            if (TrackedContracts.ContainsKey(contractID))
                return false;

            TrackedContracts.Add(contractID, new ContractTracker(contractID, Guid.Full));
            return true;
        }
    }
}

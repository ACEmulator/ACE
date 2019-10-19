
namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void HandleActionAbandonContract(uint contractId)
        {
            ContractManager.Abandon(contractId);
        }
    }
}

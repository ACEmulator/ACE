using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;

namespace ACE.Entity
{
    /// <summary>
    /// This class is the base of contracts for quest tracking.
    /// </summary>
    public class ContractTracker
    {
        public Contract ContractDetails { get; }
        /// <summary>
        /// Version of the contract.   So far I have only seen 0, but I have not done an exhaustive search.
        /// </summary>
        public uint Version => ContractDetails.Version;
        /// <summary>
        /// Id of the contract.   This is the index into the contract table in the portal.dat file
        /// </summary>
        public uint ContractId { get; set; }
        /// <summary>
        /// here are we in the quest - progress.   Starts at 0 on initial add to quest tracker.   Examples kill task kill 10 this marks you place.
        /// </summary>
        public uint Stage { get; set; } = 0;
        /// <summary>
        /// I believe this is used for timed quests - kill so many within an hour - have not found it populated in pcaps.
        /// This value is stored in seconds.   ie 1 hour = 3600 seconds.
        /// </summary>
        public ulong TimeWhenDone { get; set; } = 0;
        /// <summary>
        /// When is my quest timer up so I can do the quest again.  This will be tracked in the greater quest system. Time is stored in seconds
        /// until I can rerun the quest
        /// </summary>
        public ulong TimeWhenRepeats { get; set; } = 0;
        /// <summary>
        /// delete flag 0 is false, 1 is true - delete the contract
        /// </summary>
        public uint DeleteContract { get; set; } = 0;
        /// <summary>
        /// flag to display details of the quest and not just the list of contracts in the quest panel
        /// </summary>
        public uint SetAsDisplayContract { get; set; } = 0;
        /// <summary>
        /// Initilization method.   Loads up infromation about the contract
        /// </summary>
        /// <param name="contractId">Id of the contract.   This is the index into the contract table in the portal.dat file</param>
        public ContractTracker(uint contractId)
        {
            ContractId = contractId;
            ContractDetails = GetContractDetails();
        }
        /// <summary>
        /// This is called in initilization and gets us all of the infromation from the dat file about this quest.
        /// </summary>
        /// <returns>Contract - this class has all of the infomation from the dat file about the quest.</returns>
        public Contract GetContractDetails()
        {
            Contract contractData;
            ContractTable contractTable = ContractTable.ReadFromDat();
            return contractTable.Contracts.TryGetValue(ContractId, out contractData) ? contractData : null;
        }
    }
}

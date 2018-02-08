using ACE.DatLoader;

namespace ACE.Entity
{
    using System.CodeDom;

    /// <summary>
    /// This class is the base of contracts for quest tracking.
    /// </summary>
    public class ContractTracker
    {
        protected AceContractTracker AceContractTracker { get; set; }

        public DatLoader.Entity.Contract ContractDetails { get; }

        public uint AceObjectId
        {
            get { return AceContractTracker.AceObjectId; }
            set { AceContractTracker.AceObjectId = value; }
        }
        /// <summary>
        /// Version of the contract.   So far I have only seen 0, but I have not done an exhaustive search.
        /// </summary>
        public uint Version
        {
            get { return AceContractTracker.Version; }
            set { AceContractTracker.Version = value; }
        }

        /// <summary>
        /// Id of the contract.   This is the index into the contract table in the portal.dat file
        /// </summary>
        public uint ContractId
        {
            get { return AceContractTracker.ContractId; }
            set { AceContractTracker.ContractId = value; }
        }

        /// <summary>
        /// here are we in the quest - progress.   Starts at 0 on initial add to quest tracker.   Examples kill task kill 10 this marks you place.
        /// </summary>
        public uint Stage
        {
            get { return AceContractTracker.Stage; }
            set { AceContractTracker.Stage = value; }
        }

        /// <summary>
        /// I believe this is used for timed quests - kill so many within an hour - have not found it populated in pcaps.
        /// This value is stored in seconds.   ie 1 hour = 3600 seconds.
        /// </summary>
        public ulong TimeWhenDone
        {
            get { return AceContractTracker.TimeWhenDone; }
            set { AceContractTracker.TimeWhenDone = value; }
        }

        /// <summary>
        /// When is my quest timer up so I can do the quest again.  This will be tracked in the greater quest system. Time is stored in seconds
        /// until I can rerun the quest
        /// </summary>
        public ulong TimeWhenRepeats
        {
            get { return AceContractTracker.TimeWhenRepeats; }
            set { AceContractTracker.TimeWhenRepeats = value; }
        }

        /// <summary>
        /// delete flag 0 is false, 1 is true - delete the contract
        /// </summary>
        public uint DeleteContract
        {
            get { return AceContractTracker.DeleteContract; }
            set { AceContractTracker.DeleteContract = value; }
        }

        /// <summary>
        /// flag to display details of the quest and not just the list of contracts in the quest panel
        /// </summary>
        public uint SetAsDisplayContract
        {
            get { return AceContractTracker.SetAsDisplayContract; }
            set { AceContractTracker.SetAsDisplayContract = value; }
        }

        /// <summary>
        /// Initialization method.   Loads up information about the contract
        /// </summary>
        /// <param name="contractId">Id of the contract.   This is the index into the contract table in the portal.dat file</param>
        /// <param name="aceObjectId">This is the guid of the player to which we are adding this contract tracker</param>
        public ContractTracker(uint contractId, uint aceObjectId)
        {
            AceContractTracker = new AceContractTracker();
            ContractId = contractId;
            AceObjectId = aceObjectId;
            ContractDetails = GetContractDetails();
            Version = ContractDetails.Version;
        }

        /// <summary>
        /// This is called in initialization and gets us all of the information from the dat file about this quest.
        /// </summary>
        /// <returns>Contract - this class has all of the information from the dat file about the quest.</returns>
        public DatLoader.Entity.Contract GetContractDetails()
        {
            DatLoader.Entity.Contract contractData;
            var contractTable = DatManager.PortalDat.ContractTable;
            return contractTable.Contracts.TryGetValue(ContractId, out contractData) ? contractData : null;
        }

        public AceContractTracker SnapShotOfAceContractTracker()
        {
            AceContractTracker snapshot = (AceContractTracker)AceContractTracker.Clone();
            return snapshot;
        }
    }
}

using System;

using ACE.DatLoader;
using ACE.Entity;

namespace ACE.Server.Entity
{
    /// <summary>
    /// This class is the base of contracts for quest tracking.
    /// </summary>
    public class ContractTracker
    {
        //protected AceContractTracker AceContractTracker { get; set; }

        public DatLoader.Entity.Contract ContractDetails { get; }

        public uint AceObjectId
        {
            get { throw new NotSupportedException();  /*return AceContractTracker.AceObjectId;*/ }
            set { throw new NotSupportedException();  /*AceContractTracker.AceObjectId = value;*/ }
        }
        /// <summary>
        /// Version of the contract.   So far I have only seen 0, but I have not done an exhaustive search.
        /// </summary>
        public uint Version
        {
            get { throw new NotSupportedException();  /*return AceContractTracker.Version;*/ }
            set { throw new NotSupportedException();  /*AceContractTracker.Version = value;*/ }
        }

        /// <summary>
        /// Id of the contract.   This is the index into the contract table in the portal.dat file
        /// </summary>
        public uint ContractId
        {
            get { throw new NotSupportedException();  /*return AceContractTracker.ContractId;*/ }
            set { throw new NotSupportedException();  /*AceContractTracker.ContractId = value;*/ }
        }

        /// <summary>
        /// here are we in the quest - progress.   Starts at 0 on initial add to quest tracker.   Examples kill task kill 10 this marks you place.
        /// </summary>
        public uint Stage
        {
            get { throw new NotSupportedException();  /*return AceContractTracker.Stage;*/ }
            set { throw new NotSupportedException();  /*AceContractTracker.Stage = value;*/ }
        }

        /// <summary>
        /// I believe this is used for timed quests - kill so many within an hour - have not found it populated in pcaps.
        /// This value is stored in seconds.   ie 1 hour = 3600 seconds.
        /// </summary>
        public ulong TimeWhenDone
        {
            get { throw new NotSupportedException();  /*return AceContractTracker.TimeWhenDone;*/ }
            set { throw new NotSupportedException();  /*AceContractTracker.TimeWhenDone = value;*/ }
        }

        /// <summary>
        /// When is my quest timer up so I can do the quest again.  This will be tracked in the greater quest system. Time is stored in seconds
        /// until I can rerun the quest
        /// </summary>
        public ulong TimeWhenRepeats
        {
            get { throw new NotSupportedException();  /*return AceContractTracker.TimeWhenRepeats; */}
            set { throw new NotSupportedException();  /*AceContractTracker.TimeWhenRepeats = value;*/ }
        }

        /// <summary>
        /// delete flag 0 is false, 1 is true - delete the contract
        /// </summary>
        public uint DeleteContract
        {
            get {throw new NotSupportedException();  /* return AceContractTracker.DeleteContract;*/ }
            set {throw new NotSupportedException();  /* AceContractTracker.DeleteContract = value;*/ }
        }

        /// <summary>
        /// flag to display details of the quest and not just the list of contracts in the quest panel
        /// </summary>
        public uint SetAsDisplayContract
        {
            get { throw new NotSupportedException();  /*return AceContractTracker.SetAsDisplayContract;*/}
            set { throw new NotSupportedException();  /*AceContractTracker.SetAsDisplayContract = value;*/ }
        }

        /// <summary>
        /// Initialization method.   Loads up information about the contract
        /// </summary>
        /// <param name="contractId">Id of the contract.   This is the index into the contract table in the portal.dat file</param>
        /// <param name="aceObjectId">This is the guid of the player to which we are adding this contract tracker</param>
        public ContractTracker(uint contractId, uint aceObjectId)
        {
            /*AceContractTracker = new AceContractTracker();
            ContractId = contractId;
            AceObjectId = aceObjectId;
            ContractDetails = GetContractDetails();
            Version = ContractDetails.Version;*/
        }

        /// <summary>
        /// This is called in initialization and gets us all of the information from the dat file about this quest.
        /// </summary>
        /// <returns>Contract - this class has all of the information from the dat file about the quest.</returns>
        public DatLoader.Entity.Contract GetContractDetails()
        {
            var contractTable = DatManager.PortalDat.ContractTable;
            return contractTable.Contracts.TryGetValue(ContractId, out var contractData) ? contractData : null;
        }

        /*public AceContractTracker SnapShotOfAceContractTracker()
        {
            AceContractTracker snapshot = (AceContractTracker)AceContractTracker.Clone();
            return snapshot;
        }*/
    }
}

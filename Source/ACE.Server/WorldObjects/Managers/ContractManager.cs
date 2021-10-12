using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using log4net;

using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects.Managers
{
    public class ContractManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Player Player { get; }

        private const int MaxContracts = 100;

        public Dictionary<uint, ContractTracker> ContractTrackerTable
        {
            get
            {
                var contractIds = Player.Character.GetContractsIds(Player.CharacterDatabaseLock);

                return contractIds.ToDictionary(c => c, c => new ContractTracker(Player, c));
            }
        }

        private readonly Dictionary<uint, HashSet<string>> MonitoredQuestFlags = new Dictionary<uint, HashSet<string>>();

        public static bool Debug = false;

        /// <summary>
        /// Constructs a new ContractManager for a Player
        /// </summary>
        public ContractManager(Player player)
        {
            Player = player;

            RefreshMonitoredQuestFlags();
        }

        private void RefreshMonitoredQuestFlags()
        {
            MonitoredQuestFlags.Clear();

            var contracts = Player.Character.GetContracts(Player.CharacterDatabaseLock);

            foreach (var contract in contracts)
            {
                var datContract = GetContractFromDat(contract.ContractId);

                if (!MonitoredQuestFlags.ContainsKey(contract.ContractId))
                    MonitoredQuestFlags.Add(contract.ContractId, new HashSet<string>());

                if (!string.IsNullOrWhiteSpace(datContract.QuestflagFinished))
                    MonitoredQuestFlags[datContract.ContractId].Add(datContract.QuestflagFinished.ToLower());
                if (!string.IsNullOrWhiteSpace(datContract.QuestflagProgress))
                    MonitoredQuestFlags[datContract.ContractId].Add(datContract.QuestflagProgress.ToLower());
                if (!string.IsNullOrWhiteSpace(datContract.QuestflagRepeatTime))
                    MonitoredQuestFlags[datContract.ContractId].Add(datContract.QuestflagRepeatTime.ToLower());
                if (!string.IsNullOrWhiteSpace(datContract.QuestflagStamped))
                    MonitoredQuestFlags[datContract.ContractId].Add(datContract.QuestflagStamped.ToLower());
                if (!string.IsNullOrWhiteSpace(datContract.QuestflagStarted))
                    MonitoredQuestFlags[datContract.ContractId].Add(datContract.QuestflagStarted.ToLower());
                if (!string.IsNullOrWhiteSpace(datContract.QuestflagTimer))
                    MonitoredQuestFlags[datContract.ContractId].Add(datContract.QuestflagTimer.ToLower());
            }
        }

        public ContractTracker GetContractTracker(uint contractId)
        {
            if (GetContract(contractId) != null)
                return new ContractTracker(Player, contractId);

            return null;
        }

        /// <summary>
        /// Returns a contract from the portal dat file
        /// </summary>
        public Contract GetContractFromDat(uint contractId)
        {
            var contractTable = DatManager.PortalDat.ContractTable;
            return contractTable.Contracts.TryGetValue(contractId, out var contractData) ? contractData : null;
        }

        /// <summary>
        /// Returns an active or completed contract for this player
        /// </summary>
        public CharacterPropertiesContractRegistry GetContract(uint contractId)
        {
            return Player.Character.GetContract(contractId, Player.CharacterDatabaseLock);
        }

        /// <summary>
        /// Returns TRUE if at max capacity for Contracts for this player
        /// </summary>
        public bool IsFull => Player.Character.GetContractsCount(Player.CharacterDatabaseLock) >= MaxContracts;

        /// <summary>
        /// Adds a new contract to the player's registry
        /// </summary>
        public bool Add(int contractId)
        {
            return Add(Convert.ToUInt32(contractId));
        }

        /// <summary>
        /// Adds a new contract to the player's registry
        /// </summary>
        public bool Add(uint contractId)
        {
            var datContract = GetContractFromDat(contractId);

            if (datContract == null)
            {
                if (Debug) Console.WriteLine($"{Player.Name}.ContractManager.Add({contractId}): Contract not found in DAT file.");
                return false;
            }

            if (IsFull)
            {
                if (Player != null)
                {
                    //Player.Session.Network.EnqueueSend(new GameEventWeenieError(Player.Session, WeenieError.ContractError));
                    //what happened here in retail?

                    Player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You currently have the maximum amount of contracts for this character and cannot take on another! You must abandon at least one contract before you can accept the contract for {datContract.ContractName}.", ChatMessageType.Broadcast));
                }
                return false;
            }

            var contract = Player.Character.GetOrCreateContract(contractId, Player.CharacterDatabaseLock, out var contractWasCreated);

            if (contractWasCreated)
            {
                // add new contract entry
                contract.DeleteContract = false;
                contract.SetAsDisplayContract = false;

                if (Debug) Console.WriteLine($"{Player.Name}.ContractManager.Add({contractId}): added contract: {datContract.ContractName}");

                Player.CharacterChangesDetected = true;
                Player.Session.Network.EnqueueSend(new GameEventSendClientContractTracker(Player.Session, contract));

                RefreshMonitoredQuestFlags();
            }
            else
            {
                if (Debug) Console.WriteLine($"{Player.Name}.ContractManager.Add({contractId}): contract for {datContract.ContractName} already exists in registry.");

                // contracts dupes are also successful without actually duping into registry.
                //return false;
            }

            return true;
        }

        /// <summary>
        /// Returns TRUE if a player has a particular contract
        /// </summary>
        public bool HasContract(uint contractId)
        {
            var hasContract = GetContract(contractId) != null;

            if (Debug)
                Console.WriteLine($"{Player.Name}.HasContracts({contractId}): {hasContract}");

            return hasContract;
        }

        /// <summary>
        /// Abandon a contract in the Player's registry
        /// </summary>
        public void Abandon(uint contractId)
        {
            Erase(contractId);
        }

        /// <summary>
        /// Erase a contract in the Player's registry
        /// </summary>
        public void Erase(uint contractId)
        {
            if (Debug)
                Console.WriteLine($"{Player.Name}.ContractManager.Erase({contractId})");

            Player.Character.EraseContract(contractId, out var contractErased, Player.CharacterDatabaseLock);

            if (contractErased != null)
            {
                contractErased.DeleteContract = true;

                Player.CharacterChangesDetected = true;
                Player.Session.Network.EnqueueSend(new GameEventSendClientContractTracker(Player.Session, contractErased));

                RefreshMonitoredQuestFlags();
            }
        }

        /// <summary>
        /// Erase all contracts in registry
        /// </summary>
        public void EraseAll()
        {
            if (Debug)
                Console.WriteLine($"{Player.Name}.ContractManager.EraseAll");

            Player.Character.EraseAllContracts(out var erasedContracts, Player.CharacterDatabaseLock);

            foreach (var contract in erasedContracts)
            {
                contract.DeleteContract = true;

                Player.CharacterChangesDetected = true;
                Player.Session.Network.EnqueueSend(new GameEventSendClientContractTracker(Player.Session, contract));
            }

            RefreshMonitoredQuestFlags();
        }

        public void NotifyOfQuestUpdate(string questName)
        {
            foreach (var contracts in MonitoredQuestFlags)
            {
                if (contracts.Value.Contains(questName.ToLower()))
                    Update(contracts.Key);
            }
        }

        private void Update(uint contractId)
        {
            if (Debug)
                Console.WriteLine($"{Player.Name}.ContractManager.Update");

            var contract = GetContract(contractId);

            if (contract != null)
                Player.Session.Network.EnqueueSend(new GameEventSendClientContractTracker(Player.Session, contract));
        }
    }

    public static class ContractManagerExtensions
    {
        private static readonly HashComparer hashComparer = new HashComparer(32);   // static table size from retail pcaps

        public static void Write(this BinaryWriter writer, ContractManager contractManager)
        {
            writer.Write(contractManager.ContractTrackerTable);
        }

        public static void Write(this BinaryWriter writer, Dictionary<uint, ContractTracker> contractTable)
        {
            PackableHashTable.WriteHeader(writer, contractTable.Count, hashComparer.NumBuckets);

            var contractTrackers = new SortedDictionary<uint, ContractTracker>(contractTable, hashComparer);

            foreach (var contractTracker in contractTrackers)
            {
                writer.Write(contractTracker.Key);
                writer.Write(contractTracker.Value);
            }
        }
    }
}

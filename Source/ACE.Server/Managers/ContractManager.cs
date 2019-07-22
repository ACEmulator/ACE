using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using ACE.Database.Models.Shard;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.WorldObjects;
using System.IO;
using ACE.Server.Network.Structure;
using ACE.DatLoader;
using ACE.DatLoader.Entity;

namespace ACE.Server.Managers
{
    public class ContractManager
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Player Player { get; }

        private const int MaxContracts = 100;

        public ICollection<CharacterPropertiesContractRegistry> Contracts
        {
            get
            {
                if (Player != null)
                    return Player.Character.CharacterPropertiesContractRegistry;
                else
                    return null;
            }
        }

        public Dictionary<uint, ContractTracker> ContractTrackerTable
        {
            get
            {
                return Contracts.ToDictionary(c => c.ContractId, c => new ContractTracker(Player, c.ContractId));
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

            foreach (var contract in Contracts)
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
            else
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
            return Contracts.FirstOrDefault(c => c.ContractId == contractId);
        }

        /// <summary>
        /// Returns TRUE if at max capacity for Contracts for thiss player
        /// </summary>
        public bool IsFull => Contracts.Count >= MaxContracts;

        /// <summary>
        /// Adds a new contract to the player's registry
        /// </summary>
        public void Add(int contractId)
        {
            Add(Convert.ToUInt32(contractId));
        }

        /// <summary>
        /// Adds a new contract to the player's registry
        /// </summary>
        public void Add(uint contractId)
        {
            var datContract = GetContractFromDat(contractId);

            if (datContract == null)
            {
                if (Debug) Console.WriteLine($"{Player.Name}.ContractManager.Add({contractId}): Contract not found in DAT file.");
                return;
            }

            if (IsFull)
            {
                if (Player != null)
                {
                    //Player.Session.Network.EnqueueSend(new GameEventWeenieError(Player.Session, WeenieError.ContractError));
                    //what happened here in retail?
                }
                return;
            }

            var existing = GetContract(contractId);

            if (existing == null)
            {
                // add new contract entry
                var info = new CharacterPropertiesContractRegistry
                {
                    CharacterId = Player.Guid.Full,
                    ContractId = datContract.ContractId,
                    DeleteContract = false,
                    SetAsDisplayContract = false,
                };
                if (Debug) Console.WriteLine($"{Player.Name}.ContractManager.Add({contractId}): added contract: {datContract.ContractName}");
                Contracts.Add(info);
                if (Player != null)
                {
                    Player.CharacterChangesDetected = true;
                    Player.Session.Network.EnqueueSend(new GameEventSendClientContractTracker(Player.Session, contractId));
                }

                RefreshMonitoredQuestFlags();
            }
            else
            {
                //// update existing quest
                //existing.LastTimeCompleted = (uint)Time.GetUnixTime();
                //existing.NumTimesCompleted++;
                //if (Debug) Console.WriteLine($"{((Player != null) ? Player.Name : $"Fellowship({Fellowship.FellowshipName})")}.QuestManager.Update({quest}): updated quest ({existing.NumTimesCompleted})");
                //if (Player != null) Player.CharacterChangesDetected = true;

                if (Debug) Console.WriteLine($"{Player.Name}.ContractManager.Add({contractId}): contract for {datContract.ContractName} already exists in registry.");
            }
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

            var contract = GetContract(contractId);

            if (contract != null)
            {
                contract.DeleteContract = true;
                Contracts.Remove(contract);
                if (Player != null)
                {
                    Player.CharacterChangesDetected = true;
                    Player.Session.Network.EnqueueSend(new GameEventSendClientContractTracker(Player.Session, contract));                    
                }

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

            var contracts = Contracts.ToList();
            foreach (var contract in contracts)
            {
                contract.DeleteContract = true;
                Contracts.Remove(contract);
                if (Player != null)
                {
                    Player.CharacterChangesDetected = true;
                    Player.Session.Network.EnqueueSend(new GameEventSendClientContractTracker(Player.Session, contract));                    
                }
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

            if (Player != null)
            {
                Player.Session.Network.EnqueueSend(new GameEventSendClientContractTracker(Player.Session, contractId));
            }
        }
    }

    public static class ContractManagerExtensions
    {
        public static void Write(this BinaryWriter writer, ContractManager contractManager)
        {
            writer.Write(contractManager.ContractTrackerTable);
        }

        public static void Write(this BinaryWriter writer, Dictionary<uint, ContractTracker> contractTrackerHash)
        {
            PackableHashTable.WriteHeader(writer, contractTrackerHash.Count);
            foreach (var kvp in contractTrackerHash)
            {
                writer.Write(kvp.Key);
                writer.Write(kvp.Value);
            }
        }
    }
}

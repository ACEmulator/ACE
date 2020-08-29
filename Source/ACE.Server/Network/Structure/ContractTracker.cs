using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Server.WorldObjects;
using System.IO;

namespace ACE.Server.Network.Structure
{
    public enum ContractStage
    {
        Available           = 0x1,
        InProgress          = 0x2,
        DoneOrPendingRepeat = 0x3,
        ProgressCounter     = 0x4
    };

    public class ContractTracker
    {        
        public uint ContractId;
        public ContractStage Stage = ContractStage.Available;
        public double TimeWhenDone;
        public double TimeWhenRepeats;
        public uint Version => Contract.Version;

        public bool DeleteContract;       // Not sure if retail servers always kept contracts and used this to hide them, but we discard so this exists only to force client to remove it from list.
        public bool SetAsDisplayContract; // depreciated?

        // not sent in network structure
        private Contract _contract;
        public Contract Contract
        {
            get
            {
                if (_contract != null)
                    return _contract;

                if (DatManager.PortalDat.ContractTable.Contracts.TryGetValue(ContractId, out var contractData))
                    _contract = contractData;

                return _contract;
            }
        }

        public ContractTracker() { }

        public ContractTracker(Player player, CharacterPropertiesContractRegistry contract)
        {
            Init(contract.ContractId, player);

            DeleteContract = contract.DeleteContract;
            SetAsDisplayContract = contract.SetAsDisplayContract;
        }

        public ContractTracker(Player player, uint contractId)
        {
            Init(contractId, player);
        }

        private void Init(uint contractId)
        {
            ContractId = contractId;
        }

        private void Init(uint contractId, Player player)
        {
            Init(contractId);

            if (player == null) return;

            // Started, Stamped, Timer or Progress
            CheckAndSetStage(player);

            if (!string.IsNullOrWhiteSpace(Contract.QuestflagFinished))
            {
                if (player.QuestManager.HasQuest(Contract.QuestflagFinished))
                    Stage = ContractStage.DoneOrPendingRepeat;
            }

            if (!string.IsNullOrWhiteSpace(Contract.QuestflagRepeatTime))
            {
                if (player.QuestManager.HasQuest(Contract.QuestflagRepeatTime))
                {                    
                    TimeWhenRepeats = player.QuestManager.GetNextSolveTime(Contract.QuestflagRepeatTime).TotalSeconds;

                    if (TimeWhenRepeats > 0)
                        Stage = ContractStage.DoneOrPendingRepeat;
                    else
                    {
                        Stage = ContractStage.Available;

                        // Recheck for Started, Stamped, Timer or Progress and update accordingly
                        CheckAndSetStage(player);
                    }
                }
            }
        }

        private void CheckAndSetStage(Player player)
        {
            if (!string.IsNullOrWhiteSpace(Contract.QuestflagStarted))
            {
                if (player.QuestManager.HasQuest(Contract.QuestflagStarted))
                    Stage = ContractStage.InProgress;
            }

            if (!string.IsNullOrWhiteSpace(Contract.QuestflagStamped))
            {
                if (player.QuestManager.HasQuest(Contract.QuestflagStamped))
                    Stage = ContractStage.InProgress;
            }

            if (!string.IsNullOrWhiteSpace(Contract.QuestflagTimer))
            {
                if (player.QuestManager.HasQuest(Contract.QuestflagTimer))
                {
                    TimeWhenDone = player.QuestManager.GetNextSolveTime(Contract.QuestflagTimer).TotalSeconds;

                    Stage = ContractStage.InProgress;
                }
            }

            if (!string.IsNullOrWhiteSpace(Contract.QuestflagProgress))
            {
                if (player.QuestManager.HasQuest(Contract.QuestflagProgress))
                {
                    var quest = player.QuestManager.GetQuest(Contract.QuestflagProgress);
                    var progress = quest.NumTimesCompleted;

                    Stage = progress > 0 ? ContractStage.ProgressCounter + progress : ContractStage.InProgress;
                }
            }
        }
    }

    public static class ContractTrackerExtensions
    {
        public static void Write(this BinaryWriter writer, ContractTracker contractTracker)
        {
            writer.Write(contractTracker.Version);
            writer.Write(contractTracker.ContractId);
            writer.Write((uint)contractTracker.Stage);
            writer.Write(contractTracker.TimeWhenDone);
            writer.Write(contractTracker.TimeWhenRepeats);

            // This is not written here.
            //writer.Write(Convert.ToUInt32(contractTracker.DeleteContract));
            //writer.Write(Convert.ToUInt32(contractTracker.SetAsDisplayContract));
        }
    }
}

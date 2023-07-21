using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Sequence;
using ACE.Server.Network.Structure;
using ACE.Server.Physics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;
using ACE.Server.WorldObjects.Managers;

using Character = ACE.Database.Models.Shard.Character;
using MotionTable = ACE.DatLoader.FileTypes.MotionTable;
using ACE.Server.Entity.TownControl;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public void HandleTownControlQuestStamps()
        {
            try
            {
                //Remove all town control related quest stamps
                var playerQuests = this.QuestManager.GetQuests();
                if (playerQuests != null && playerQuests.Count() > 0)
                {
                    var playerTownControlQuests = playerQuests.Where(x => x.QuestName.Contains("TownControlOwner"));

                    foreach (var qst in playerTownControlQuests)
                    {
                        this.QuestManager.Erase(qst.QuestName);
                    }
                }

                //Get a list of towns currently owned by player's monarchy and add the quest stamps
                var townList = DatabaseManager.TownControl.GetAllTowns();
                var allegiance = AllegianceManager.GetAllegiance(this);

                if (allegiance == null || !allegiance.MonarchId.HasValue)
                {
                    return;
                }

                var ownedTowns = townList.Where(x => x.CurrentOwnerID.HasValue && x.CurrentOwnerID.Value == allegiance.MonarchId.Value);

                if (ownedTowns == null || ownedTowns.Count() == 0)
                {
                    return;
                }

                foreach (var town in ownedTowns)
                {
                    this.QuestManager.Update(town.TownName.Trim().Replace(" ", "") + "TownControlOwner");
                }
            }
            catch(Exception ex)
            {
                log.Error($"Exception in HandleTownControlQuestStamps. Ex: {ex}");
            }
        }

        public int GetTownOwnershipCount()
        {
            int townsOwned = 0;

            try
            {
                var alleg = AllegianceManager.GetAllegiance(this);

                if (alleg != null && alleg.MonarchId.HasValue && TownControlAllegiances.IsAllowedAllegiance((int)alleg.MonarchId.Value))
                {
                    var towns = DatabaseManager.TownControl.GetAllTowns();
                    if (towns != null)
                    {
                        foreach (var town in towns)
                        {
                            if (town.CurrentOwnerID == alleg.MonarchId.Value)
                            {
                                townsOwned++;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error($"Error in GetTownOwnershipCount. Ex: {ex}");
            }

            return townsOwned;
        }
    }
}

using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        ///  The fellowship that this player belongs to
        /// </summary>
        public Fellowship Fellowship;

        public bool FellowVitalUpdate;

        // todo: Figure out if this is the best place to do this, and whether there are concurrency issues associated with it.
        public void FellowshipCreate(string fellowshipName, bool shareXP)
        {
            Fellowship = new Fellowship(this, fellowshipName, shareXP);
        }

        public void HandleActionFellowshipChangeOpenness(bool openness)
        {
            if (Fellowship != null)
            {
                if (!Fellowship.IsLocked)
                    Fellowship.UpdateOpenness(openness);
                else
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.FellowshipIsLocked));
            }
        }

        public void HandleActionFellowshipChangeLock(bool lockState, string lockName)
        {
            if (Fellowship != null)
                Fellowship.UpdateLock(lockState, lockName);
        }

        public void FellowshipQuit(bool disband)
        {
            if (Fellowship != null)
                Fellowship.QuitFellowship(this, disband);
        }

        public void FellowshipDismissPlayer(uint dismissGuid)
        {
            if (Fellowship == null) return;

            if (Guid.Full != Fellowship.FellowshipLeaderGuid)
            {
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustBeLeaderOfFellowship));
                return;
            }

            if (Guid.Full == dismissGuid)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat("You can't dismiss yourself from the fellowship", ChatMessageType.Broadcast));
                return;
            }

            var fellowToDismiss = PlayerManager.GetOnlinePlayer(dismissGuid);

            if (fellowToDismiss == null)
                return;

            Fellowship.RemoveFellowshipMember(fellowToDismiss, this);
        }

        public void FellowshipRecruit(Player newPlayer)
        {
            if (newPlayer == null) return;

            if (newPlayer.GetCharacterOption(CharacterOption.IgnoreFellowshipRequests))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{newPlayer.Name} is not accepting fellowship requests.", ChatMessageType.Fellowship));                
                Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.FellowshipIgnoringRequests));
            }
            else if (Fellowship != null)
            {
                if (Guid.Full == Fellowship.FellowshipLeaderGuid || Fellowship.Open)
                    Fellowship.AddFellowshipMember(this, newPlayer);
                else
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouMustBeLeaderOfFellowship));
            }
        }

        public void FellowshipNewLeader(uint newLeaderGuid)
        {
            if (Fellowship == null || Guid.Full == newLeaderGuid)
                return;

            if (Guid.Full != Fellowship.FellowshipLeaderGuid)
            {
                log.Warn($"{Name} tried to assign new fellowship leader from {Fellowship.FellowshipLeaderGuid:X8} to {newLeaderGuid:X8}");
                return;
            }

            var newLeader = PlayerManager.GetOnlinePlayer(newLeaderGuid);

            if (newLeader == null)
                return;

            if (newLeader.Fellowship != Fellowship)
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{newLeader.Name} is not a member of the fellowship!", ChatMessageType.Broadcast));
                return;
            }

            Fellowship.AssignNewLeader(this, newLeader);
        }

        public bool FellowshipPanelOpen { get; set; }

        /// <summary>
        /// Called when player opens / closes the fellowship panel
        /// </summary>
        public void HandleFellowshipUpdateRequest(bool panelOpen)
        {
            FellowshipPanelOpen = panelOpen;

            if (Fellowship != null && FellowshipPanelOpen)
                Session.Network.EnqueueSend(new GameEventFellowshipFullUpdate(Session));
        }
    }
}

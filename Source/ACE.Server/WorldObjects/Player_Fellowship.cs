
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        ///  The fellowship that this player belongs to
        /// </summary>
        public Fellowship Fellowship;

        // todo: Figure out if this is the best place to do this, and whether there are concurrency issues associated with it.
        public void FellowshipCreate(string fellowshipName, bool shareXP)
        {
            Fellowship = new Fellowship(this, fellowshipName, shareXP);
        }

        public void HandleActionFellowshipChangeOpenness(bool openness)
        {
            if (Fellowship != null)
                Fellowship.UpdateOpenness(openness);
        }

        public void FellowshipQuit(bool disband)
        {
            if (Fellowship != null)
                Fellowship.QuitFellowship(this, disband);
        }

        public void FellowshipDismissPlayer(Player player)
        {
            if (Fellowship == null) return;

            if (Guid.Full == Fellowship.FellowshipLeaderGuid)
                Fellowship.RemoveFellowshipMember(player);
            else
                Session.Network.EnqueueSend(new GameMessageSystemChat("You are not the fellowship leader.", ChatMessageType.Fellowship));
        }

        public void FellowshipRecruit(Player newPlayer)
        {
            if (newPlayer == null) return;

            if (newPlayer.GetCharacterOption(CharacterOption.IgnoreFellowshipRequests))
            {
                Session.Network.EnqueueSend(new GameMessageSystemChat($"{newPlayer.Name} is not accepting fellowing requests.", ChatMessageType.Fellowship));
            }
            else if (Fellowship != null)
            {
                if (Guid.Full == Fellowship.FellowshipLeaderGuid || Fellowship.Open)
                    Fellowship.AddFellowshipMember(this, newPlayer);
                else
                    Session.Network.EnqueueSend(new GameMessageSystemChat("You are not the fellowship leader.", ChatMessageType.Fellowship));
            }
        }

        public void FellowshipNewLeader(Player newLeader)
        {
            if (Fellowship == null || newLeader == null)
                return;

            Fellowship.AssignNewLeader(newLeader);
        }
    }
}

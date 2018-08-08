using System.Linq;

using ACE.Entity;

namespace ACE.Database.Models.Shard
{
    public static class CharacterExtensions
    {
        public static bool TryRemoveFriend(this Character character, ObjectGuid friendGuid, out CharacterPropertiesFriendList entity)
        {
            entity = character.CharacterPropertiesFriendList.FirstOrDefault(x => x.FriendId == friendGuid.Full);

            if (entity != null)
            {
                character.CharacterPropertiesFriendList.Remove(entity);
                entity.Character = null;
                return true;
            }

            return false;
        }
    }
}

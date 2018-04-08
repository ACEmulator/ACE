using System.Collections.Generic;
using System.Linq;
using ACE.Database.Models.World;

namespace ACE.Server.Entity
{
    public static class EmoteCache
    {
        public static Dictionary<uint, List<WeeniePropertiesEmote>> Emotes;
        public static Dictionary<uint, List<WeeniePropertiesEmoteAction>> Actions;

        static EmoteCache()
        {
            Emotes = new Dictionary<uint, List<WeeniePropertiesEmote>>();
            Actions = new Dictionary<uint, List<WeeniePropertiesEmoteAction>>();
        }

        public static List<WeeniePropertiesEmote> GetEmotes(uint objectId)
        {
            Emotes.TryGetValue(objectId, out var emotes);
            if (emotes == null)
            {
                using (var ctx = new WorldDbContext())
                    emotes = ctx.WeeniePropertiesEmote.Where(e => e.ObjectId == objectId).ToList();

                Emotes.Add(objectId, emotes);
            }
            return emotes;
        }

        public static List<WeeniePropertiesEmoteAction> GetActions(uint objectId)
        {
            Actions.TryGetValue(objectId, out var actions);
            if (actions == null)
            {
                using (var ctx = new WorldDbContext())
                    actions = ctx.WeeniePropertiesEmoteAction.Where(e => e.ObjectId == objectId).ToList();

                Actions.Add(objectId, actions);
            }
            return actions;
        }
    }
}

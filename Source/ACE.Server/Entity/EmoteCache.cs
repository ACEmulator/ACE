using System.Collections.Generic;
using System.Linq;
using ACE.Database.Models.World;
using Microsoft.EntityFrameworkCore;

namespace ACE.Server.Entity
{
    public static class EmoteCache
    {
        public static Dictionary<uint, List<WeeniePropertiesEmote>> Emotes;

        static EmoteCache()
        {
            Emotes = new Dictionary<uint, List<WeeniePropertiesEmote>>();
        }

        public static List<WeeniePropertiesEmote> GetEmotes(uint objectId)
        {
            Emotes.TryGetValue(objectId, out var emotes);
            if (emotes == null)
            {
                // todo we shouldn't instantiate a worlddbcontext here. Instead, we should use the WorldDatabase class.
                // I'm not even sure if we need an emote cache. The emote table should exist as a property of the weenie.
                // If we do want to separate emotes from their weenies, the cache should probably be in the WorldDatabase as well, similar to how we cache other world objects. Mag-nus 2018-08-02
                using (var ctx = new WorldDbContext())
                    emotes = ctx.WeeniePropertiesEmote.Include(r => r.WeeniePropertiesEmoteAction).Where(e => e.ObjectId == objectId).ToList();

                Emotes.Add(objectId, emotes);
            }

            return emotes;
        }
    }
}

using System.Collections.Generic;

namespace ACE.Server.WebApi.Model.Character
{
    public class CharacterListModel : BaseAuthenticatedModel
    {
        public List<Database.Models.Shard.Character> Characters { get; set; }
    }
}

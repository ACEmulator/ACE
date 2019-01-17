using ACE.Database.Models.Shard;
using System.Collections.Generic;

namespace ACE.Server.WebApi.Model
{
    public class CharacterListModel : BaseModel
    {
        public List<Character> Characters { get; set; }
    }
}

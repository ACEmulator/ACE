using ACE.Database.Models.Shard;
using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.Web.Model
{
    public class IndexModel : BaseModel
    {
        public List<Character> Characters { get; set; }
    }
}

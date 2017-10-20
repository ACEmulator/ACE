using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Api.Common
{
    public class TokenInfo
    {
        public string Name { get; set; }

        public uint? AccountId { get; set; }

        public Guid AccountGuid { get; set; }

        public string IssuingServer { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Common
{
    public class ChatConfiguration
    {
        public string DiscordToken { get; set; }
        public long GeneralChannelId { get; set; }
        public long TradeChannelId { get; set; }
        public long ServerId { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Log
{
    public partial class Vote
    {
        public uint Id { get; set; }
        public string VoteCode { get; set; }
        public uint AccountId { get; set; }
        public string AccountName { get; set; }
        public uint CharacterId { get; set; }
        public string CharacterName { get; set; }
        public bool VoteResponse { get; set; }
        public DateTime VoteDateTime { get; set; }
    }
}

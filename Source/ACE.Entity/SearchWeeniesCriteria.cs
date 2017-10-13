using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class SearchWeeniesCriteria
    {
        public string PartialName { get; set; }

        public uint? WeenieClassId { get; set; }

        public WeenieType? WeenieType { get; set; }

        public ItemType? ItemType { get; set; }

        public Guid? ContentGuid { get; set; }

        public bool? UserModified { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class CookBook
    {
        public uint Id { get; set; }
        public uint RecipeId { get; set; }
        public uint SourceWCID { get; set; }
        public uint TargetWCID { get; set; }
        public DateTime LastModified { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}

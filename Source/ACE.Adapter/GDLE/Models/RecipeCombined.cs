using System.Collections.Generic;

namespace ACE.Adapter.GDLE.Models
{
    public class RecipeCombined
    {
        public uint key { get; set; }
        public string desc { get; set; }
        public Recipe recipe { get; set; }
        public List<RecipePrecursor> precursors { get; set; }
    }
}

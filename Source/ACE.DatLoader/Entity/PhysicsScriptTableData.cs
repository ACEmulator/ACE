using System.Collections.Generic;

namespace ACE.DatLoader.Entity
{
    public class PhysicsScriptTableData
    {
        public List<ScriptAndModData> Scripts { get; set; } = new List<ScriptAndModData>();

        public static PhysicsScriptTableData Read(DatReader datReader)
        {
            PhysicsScriptTableData obj = new PhysicsScriptTableData();

            uint num_scripts = datReader.ReadUInt32();
            for (uint i = 0; i < num_scripts; i++)
                obj.Scripts.Add(ScriptAndModData.Read(datReader));

            return obj;
        }
    }
}

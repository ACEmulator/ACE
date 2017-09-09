namespace ACE.DatLoader.Entity
{
    public class ScriptAndModData
    {
        public float Mod { get; set; }
        public uint ScriptId { get; set; }

        public static ScriptAndModData Read(DatReader datReader)
        {
            ScriptAndModData obj = new ScriptAndModData();

            obj.Mod = datReader.ReadSingle();
            obj.ScriptId = datReader.ReadUInt32();

            return obj;
        }
    }
}

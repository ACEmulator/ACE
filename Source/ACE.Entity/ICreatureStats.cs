namespace ACE.Entity
{
    public interface ICreatureStats
    {
        uint AceObjectId { get; }

        uint Strength { get; }
        
        uint Endurance { get; }
        
        uint Coordination { get; }
        
        uint Quickness { get; }
        
        uint Focus { get; }
        
        uint Self { get; }
    }
}

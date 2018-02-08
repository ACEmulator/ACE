namespace ACE.Server.Network.Sequence
{
    public interface ISequence
    {
        byte[] NextBytes { get; }
        byte[] CurrentBytes { get; }
    }
}

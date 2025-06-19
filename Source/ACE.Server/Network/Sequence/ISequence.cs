// UTF-8 BOM removed to ensure consistent encoding
namespace ACE.Server.Network.Sequence
{
    public interface ISequence
    {
        byte[] NextBytes { get; }
        byte[] CurrentBytes { get; }
    }
}

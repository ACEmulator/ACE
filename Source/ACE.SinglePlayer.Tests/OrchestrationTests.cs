using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.SinglePlayer.Orchestration;
using ACE.SinglePlayer.Processes;

namespace ACE.SinglePlayer.Tests;

[TestClass]
public sealed class OrchestrationTests
{
    [TestMethod]
    public void DuplicatePlayOperationIsPrevented()
    {
        var gate = new PlayOperationGate();
        Assert.IsTrue(gate.TryEnter());
        Assert.IsFalse(gate.TryEnter());
        Assert.IsTrue(gate.IsActive);
        gate.Exit();
        Assert.IsTrue(gate.TryEnter());
    }

    [TestMethod]
    public void OwnershipRequiresPidStartTimeAndExecutablePath()
    {
        var start = DateTime.UtcNow;
        var executable = Path.Combine(Path.GetTempPath(), "ACE.Server.exe");
        var record = new ProcessOwnershipRecord { ProcessId = 123, StartTimeUtc = start, ExecutablePath = executable };
        Assert.IsTrue(record.Matches(123, start.AddMilliseconds(100), executable));
        Assert.IsFalse(record.Matches(124, start, executable));
        Assert.IsFalse(record.Matches(123, start.AddSeconds(2), executable));
        Assert.IsFalse(record.Matches(123, start, Path.Combine(Path.GetTempPath(), "Other.Server.exe")));
    }
}

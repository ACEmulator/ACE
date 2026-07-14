using System.Text.Json.Nodes;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.SinglePlayer.Mods;

namespace ACE.SinglePlayer.Tests;

[TestClass]
public sealed class ModTests
{
    [TestMethod]
    public void ServerMetadataParsesCurrentAceFields()
    {
        var directory = TestPaths.CreateTemporaryDirectory();
        try
        {
            File.WriteAllText(Path.Combine(directory, "Meta.json"), """
            { "Name":"Test Mod", "Author":"Author", "Description":"Description", "Version":"1.2.3", "Priority":7, "Enabled":false }
            """);
            var record = AceServerModProvider.Read(directory);
            Assert.IsFalse(record.IsMalformed);
            Assert.AreEqual("Test Mod", record.Name);
            Assert.AreEqual("Author", record.Author);
            Assert.AreEqual((uint)7, record.Priority);
            Assert.IsFalse(record.Enabled);
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [TestMethod]
    public async Task EnabledTogglePreservesUnknownJsonFields()
    {
        var directory = TestPaths.CreateTemporaryDirectory();
        try
        {
            var path = Path.Combine(directory, "Meta.json");
            await File.WriteAllTextAsync(path, """{"Name":"Test","Enabled":true,"FutureField":{"answer":42}}""");
            await ModMetadataEditor.SetEnabledAsync(path, false);
            var metadata = ModMetadataEditor.Parse(await File.ReadAllTextAsync(path));
            Assert.IsFalse(metadata["Enabled"]!.GetValue<bool>());
            Assert.AreEqual(42, metadata["FutureField"]!["answer"]!.GetValue<int>());
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }

    [TestMethod]
    public void MalformedMetadataIsVisibleInsteadOfSilentlySkipped()
    {
        var directory = TestPaths.CreateTemporaryDirectory();
        try
        {
            File.WriteAllText(Path.Combine(directory, "Meta.json"), "{not json");
            var record = AceServerModProvider.Read(directory);
            Assert.IsTrue(record.IsMalformed);
            Assert.AreEqual(CompatibilityStatus.LoadFailed, record.CompatibilityStatus);
            Assert.IsFalse(string.IsNullOrWhiteSpace(record.LastLoadError));
        }
        finally
        {
            Directory.Delete(directory, true);
        }
    }
}

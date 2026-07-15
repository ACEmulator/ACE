using System.Text.Json.Nodes;
using System.IO.Compression;
using System.Security.Cryptography;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.SinglePlayer.Mods;

namespace ACE.SinglePlayer.Tests;

[TestClass]
public sealed class ModTests
{
    [TestMethod]
    public void AquafirCatalogHasUsefulDescriptionsAndSafetyPolicies()
    {
        Assert.AreEqual(22, AquafirSampleCatalog.Entries.Count);
        Assert.AreEqual(22, AquafirSampleCatalog.Entries.Select(entry => entry.Id).Distinct(StringComparer.OrdinalIgnoreCase).Count());
        Assert.IsTrue(AquafirSampleCatalog.Entries.All(entry => !string.IsNullOrWhiteSpace(entry.Description)));
        Assert.IsTrue(AquafirSampleCatalog.Entries.All(entry => !entry.Description.Contains("non-existent mod", StringComparison.OrdinalIgnoreCase)));
        Assert.IsTrue(AquafirSampleCatalog.Entries.All(entry => !string.IsNullOrWhiteSpace(entry.SafetyNotice)));

        var criticalOverride = AquafirSampleCatalog.Entries.Single(entry => entry.Id == "aquafir.critical-override");
        Assert.AreEqual(ModCatalogAvailability.Ready, criticalOverride.Availability);
        Assert.AreEqual(ModRemovalPolicy.Safe, criticalOverride.RemovalPolicy);
    }

    [TestMethod]
    public void CatalogBlocksMissingDependenciesBeforeInstall()
    {
        var dependency = new ModCatalogEntry(
            "dependency", "Dependency", "Author", "Description", "Details", "", ModCatalogAvailability.Ready,
            ModDataImpact.None, ModRemovalPolicy.Safe, "Safe", PackageRelativePath: "dependency.zip");
        var dependent = new ModCatalogEntry(
            "dependent", "Dependent", "Author", "Description", "Details", "", ModCatalogAvailability.Ready,
            ModDataImpact.None, ModRemovalPolicy.Safe, "Safe", DependencyIds: new[] { "dependency" }, PackageRelativePath: "dependent.zip");

        var service = new ModCatalogService(new[] { dependency, dependent }, TestPaths.CreateTemporaryDirectory());
        var result = service.Merge(Array.Empty<ModRecord>()).Single(item => item.Catalog.Id == "dependent");

        Assert.AreEqual(CompatibilityStatus.MissingDependency, result.CompatibilityStatus);
        Assert.IsTrue(result.CompatibilityMessage.Contains("Dependency", StringComparison.Ordinal));
    }

    [TestMethod]
    public async Task ValidatedPackageInstallsAtomically()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var package = Path.Combine(root, "test.zip");
            CreatePackage(package);
            var installer = new ModPackageInstaller();
            var installed = await installer.InstallAsync(
                package, "test.mod", Path.Combine(root, "Mods"), Path.Combine(root, "Staging"));

            Assert.AreEqual(Path.Combine(root, "Mods", "TestMod"), installed);
            Assert.IsTrue(File.Exists(Path.Combine(installed, "TestMod.dll")));
            Assert.IsTrue(File.Exists(Path.Combine(installed, "Meta.json")));
            Assert.IsFalse(Directory.EnumerateFileSystemEntries(Path.Combine(root, "Staging")).Any());
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public async Task PackageTraversalIsRejected()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var package = Path.Combine(root, "unsafe.zip");
            CreatePackage(package, "mod/../escaped.txt");
            var installer = new ModPackageInstaller();

            await Assert.ThrowsAsync<InvalidDataException>(() => installer.InstallAsync(
                package, "test.mod", Path.Combine(root, "Mods"), Path.Combine(root, "Staging")));
            Assert.IsFalse(File.Exists(Path.Combine(root, "escaped.txt")));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

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

    private static void CreatePackage(string path, string? additionalEntry = null)
    {
        using (var archive = ZipFile.Open(path, ZipArchiveMode.Create))
        {
            WriteEntry(archive, "ace-mod.json", """
                {"formatVersion":1,"id":"test.mod","name":"Test Mod","version":"1.0.0","folderName":"TestMod","entryAssembly":"TestMod.dll"}
                """);
            WriteEntry(archive, "mod/Meta.json", """{"Name":"Test Mod","Enabled":true}""");
            WriteEntry(archive, "mod/TestMod.dll", "test assembly placeholder");
            if (additionalEntry is not null)
                WriteEntry(archive, additionalEntry, "unsafe");
        }

        File.WriteAllText(path + ".sha256", Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(path))));
    }

    private static void WriteEntry(ZipArchive archive, string name, string content)
    {
        var entry = archive.CreateEntry(name);
        using var writer = new StreamWriter(entry.Open());
        writer.Write(content);
    }
}

using System.Text.Json.Nodes;
using System.IO.Compression;
using System.Security.Cryptography;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.Server.Command;
using ACE.Server.Entity;
using ACE.Server.WorldObjects;
using ACE.SinglePlayer.Mods;

using HarmonyLib;

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

        foreach (var preview in AquafirSampleCatalog.Entries.Where(entry => entry.Availability == ModCatalogAvailability.Preview))
        {
            Assert.IsFalse(string.IsNullOrWhiteSpace(preview.PackageRelativePath));
            Assert.IsFalse(string.IsNullOrWhiteSpace(preview.SourceUrl));
            Assert.IsFalse(string.IsNullOrWhiteSpace(preview.PortSourceUrl));
        }
        Assert.AreEqual(2, AquafirSampleCatalog.Entries.Count(entry => entry.Availability == ModCatalogAvailability.Preview));
    }

    [TestMethod]
    public void CuratedCatalogIncludesCustomClothingBaseAsWarnedPreview()
    {
        Assert.AreEqual(24, CuratedModCatalog.Entries.Count);
        var entry = CuratedModCatalog.Entries.Single(item => item.Id == "optimshi.custom-clothing-base");
        Assert.AreEqual("OptimShi", entry.Author);
        Assert.AreEqual(ModCatalogAvailability.Preview, entry.Availability);
        Assert.AreEqual(ModDataImpact.WorldData, entry.DataImpact);
        Assert.AreEqual(ModRemovalPolicy.DoNotRemove, entry.RemovalPolicy);
        Assert.IsTrue(entry.SourceUrl.Contains("OptimShi/CustomClothingBase", StringComparison.Ordinal));
        Assert.IsTrue(entry.PreviewNotice.Contains("not been thoroughly tested", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(entry.PackageRelativePath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void CuratedCatalogIncludesUniqueWeeniesProcAsWarnedPreview()
    {
        var entry = CuratedModCatalog.Entries.Single(item =>
            item.Id == "titaniumweiner.ace-unique-weenies-proc");

        Assert.AreEqual("Expanded Cast on Strike", entry.Name);
        Assert.AreEqual("titaniumweiner", entry.Author);
        Assert.AreEqual(ModCatalogAvailability.Preview, entry.Availability);
        Assert.AreEqual(ModDataImpact.SettingsOnly, entry.DataImpact);
        Assert.AreEqual(ModRemovalPolicy.Safe, entry.RemovalPolicy);
        Assert.IsTrue(entry.SourceUrl.Contains("titaniumweiner/ACEUniqueWeenies", StringComparison.Ordinal));
        Assert.IsTrue(entry.PreviewNotice.Contains("not been thoroughly tested", StringComparison.OrdinalIgnoreCase));
        Assert.IsTrue(entry.PackageRelativePath.EndsWith(".zip", StringComparison.OrdinalIgnoreCase));
    }

    [TestMethod]
    public void HelloCommandRegistersAndRemovesCurrentAceCommands()
    {
        var mod = new HelloCommand.Mod();
        try
        {
            mod.Initialize();
            Assert.AreEqual(1, CommandManager.GetCommandByName("hello").Count());
            Assert.AreEqual(1, CommandManager.GetCommandByName("bye").Count());
        }
        finally
        {
            mod.Dispose();
        }

        Assert.AreEqual(0, CommandManager.GetCommandByName("hello").Count());
        Assert.AreEqual(0, CommandManager.GetCommandByName("bye").Count());
    }

    [TestMethod]
    public void SocietyTailoringPatchTargetsCurrentAceSignature()
    {
        var original = AccessTools.Method(typeof(Tailoring), nameof(Tailoring.VerifyUseRequirements),
            new[] { typeof(Player), typeof(WorldObject), typeof(WorldObject) });
        Assert.IsNotNull(original);

        var mod = new SocietyTailoring.Mod();
        try
        {
            mod.Initialize();
            var patchInfo = Harmony.GetPatchInfo(original);
            Assert.IsNotNull(patchInfo);
            Assert.IsTrue(patchInfo.Prefixes.Any(patch => patch.owner == "aquafir.SocietyTailoring.ace-single-player"));
        }
        finally
        {
            mod.Dispose();
        }

        var remaining = Harmony.GetPatchInfo(original);
        Assert.IsTrue(remaining is null || remaining.Prefixes.All(patch => patch.owner != "aquafir.SocietyTailoring.ace-single-player"));
    }

    [TestMethod]
    public void UniqueWeeniesProcPatchTargetsCurrentAceSignatureAndCanBeRemoved()
    {
        var original = AccessTools.Method(typeof(WorldObject), nameof(WorldObject.TryProcEquippedItems),
            new[] { typeof(WorldObject), typeof(Creature), typeof(bool), typeof(WorldObject) });
        Assert.IsNotNull(original);

        var mod = new ACEUniqueWeeniesProc.Mod();
        try
        {
            mod.Initialize();
            var patchInfo = Harmony.GetPatchInfo(original);
            Assert.IsNotNull(patchInfo);
            Assert.IsTrue(patchInfo.Prefixes.Any(patch =>
                patch.owner == "titaniumweiner.ACEUniqueWeeniesProc.ace-single-player"));
        }
        finally
        {
            mod.Dispose();
        }

        var remaining = Harmony.GetPatchInfo(original);
        Assert.IsTrue(remaining is null || remaining.Prefixes.All(patch =>
            patch.owner != "titaniumweiner.ACEUniqueWeeniesProc.ace-single-player"));
    }

    [TestMethod]
    public void UniqueWeeniesProcFilterMatchesDocumentedBehavior()
    {
        Assert.IsTrue(ACEUniqueWeeniesProc.ACEUniqueWeeniesProcPatch.IsEligibleEquippedProc(
            hasProc: true, cloakWeaveProc: null, procSpellSelfTargeted: false, selfTarget: false));
        Assert.IsTrue(ACEUniqueWeeniesProc.ACEUniqueWeeniesProcPatch.IsEligibleEquippedProc(
            hasProc: true, cloakWeaveProc: 2, procSpellSelfTargeted: true, selfTarget: true));
        Assert.IsFalse(ACEUniqueWeeniesProc.ACEUniqueWeeniesProcPatch.IsEligibleEquippedProc(
            hasProc: true, cloakWeaveProc: 1, procSpellSelfTargeted: false, selfTarget: false));
        Assert.IsFalse(ACEUniqueWeeniesProc.ACEUniqueWeeniesProcPatch.IsEligibleEquippedProc(
            hasProc: false, cloakWeaveProc: null, procSpellSelfTargeted: false, selfTarget: false));
        Assert.IsFalse(ACEUniqueWeeniesProc.ACEUniqueWeeniesProcPatch.IsEligibleEquippedProc(
            hasProc: true, cloakWeaveProc: null, procSpellSelfTargeted: true, selfTarget: false));
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
    public void PreviewPackageIsInstallableButKeepsLimitedTestingWarning()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            File.WriteAllText(Path.Combine(root, "preview.zip"), "package placeholder");
            var preview = new ModCatalogEntry(
                "preview", "Preview", "Author", "Description", "Details", "https://example.invalid/original",
                ModCatalogAvailability.Preview, ModDataImpact.None, ModRemovalPolicy.Safe, "Back up first.",
                PackageRelativePath: "preview.zip", PortSourceUrl: "https://example.invalid/port");

            var item = new ModCatalogService(new[] { preview }, root).Merge(Array.Empty<ModRecord>()).Single();

            Assert.AreEqual(CompatibilityStatus.Compatible, item.CompatibilityStatus);
            Assert.AreEqual("Preview - limited testing", item.Status);
            Assert.IsTrue(item.CompatibilityMessage.Contains("not received thorough in-game testing", StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public void CatalogListsInstalledAndReadyToInstallModsBeforeUnavailableMods()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            File.WriteAllText(Path.Combine(root, "ready.zip"), "package placeholder");
            var unavailable = new ModCatalogEntry(
                "unavailable", "A Needs Port", "Author", "Description", "Details", "",
                ModCatalogAvailability.NeedsPort, ModDataImpact.None, ModRemovalPolicy.Safe, "Safe");
            var ready = new ModCatalogEntry(
                "ready", "Z Ready", "Author", "Description", "Details", "",
                ModCatalogAvailability.Ready, ModDataImpact.None, ModRemovalPolicy.Safe, "Safe",
                PackageRelativePath: "ready.zip");
            var installed = new ModRecord { Name = "Installed", Type = ModType.AceServer, Enabled = true };

            var result = new ModCatalogService(new[] { unavailable, ready }, root).Merge(new[] { installed });

            Assert.AreEqual("Installed", result[0].Name);
            Assert.AreEqual("Z Ready", result[1].Name);
            Assert.AreEqual("A Needs Port", result[2].Name);
        }
        finally
        {
            Directory.Delete(root, true);
        }
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
    public async Task ValidatedPackageCanBeInspectedBeforeImport()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var package = Path.Combine(root, "test.zip");
            CreatePackage(package);
            var manifest = await new ModPackageInstaller().InspectAsync(package);

            Assert.AreEqual("test.mod", manifest.Id);
            Assert.AreEqual("Test Mod", manifest.Name);
            Assert.AreEqual("1.0.0", manifest.Version);
            Assert.AreEqual("TestMod.dll", manifest.EntryAssembly);
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

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.SinglePlayer.CustomContent;

namespace ACE.SinglePlayer.Tests;

[TestClass]
public sealed class CustomWeenieTests
{
    private const string ValidAceForgeSql = """
        /* ===== FILE: 850001 Test Keeper.sql ===== */
        DELETE FROM `weenie` WHERE `class_Id` = 850001;

        INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
        VALUES (850001, 'Test Keeper; Sir ''Semicolon''', 10, '2026-07-16 00:00:00') /* Creature */;

        INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
        VALUES (850001, 1, 16)
             , (850001, 5, 10);
        """;

    [TestMethod]
    public void AceForgeWeenieSqlIsPreviewedWithWcidNameAndType()
    {
        var result = new CustomWeenieSqlInspector().Inspect("850001 Test Keeper.sql", ValidAceForgeSql);

        Assert.AreEqual(1, result.Definitions.Count);
        Assert.AreEqual(0, result.Issues.Count);
        Assert.AreEqual((uint)850001, result.Definitions[0].ClassId);
        Assert.AreEqual("Test Keeper; Sir 'Semicolon'", result.Definitions[0].ClassName);
        Assert.AreEqual("Creature", result.Definitions[0].TypeName);
        Assert.AreEqual(64, result.Definitions[0].Sha256.Length);
    }

    [TestMethod]
    public void AceForgeEmoteParentPatternIsAllowed()
    {
        var sql = ValidAceForgeSql + """

            INSERT INTO `weenie_properties_emote` (`object_Id`, `category`, `probability`)
            VALUES (850001, 10, 1);
            SET @parent_id = LAST_INSERT_ID();
            INSERT INTO `weenie_properties_emote_action` (`emote_Id`, `order`, `type`)
            VALUES (@parent_id, 0, 10);
            """;

        var result = new CustomWeenieSqlInspector().Inspect("emote.sql", sql);

        Assert.AreEqual(1, result.Definitions.Count);
        Assert.AreEqual(0, result.Issues.Count);
    }

    [TestMethod]
    public void ImportRejectsStatementsOutsideTheWeenieAllowlist()
    {
        var sql = ValidAceForgeSql + "\r\nDROP TABLE `weenie`;";

        var result = new CustomWeenieSqlInspector().Inspect("unsafe.sql", sql);

        Assert.AreEqual(0, result.Definitions.Count);
        Assert.AreEqual(1, result.Issues.Count);
        StringAssert.Contains(result.Issues[0].Message, "Only DELETE");
    }

    [TestMethod]
    public void ImportRejectsPropertyRowsForAnotherWcid()
    {
        var sql = ValidAceForgeSql.Replace("(850001, 5, 10)", "(850002, 5, 10)", StringComparison.Ordinal);

        var result = new CustomWeenieSqlInspector().Inspect("wrong-owner.sql", sql);

        Assert.AreEqual(0, result.Definitions.Count);
        StringAssert.Contains(result.Issues[0].Message, "different WCID");
    }

    [TestMethod]
    public void ImportRejectsExecutableMysqlComments()
    {
        var sql = ValidAceForgeSql + "\r\n/*!50000 DROP TABLE weenie */;";

        var result = new CustomWeenieSqlInspector().Inspect("conditional.sql", sql);

        Assert.AreEqual(0, result.Definitions.Count);
        StringAssert.Contains(result.Issues[0].Message, "Executable SQL comments");
    }

    [TestMethod]
    public void QuestOnlyAceForgeExportIsReportedAsUnsupported()
    {
        const string sql = "DELETE FROM quest WHERE name = 'Test'; INSERT INTO quest (id, name) VALUES (1, 'Test');";

        var result = new CustomWeenieSqlInspector().Inspect("quest.sql", sql);

        Assert.AreEqual(0, result.Definitions.Count);
        StringAssert.Contains(result.Issues[0].Message, "quest, event, recipe");
    }

    [TestMethod]
    public void DuplicateWcidsInASelectionAreSkipped()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var first = Path.Combine(root, "first.sql");
            var second = Path.Combine(root, "second.sql");
            File.WriteAllText(first, ValidAceForgeSql);
            File.WriteAllText(second, ValidAceForgeSql);

            var result = new CustomWeenieSqlInspector().InspectFiles(new[] { first, second });

            Assert.AreEqual(0, result.Definitions.Count);
            Assert.AreEqual(2, result.Issues.Count);
            Assert.IsTrue(result.Issues.All(issue => issue.Message.Contains("also defined", StringComparison.Ordinal)));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }

    [TestMethod]
    public void BackupProgramIsFoundBesideMariaDbServer()
    {
        var root = TestPaths.CreateTemporaryDirectory();
        try
        {
            var server = Path.Combine(root, "mariadbd.exe");
            var dump = Path.Combine(root, "mariadb-dump.exe");
            File.WriteAllText(server, string.Empty);
            File.WriteAllText(dump, string.Empty);

            Assert.AreEqual(dump, MariaDbWorldBackupService.FindDumpExecutable(server));
        }
        finally
        {
            Directory.Delete(root, true);
        }
    }
}

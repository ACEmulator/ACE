using Microsoft.VisualStudio.TestTools.UnitTesting;

using ACE.SinglePlayer.UI;
using ACE.SinglePlayer.Models;

namespace ACE.SinglePlayer.Tests;

[TestClass]
public sealed class UiLayoutTests
{
    [TestMethod]
    public void ModLibraryCanBeConstructedBeforeWindowsCompletesLayout()
    {
        Exception? failure = null;
        var thread = new Thread(() =>
        {
            try
            {
                var settings = new LauncherSettings
                {
                    ModsDirectory = Path.Combine(Path.GetTempPath(), "ACE.SinglePlayer.Tests", "MissingMods"),
                    RuntimeDirectory = Path.Combine(Path.GetTempPath(), "ACE.SinglePlayer.Tests", "Runtime")
                };
                using var form = new ModsForm(settings, () => false);
            }
            catch (Exception ex)
            {
                failure = ex;
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();

        Assert.IsTrue(thread.Join(TimeSpan.FromSeconds(10)), "The Mods window constructor did not finish.");
        Assert.IsNull(failure, failure?.ToString());
    }

    [TestMethod]
    public void ModLibrarySplitterUsesPreferredLayoutAfterFormIsSized()
    {
        var layout = ModsForm.CalculateSplitterLayout(1224, 6);

        Assert.AreEqual(730, layout.Distance);
        Assert.AreEqual(540, layout.Panel1MinSize);
        Assert.AreEqual(360, layout.Panel2MinSize);
    }

    [TestMethod]
    [DataRow(900, 6)]
    [DataRow(400, 6)]
    [DataRow(100, 6)]
    public void ModLibrarySplitterAlwaysFitsNarrowOrScaledLayout(int width, int splitterWidth)
    {
        var layout = ModsForm.CalculateSplitterLayout(width, splitterWidth);
        var available = Math.Max(0, width - splitterWidth);

        Assert.IsGreaterThanOrEqualTo(layout.Distance, layout.Panel1MinSize);
        Assert.IsGreaterThanOrEqualTo(available - layout.Distance, layout.Panel2MinSize);
        Assert.IsLessThanOrEqualTo(layout.Panel1MinSize + layout.Panel2MinSize, available);
    }
}

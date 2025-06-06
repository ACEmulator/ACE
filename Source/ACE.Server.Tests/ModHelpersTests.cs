using System;
using System.IO;
using System.Threading.Tasks;
using HarmonyLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACE.Server.Mods;

namespace ACE.Server.Tests
{
    [TestClass]
    public class ModHelpersTests
    {
        private static bool _shouldThrow = true;

        [TestMethod]
        public async Task ReadWithRetryAsync_ClosesFile_OnException()
        {
            var path = Path.GetTempFileName();
            await File.WriteAllTextAsync(path, "data");
            var file = new FileInfo(path);

            var harmony = new Harmony("ModHelpersTests.ReadFailure");
            harmony.Patch(
                AccessTools.Method(typeof(StreamReader), nameof(StreamReader.ReadToEndAsync)),
                prefix: new HarmonyMethod(typeof(ModHelpersTests).GetMethod(nameof(FailPrefix), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static))
            );

            try
            {
                await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () => await file.ReadWithRetryAsync(TimeSpan.FromMilliseconds(1), 1));
            }
            finally
            {
                harmony.UnpatchAll(harmony.Id);
            }

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
            }

            File.Delete(path);
        }

        private static bool FailPrefix(ref Task<string> __result)
        {
            if (_shouldThrow)
            {
                _shouldThrow = false;
                __result = Task.FromException<string>(new InvalidOperationException("fail"));
                return false;
            }
            return true;
        }
    }
}

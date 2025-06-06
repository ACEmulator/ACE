using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ACE.Server.Mods;

namespace ACE.Server.Tests
{
    [TestClass]
    public class ModHelperTests
    {
        [TestMethod]
        public async Task RetryWrite_FailsWhenFileLocked()
        {
            var path = Path.GetTempFileName();
            var file = new FileInfo(path);

            try
            {
                using var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                var result = await file.RetryWrite("data", 1);
                Assert.IsFalse(result);
            }
            finally
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
        }
    }
}

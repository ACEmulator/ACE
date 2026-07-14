using System.Text;

namespace ACE.SinglePlayer.Database;

public static class WorldSqlPackageInspector
{
    private static readonly byte[] RequiredHumanInsert =
        Encoding.ASCII.GetBytes("INSERT INTO `weenie` VALUES (1,");

    public static async Task<bool> ContainsRequiredWorldDataAsync(string path, CancellationToken cancellationToken)
    {
        if (!File.Exists(path))
            return false;

        await using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read,
            bufferSize: 64 * 1024, FileOptions.Asynchronous | FileOptions.SequentialScan);
        var buffer = new byte[64 * 1024];
        var matched = 0;

        while (true)
        {
            var read = await stream.ReadAsync(buffer, cancellationToken);
            if (read == 0)
                return false;

            for (var index = 0; index < read; index++)
            {
                var value = buffer[index];
                if (value == RequiredHumanInsert[matched])
                {
                    matched++;
                    if (matched == RequiredHumanInsert.Length)
                        return true;
                }
                else
                {
                    matched = value == RequiredHumanInsert[0] ? 1 : 0;
                }
            }
        }
    }
}

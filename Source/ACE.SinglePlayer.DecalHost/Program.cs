using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace ACE.SinglePlayer.DecalHost;

internal static class Program
{
    public static int Main(string[] args)
    {
        try
        {
            var options = Parse(args);
            var client = Required(options, "--client");
            var decal = Required(options, "--decal");
            var injector = Required(options, "--injector");
            var account = Required(options, "--account");
            var password = Required(options, "--password");
            var host = Required(options, "--host");
            var port = Required(options, "--port");

            if (Environment.Is64BitProcess)
                throw new PlatformNotSupportedException("The Decal launch helper must run as a 32-bit process.");
            RequireFile(client, "Asheron's Call client");
            RequireFile(decal, "Decal Inject.dll");
            RequireFile(injector, "Thwarg injector.dll");

            var commandLine = BuildCommandLine(new[]
            {
                client, "-a", account, "-v", password, "-h", $"{host}:{port}"
            });
            var processId = LaunchWithInstalledThwargInjector(
                injector,
                commandLine,
                Path.GetDirectoryName(client)!,
                decal);
            if (processId <= 0)
                throw new InvalidOperationException("Thwarg's injector did not return a game process.");

            Console.Out.Write(JsonSerializer.Serialize(new
            {
                ProcessId = processId,
                DecalStartupInvoked = true
            }));
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.Write("The Decal startup path failed: " + Describe(ex));
            return 1;
        }
    }

    private static int LaunchWithInstalledThwargInjector(
        string injectorPath,
        string commandLine,
        string workingDirectory,
        string decalInjectPath)
    {
        var library = NativeLibrary.Load(injectorPath);
        try
        {
            var export = NativeLibrary.GetExport(library, "LaunchInjected");
            var launch = Marshal.GetDelegateForFunctionPointer<LaunchInjectedDelegate>(export);
            return launch(commandLine, workingDirectory, decalInjectPath, "DecalStartup");
        }
        finally
        {
            NativeLibrary.Free(library);
        }
    }

    private static void RequireFile(string path, string description)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"{description} is missing.", path);
    }

    private static string Describe(Exception exception)
    {
        if (exception is Win32Exception windowsException)
        {
            var systemMessage = new Win32Exception(windowsException.NativeErrorCode).Message;
            return $"{windowsException.Message} (Windows error {windowsException.NativeErrorCode}: {systemMessage})";
        }

        return exception.Message;
    }

    internal static string BuildCommandLine(IEnumerable<string> arguments) =>
        string.Join(" ", arguments.Select(QuoteArgument));

    internal static string QuoteArgument(string value)
    {
        if (value.Length > 0 && !value.Any(character => char.IsWhiteSpace(character) || character == '"'))
            return value;

        var result = new StringBuilder().Append('"');
        var backslashes = 0;
        foreach (var character in value)
        {
            if (character == '\\')
            {
                backslashes++;
                continue;
            }

            if (character == '"')
                result.Append('\\', backslashes * 2 + 1).Append('"');
            else
                result.Append('\\', backslashes).Append(character);
            backslashes = 0;
        }

        result.Append('\\', backslashes * 2).Append('"');
        return result.ToString();
    }

    private static Dictionary<string, string> Parse(string[] args)
    {
        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var index = 0; index < args.Length; index += 2)
        {
            if (index + 1 >= args.Length || !args[index].StartsWith("--", StringComparison.Ordinal))
                throw new ArgumentException("Invalid Decal host arguments.");
            values[args[index]] = args[index + 1];
        }
        return values;
    }

    private static string Required(IReadOnlyDictionary<string, string> values, string name) =>
        values.TryGetValue(name, out var value) && !string.IsNullOrWhiteSpace(value)
            ? value
            : throw new ArgumentException($"Missing {name}.");

    [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
    private delegate int LaunchInjectedDelegate(
        string commandLine,
        string workingDirectory,
        string injectDllPath,
        [MarshalAs(UnmanagedType.LPStr)] string initializeFunction);
}

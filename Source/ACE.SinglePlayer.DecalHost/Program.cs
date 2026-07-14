using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;

namespace ACE.SinglePlayer.DecalHost;

internal static class Program
{
    private const uint CreateSuspended = 0x00000004;
    private const uint MemCommit = 0x00001000;
    private const uint MemReserve = 0x00002000;
    private const uint MemRelease = 0x00008000;
    private const uint PageReadWrite = 0x04;
    private const uint Infinite = 0xFFFFFFFF;
    private const uint Th32csSnapModule = 0x00000008;
    private const uint Th32csSnapModule32 = 0x00000010;
    private const uint DontResolveDllReferences = 0x00000001;
    private static readonly IntPtr InvalidHandleValue = new(-1);

    public static int Main(string[] args)
    {
        ProcessInformation process = default;
        try
        {
            var options = Parse(args);
            var client = Required(options, "--client");
            var decal = Required(options, "--decal");
            var account = Required(options, "--account");
            var password = Required(options, "--password");
            var host = Required(options, "--host");
            var port = Required(options, "--port");

            if (!File.Exists(client) || !File.Exists(decal))
                throw new FileNotFoundException("A required client or Decal file is missing.");

            var commandLine = BuildCommandLine(new[]
            {
                client, "-a", account, "-v", password, "-h", $"{host}:{port}", "-rodat", "off"
            });
            var startup = new StartupInfo { Size = Marshal.SizeOf<StartupInfo>() };
            if (!CreateProcess(client, new StringBuilder(commandLine), IntPtr.Zero, IntPtr.Zero, false,
                    CreateSuspended, IntPtr.Zero, Path.GetDirectoryName(client), ref startup, out process))
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to create the game process.");

            InjectLibraryAndInvoke(process.Process, (uint)process.ProcessId, decal, "DecalStartup");
            if (ResumeThread(process.Thread) == uint.MaxValue)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to resume the game process.");

            Console.Out.Write(JsonSerializer.Serialize(new
            {
                ProcessId = process.ProcessId,
                DecalStartupInvoked = true
            }));
            return 0;
        }
        catch (Exception ex)
        {
            if (process.Process != IntPtr.Zero)
                TerminateProcess(process.Process, 1);
            Console.Error.Write("The Decal startup path failed: " + ex.Message);
            return 1;
        }
        finally
        {
            if (process.Thread != IntPtr.Zero)
                CloseHandle(process.Thread);
            if (process.Process != IntPtr.Zero)
                CloseHandle(process.Process);
        }
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

    private static void InjectLibraryAndInvoke(IntPtr process, uint processId, string libraryPath, string exportName)
    {
        var bytes = Encoding.Unicode.GetBytes(Path.GetFullPath(libraryPath) + '\0');
        var remoteMemory = VirtualAllocEx(process, IntPtr.Zero, (nuint)bytes.Length, MemReserve | MemCommit, PageReadWrite);
        if (remoteMemory == IntPtr.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to allocate injection memory.");

        try
        {
            if (!WriteProcessMemory(process, remoteMemory, bytes, (nuint)bytes.Length, out var written) || written != (nuint)bytes.Length)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to write the Decal library path.");

            var localKernel = GetModuleHandle("kernel32.dll");
            var localLoadLibrary = GetProcAddress(localKernel, "LoadLibraryW");
            var remoteKernel = FindRemoteModule(processId, "kernel32.dll");
            var remoteLoadLibrary = remoteKernel + checked((int)(localLoadLibrary.ToInt64() - localKernel.ToInt64()));
            var remoteLibrary = RunRemoteThread(process, remoteLoadLibrary, remoteMemory, "Unable to load Decal Inject.dll.");
            if (remoteLibrary == 0)
                throw new InvalidOperationException("Decal Inject.dll did not load in the game process.");

            var localLibrary = LoadLibraryEx(libraryPath, IntPtr.Zero, DontResolveDllReferences);
            if (localLibrary == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to inspect Decal Inject.dll.");
            try
            {
                var localExport = GetProcAddress(localLibrary, exportName);
                if (localExport == IntPtr.Zero)
                    throw new MissingMethodException($"Decal Inject.dll does not export {exportName}.");
                var exportOffset = checked((int)(localExport.ToInt64() - localLibrary.ToInt64()));
                var remoteExport = new IntPtr(unchecked((int)((long)remoteLibrary + exportOffset)));
                RunRemoteThread(process, remoteExport, IntPtr.Zero, $"Unable to invoke Decal {exportName}.");
            }
            finally
            {
                FreeLibrary(localLibrary);
            }
        }
        finally
        {
            VirtualFreeEx(process, remoteMemory, 0, MemRelease);
        }
    }

    private static uint RunRemoteThread(IntPtr process, IntPtr startAddress, IntPtr parameter, string error)
    {
        var thread = CreateRemoteThread(process, IntPtr.Zero, 0, startAddress, parameter, 0, out _);
        if (thread == IntPtr.Zero)
            throw new Win32Exception(Marshal.GetLastWin32Error(), error);
        try
        {
            if (WaitForSingleObject(thread, Infinite) == uint.MaxValue || !GetExitCodeThread(thread, out var exitCode))
                throw new Win32Exception(Marshal.GetLastWin32Error(), error);
            return exitCode;
        }
        finally
        {
            CloseHandle(thread);
        }
    }

    private static IntPtr FindRemoteModule(uint processId, string moduleName)
    {
        var snapshot = CreateToolhelp32Snapshot(Th32csSnapModule | Th32csSnapModule32, processId);
        if (snapshot == InvalidHandleValue)
            throw new Win32Exception(Marshal.GetLastWin32Error(), "Unable to inspect the game process modules.");
        try
        {
            var entry = new ModuleEntry32 { Size = (uint)Marshal.SizeOf<ModuleEntry32>() };
            if (Module32First(snapshot, ref entry))
            {
                do
                {
                    if (string.Equals(entry.Module, moduleName, StringComparison.OrdinalIgnoreCase))
                        return entry.ModuleBaseAddress;
                } while (Module32Next(snapshot, ref entry));
            }
        }
        finally
        {
            CloseHandle(snapshot);
        }
        throw new InvalidOperationException($"The game process did not load {moduleName}.");
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

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct StartupInfo
    {
        public int Size;
        public string? Reserved;
        public string? Desktop;
        public string? Title;
        public int X;
        public int Y;
        public int XSize;
        public int YSize;
        public int XCountChars;
        public int YCountChars;
        public int FillAttribute;
        public int Flags;
        public short ShowWindow;
        public short Reserved2;
        public IntPtr ReservedPointer;
        public IntPtr StandardInput;
        public IntPtr StandardOutput;
        public IntPtr StandardError;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct ProcessInformation
    {
        public IntPtr Process;
        public IntPtr Thread;
        public int ProcessId;
        public int ThreadId;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct ModuleEntry32
    {
        public uint Size;
        public uint ModuleId;
        public uint ProcessId;
        public uint GlobalUsage;
        public uint ProcessUsage;
        public IntPtr ModuleBaseAddress;
        public uint ModuleBaseSize;
        public IntPtr ModuleHandle;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)] public string Module;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)] public string ExePath;
    }

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CreateProcess(string applicationName, StringBuilder commandLine, IntPtr processAttributes,
        IntPtr threadAttributes, bool inheritHandles, uint creationFlags, IntPtr environment, string? currentDirectory,
        ref StartupInfo startupInfo, out ProcessInformation processInformation);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr VirtualAllocEx(IntPtr process, IntPtr address, nuint size, uint allocationType, uint protect);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool VirtualFreeEx(IntPtr process, IntPtr address, nuint size, uint freeType);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool WriteProcessMemory(IntPtr process, IntPtr address, byte[] buffer, nuint size, out nuint written);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr CreateRemoteThread(IntPtr process, IntPtr attributes, nuint stackSize,
        IntPtr startAddress, IntPtr parameter, uint creationFlags, out uint threadId);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern uint WaitForSingleObject(IntPtr handle, uint milliseconds);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetExitCodeThread(IntPtr thread, out uint exitCode);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr GetModuleHandle(string moduleName);

    [DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern IntPtr GetProcAddress(IntPtr module, string procedureName);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr LoadLibraryEx(string fileName, IntPtr file, uint flags);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool FreeLibrary(IntPtr module);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr CreateToolhelp32Snapshot(uint flags, uint processId);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool Module32First(IntPtr snapshot, ref ModuleEntry32 entry);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool Module32Next(IntPtr snapshot, ref ModuleEntry32 entry);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern uint ResumeThread(IntPtr thread);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseHandle(IntPtr handle);

    [DllImport("kernel32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool TerminateProcess(IntPtr process, uint exitCode);
}

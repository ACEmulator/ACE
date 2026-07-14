using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace ACE.SinglePlayer.Infrastructure;

public interface ISecretProtector
{
    string Protect(string value);
    string Unprotect(string protectedValue);
}

public sealed class SecretProtector : ISecretProtector
{
    private const int CryptprotectUiForbidden = 0x1;

    public string Protect(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        return Convert.ToBase64String(Transform(Encoding.UTF8.GetBytes(value), protect: true));
    }

    public string Unprotect(string protectedValue)
    {
        ArgumentNullException.ThrowIfNull(protectedValue);
        if (protectedValue.Length == 0)
            return string.Empty;

        return Encoding.UTF8.GetString(Transform(Convert.FromBase64String(protectedValue), protect: false));
    }

    public static string GeneratePassword()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    private static byte[] Transform(byte[] input, bool protect)
    {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("Windows DPAPI is required to protect launcher secrets.");

        var inputBlob = new DataBlob();
        var outputBlob = new DataBlob();
        try
        {
            inputBlob.Size = input.Length;
            inputBlob.Data = Marshal.AllocHGlobal(input.Length);
            Marshal.Copy(input, 0, inputBlob.Data, input.Length);

            var succeeded = protect
                ? CryptProtectData(ref inputBlob, null, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, CryptprotectUiForbidden, ref outputBlob)
                : CryptUnprotectData(ref inputBlob, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, CryptprotectUiForbidden, ref outputBlob);

            if (!succeeded)
                throw new Win32Exception(Marshal.GetLastWin32Error());

            var result = new byte[outputBlob.Size];
            Marshal.Copy(outputBlob.Data, result, 0, outputBlob.Size);
            return result;
        }
        finally
        {
            if (inputBlob.Data != IntPtr.Zero)
            {
                for (var i = 0; i < input.Length; i++)
                    Marshal.WriteByte(inputBlob.Data, i, 0);
                Marshal.FreeHGlobal(inputBlob.Data);
            }

            if (outputBlob.Data != IntPtr.Zero)
                LocalFree(outputBlob.Data);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct DataBlob
    {
        public int Size;
        public IntPtr Data;
    }

    [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CryptProtectData(ref DataBlob dataIn, string? description, IntPtr optionalEntropy,
        IntPtr reserved, IntPtr promptStruct, int flags, ref DataBlob dataOut);

    [DllImport("crypt32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CryptUnprotectData(ref DataBlob dataIn, IntPtr description, IntPtr optionalEntropy,
        IntPtr reserved, IntPtr promptStruct, int flags, ref DataBlob dataOut);

    [DllImport("kernel32.dll")]
    private static extern IntPtr LocalFree(IntPtr memory);
}

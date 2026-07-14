using System.Security.AccessControl;
using System.Security.Principal;

namespace ACE.SinglePlayer.Infrastructure;

public static class FilePermissionHardener
{
    public static void RestrictToCurrentUser(string path)
    {
        if (!OperatingSystem.IsWindows())
            return;

        var user = WindowsIdentity.GetCurrent().User
            ?? throw new InvalidOperationException("The current Windows user has no security identifier.");
        var security = new FileSecurity();
        security.SetOwner(user);
        security.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
        security.AddAccessRule(new FileSystemAccessRule(user, FileSystemRights.FullControl, AccessControlType.Allow));
        new FileInfo(path).SetAccessControl(security);
    }

    public static void RestrictDirectoryToCurrentUser(string path)
    {
        if (!OperatingSystem.IsWindows())
            return;

        var user = WindowsIdentity.GetCurrent().User
            ?? throw new InvalidOperationException("The current Windows user has no security identifier.");
        var security = new DirectorySecurity();
        security.SetOwner(user);
        security.SetAccessRuleProtection(isProtected: true, preserveInheritance: false);
        security.AddAccessRule(new FileSystemAccessRule(user, FileSystemRights.FullControl,
            InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit,
            PropagationFlags.None, AccessControlType.Allow));
        new DirectoryInfo(path).SetAccessControl(security);
    }
}

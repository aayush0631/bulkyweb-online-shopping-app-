using System.Runtime.InteropServices;

namespace FileSync.Services.Helper;

[StructLayout(LayoutKind.Sequential)]
internal class NETRESOURCE
{
    public int dwScope;
    public int dwType;
    public int dwDisplayType;
    public int dwUsage;

    public string lpLocalName = string.Empty;

    public string lpRemoteName = string.Empty;

    public string lpComment = string.Empty;

    public string lpProvider = string.Empty;
}
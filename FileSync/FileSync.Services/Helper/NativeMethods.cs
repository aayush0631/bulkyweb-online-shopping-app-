using System;
using System.Runtime.InteropServices;

namespace FileSync.Services.Helper
{
    /// <summary>
    /// Contains native Windows networking API methods (P/Invoke).
    /// These methods allow the application to connect to and disconnect
    /// from remote network shares (SMB) using Windows credentials.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Connects to a remote network share (e.g., \\Server\SharedFolder)
        /// using the specified username and password.
        ///
        /// This is a wrapper around the Windows API function
        /// WNetAddConnection2 found in mpr.dll.
        /// </summary>
        /// <param name="lpNetResource">
        /// Information about the network resource (remote path, resource type, etc.).
        /// </param>
        /// <param name="lpPassword">
        /// Password for the remote user account.
        /// </param>
        /// <param name="lpUsername">
        /// Username to authenticate with.
        /// Example: "Administrator" or "DESKTOP-ABC123\\Administrator"
        /// </param>
        /// <param name="dwFlags">
        /// Connection options (typically 0 for a temporary connection).
        /// </param>
        /// <returns>
        /// Returns 0 if the connection succeeds.
        /// Otherwise, returns a Windows error code.
        /// </returns>
        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        public static extern int WNetAddConnection2(
            NETRESOURCE lpNetResource,
            string lpPassword,
            string lpUsername,
            int dwFlags);

        /// <summary>
        /// Disconnects a previously established network connection.
        ///
        /// This is a wrapper around the Windows API function
        /// WNetCancelConnection2 found in mpr.dll.
        /// </summary>
        /// <param name="lpName">
        /// The remote network path to disconnect.
        /// Example: "\\\\192.168.1.100\\SharedFolder"
        /// </param>
        /// <param name="dwFlags">
        /// Disconnect options (typically 0).
        /// </param>
        /// <param name="ffroce">
        /// True to force the disconnection even if files are still open.
        /// False to disconnect only if the resource is not in use.
        /// </param>
        /// <returns>
        /// Returns 0 if the disconnection succeeds.
        /// Otherwise, returns a Windows error code.
        /// </returns>
        [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
        public static extern int WNetCancelConnection2(
            string lpName,
            int dwFlags,
            bool ffroce);
    }
}
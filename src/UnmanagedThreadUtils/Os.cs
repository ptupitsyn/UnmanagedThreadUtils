using System;

namespace UnmanagedThreadUtils
{
    /// <summary>
    /// Operating system / platform detector.
    /// </summary>
    internal static class Os
    {
        /// <summary>
        /// Initializes the <see cref="Os"/> class.
        /// </summary>
        static Os()
        {
            var platform = Environment.OSVersion.Platform;

            IsLinux = platform == PlatformID.Unix
                      || platform == PlatformID.MacOSX
                      || (int) Environment.OSVersion.Platform == 128;

            IsWindows = platform == PlatformID.Win32NT
                        || platform == PlatformID.Win32S
                        || platform == PlatformID.Win32Windows;

            IsMacOs = IsLinux && Shell.BashExecute("uname").Contains("Darwin");
        }

        /// <summary>
        /// Windows.
        /// </summary>
        public static bool IsWindows { get; }

        /// <summary>
        /// Linux.
        /// </summary>
        public static bool IsLinux { get; }

        /// <summary>
        /// MacOs.
        /// </summary>
        public static bool IsMacOs { get; }
    }
}

namespace UnmanagedThreadUtils
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// OS helper.
    /// </summary>
    internal static class Os
    {
        /// <summary>
        /// Initializes static members of the <see cref="Os"/> class.
        /// </summary>
        static Os()
        {
            IsLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            IsMacOs = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        /// <summary>
        /// Gets a value indicating whether current OS is Windows.
        /// </summary>
        public static bool IsWindows { get; }

        /// <summary>
        /// Gets a value indicating whether current OS is Linux.
        /// </summary>
        public static bool IsLinux { get; }

        /// <summary>
        /// Gets a value indicating whether current OS is macOS.
        /// </summary>
        public static bool IsMacOs { get; }
    }
}

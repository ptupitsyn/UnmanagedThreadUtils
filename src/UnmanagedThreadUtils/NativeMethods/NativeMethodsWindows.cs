// ReSharper disable InconsistentNaming
namespace UnmanagedThreadUtils.NativeMethods
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Native methods for Windows.
    /// </summary>
    internal static class NativeMethodsWindows
    {
        /// <summary>
        /// Indicates an error from <see cref="FlsAlloc"/>.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310", Justification = "Reviewed.")]
        public const int FLS_OUT_OF_INDEXES = -1;

        /// <summary>
        /// Allocates a fiber local storage (FLS) index.
        /// </summary>
        /// <param name="destructorCallback">A pointer to the application-defined callback function.</param>
        /// <returns>If the function succeeds, the return value is an FLS index initialized to zero.
        /// If the function fails, the return value is FLS_OUT_OF_INDEXES.</returns>
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int FlsAlloc(IntPtr destructorCallback);

        /// <summary>
        /// Releases a fiber local storage (FLS) index, making it available for reuse.
        /// </summary>
        /// <param name="dwFlsIndex">The FLS index that was allocated by the <see cref="FlsAlloc"/> function.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.</returns>
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FlsFree(int dwFlsIndex);

        /// <summary>
        /// Stores a value in the calling fiber's fiber local storage (FLS) slot for the specified FLS index.
        /// </summary>
        /// <param name="dwFlsIndex">The FLS index that was allocated by the <see cref="FlsAlloc"/> function.</param>
        /// <param name="lpFlsData">The value to be stored in the FLS slot for the calling fiber.</param>
        /// <returns>If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero.</returns>
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FlsSetValue(int dwFlsIndex, IntPtr lpFlsData);
    }
}

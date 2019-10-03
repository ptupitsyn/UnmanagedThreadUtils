namespace UnmanagedThreadUtils.NativeMethods
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    internal static class NativeMethodsWindows
    {
        // ReSharper disable once InconsistentNaming
        public const int FLS_OUT_OF_INDEXES = -1;

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern int FlsAlloc(IntPtr destructorCallback);

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FlsFree(int dwFlsIndex);

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FlsSetValue(int dwFlsIndex, IntPtr lpFlsData);
    }
}

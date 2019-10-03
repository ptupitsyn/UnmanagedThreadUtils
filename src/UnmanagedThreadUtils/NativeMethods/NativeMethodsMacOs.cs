namespace UnmanagedThreadUtils.NativeMethods
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Native methods for MacOS.
    /// </summary>
    internal static class NativeMethodsMacOs
    {
        /// <summary>
        /// Create a thread-specific data key visible to all threads in the process.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="destructorCallback">Destructor callback pointer.</param>
        /// <returns>Zero when successful; error code otherwise.</returns>
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]        [DllImport("libSystem.dylib")]
        public static extern int pthread_key_create(IntPtr key, IntPtr destructorCallback);

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]        [DllImport("libSystem.dylib")]
        public static extern int pthread_key_delete(int key);

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]        [DllImport("libSystem.dylib")]
        public static extern int pthread_setspecific(int key, IntPtr value);
    }
}

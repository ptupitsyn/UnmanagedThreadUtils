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
        /// Creates a thread-specific data key visible to all threads in the process.
        /// </summary>
        /// <param name="key">Key.</param>
        /// <param name="destructorCallback">Destructor callback pointer.</param>
        /// <returns>Zero when successful; error code otherwise.</returns>
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        [DllImport("libSystem.dylib")]
        public static extern int pthread_key_create(IntPtr key, IntPtr destructorCallback);

        /// <summary>
        /// Deletes a thread-specific data key previously returned by <see cref="pthread_key_create"/>.
        /// </summary>
        /// <param name="key">Key to delete.</param>
        /// <returns>Zero when successful; error code otherwise.</returns>
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        [DllImport("libSystem.dylib")]
        public static extern int pthread_key_delete(int key);

        /// <summary>
        /// Associates a thread-specific value with a key obtained via a previous call to <see cref="pthread_key_create" />.
        /// </summary>
        /// <param name="key">Key to associate a value with.</param>
        /// <param name="value">Value to associate with the key.</param>
        /// <returns>Zero when successful; error code otherwise.</returns>
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300:ElementMustBeginWithUpperCaseLetter", Justification = "Reviewed.")]
        [DllImport("libSystem.dylib")]
        public static extern int pthread_setspecific(int key, IntPtr value);
    }
}

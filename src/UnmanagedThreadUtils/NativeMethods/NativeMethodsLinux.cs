namespace UnmanagedThreadUtils.NativeMethods
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;

    internal class NativeMethodsLinux
    {
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [DllImport("libcoreclr.so")]
        public static extern int pthread_key_create(IntPtr key, IntPtr destructorCallback);

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [DllImport("libcoreclr.so")]
        public static extern int pthread_key_delete(int key);

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [DllImport("libcoreclr.so")]
        public static extern int pthread_setspecific(int key, IntPtr value);

        public static void CheckResult(int res)
        {
            if (res != 0)
            {
                throw new InvalidOperationException("Native call failed: " + res);
            }
        }
    }
}

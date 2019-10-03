using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace UnmanagedThreadUtils.NativeMethods
{
    public class NativeMethodsMacOs
    {
        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [DllImport("libSystem.dylib")]
        public static extern int pthread_key_create(IntPtr key, IntPtr destructorCallback);

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [DllImport("libSystem.dylib")]
        public static extern int pthread_key_delete(int key);

        [SuppressMessage("Microsoft.Design", "CA1060:MovePInvokesToNativeMethodsClass", Justification = "Reviewed.")]
        [DllImport("libSystem.dylib")]
        public static extern int pthread_setspecific(int key, IntPtr value);
    }
}

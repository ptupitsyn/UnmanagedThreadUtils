namespace UnmanagedThreadUtils
{
    using System;
    using System.Runtime.InteropServices;
    using NativeMethods;

    /// <summary>
    /// Unmanaged thread utils.
    /// </summary>
    public static class UnmanagedThread
    {
        /// <summary>
        /// Thread exit callback delegate.
        /// </summary>
        /// <param name="threadLocalValue">Value from <see cref="EnableCurrentThreadExitEvent"/>.</param>
        public delegate void ThreadExitCallback(IntPtr threadLocalValue);

        /// <summary>
        /// Sets the thread exit callback, and returns an id to pass to <see cref="EnableCurrentThreadExitEvent"/>.
        /// </summary>
        /// <param name="callback">
        /// Callback delegate.
        /// </param>
        /// <returns>Callback ID.</returns>
        public static int SetThreadExitCallback(ThreadExitCallback callback)
        {
            var handle = GCHandle.Alloc(callback);

            return SetThreadExitCallback(GCHandle.ToIntPtr(handle));
        }

        /// <summary>
        /// Sets the thread exit callback, and returns an id to pass to <see cref="EnableCurrentThreadExitEvent"/>.
        /// </summary>
        /// <param name="callbackPtr">
        /// Pointer to a callback function that matches <see cref="ThreadExitCallback"/>.
        /// </param>
        /// <returns>Callback ID.</returns>
        public static unsafe int SetThreadExitCallback(IntPtr callbackPtr)
        {
            if (callbackPtr == IntPtr.Zero)
            {
                throw new ArgumentException("Should not be Zero", nameof(callbackPtr));
            }

            if (Os.IsWindows)
            {
                var res = NativeMethodsWindows.FlsAlloc(callbackPtr);

                if (res == NativeMethodsWindows.FLS_OUT_OF_INDEXES)
                {
                    throw new InvalidOperationException("FlsAlloc failed: " + Marshal.GetLastWin32Error());
                }

                return res;
            }

            if (Os.IsMacOs)
            {
                int tlsIndex;
                var res = NativeMethodsMacOs.pthread_key_create(new IntPtr(&tlsIndex), callbackPtr);

                NativeMethodsLinux.CheckResult(res);

                return tlsIndex;
            }

            if (Os.IsLinux)
            {
                int tlsIndex;
                var res = NativeMethodsLinux.pthread_key_create(new IntPtr(&tlsIndex), callbackPtr);

                NativeMethodsLinux.CheckResult(res);

                return tlsIndex;
            }

            throw new InvalidOperationException("Unsupported OS: " + Environment.OSVersion);
        }

        /// <summary>
        /// Removes thread exit callback that has been set with <see cref="SetThreadExitCallback"/>.
        /// NOTE: callback may be called as a result of this method call on some platforms.
        /// </summary>
        /// <param name="callbackId">Callback id returned from <see cref="SetThreadExitCallback"/>.</param>
        public static void RemoveThreadExitCallback(int callbackId)
        {
            if (Os.IsWindows)
            {
                var res = NativeMethodsWindows.FlsFree(callbackId);

                if (!res)
                {
                    throw new InvalidOperationException("FlsFree failed: " + Marshal.GetLastWin32Error());
                }
            }
            else if (Os.IsMacOs)
            {
                var res = NativeMethodsMacOs.pthread_key_delete(callbackId);
                NativeMethodsLinux.CheckResult(res);
            }
            else if (Os.IsLinux)
            {
                var res = NativeMethodsLinux.pthread_key_delete(callbackId);
                NativeMethodsLinux.CheckResult(res);
            }
            else
            {
                throw new InvalidOperationException("Unsupported OS: " + Environment.OSVersion);
            }
        }

        /// <summary>
        /// Enables thread exit event for current thread.
        /// </summary>
        /// <param name="callbackId">Callback ID from <see cref="SetThreadExitCallback"/>.</param>
        /// <param name="threadLocalValue">Thread-local value to be passed to the callback.</param>
        public static void EnableCurrentThreadExitEvent(int callbackId, IntPtr threadLocalValue)
        {
            if (threadLocalValue == IntPtr.Zero)
            {
                throw new ArgumentException("Should not be Zero", nameof(threadLocalValue));
            }

            // Store any value so that destructor callback is fired.
            if (Os.IsWindows)
            {
                var res = NativeMethodsWindows.FlsSetValue(callbackId, threadLocalValue);

                if (!res)
                {
                    throw new InvalidOperationException("FlsSetValue failed: " + Marshal.GetLastWin32Error());
                }
            }
            else if (Os.IsMacOs)
            {
                var res = NativeMethodsMacOs.pthread_setspecific(callbackId, threadLocalValue);
                NativeMethodsLinux.CheckResult(res);
            }
            else if (Os.IsLinux)
            {
                var res = NativeMethodsLinux.pthread_setspecific(callbackId, threadLocalValue);
                NativeMethodsLinux.CheckResult(res);
            }
            else
            {
                throw new InvalidOperationException("Unsupported OS: " + Environment.OSVersion);
            }
        }
    }
}

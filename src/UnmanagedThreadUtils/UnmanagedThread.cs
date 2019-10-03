using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnmanagedThreadUtils.NativeMethods;

namespace UnmanagedThreadUtils
{
    public class UnmanagedThread
    {
        /// <summary>
        /// Delegate for <see cref="SetThreadExitCallback"/>.
        /// </summary>
        /// <param name="threadLocalValue">Value from <see cref="EnableCurrentThreadExitEvent"/></param>
        public delegate void ThreadExitCallback(IntPtr threadLocalValue);

        /// <summary>
        /// Sets the thread exit callback, and returns an id to pass to <see cref="EnableCurrentThreadExitEvent"/>.
        /// </summary>
        /// <param name="callbackPtr">
        /// Pointer to a callback function that matches <see cref="ThreadExitCallback"/>.
        /// </param>
        public static unsafe int SetThreadExitCallback(IntPtr callbackPtr)
        {
            Debug.Assert(callbackPtr != IntPtr.Zero);

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
        public static void EnableCurrentThreadExitEvent(int callbackId, IntPtr threadLocalValue)
        {
            Debug.Assert(threadLocalValue != IntPtr.Zero);

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

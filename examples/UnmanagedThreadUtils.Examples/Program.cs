namespace UnmanagedThreadUtils.Examples
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;

    /// <summary>
    /// Example.
    /// </summary>
    public static class Program
    {
        private const int ThreadCount = 3;

        // Use static field to make sure that delegate is alive.
        private static readonly UnmanagedThread.ThreadExitCallback ThreadExitCallbackDelegate = OnThreadExit;

        private static readonly CountdownEvent UnmanagedThreadsExitEvent = new CountdownEvent(ThreadCount);

        /// <summary>
        /// Entry point.
        /// </summary>
        public static void Main()
        {
            var threadExitCallbackDelegatePtr = Marshal.GetFunctionPointerForDelegate(ThreadExitCallbackDelegate);
            var callbackId = UnmanagedThread.SetThreadExitCallback(threadExitCallbackDelegatePtr);

            for (var i = 1; i <= ThreadCount; i++)
            {
                var threadLocalVal = i;

                var thread = new Thread(_ =>
                {
                    Console.WriteLine($"Managed thread #{threadLocalVal} started.");
                    UnmanagedThread.EnableCurrentThreadExitEvent(callbackId, new IntPtr(threadLocalVal));
                    Thread.Sleep(100);
                    Console.WriteLine($"Managed thread #{threadLocalVal} ended.");
                });

                thread.Start();
            }

            UnmanagedThreadsExitEvent.Wait();
            Console.WriteLine("All unmanaged threads have exited.");

            UnmanagedThread.RemoveThreadExitCallback(callbackId);
        }

        private static void OnThreadExit(IntPtr data)
        {
            Console.WriteLine($"Unmanaged thread #{data.ToInt64()} is exiting.");
            UnmanagedThreadsExitEvent.Signal();
        }
    }
}

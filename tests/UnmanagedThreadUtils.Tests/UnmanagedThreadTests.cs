namespace UnmanagedThreadUtils.Tests
{
    using System;
    using System.Runtime.InteropServices;
    using System.Threading;
    using NUnit.Framework;

    /// <summary>
    /// Tests for <see cref="UnmanagedThread"/> class.
    /// </summary>
    public class UnmanagedThreadTests
    {
        /// <summary>
        /// Tests that ThreadExit event fires when enabled
        /// with <see cref="UnmanagedThread.EnableCurrentThreadExitEvent"/>.
        /// </summary>
        /// <param name="enableThreadExitCallback">Whether to enable thread exit callback.</param>
        [Test]
        public void TestThreadExitFiresWhenEnabled([Values(true, false)] bool enableThreadExitCallback)
        {
            using (var evt = new ManualResetEventSlim())
            {
                var threadLocalVal = new IntPtr(42);
                var resultThreadLocalVal = IntPtr.Zero;

                UnmanagedThread.ThreadExitCallback callback = val =>
                {
                    // ReSharper disable once AccessToDisposedClosure
                    evt.Set();
                    resultThreadLocalVal = val;
                };

                GC.KeepAlive(callback);
                var callbackId = UnmanagedThread.SetThreadExitCallback(Marshal.GetFunctionPointerForDelegate(callback));

                try
                {
                    ParameterizedThreadStart threadStart = _ =>
                    {
                        if (enableThreadExitCallback)
                        {
                            UnmanagedThread.EnableCurrentThreadExitEvent(callbackId, threadLocalVal);
                        }
                    };

                    var t = new Thread(threadStart);

                    t.Start();
                    t.Join();

                    var threadExitCallbackCalled = evt.Wait(TimeSpan.FromSeconds(1));

                    Assert.AreEqual(enableThreadExitCallback, threadExitCallbackCalled);
                    Assert.AreEqual(enableThreadExitCallback ? threadLocalVal : IntPtr.Zero, resultThreadLocalVal);
                }
                finally
                {
                    UnmanagedThread.RemoveThreadExitCallback(callbackId);
                }
            }
        }

        /// <summary>
        /// Tests that invalid callback id causes and exception.
        /// </summary>
        [Test]
        public void TestInvalidCallbackIdThrowsException()
        {
            Assert.Throws<InvalidOperationException>(() =>
                UnmanagedThread.EnableCurrentThreadExitEvent(int.MaxValue, new IntPtr(1)));

            Assert.Throws<InvalidOperationException>(() =>
                UnmanagedThread.RemoveThreadExitCallback(int.MaxValue));
        }
    }
}

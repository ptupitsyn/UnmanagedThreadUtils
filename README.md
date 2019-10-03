# Unmanaged Thread Utils
Unmanaged thread utils for .NET

[![Build Status](https://travis-ci.com/ptupitsyn/UnmanagedThreadUtils.svg?branch=master)](https://travis-ci.com/ptupitsyn/UnmanagedThreadUtils)
![Nuget](https://img.shields.io/nuget/v/UnmanagedThreadUtils)

## Thread Exit Callback

* Provides a way to subscribe to a Thread Exit event 
* The event is fired on the Thread that is about to exit 
* Cross-platform (Windows, Linux, MacOS)

**Example:**
```csharp
// Use static field to make sure that delegate is alive.
private static readonly UnmanagedThread.ThreadExitCallback ThreadExitCallbackDelegate = OnThreadExit;

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
        });

        thread.Start();
    }

    UnmanagedThread.RemoveThreadExitCallback(callbackId);
}

private static void OnThreadExit(IntPtr data)
{
    Console.WriteLine($"Unmanaged thread #{data.ToInt64()} is exiting.");
}
```

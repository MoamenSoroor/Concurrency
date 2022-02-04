# Concurrency in .Net
This is my practices on all forms of Concurrency and Parallel Progamming in .Net with C#
based on the following resources: 
* <a href="https://www.amazon.com/Concurrency-Cookbook-Asynchronous-Multithreaded-Programming/dp/149205450X" target="_blank" rel="noopener noreferrer">Concurrency in C# Cookbook, 2nd Edition </a>
* <a href="https://www.apress.com/gp/book/9781484230183" target="_blank" rel="noopener noreferrer">Pro C# 7 With .NET and .NET Core (Authors: Troelsen, Andrew, Japikse, Philip) </a>
* <a href="https://www.amazon.com/C-7-0-Nutshell-Definitive-Reference/dp/1491987650" target="_blank" rel="noopener noreferrer">C# 7.0 in a Nutshell: The Definitive Reference by Joseph Albahari (Author), Ben Albahari (Author) </a> 
* <a href="https://app.pluralsight.com/library/courses/applying-asynchronous-programming-c-sharp " target="_blank" rel="noopener noreferrer">applying-asynchronous-programming-c-sharp</a>
* <a href="https://app.pluralsight.com/library/courses/getting-started-with-asynchronous-programming-dotnet/table-of-contents" target="_blank" rel="noopener noreferrer">getting-started-with-asynchronous-programming-dotnet</a> 

## Convered Topics in Details:
-------------------------------------------------------

## Low Level Threading
  - I/O-bound versus compute-bound
  - I/O-bound operations - synchronously And asynchronously waits
  - Blocking versus spinning
  - Create a Thread
  - Sleep And Yield
  - Join and Sleep
  - Join With Timeout
  - Blocking, and ThreadState property, and IsAlive Property
  - Local Versus Shared State: Local State Case
  - Local Versus Shared State: Instance Case 
  - Local Versus Shared State: Lamda Expression and Delegate Case
  - Local Versus Shared State: Static Case 
  - Race Conditions: Lack Of Thread Safety In Shared State
  - Locking and Thread Safety
  - Passing Data to a Thread via Lamda Expression
  - Passing Data to a Thread Start method via ParameterizedThreadStart Delegate
  - Lambda expressions and captured variables
  - Exception Handling
  - Foreground Versus Background Threads
  - Thread Priority
  - Signaling
  - Threading in Rich-Client Applications
  - Synchronization Contexts
  - The Thread Pool
  - Hygiene in the thread pool
  
## Asynchronous Programming
  - Limitations of Threads
  - Tasks
  - Starting a Task
  - Wait a Task
  - Long - running tasks
  - Task Return Value
  - Exceptions
  - Exceptions and autonomous tasks
  - Continuations With TaskAwaiter<TResult>
  - Continuation With TaskAwaiter<TResult> and ConfigureAwait Method
  - Continuations With ContinueWith Method
  - Success and Failure in Continuation With ContinueWith Method
  - CancelationTokenSource and CancellationToken
  - Continuation with WhenAll
  - Continuation with WhenAll and WhenAny
  - Task.Factory.StartNew() Method
  - Task.Factory.StartNew() Child Tasks Attachment
  - Differences  Between Task.Run and Task.Factory.StartNew() Method 
  - Implementing Progress reporting with IProgress and Progress
  - TaskCompletionSource
  - Create alike Task.Run Method With TaskCompletionSource
  - The Real Power of the TaskCompletionSource
  - Delay With TaskCompletionSource
  
## Asynchronous Programming
  - Principles of Asynchrony
  - Asynchronous Functions
  - Awaiting a nongeneric task
  - Capturing local state
  - Building Asynchronous Functions
  - Returning Task<TResult>
  - the basic principle of how to design with asynchronous functions in C#
  - Asynchronous call graph execution
  - Parallelism
  - Asynchronous Lambda Expressions
  - Asynchronous Streams IAsyncEnumerator and IAsyncEnumerable
  - Asynchronous Streams: Querying IAsyncEnumerable<T>

## Parallel Programming
  - Parallel.Invoke
  - Parallel.Invoke : Case Of Exceptions
  - Parallel.ForEach Method
  - Parallel.ForEach: Break and Stop Methods
  - Parallel Aggregation
  

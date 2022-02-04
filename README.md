# Concurrency
explore all forms of Concurrency and Parallel Progamming in .Net with C#

Convered Topics: 
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
  
  

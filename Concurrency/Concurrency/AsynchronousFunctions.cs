﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{

    #region Principles of Asynchrony
    // ------------------------ Principles of Asynchrony -------------------------

    // Synchronous Versus Asynchronous Operations
    // ------------------------------------------
    // A synchronous operation does its work before returning to the caller.
    //
    // An asynchronous operation does(most or all of) its work after returning to the
    // caller.
    // 
    // Asynchronous methods are less common, and initiate concurrency, because 
    // work continues in parallel to the caller.Asynchronous methods typically return 
    // quickly (or immediately) to the caller; hence they are also called nonblocking methods.
    // 
    // Most of the asynchronous methods that we’ve seen so far can be described as
    // general-purpose methods:
    // • Thread.Start
    // • Task.Run
    // • Methods that attach continuations to tasks


    public class AsynchronousProgramming
    {
        // Test Method
        public static void Test()
        {
            for (int i = 0; i < 10; i++)
            {
                int temp = i;   // to avoid capturing
                var awaiter = GetPrimesCountAsync(i * 1000000 + 2, 1000000).GetAwaiter();
                awaiter.OnCompleted(() =>
                        Console.WriteLine(awaiter.GetResult() + " primes between... "
                        + (temp * 1000000) + " and " + ((temp + 1) * 1000000 - 1)));

            }
            Console.WriteLine("Done");

        }
        static Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
                ParallelEnumerable.Range(start, count).Count(n =>
                    Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }

    }

    // --------------------------------------------------------------
    #endregion

    #region Asynchronous Functions
    // ------------------------ Asynchronous Functions -------------------------
    // Methods with the async modifier are called asynchronous functions
    // 
    // Awaiting
    // --------
    // The await keyword simplifies the attaching of continuations.Starting with a basic
    // scenario, the compiler expands:
    // 
    //  var result = await expression;
    //  statement(s);
    // 
    //  into something functionally similar to:
    // 
    //  var awaiter = expression.GetAwaiter();
    //  awaiter.OnCompleted(() =>
    //  {
    //      var result = awaiter.GetResult();
    //      statement(s);
    //  });
    //
    // how execution proceeds through an asynchronous function
    // -------------------------------------------------------
    // Upon encountering an await expression, execution (normally) returns to the caller
    // .But before returning, the runtime attaches a continuation to the awaited task, 
    // ensuring that when the task completes, execution jumps back into the method and continues 
    // where it left off. 
    // If the task faults, its exception is re-thrown, otherwise its return value 
    // is assigned to the await expression.
    // 
    // 
    // async modifier
    // --------------
    // The async modifier tells the compiler to treat await as a keyword rather than an
    // identifier should an ambiguity arise within that method(this ensures that code
    // written prior to C# 5 that might use await as an identifier will still compile without
    // error). The async modifier can be applied only to methods(and lambda expres‐
    // sions) that return void or(as we’ll see later) a Task or Task<TResult>.

    // The async modifier is similar to the unsafe modifier in that it
    // has no effect on a method’s signature or public metadata; it
    // affects only what happens inside the method.For this reason,
    // it makes no sense to use async in an interface. However it is
    // legal, for instance, to introduce async when overriding a nonasync 
    // virtual method, as long as you keep the signature the same.

    // Methods with the async modifier are called asynchronous functions

    // awaitable object 
    // ===============================================================
    // 
    //      int result = await GetPrimesCountAsync(2, 1000_000);
    //      Console.WriteLine("result: " + result);
    // 
    // The expression upon which you await is typically a task; however, any object with a
    // GetAwaiter method that returns an awaitable object (implementing INotifyComple
    // tion.OnCompleted and with an appropriately typed GetResult method and a bool
    // IsCompleted property) will satisfy the compiler.
    // -------------------------------------------------------------------

    public class AsynchronousFunctions
    {
        // Test Method
        // async is applied only to void , Task, and Task<TResult>
        public static async void Test()
        {
            // when execution reachs awaits , control get back to who called the method.
            // and the next to await will execute when awaited task finish.
            int result = await GetPrimesCountAsync(2, 1000_000);
            Console.WriteLine("result: " + result);
        }

        // We can summarize everything we just said by looking at the logical
        // expansion of the preceding asynchronous method:
        void DisplayPrimesCount()
        {
            var awaiter = GetPrimesCountAsync(2, 1000000).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                int result = awaiter.GetResult();
                Console.WriteLine(result);
            });
        }

        static Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
                ParallelEnumerable.Range(start, count).Count(n =>
                    Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }


    }

    // --------------------------------------------------------------
    #endregion

    #region Awaiting a nongeneric task
    // ------------------------ Awaiting a nongeneric task -------------------------

    public class AwaitingNongenericTask
    {
        // Test Method
        public static async Task Test()
        {
            await Task.Delay(3000);
            Console.WriteLine("Three Seconds Passed!");

        }

        public static async Task Test2()
        {
            for (int i = 0; i < 5; i++)
            {
                await Task.Delay(2000);
                Console.WriteLine($"{i}- Three Seconds Passed!");
            }
            Console.WriteLine("Done!");


        }

        public static void CallTest2()
        {
            Test2().ContinueWith(t => Console.WriteLine("Second Done"));
            Console.WriteLine("Second Done!");


        }

        public static void Test3()
        {
            for (int i = 0; i < 5; i++)
            {
                Task.Delay(2000).GetAwaiter().OnCompleted(
                    () => Console.WriteLine($"{i}- Three Seconds Passed!"));


            }
            Console.WriteLine("Done!");


        }
    }

    // --------------------------------------------------------------
    #endregion

    #region Capturing local state
    // ------------------------ Capturing local state -------------------------
    // The real power of await expressions is that they can appear almost anywhere in
    // code.Specifically, an await expression can appear in place of any expression
    // (within an asynchronous function) except for inside:
    // 1- a lock expression
    // 2- unsafe context
    // 3- an executable’s entry point(main method).
    public class CapturingLocalState
    {
        // Test Method
        public static async void Test()
        {
            for (int i = 0; i < 10; i++)
                Console.WriteLine(await GetPrimesCountAsync(i * 1000000 + 2, 1000000));

        }

        static Task<int> GetPrimesCountAsync(int start, int count)
        {
            return Task.Run(() =>
                ParallelEnumerable.Range(start, count).Count(n =>
                    Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0)));
        }
    }

    // --------------------------------------------------------------
    #endregion

    #region Building Asynchronous Functions
    // ------------------------ Building Asynchronous Functions -------------------------
    // With any asynchronous function, you can replace the void return type with a Task
    // to make the method itself usefully asynchronous(and awaitable). No further
    // changes are required.
    // This makes it easy to create asynchronous call chains.
    // 
    // Hence, whenever a task-returning asynchronous method finishes, execution jumps
    // back to whoever awaited it

    public class BuildingAsyncFunctions
    {
        public static void Test()
        {
            Go().ContinueWith(d => Console.WriteLine("Done!"));
        }

        public static async Task Go()
        {
            Console.WriteLine("Go");
            await PrintAnswerToLife();
        }


        // Notice that we don’t explicitly return a task in the method body.The compiler 
        // manufactures the task, which it signals upon completion of the method(or upon an
        // unhandled exception). This makes it easy to create asynchronous call chains
        public static async Task PrintAnswerToLife()
        {
            await Task.Delay(5000);
            int answer = 21 * 2;
            Console.WriteLine(answer);
        }

        // we can expand PrintAnswerToLife into the following functional equivalent
        public static Task PrintAnswerToLifeExapansion()
        {
            var tcs = new TaskCompletionSource<object>();
            var awaiter = Task.Delay(5000).GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                try
                {
                    awaiter.GetResult(); // Re-throw any exceptions
                    int answer = 21 * 2;
                    Console.WriteLine(answer);
                    tcs.SetResult(null);
                }
                catch (Exception ex) { tcs.SetException(ex); }
            });
            return tcs.Task;
        }



        // Returning Task<TResult>


    }

    // --------------------------------------------------------------
    #endregion

    #region Returning Task<TResult>
    // ------------------------ Returning Task<TResult> -------------------------

    public class AsyncronousMethodChains
    {
        // Test Method
        public static void Test()
        {
            Go();

        }

        public static async Task Go()
        {
            await PrintAnswerToLife();
            Console.WriteLine("Done!");
        }


        public static async Task PrintAnswerToLife()
        {
            int result = await GetAnswerToLife();
            Console.WriteLine(result);
        }

        public static async Task<int> GetAnswerToLife()
        {
            await Task.Delay(5000);
            int answer = 21 * 2;
            return answer; // Method has return type Task<int> we return int
        }


    }


    public class SyncronousMethodChains
    {
        // Test Method
        public static void Test()
        {
            Go();

        }

        public static void Go()
        {
            PrintAnswerToLife();
            Console.WriteLine("Done!");
        }


        public static void PrintAnswerToLife()
        {
            int result = GetAnswerToLife();
            Console.WriteLine(result);
        }

        public static int GetAnswerToLife()
        {
            Thread.Sleep(5000);
            int answer = 21 * 2;
            return answer; // Method has return type Task<int> we return int
        }


    }
    // --------------------------------------------------------------
    #endregion

    #region  the basic principle of how to design with asynchronous functions in C#
    // ------------------------ the basic principle of how to design with asynchronous functions in C# -------------------------
    // This also illustrates the basic principle of how to design with
    // asynchronous functions in C#:
    // 
    // 1. Write your methods synchronously.
    // 
    // 2. Replace synchronous method calls with asynchronous
    //    method calls, and await them.
    // 
    // 3. Except for “top-level” methods (typically event handlers
    //    for UI controls), upgrade your asynchronous methods’
    //    return types to Task or Task<TResult> so that they’re
    //    awaitable.

    // The compiler’s ability to manufacture tasks for asynchronous functions means that
    // for the most part, you need to explicitly instantiate a TaskCompletionSource only in
    // bottom-level methods that initiate I/O-bound concurrency. (And for methods that
    // initiate compute-bound currency, you create the task with Task.Run.



    // --------------------------------------------------------------
    #endregion

    #region Asynchronous call graph execution
    // ------------------------ Asynchronous call graph execution -------------------------
    // To see exactly how this executes, it’s helpful to rearrange our code as follows:
    public class AsynchronousCallGraph
    {
        // Test Method
        static async Task Test()
        {
            var task = PrintAnswerToLife();
            await task;
            Console.WriteLine("Done");
        }
        static async Task PrintAnswerToLife()
        {
            var task = GetAnswerToLife();
            int answer = await task;
            Console.WriteLine(answer);
        }
        static async Task<int> GetAnswerToLife()
        {
            var task = Task.Delay(5000);
            await task;
            int answer = 21 * 2;
            return answer;
        }


    }

    // --------------------------------------------------------------
    #endregion

    #region Parallelism
    // ------------------------ Parallelism -------------------------
    // Calling an asynchronous method without awaiting it allows the code that follows to
    // execute in parallel.
    public class Parallelism
    {
        // Test Method
        public static void Test()
        {
            Go().ContinueWith(_ => _);   // parallelism

        }

        // run two asynchronous operations in parallel
        // By awaiting both operations afterward, we “end” the parallelism at that point.
        // Later, we’ll describe how the WhenAll task combinator helps with this pattern.
        static async Task Go()
        {
            var task1 = PrintAnswerToLife();
            var task2 = PrintAnswerToLife();
            await task1;
            await task2;
            Console.WriteLine("Done");
        }
        static async Task PrintAnswerToLife()
        {
            var task = GetAnswerToLife();
            int answer = await task;
            Console.WriteLine(answer);
        }
        static async Task<int> GetAnswerToLife()
        {
            var task = Task.Delay(5000);
            await task;
            int answer = 21 * 2;
            return answer;
        }


    }

    // --------------------------------------------------------------
    #endregion

    #region Asynchronous Lambda Expressions
    // ------------------------ Asynchronous Lambda Expressions -------------------------

    public class AsynchronousLambdaExpressions
    {
        // Test Method
        public static async void Test()
        {
            Func<Task> unnamed = async () =>
            {
                await Task.Delay(1000);
                Console.WriteLine("Foo");
            };

            await unnamed();
            await NamedMethod();



        }



        static async Task NamedMethod()
        {
            await Task.Delay(1000);
            Console.WriteLine("Foo");
        }

    }

    // --------------------------------------------------------------
    #endregion

    

    #region Asynchronous Streams IAsyncEnumerator and IAsyncEnumerable

    // To generate an asynchronous stream, you write a method that combines
    // the principles of iterators and asynchronous methods.In other words,
    // your method should include both yield return and await, and it should
    // return IAsyncEnumerable<T>

    // NOTE That IAsyncEnumerable<int> is different from Task<IEnumerable<int>>

    //public interface IAsyncEnumerable<out T>
    //{
    //    //
    //    // Summary:
    //    //     Returns an enumerator that iterates asynchronously through the collection.
    //    IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default);
    //}


    //public interface IAsyncEnumerator<out T> : IAsyncDisposable
    //{
    //    //
    //    // Summary:
    //    //     Gets the element in the collection at the current position of the enumerator.

    //    T Current { get; }

    //    //
    //    // Summary:
    //    //     Advances the enumerator asynchronously to the next element of the collection.
    //    ValueTask<bool> MoveNextAsync();
    //}

    public class TestAsyncEnumerator
    {

        public static async Task Test()
        {
            await foreach (var item in RangeAsync(10, 20, 200))
            {
                Console.WriteLine(item);
            }
        }

        private static async IAsyncEnumerable<int> RangeAsync(int start, int count, int delay)
        {

            for (int i = start; i < start + count; i++)
            {
                await Task.Delay(delay);
                yield return i;
            }
        }






    }

    #endregion


    #region Asynchronous Streams: Querying IAsyncEnumerable<T>


    public class TestQueryingAsyncEnumerator
    {

        public static async Task Test()
        {

            //IAsyncEnumerable<int> query =
            //  from i in RangeAsync(0, 10, 500)
            //  where i % 2 == 0   // Even numbers only.
            //  select i * 10;     // Multiply by 10.

            //await foreach (var number in query)
            //    Console.WriteLine(number);
        }

        private static async IAsyncEnumerable<int> RangeAsync(int start, int count, int delay)
        {

            for (int i = start; i < start + count; i++)
            {
                await Task.Delay(delay);
                yield return i;
            }
        }






    }

    #endregion



    // Exponential backoff 
    #region Exponential backoff 
    // 
    // Exponential backoff 
    // --------------------------------------------------------------------------------
    // 

    // 
    public class ExponentialBackoff 
    {
        public static void Test()
        {
            


        }



    }

    #endregion



    #region Continuation with WhenAll
    public class ContinuationWithWhenAllAsyncAwait
    {
        // Test Method
        public static async void Test()
        {
            List<Task<List<string>>> tasks = new List<Task<List<string>>>();

            for (int i = 0; i < 10; i++)
            {
                // each task will return list of string
                var task = Task.Delay(500).ContinueWith(_ =>
                {
                    return Enumerable.Range(0, 4).Select(a => GenerateRandomStringWithRandomChar(3)).ToList();
                });
                tasks.Add(task);
            }

            // run the previous tasks in parallel

            var allResults = await Task.WhenAll(tasks);

            IList<string> allStrings = allResults.SelectMany(a=> a).ToList();

            Console.WriteLine(string.Join(", ", allStrings));
            
        }

        public static string GenerateRandomStringWithRandomChar(int length)
        {

            Random rand = new Random();
            return string.Concat(Enumerable.Range(0, length).Select(n2 => (char)rand.Next('A', 'Z')));

        }

    }

    #endregion


    #region Continuation with WhenAll and WhenAny
    // whenAll is used when we need to run many tasks in parallel and get result when all of them finish
    // whenAny is used when we doesn't need to wait for all tasks to finish their work, and we want to continue
    // with the one that has finished.
    class TestWhenAllAndWhenAnyAsyncAwait
    {
        // Test Method
        public static void Test()
        {

            var rand = new Random();

            var source = new CancellationTokenSource();



            var mytask = RunIOBoundOperation(source.Token, rand.Next(2000));

            if (rand.Next(10) % 2 == 0)
                source.Cancel();

            //success
            mytask.ContinueWith(d =>
            {
                Console.WriteLine(string.Join(", ", d.Result));
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            //failure
            mytask.ContinueWith(d =>
            {
                if (d.IsFaulted)
                    Console.WriteLine($"Faulted {d.Exception.InnerException?.Message}");
                if (d.IsCanceled)
                    Console.WriteLine($"Operation has been cancelled.");

            }, TaskContinuationOptions.NotOnRanToCompletion);

            Console.WriteLine("I am Free not tied with task ^_^");

        }

        public static Task<List<string>> RunIOBoundOperation(CancellationToken cancelToken, int? timeout = default)
        {
            Task<List<string>> operation = Task.Run(() =>
            {

                List<Task<string>> dataTasks = new List<Task<string>>();
                for (int i = 0; i < 10; i++)
                {
                    if (cancelToken.IsCancellationRequested) break;
                    var delayTask = Task.Delay(1000, cancelToken);
                    // continuation on the previous delay
                    // NOTE that i passed string count {i} as argument to avoid closure,
                    //  - as callbacks doesn't caputre the changing value of except if you copy it at the
                    //    definition scope of the callback
                    //  - also we can avoid the overhead of generated code for closures
                    // 
                    var dataTask = delayTask.ContinueWith((dtask, str) => str as string, $"count {i}");
                    dataTasks.Add(dataTask);
                }


                // when all
                var allTasks = Task.WhenAll(dataTasks);

                // timeout case
                if (timeout.HasValue && timeout.Value > 0)
                {
                    var timeOutTask = Task.Delay(timeout.Value, cancelToken);

                    // when any
                    var isTimeoutTask = Task.WhenAny(allTasks, timeOutTask);

                    var finalTask = isTimeoutTask.ContinueWith(a =>
                    {
                        if (a.Result == timeOutTask)
                            throw new OperationCanceledException();
                        return allTasks.Result.ToList();
                    }, cancelToken);
                    return finalTask;

                }
                else
                    //without timeout
                    return allTasks.ContinueWith(re => re.Result.ToList(), cancelToken);


            });
            return operation;
        }
    }

    #endregion



}



using Concurrency.Shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    #region Introduction
    // introcuction
    // -----------------------------------------------------------------------------------
    //    Parallel programming in .NET can be achieved in a few different ways.There's
    // the lower level thread class, which gives you complete control of an actual
    // thread.With complete control also comes great responsibility.You'd have to write a
    // lot of code on your own to effectively introduce parallelism using the thread
    // class. Then we have the Task Parallel Library, which provides an abstract concept
    // of looking at an operation that shouldn't block. The Task should be the preferred
    // way of running operations where you don't want to block the current
    // context.With a Task, you don't have to care about if it's a new thread or if it's reusing
    // an existing thread.The Task can schedule work on a thread pool, which may or may
    // not spawn an actual thread.It may also reuse a pre‑existing thread.This means
    // that you don't have to use the lower‑level threading, and can instead rely on
    // the framework and the language to give you some help.The Task Parallel Library
    // also introduces helper classes to easier introduce parallelism in your code.
    // You've got a class called Parallel, which provides functionality for running a set of
    // operations across the available resources, but also ways to easily convert a
    // For and a ForEach loop into a parallel loop. When processing collections, you may
    // have encountered LINQ, and there's a very handy extension with the Task Parallel
    // Library which provides a way to write Parallel LINQ.All the parallel helpers
    // in the Task Parallel Library are built on top of the Task.When Microsoft
    // introduced and started building these features, they were commonly known as the Parallel
    // Extensions.Obviously, there are also other libraries out there that will allow
    // you to run background work and process data asynchronously.It's time to explore
    // the parallel portion of the Task Parallel Library.


    // Parallel programming should be used any time you have a fair amount of computation
    // work that can be split up into independent chunks.Parallel programming increases
    // the CPU usage temporarily to improve throughput; this is desirable on client
    // systems where CPUs are often idle, but it’s usually not appropriate for server
    // systems.


    // There are two forms of parallelism:
    // data parallelism and task parallelism.
    //
    // Data parallelism
    //----------------------------
    // is when you have a bunch of data items to process, and the processing of each
    // piece of data is mostly independent from the other pieces.
    //
    //
    // Task parallelism
    // ---------------------------
    // is when you have a pool of work to do, and each piece of work is mostly independent
    // from the other pieces.
    // Task parallelism may be dynamic; if one piece of work results
    // in several additional pieces of work, they can be added to the pool of work.
    #endregion


    #region Introduction to Parallel Class
    //    we're going to use the Parallel class. This introduces a few helpers that will
    //allow us to more easily approach this. Internally.it will, in fact, use the
    //task from the Task parallel library. It will also provide some configuration
    //capabilities to override some behavior. Out of the box, it will take care of
    //calculating the most efficient way of dividing the tasks it's given among the available
    //cores.This means distributing the work effectively across the different cores
    //that you have available on your system.If we simply spawn a lot of tasks using
    //Task.Run, not only will it not be as efficient as it could be, it will also
    //require us to write a lot more code than we have to.Instead of doing that, we can use
    //these methods available on the Parallel class.

    //We could use Parallel for, which is really the same as a normal for loop,
    //but it will run in parallel and be
    //configured to distribute the work according to the system it's running on.
    //
    //You could also use the parallel ForEach, which is a parallel version of the normal
    //ForEach loop.
    //
    //You've also got a method called Invoke
    //on the Parallel class. This takes a list of actions.These actions will be
    //distributed and executed in parallel.
    //
    //None of the methods on the parallel class will
    //guarantee that it's executing in parallel because it really depends on the system.
    //I've had a look at the internals of Parallel.Invoke, and depending on how many
    //actions you pass to it, it might execute them using a parallel for loop.This
    //just proves that it does some really heavy lifting and provide some great
    //functionality and optimizations which you shouldn't take lightly. Parallel.Invoke doesn't
    //return anything, and the actions you pass to it are distributed to run as
    //effectively as possible and run independent from each other.
    #endregion


    #region Important notes

    // Parallel methods :
    // - they are all blocking the calling thread until finish the work

    // - if exception is thrown in one of the currently executing tasks,
    //  execution of the other ones will not be stopped.
    // 

    // there are parallel options that can be passed to control the operation
    //  MaxDegreeOfParallelism can be used to control the number of cores that Prallel opertion
    // will be executed on. it is recommended to let it to it's default, as it will be automatically 
    // adjusted to consume the whole cpu cores as possible as they can. and that is will be different 
    // from a user to another. so if you hard coded MaxDegreeOfParallelism it will reduce the 
    // parallelism of the execution, and you will not get the results as fast as you can
    // except if you intend to execute the operation in specific number of cores, and let the 
    // other ones.
    // 

    // don't run parallel code on asp.net server as it will introduce bad user experience 
    // to users, imagine that one user made a request with heavy computations that run on parallel,
    // the rest of the users will found the web site slow. imagine that the others make the same request

    #endregion

    #region Parallel.Invoke
    
    // we used it when there are many actions that are not related together.

    // if one of the actions that passed to invoke method has failed with exception,
    // the other scheduled actions to execute will continue, and exception
    // will propagated to the caller of Parallel.Invoke Method, but it will retruned
    // after all scheduled actions finish.

    public class ParallelInvokeMethod
    {
        // Test Method
        public static void Test()
        {
            Stopwatch watcher = Stopwatch.StartNew();
            ConcurrentBag<int> data = new ConcurrentBag<int>();
            Parallel.Invoke(
                // new ParallelOptions() {  MaxDegreeOfParallelism=2, CancellationToken=default, TaskScheduler= TaskScheduler.Current}
                () =>
                {
                    int result = Calculate(100_000,200_000);
                    data.Add(result);
                },
                () =>
                {
                    int result = Calculate(200_000, 300_000);
                    data.Add(result);
                },
                () =>
                {
                    int result = Calculate(300_000, 400_000);
                    data.Add(result);
                },
                () =>
                {
                    int result = Calculate(400_000, 500_000);
                    data.Add(result);
                }

                );

            watcher.Stop();
            Console.WriteLine($"Operation Finished in {watcher.ElapsedMilliseconds} ms");
            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }


        // Calculete prime NUmbers within a range of numbers
        static int Calculate(int from, int count)
        {
            return Enumerable.Range(from, count).Count(n
                     => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
        }

    }




    #endregion

    #region Parallel.Invoke : Case Of Exceptions
    // 
    // Parallel.Invoke : Case Of Exceptions
    // --------------------------------------------------------------------------------
    // 

    // 
    public class ParallelInvokeExceptions
    {
        // Test Method
        public static void Test()
        {
            Stopwatch watcher = Stopwatch.StartNew();
            ConcurrentBag<int> data = new ConcurrentBag<int>();

            try
            {

                Parallel.Invoke(
                    //new ParallelOptions() {
                    //    MaxDegreeOfParallelism = 1, 
                    //},
                    () =>
                    {
                        int result = Calculate(100_000, 200_000);
                        data.Add(result);
                    },
                    () =>
                    {
                        throw new Exception("failed operation 1.");
                    //int result = Calculate(200_000, 300_000);
                    //data.Add(result);
                    },
                    () =>
                    {
                        throw new Exception("failed operation 2.");
                        //int result = Calculate(200_000, 300_000);
                        //data.Add(result);
                    },
                    () =>
                    {
                        // will executes even if exception thrown in others
                        int result = Calculate(300_000, 400_000);
                        data.Add(result);
                    },
                    () =>
                    {
                        // will executes even if exception thrown in others
                        int result = Calculate(400_000, 500_000);
                        data.Add(result);
                    }

                    );

            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"Exception Thrown: {ex.Message}");
                Console.WriteLine(string.Join(Environment.NewLine,
                    // to access all the thrown exceptions from multiple actions that ran in Parallel
                    ex.InnerExceptions.Select(e => $"InnerExp: {e.Message}")));
            }
            watcher.Stop();
            Console.WriteLine($"Operation Finished in {watcher.ElapsedMilliseconds} ms");

            // NOTE: that the three other operations has been continued
            //       
            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }


        // Calculete prime NUmbers within a range of numbers
        static int Calculate(int from, int count)
        {
            return Enumerable.Range(from, count).Count(n
                     => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
        }
    }

    #endregion


    #region Parallel.ForEach Method
    // 
    // Parallel.ForEach Method
    // --------------------------------------------------------------------------------
    // 

    // 
    public class ParallelForEach
    {
        public static void Test()
        {

            Stopwatch watcher = Stopwatch.StartNew();
            ConcurrentBag<int> data = new ConcurrentBag<int>();
            
            var primesTo = new List<int>()
            { 
                100_000, 200_000, 300_000, 400_000, 500_000,
                450_000, 350_000, 500_000, 900_000
            };


            Parallel.ForEach(primesTo, (int value, ParallelLoopState state) =>
            {
                var result = Calculate(2, value);
                data.Add(result);
            });


            watcher.Stop();
            Console.WriteLine($"Operation Finished in {watcher.ElapsedMilliseconds} ms");
            foreach (var item in data)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine();
        }


        // Calculete prime NUmbers within a range of numbers
        static int Calculate(int from, int count)
        {
            return Enumerable.Range(from, count).Count(n
                     => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
        }
    }

    #endregion


    #region Parallel.ForEach: Break and Stop Methods
    // 
    // Parallel.ForEach: Break and Stop Methods
    // --------------------------------------------------------------------------------
    // Break: any non scheduled iteration after the iteration of Break will n't be executed
    // and all iterations before break will be executed even if it's not schedulted
    // so if a thread lagged before the current iteration, it will be executed

    // Stop: any non scheduled iteration will not be executed.
    // so if a thread lagged before the current iteration, it will not be executed
    //

    // 
    // ShouldExitCurrentIteration
    // ---------------------------
    // If your loop body is long, you might want other threads to break partway through
    // the method body in case of an early Break or Stop. You can do this by polling the
    // ShouldExitCurrentIteration property at various places in your code; this property
    // becomes true immediately after a Stop—or soon after a Break.

    // ShouldExitCurrentIteration also becomes true after a cancellation request—or if
    // an exception is thrown in the loop.




    public class ParallelForEachBreak
    {
        public static void Test()
        {
            
            Stopwatch watcher = Stopwatch.StartNew();
            ConcurrentBag<(int iteration, int result)> data = new();

            var primesTo = new List<int>()
            {
                100_000, 200_000, 300_000, 400_000, 500_000,
                450_000, 350_000, 500_000, 900_000
            };


            ParallelLoopResult loopResult = Parallel.ForEach(primesTo
                , (int value, ParallelLoopState state) =>
            {
                if(value  > 400_000 || state.ShouldExitCurrentIteration ) 
                {
                    Thread.Sleep(100); // Only to delay the current iteration
                    // this will break the loop,
                    // but note that all brevious iteration will be executed
                    // and the next ones will not.
                    // 
                    state.Break();
                    return;
                }
                var result = Calculate(2, value);
                if(!state.ShouldExitCurrentIteration)
                    data.Add((value,result));
            });


            watcher.Stop();
            Console.WriteLine($"Operation Finished in {watcher.ElapsedMilliseconds} ms");
            foreach (var item in data)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("Completed: {0}",loopResult.IsCompleted);

            // If LowestBreakIteration returns null, it means that you called Stop
            // (rather than Break) on the loop.
            Console.WriteLine("LowestBreakIteration: {0}", loopResult.LowestBreakIteration);
            Console.WriteLine();
        }


        // Calculete prime NUmbers within a range of numbers
        static int Calculate(int from, int count)
        {
            return Enumerable.Range(from, count).Count(n
                     => Enumerable.Range(2, (int)Math.Sqrt(n) - 1).All(i => n % i > 0));
        }
    }

    #endregion


    #region 4.2 Parallel Aggregation
    // 
    // 4.2 Parallel Aggregation
    // --------------------------------------------------------------------------------
    // 

    // 
    public class parallelAggregation
    {
        public static void Test()
        {
            var result = ParallelEnumerable.Range(1, 1000_000);
            TimeIt(() => Console.WriteLine(ParallelSum(result)));


            TimeIt(() => Console.WriteLine(ParallelSumWithPLINQ(result)));


            // output On My 8 Core CPU:
            // ------------------------
            // 500000500000
            // it takes 100 ms.
            // 500000500000
            // it takes 17 ms.


        }

        // Note: this is not the most efficient implementation.
        // This is just an example of using a lock to protect shared state.
        static long ParallelSum(IEnumerable<int> values)
        {
            object mutex = new object();
            long result = 0;
            Parallel.ForEach(source: values,
                localInit: () => (long)0,
                body: (item, state, localValue) => localValue + item,
                localFinally: localValue =>
                {
                    lock (mutex)
                        result += localValue;
                });

            //Parallel.ForEach(
            //    values,
            //    ()=>0,
            //    (value,state,localValue)=> value + localValue, 
            //    localValue => { lock (mutex) result += localValue; }
            //);


            return result;
        }


        static long ParallelSumWithPLINQ(IEnumerable<int> values)
        {
            return values.AsParallel().Sum(v=> (long)v);
        }


        public static void TimeIt(Action action)
        {
            var watch = Stopwatch.StartNew();
            action();
            watch.Stop();
            Console.WriteLine($"it takes {watch.ElapsedMilliseconds} ms.");
        }

    }

    #endregion

    #region Dynamic Parallelism
    // 
    // Dynamic Parallelism
    // --------------------------------------------------------------------------------
    // 
    // Problem
    // You have a more complex parallel situation where the structure and number of
    // parallel tasks depend on information known only at runtime.
    // 
    // solution: 
    // The Task Parallel Library(TPL) is centered around the Task type.The Parallel
    // class and Parallel LINQ are just convenience wrappers around the powerful Task.
    // When you need dynamic parallelism, it’s easiest to use the Task type directly.


    // The AttachedToParent flag ensures that the Task for each branch is linked to the
    // Task for their parent node.This creates parent/child relationships among the Task
    // instances that mirror the parent/child relationships in the tree nodes.

    public class DynamicParallelism
    {
        public static void Test()
        {
            BinaryTree<long> numbersTree = new BinaryTree<long>();
            numbersTree.AddNode(50);
            numbersTree.AddNode(40);
            numbersTree.AddNode(60);
            numbersTree.AddNode(30);
            numbersTree.AddNode(35);
            numbersTree.AddNode(25);
            numbersTree.AddNode(55);
            numbersTree.AddNode(70);
            numbersTree.AddNode(80);
            numbersTree.AddNode(75);
            ConcurrentBag<long> flattenBinaryTree = new ConcurrentBag<long>();
            numbersTree.TraverseInParallel((value) => flattenBinaryTree.Add(value));
            Console.WriteLine(string.Join(", ",flattenBinaryTree));

        }

        // If you don’t have a parent/child kind of situation, you can schedule any
        // task to run after another by using a task continuation.


        public static void DynamicParallelismNoParentChildRelationship()
        {
            Task task = Task.Factory.StartNew(
                () => Thread.Sleep(TimeSpan.FromSeconds(2)),
                CancellationToken.None,
                TaskCreationOptions.None,
                TaskScheduler.Default);

            Task continuation = task.ContinueWith(
                t => Trace.WriteLine("Task is done"),
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);
        }


    }



    #endregion






}

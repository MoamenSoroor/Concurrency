using System;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrency
{
    #region Synchronization
    // 
    // Synchronization
    // --------------------------------------------------------------------------------
    // There are two major types of synchronization: communication and data protection.
    // Communication is used when one piece of code needs to notify another piece of
    // code of some condition (e.g., a new message has arrived).

    // You need to use synchronization to protect shared data when all three of these
    // conditions are true:
    //  1- Multiple pieces of code are running concurrently.
    //  2- These pieces are accessing(reading or writing) the same data.
    //  3- At least one piece of code is updating(writing) the data.


    //  The synchronization constructs can be divided into three categories:
    //  1- Exclusive locking
    //      Exclusive locking constructs allow just one thread to perform some activity or execute
    //      a section of code at a time.Their primary purpose is to let threads access shared
    //      writing state without interfering with one another. The exclusive locking constructs
    //      are lock, Mutex, and SpinLock.
    // 
    //  2- Nonexclusive locking
    //      Nonexclusive locking lets you limit concurrency. The nonexclusive locking constructs
    //      are Semaphore(Slim) and ReaderWriterLock(Slim).
    //  3- Signaling
    //      These allow a thread to block until receiving one or more notifications from other
    //      thread(s). The signaling constructs include ManualResetEvent(Slim), AutoResetEvent,
    //      CountdownEvent, and Barrier. The former three are referred to as event wait handles.


    // It’s also possible (and tricky) to perform certain concurrent operations on shared state
    // without locking through the use of nonblocking synchronization constructs. These are Thread.
    // MemoryBarrier, Thread.VolatileRead, Thread.VolatileWrite, the volatile keyword, and
    // the Interlocked class.


    // Exclusive Locking
    // There are three exclusive locking constructs: the lock statement, Mutex, and SpinLock.
    // - The lock construct is the most convenient and widely used, whereas the other two target niche scenarios:
    // - Mutex lets you span multiple processes (computer-wide locks).
    // - SpinLock implements a micro-optimization that can lessen context switches in high-concurrency scenarios


    // The lock Statement
    // -----------------------------


    // thread unsafe code 
    // 
    class ThreadUnsafe
    {
        static int _val1 = 1, _val2 = 1;

        static void Go()
        {
            if (_val2 != 0)
            {
                // may throw exception : in case when one thread has passed if check
                // and the other set value with zero.
                Console.WriteLine(_val1 / _val2);
            }
            _val2 = 0;
        }


        public static void Test()
        {
            // execute in parallel
            Go();
            new Thread(Go).Start();
        }

    }
    public class SynchronizationWithLock
    {
        public static void Test()
        {

        }
    }

    #endregion

    #region Nested Locking

    // 
    // Nested Locking

    // --------------------------------------------------------------------------------
    // 

    // 
    public class NestedLocking
    {
        static object locker = new object();

        public static void Test()
        {

            lock (locker)
            {
                AnotherMethod();
                // We still have the lock - because locks are reentrant.
            }

           
        }
        static void AnotherMethod()
        {
            lock (locker) { Console.WriteLine("Another method"); }
        }
    }

    #endregion


    #region DeadLocks

    // 
    // DeadLocks

    // --------------------------------------------------------------------------------
    // 

    // 
    public class DeadLocks
    {
        static object locker = new object();

        public static void Test()
        {
            Task.WaitAll(Task.Run(AnotherMethod), Task.Run(AnotherMethod2));


        }
        static void AnotherMethod()
        {
            lock (locker) { Thread.Sleep(2000); Console.WriteLine("Another method 1"); }
        }

        static void AnotherMethod2()
        {
            // will deadlocked until the AnotherMethod free the lock
            lock (locker) { Console.WriteLine("Another method 2"); }
        }
    }


    public class DeadLockWithTwoNestedLocks
    {
        private object locker1 = new object();
        private object locker2= new object();
        public static void Test()
        {
            DeadLockWithTwoNestedLocks obj = new DeadLockWithTwoNestedLocks();

            Task.Run(obj.FirstMethod);

            obj.SecondMethod();
        }

        public void FirstMethod()
        {
            lock (locker1)
            {
                Thread.Sleep(1000);
                lock (locker2)
                {
                    Console.WriteLine("operation 1.");
                }
            }
        }

        public void SecondMethod()
        {
            lock (locker2)
            {
                Thread.Sleep(1000);
                lock (locker1)
                {
                    Console.WriteLine("operation 2.");
                }
            }
        }



    }

    #endregion

}

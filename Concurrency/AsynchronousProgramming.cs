using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Concurrency
{
    

    #region Introduction to Asynchronous Programming
    //    Asynchronous programming
    //A form of concurrency that uses futures or callbacks to avoid unnecessary threads.

    //A future(or promise) is a type that represents some operation that will complete
    //in the future.Some modern future types in .NET are Task and Task<TResult>.
    //Older asynchronous APIs use callbacks or events instead of futures.
    //Asynchronous programming is centered around the idea of an asynchronous
    //operation: some operation that is started that will complete some time later.
    //While the operation is in progress, it doesn’t block the original thread;
    //the thread that starts the operation is free to do other work.When the operation
    //completes, it notifies its future or invokes its callback or event to let the
    //application know the operation is finished.
    
    #endregion

}

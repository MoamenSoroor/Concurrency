using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    // 
    public class Synchronization
    {
        public static void Test()
        {

        }
    }

    #endregion



}

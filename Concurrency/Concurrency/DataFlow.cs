using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace Concurrency
{

    #region What is Data Flow
    // 
    // What is Data Flow
    // --------------------------------------------------------------------------------
    // TPL Dataflow is a powerful library that enables you to create a mesh or pipeline
    // and then (asynchronously) send your data through it. Dataflow is a very
    // declarative style of coding:
    // 
    // Each mesh is comprised of various blocks that are linked to each other.
    // The individual blocks are simple and are responsible for a single step in the
    // data processing.When a block finishes working on its data, it will pass its
    // result along to any linked blocks.
    // 
    // 
    // To use TPL Dataflow, install the NuGet package System.Threading.Tasks.Dataflow
    // into your application. (Not the case in .net 6)
    // 
    public class TestDataFlow
    {
        public static void Test()
        {
            TransformBlock<int, int> mulBlock = new TransformBlock<int, int>(a => a * a);
            TransformBlock<int, int> addOneBlock = new TransformBlock<int, int>(a => a + 1);
            mulBlock.LinkTo(addOneBlock);

            mulBlock.Post(10);

            

        }
    }

    #endregion



    class DataFlow
    {
    }
}

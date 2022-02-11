using Concurrency.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Concurrency
{
    class Program
    {
        static void Main(string[] args)
        {
            //TaskFactoryStartNew_ChildTasksAttachment.Test();

            //ParallelInvokeMethod.Test();
            //ParallelInvokeExceptions.Test
            //ParallelForEach.Test();
            //ParallelForEachBreak.Test();
            //parallelAggregation.Test();

            //Blocking.Test();
            //LackOfThreadSafetyInSharedState.Test();
            //LackOfThreadSafetyInSharedState2.Test();
            //RunScriptToGetRegionTitles();


            JoiningThreadsDoesNotPropagateExceptions.Test();

            //BinaryTreeTester.Test();
            //DynamicParallelism.Test();

            //TestDataFlow.Test();
        }

        static void RunScriptToGetRegionTitles()
        {
            string mainDir = @"D:/Projects/CSharpProjects/Concurrency/Concurrency/Concurrency";
            List<string> files = Directory.GetFiles(mainDir).ToList();

            string regionRegex = @"\#region\s*((\S*\s)+?\n)";

            Enumerable.Repeat(regionRegex, files?.Count ?? 0)
                .Zip(files,(r,f)=>(regex:r, filePath:f))
                .AsParallel()
                .ForAll(tuple =>
                {
                    try
                    {

                        string content = File.ReadAllText(tuple.filePath);
                        List<string> result = Regex.Matches(content, tuple.regex,RegexOptions.Multiline)
                        .Cast<Match>()
                        .Select(m => $"- {m.Groups?[1]?.Value ?? ""}")
                        .Where(m => !string.IsNullOrWhiteSpace(m))
                        .ToList();
                        string newFile = $@"D:/Projects/CSharpProjects/Concurrency/OutputOfScript/{Path.GetFileNameWithoutExtension(tuple.filePath)}.txt";

                        File.WriteAllText(newFile, string.Join(Environment.NewLine, result));

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });



        }

    }



}

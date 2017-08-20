namespace Benchmark
{
    using System;
    using BenchmarkDotNet.Running;

    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<WriteLineProtocol>();

            Console.ReadKey();
        }
    }
}

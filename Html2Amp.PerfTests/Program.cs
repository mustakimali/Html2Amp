using System;
using BenchmarkDotNet.Running;
using Html2Amp.PerfTests.ElementExtensionsTests;

namespace Html2Amp.PerfTests
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<ElementExtensions_CopyTo>();
            BenchmarkRunner.Run<HtmlConverting>();
        }
    }
}

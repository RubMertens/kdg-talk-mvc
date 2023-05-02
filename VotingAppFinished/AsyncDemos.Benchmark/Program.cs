// See https://aka.ms/new-console-template for more information

using System.Net.NetworkInformation;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Microsoft.Diagnostics.Tracing.Parsers.FrameworkEventSource;

public class Program
{
    public static void Main()
    {
        var summary = BenchmarkRunner.Run<ShittyServerBenchmarks>();
        
        Console.WriteLine(summary);
    }
}

public class ShittyServerBenchmarks
{
    private readonly SingleThreadServer _singleThreadServer = new SingleThreadServer();
    private readonly ThreadingServer _threadingServer = new ThreadingServer();
    private readonly ThreadPoolServer _threadPoolServer = new ThreadPoolServer();

    private int IterationCount = 50;
    private int ThreadSleepDelayMs = 10;


    private string HandleRequest(string req)
    {
        // Console.WriteLine($"handling {req}");
        Thread.Sleep(ThreadSleepDelayMs);
        var reversed = new string(req.Reverse().ToArray());
        return reversed;
        // Console.WriteLine($"handled {reversed}");
    }
    [Benchmark()]
    public void RunSingleThread()
    {
        for (int i = 0; i < IterationCount; i++)
        {
            _singleThreadServer.HandleRequest("data"+i, HandleRequest);
        }
    }
    
    [Benchmark()]
    public void RunNewThreads()
    {
        for (int i = 0; i < IterationCount; i++)
        {
            _threadingServer.HandleRequest("data"+i, HandleRequest);
        }
    }
    
    [Benchmark()]
    public void RunThreadPool()
    {
        for (int i = 0; i < IterationCount; i++)
        {
            _threadPoolServer.HandleRequest("data"+i, HandleRequest);
        }
    }
}
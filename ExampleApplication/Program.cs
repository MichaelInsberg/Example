using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Example.ExampleApplication;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Started application");
        var workerList = new List<Worker>();
        var cancellationTokenSource = new CancellationTokenSource();
        Console.WriteLine("Start worker");
        for (int index = 0; index < 10; index++)
        {
            var worker = new Worker($"Worker nr. {index}");
            worker.Start(cancellationTokenSource.Token);
            workerList.Add(worker);
        }

        Console.WriteLine("Press any key to terminate the application");
        Console.ReadLine();

        cancellationTokenSource.Cancel();
        Console.WriteLine("Stop worker");
        foreach (var worker in workerList)
        {
            worker.Stop();
        }

        Console.WriteLine("All tasks finished. Application terminated");
    }
}
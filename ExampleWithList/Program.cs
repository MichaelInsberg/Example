using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Example.CommandLibrary;

namespace ExampleWithList
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start application");

            var queue = new ConcurrentList<WriteCommand>();
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;
            var tasks = new List<Task>();

            var writerTask = Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    queue.Add(new WriteCommand(Commands.SetRs232BaudRate));
                    Console.WriteLine("Enqueue new command");
                    Thread.Sleep(1000);

                }
            }, token);

            tasks.Add(writerTask);

            var readerTask = Task.Factory.StartNew(() =>
            {
                while (!token.IsCancellationRequested)
                {
                    while (queue.Count != 0)
                    {
                        Console.WriteLine($"{queue.Count} item(s) in queue");
                        var result = queue.TryGetFirstItem();
                        if (result.IsResultValid)
                        {
                            Console.WriteLine($"GetFirstItem: {result.Value}");
                        }
                        Thread.Sleep(100);
                    }
                }
            }, token);

            tasks.Add(readerTask);

            Console.WriteLine("Press any key to terminate the application");
            Console.ReadLine();

            cancellationTokenSource.Cancel();

            Task.WaitAll(tasks.ToArray());
            foreach (var task in tasks)
            {
                task.Dispose();
            }

            Console.WriteLine("All tasks finished. Application terminated");
        }
    }
}

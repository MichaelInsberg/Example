using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Example.CommandLibary;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Start application");

        var queue = new ConcurrentQueue<WriteCommand>();
        var cancellationTokenSource = new CancellationTokenSource();
        var tasks = new List<Task>();

        var writerTask = Task.Factory.StartNew(() =>
        {
            queue.Enqueue(new WriteCommand(Commands.SetRs232BaudRate));
            Console.WriteLine("Enqueue new command");
            Thread.Sleep(1000);
        }, cancellationTokenSource.Token);

        tasks.Add(writerTask);

        var readerTask = Task.Factory.StartNew(() =>
        {
            while (!queue.IsEmpty)
            {
                Console.WriteLine(queue.TryDequeue(out WriteCommand command)
                    ? $"Dequeue command: {command}"
                    : $"Failed to dequeue command");
            }
            Thread.Sleep(100);
        }, cancellationTokenSource.Token);

        tasks.Add(readerTask);

        Console.WriteLine("Press any key to terminate the application");
        Console.ReadLine();

        cancellationTokenSource.Cancel();

        Task.WaitAll(tasks.ToArray());

        Console.WriteLine("All tasks finished. Application terminated");
    }
}
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Example.CommandLibrary;

namespace Example.ExampleApplication
{
    public class Worker
    {
        private List<Task> tasks;
        private bool started;
        private readonly List<byte> dummyParameterList;
        private readonly string name;

        public Worker(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            this.name = name;
            started = false;
            tasks = new List<Task>();
            dummyParameterList = new List<byte>();
            for (int i = 0; i < 1000; i++)
            {
                dummyParameterList.Add(255);
            }
        }

        public void Start(CancellationToken token)
        {
            if (started)
            {
                return;
            }
            started = true;
            var queue = new ConcurrentQueue<WriteCommand>();

            var producerTask= new Task(()=> {
                Thread.CurrentThread.Name = $"{name} producer. Thread ID {Thread.CurrentThread.ManagedThreadId}";
                while (!token.IsCancellationRequested)
                {
                    queue.Enqueue(new WriteCommand(Commands.SetRs232BaudRate, dummyParameterList.ToArray()));
                    Console.WriteLine($"Enqueue new command {Thread.CurrentThread.Name}");
                    Thread.Sleep(1000);

                }
            }, token, TaskCreationOptions.LongRunning);
            producerTask.Start();

            tasks.Add(producerTask);

            var consumerTask = new Task(() =>
            {
                Thread.CurrentThread.Name = $"{name} Consumer. Thread ID {Thread.CurrentThread.ManagedThreadId}";
                while (!token.IsCancellationRequested)
                {
                    while (!queue.IsEmpty)
                    {
                        Console.WriteLine(queue.TryDequeue(out WriteCommand command)
                            ? $"Dequeue command: {command} {Thread.CurrentThread.Name}"
                            : $"Failed to dequeue command {Thread.CurrentThread.Name}");
                        //command = null;
                    }

                    Thread.Sleep(100);
                }
            }, token, TaskCreationOptions.LongRunning);
            consumerTask.Start();

            tasks.Add(consumerTask);
        }

        public void Stop()
        {
            if (started)
            {
                try
                {
                    Task.WaitAll(tasks.ToArray());
                }
                catch (Exception)
                {
                    //Do nothing
                }
            }

            foreach (var task in tasks)
            {
                task.Dispose();
            }
        }

    }
}

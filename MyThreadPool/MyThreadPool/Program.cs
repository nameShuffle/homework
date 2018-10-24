using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            int del()
            {
                int b = 0;
                int a = 5 / b;

                return a;
            }

            MyThreadPool pool = new MyThreadPool(5);
            var task = pool.AddTask(del);
            Console.WriteLine(task.Result);
            Console.ReadKey();
            /*MyThreadPool pool = new MyThreadPool(5);
            Thread.Sleep(500);
            using (var currentProcess = System.Diagnostics.Process.GetCurrentProcess())
            {
                Console.WriteLine(currentProcess.Threads
                    .OfType<System.Diagnostics.ProcessThread>()
                    .Where(t => t.ThreadState == System.Diagnostics.ThreadState.Running)
                    .Count());
            }

            Console.ReadKey();*/
            /*
            int function()
            {
                System.Threading.Thread.Sleep(500);
                return 21;
            }

            int functiontwo(int arg)
            {
                return arg + 10;
            }
            MyThreadPool pool = new MyThreadPool(5);
            var first = pool.AddTask(function);
            var second = first.ContinueWith(functiontwo);
            System.Threading.Thread.Sleep(5000);
            if (first.IsCompleted)
                Console.WriteLine(first.Result);
            else
                Console.WriteLine("no result to first");

            if (second.IsCompleted)
                Console.WriteLine(second.Result);
            else
                Console.WriteLine("no resul to second");


            Console.ReadKey();
            */
        }
        
    }
}

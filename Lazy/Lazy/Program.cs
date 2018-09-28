using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Lazy
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            var lazy = LazyFactory.CreateSimpleLazy(() =>
             {
                 Console.WriteLine("calculating...");
                 return 2;
             });

            Console.WriteLine("starting...");
            Console.WriteLine("result = {0}", lazy.Get);
            Console.WriteLine("result number two = {0}", lazy.Get);
            Console.ReadKey();

            var protectedLazy = LazyFactory.CreateProtectedLazy(() =>
            {
                Console.WriteLine("super calculating...");
                return 2;
            });

            var threads = new Thread[10];

            for (int i = 0; i < 10; i++)
            {
                int threadNumber = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 5; j++)
                        Console.WriteLine($"result of thread number {threadNumber} : {protectedLazy.Get}");
                    Thread.Sleep(10000);
                }
                );
                
            }

            foreach (var thread in threads)
                thread.Start();

            foreach (var thread in threads)
                thread.Join();

            Console.ReadKey();
            */
        }
    }
}

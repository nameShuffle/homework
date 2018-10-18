using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyThreadPool
{
    class Program
    {
        static void Main(string[] args)
        {
            int function()
            {
                return 21;
            }

            MyThreadPool pool = new MyThreadPool(5);
            var first = pool.AddTask(function);
            System.Threading.Thread.Sleep(5000);
            if (first.IsCompleted)
                Console.WriteLine(first.Result);
            else
                Console.WriteLine("no result");

            Console.ReadKey();

        }
        
    }
}

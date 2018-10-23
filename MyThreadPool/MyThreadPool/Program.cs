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

        }
        
    }
}

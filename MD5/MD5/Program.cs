using System;

namespace MD5
{
    class Program
    {
        static void Main(string[] args)
        {
            var sum = new CheckSum();
            Console.WriteLine(sum.CheckSumFull("G:/спбгу/соцстипендия"));
            Console.ReadKey();

            var sumTh = new CheckSumThreads();
            Console.WriteLine(sumTh.CheckSumFull("G:/спбгу/соцстипендия"));
            Console.ReadKey();
        }
    }
}

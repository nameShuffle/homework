using System;
using System.Collections.Generic;

namespace MyNUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите путь для тестирования:");
            string path = Console.ReadLine();

            List<TestResult> results = new List<TestResult>();

            UnitTesting testingSystem = new UnitTesting();
            results = testingSystem.StartUnitTesting(path);

            //вывод на консоль результатов 
            foreach (var result in results)
            {
                Console.WriteLine();
                Console.WriteLine($"Test Class : {result.TypeName}");
                Console.WriteLine($" Test Name : {result.TestName}");
                if (result.WhyIgnored != null)
                {
                    Console.WriteLine($" Test was ignored : {result.WhyIgnored}");
                    continue;
                }

                if (result.IsOk)
                {
                    Console.WriteLine($" Test Status : test successfully completed");
                    if (result.Expected != null)
                    {
                        Console.WriteLine($" Expected exception : {result.Expected.Name}");
                    }
                }
                else
                {
                    Console.WriteLine($" Test Status : test completed with exceptions");
                    if (result.Expected != null)
                    {
                        Console.WriteLine($" Expected exception : {result.Expected.Name}");
                    }
                    Console.WriteLine($" Exception : {result.RealException.GetType().Name}");
                }
                Console.WriteLine($" Time : {result.Time}");
            }

            Console.ReadKey();
        }
    }
}

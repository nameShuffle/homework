using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MyNUnit
{
    class UnitTesting
    {
        private List<TestInfo> beforeClassTestMethods = new List<TestInfo>();
        private List<TestInfo> afterClassTestMethods = new List<TestInfo>();
        private List<TestInfo> beforeTestMethods = new List<TestInfo>();
        private List<TestInfo> afterTestMethods = new List<TestInfo>();
        private List<TestInfo> testMethods = new List<TestInfo>();

        /// <summary>
        /// Класс, инкапсулирующий результаты выполнения тестов:
        /// статус теста (прошел или не прошел), время исполнения теста.
        /// Для обычных тестов тестов хранится дополнительная информация.
        /// </summary>
        private class TestResult
        {
            public TestResult(string time, string whyIgnored, bool isOk)
            {
                this.Time = Time;
                this.IsOk = isOk;
                this.WhyIgnored = whyIgnored;
            }

            public string Time { get; }
            public bool IsOk { get; }
            public string WhyIgnored { get; }
        }

        private class TestInfo
        {
            public TestInfo(MethodInfo method, Type type)
            {
                this.TestMethod = method;
                this.TestType = type;
            }

            public MethodInfo TestMethod { get; }
            public Type TestType { get; }
        }

        UnitTesting(string path)
        {
            var dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                Console.WriteLine("Проверьте корректность введенных данных");
            }

            RunTests(path);
        }

        private void RunTests(string path)
        {
            var testsDllAssemblies = Directory.GetFiles(path, "*.dll");
            var testsExeAssemblies = Directory.GetFiles(path, "*.exe");

            foreach (var dllFile in testsDllAssemblies)
            {
                foreach (var type in Assembly.LoadFile(dllFile).GetExportedTypes())
                {
                    GetTestMethods(type);
                }
            }

            foreach (var exeFile in testsDllAssemblies)
            {
                foreach (var type in Assembly.LoadFile(exeFile).GetExportedTypes())
                {
                    GetTestMethods(type);
                }
            }

            if (this.testMethods.Count == 0)
            {
                return;
            }

            foreach (var test in beforeClassTestMethods)
            {
                var testTask = new Task<TestResult>(newTest => RunTest((TestInfo)newTest), test);
                testTask.Start();
            }
            
        }

        private TestResult RunTest(TestInfo test, bool isSimpleTest)
        {
            try
            {
                test.TestMethod.Invoke(test.TestType, null);
                var result = new TestResult(null, null, true);
                return result;
            }
            catch (Exception ex)
            {
                //тут нужно сравнить исключение с ожидаемым, если оно все же возникло
                return null;
            }
            
        }

        private void GetTestMethods(Type type)
        {
            foreach (var method in type.GetMethods())
            {
                foreach (var attr in Attribute.GetCustomAttributes(method))
                {
                    if (attr.GetType() == typeof(BeforeClassAttribute))
                    {
                        var test = new TestInfo(method, type);
                        beforeClassTestMethods.Add(test);
                    }

                    if (attr.GetType() == typeof(AfterClassAttribute))
                    {
                        var test = new TestInfo(method, type);
                        afterClassTestMethods.Add(test);
                    }

                    if (attr.GetType() == typeof(BeforeAttribute))
                    {
                        var test = new TestInfo(method, type);
                        beforeTestMethods.Add(test);
                    }

                    if (attr.GetType() == typeof(AfterAttribute))
                    {
                        var test = new TestInfo(method, type);
                        afterTestMethods.Add(test);
                    }

                    if (attr.GetType() == typeof(TestAttribute))
                    {
                        var test = new TestInfo(method, type);
                        testMethods.Add(test);
                    }
                }

            }
        }
    }
}

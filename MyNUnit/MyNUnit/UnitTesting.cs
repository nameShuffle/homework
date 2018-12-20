using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace MyNUnit
{
    public class UnitTesting
    {   
        private List<TestResult> testResults = new List<TestResult>();
        private Object lockObject = new Object();

        /// <summary>
        /// Основной метод, активирует работу по запуску тестов по переданному пути.
        /// Если путь был введен неверно, сообщает об ошибке в консоль.
        /// </summary>
        /// <param name="path">Путь, по которому нужно найти тесты и запустить их.</param>
        public List<TestResult> StartUnitTesting(string path)
        {
            try
            {
                var dir = new DirectoryInfo(path);
                if (!dir.Exists)
                {
                    Console.WriteLine("Проверьте корректность введенных данных");
                }

                RunTests(path);
            }
            catch
            {
                Console.WriteLine("Проверьте корректность введенных данных");
            }

            return this.testResults;
        }

        /// <summary>
        /// Метод ищет сборки по указанному пути, разбирает типа в каждой сборке и запускает
        /// для каждого типа методы, распределяющий тесты по нужным спискам.
        /// </summary>
        /// <param name="path">Путь, в котором осуществляется поиск.</param>
        private void RunTests(string path)
        {
            var testsDllAssemblies = Directory.GetFiles(path, "*.dll");
            var testsExeAssemblies = Directory.GetFiles(path, "*.exe");

            List<Task> tasksDlls = new List<Task>();
            List<Task> tasksExes = new List<Task>();

            foreach (var dllFile in testsDllAssemblies)
            {
                foreach (var type in Assembly.LoadFile(dllFile).GetExportedTypes())
                {
                    var handleType = new Task(curType => RunTestsInClass((Type)curType), type);
                    tasksDlls.Add(handleType);
                    handleType.Start();
                }
            }

            /*foreach (var exeFile in testsExeAssemblies)
            {
                foreach (var type in Assembly.LoadFile(exeFile).GetExportedTypes())
                {
                    var handleType = new Task(curType => RunTestsInClass((Type)curType), type);
                    tasksExes.Add(handleType);
                    handleType.Start();
                }
            }*/

            foreach (var task in tasksDlls)
            {
                task.Wait();
            }
            foreach (var task in tasksExes)
            {
                task.Wait();
            }
        }

        /// <summary>
        /// Производит запуск тестов из переданного тестового класса.
        /// </summary>
        /// <param name="type"></param>
        private void RunTestsInClass(Type type)
        {
            List<TestInfo> beforeClassTestMethods = new List<TestInfo>();
            List<TestInfo> afterClassTestMethods = new List<TestInfo>();
            List<TestInfo> beforeTestMethods = new List<TestInfo>();
            List<TestInfo> afterTestMethods = new List<TestInfo>();
            List<TestInfo> testMethods = new List<TestInfo>();
            GetTestMethods(type, beforeClassTestMethods, afterClassTestMethods, beforeTestMethods, afterTestMethods,
                testMethods);
            StartTesting(beforeClassTestMethods, this.testResults);
            StartTesting(testMethods, beforeTestMethods, afterTestMethods, this.testResults);
            StartTesting(afterClassTestMethods, this.testResults);
        }

        /// <summary>
        /// Метод, использующийся для запуска тестов, помеченных атрибутами BeforeClass и AfterClass.
        /// </summary>
        /// <param name="tests">Тесты для запуска.</param>
        /// <param name="results">Список для сохранения результатов.</param>
        private void StartTesting(List<TestInfo> tests, List<TestResult> results)
        {
            List<Task<TestResult>> tasks = new List<Task<TestResult>>();
            foreach (var test in tests)
            {
                var testTask = new Task<TestResult>(newTest => RunTest((TestInfo)newTest), test);
                tasks.Add(testTask);
                testTask.Start();
            }
            foreach (var test in tasks)
            {
                lock(lockObject)
                {
                    results.Add(test.Result);
                }
            }
        }

        /// <summary>
        /// Перегруженный метод, используется для запуска основных тестов методов.
        /// При каждом запуске основного тестового метода производится запуск методов, 
        /// помеченных атрибутом Before. После заапуска каждого основного тестового метода
        /// производится запуск методов, помеченных атрибутом After.
        /// </summary>
        /// <param name="tests">Основные тесты.</param>
        /// <param name="beforeTests">Методы, помеченные атрибутом Before.</param>
        /// <param name="afterTests">Методы, помеченные атрибутом After.</param>
        /// <param name="results">Список для сохранения результатов.</param>
        private void StartTesting(List<TestInfo> tests, List<TestInfo> beforeTests, 
            List<TestInfo> afterTests, List<TestResult> results)
        {
            List<Task<TestResult>> tasks = new List<Task<TestResult>>();
            List<Task<TestResult>> beforeTasks = new List<Task<TestResult>>();
            List<Task<TestResult>> afterTasks = new List<Task<TestResult>>();
            foreach (var beforeTest in beforeTests)
            {
                var testTask = new Task<TestResult>(newTest => RunTest((TestInfo)newTest), beforeTest);
                beforeTasks.Add(testTask);
            }
            foreach (var afterTest in afterTests)
            {
                var testTask = new Task<TestResult>(newTest => RunTest((TestInfo)newTest), afterTest);
                beforeTasks.Add(testTask);
            }
            foreach (var test in tests)
            {
                foreach (var beforeTask in beforeTasks)
                {
                    beforeTask.Start();
                }
                foreach (var beforeTask in beforeTasks)
                {
                    beforeTask.Wait();
                }
                var testTask = new Task<TestResult>(newTest => RunTest((TestInfo)newTest), test);
                tasks.Add(testTask);
                testTask.Start();
                foreach (var afterTask in afterTasks)
                {
                    afterTask.Start();
                }
                foreach (var afterTask in afterTasks)
                {
                    afterTask.Wait();
                }
            }
            foreach (var beforeTask in beforeTasks)
            {
                lock (lockObject)
                {
                    results.Add(beforeTask.Result);
                }
            }
            foreach (var afterTask in afterTasks)
            {
                lock(lockObject)
                {
                    results.Add(afterTask.Result);
                }
            }
            foreach (var task in tasks)
            {
                lock(lockObject)
                {
                    results.Add(task.Result);
                }
            }
        }

        /// <summary>
        /// Данный метод запускает тестовый метод и выполняет необходимые проверки.
        /// </summary>
        /// <param name="test">Тестовый метод.</param>
        /// <returns>Возвращает результат выполнения теста в виде TestResult.</returns>
        private TestResult RunTest(TestInfo test)
        {
            var result = new TestResult(test.TestType.GetType().Name, test.TestMethod.Name);
            if (test.Attr.GetType() == typeof(TestAttribute))
            {
                if (((TestAttribute)test.Attr).Ignore != null)
                {
                    result.IsOk = true;

                    result.WhyIgnored = ((TestAttribute)test.Attr).Ignore;
                    return result;
                }
            }
            Stopwatch stopwatch = Stopwatch.StartNew();
            try
            {
                test.TestMethod.Invoke(test.TestType, null);   
                
                result.IsOk = true;
            }
            catch (Exception ex)
            {
                if (test.Attr.GetType() == typeof(TestAttribute))
                {
                    if (((TestAttribute)test.Attr).Expected != null)
                    {
                        var exc = ((TestAttribute)test.Attr).Expected;
                        result.Expected = exc;
                        if (ex.InnerException.GetType() == exc)
                        {
                            result.IsOk = true;
                        }
                        else
                        {
                            result.IsOk = false;
                            result.RealException = ex.InnerException;
                        }
                    }
                    else
                    {
                        result.IsOk = false;
                        result.RealException = ex.InnerException;
                    }
                }
                else
                {
                    result.IsOk = false;
                    result.RealException = ex.InnerException;
                }
            }
            finally
            {
                stopwatch.Stop();
            }

            result.Time = stopwatch.ElapsedMilliseconds;

            return result;
        }

        /// <summary>
        /// Метод распределяет методы из класса по спискам в зависимости от его атрибутов.
        /// </summary>
        /// <param name="type">Класс, в котором производится поиск.</param>
        /// <param name="beforeClassTestMethods">Список для методов с атрибутом BeforeClass.</param>
        /// <param name="afterClassTestMethods">Список для методов с атрибутом AfterClass.</param>
        /// <param name="beforeTestMethods">Список для методов с атрибутом Before.</param>
        /// <param name="afterTestMethods">Список для методов с атрибутом After.</param>
        /// <param name="testMethods">Список для стандартных тестовых методов.</param>
        private void GetTestMethods(Type type, List<TestInfo> beforeClassTestMethods,
            List<TestInfo> afterClassTestMethods, List<TestInfo> beforeTestMethods,
            List<TestInfo> afterTestMethods, List<TestInfo> testMethods)
        {
            object obj = Activator.CreateInstance(type);

            foreach (var method in type.GetMethods())
            {
                foreach (var attr in Attribute.GetCustomAttributes(method))
                {
                    if (attr.GetType() == typeof(BeforeClassAttribute))
                    {
                        var test = new TestInfo(method, obj, attr);
                        beforeClassTestMethods.Add(test);
                    }

                    if (attr.GetType() == typeof(AfterClassAttribute))
                    {
                        var test = new TestInfo(method, obj, attr);
                        afterClassTestMethods.Add(test);
                    }

                    if (attr.GetType() == typeof(BeforeAttribute))
                    {
                        var test = new TestInfo(method, obj, attr);
                        beforeTestMethods.Add(test);
                    }

                    if (attr.GetType() == typeof(AfterAttribute))
                    {
                        var test = new TestInfo(method, obj, attr);
                        afterTestMethods.Add(test);
                    }

                    if (attr.GetType() == typeof(TestAttribute))
                    {
                        var test = new TestInfo(method, obj, attr);
                        testMethods.Add(test);
                    }
                }

            }
        }
    }
}

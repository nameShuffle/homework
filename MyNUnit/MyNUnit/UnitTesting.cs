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

        bool isBeforeClassCompleted = true;
        bool isBeforeCompleted = true;

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
                    return null;
                }

                RunTests(path);
            }
            catch
            {
                Console.WriteLine("Проверьте корректность введенных данных");
                return null;
            }

            return this.testResults;
        }

        /// <summary>
        /// Метод ищет сборки по указанному пути, разбирает типы в каждой сборке и запускает
        /// для каждого типа методы, распределяющий тесты по нужным спискам.
        /// </summary>
        /// <param name="path">Путь, в котором осуществляется поиск.</param>
        private void RunTests(string path)
        {
            var testsDllAssemblies = Directory.GetFiles(path, "*.dll");

            var tasksDlls = new List<Task>();

            foreach (var dllFile in testsDllAssemblies)
            {
                foreach (var type in Assembly.LoadFile(dllFile).GetExportedTypes())
                {
                    var handleType = new Task(curType => RunTestsInClass((Type)curType), type);
                    tasksDlls.Add(handleType);
                    handleType.Start();
                }
            }

            foreach (var task in tasksDlls)
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
            var beforeClassTestMethods = new List<TestInfo>();
            var afterClassTestMethods = new List<TestInfo>();
            var beforeTestMethods = new List<TestInfo>();
            var afterTestMethods = new List<TestInfo>();
            var testMethods = new List<TestInfo>();
            GetTestMethods(type, beforeClassTestMethods, afterClassTestMethods, beforeTestMethods, afterTestMethods,
                testMethods);
            
            StartTesting(beforeClassTestMethods);
            if(this.isBeforeClassCompleted)
            {
                StartTesting(testMethods, beforeTestMethods, afterTestMethods);
            }
            StartTesting(afterClassTestMethods);
        }

        /// <summary>
        /// Метод, использующийся для запуска тестов, помеченных атрибутами BeforeClass и AfterClass.
        /// </summary>
        /// <param name="tests">Тесты для запуска.</param>
        /// <param name="results">Список для сохранения результатов.</param>
        private void StartTesting(List<TestInfo> tests)
        {
            List<Task<TestResult>> tasks = new List<Task<TestResult>>();
            
            foreach (var test in tests)
            {
                var testTask = new Task<TestResult>(newTest => RunTest((TestInfo)newTest), test);
                testTask.Start();
                tasks.Add(testTask);
            }
            foreach (var test in tasks)
            {
                lock (lockObject)
                {
                    var result = test.Result;
                    this.testResults.Add(result);
                    if (!result.IsOk)
                    {
                        this.isBeforeClassCompleted = false;
                    }
                }
            }
        }

        /// <summary>
        /// Перегруженный метод, используется для параллельного 
        /// запуска основных тестов методов.
        /// </summary>
        /// <param name="tests">Основные тесты.</param>
        /// <param name="beforeTests">Методы, помеченные атрибутом Before.</param>
        /// <param name="afterTests">Методы, помеченные атрибутом After.</param>
        private void StartTesting(List<TestInfo> tests, List<TestInfo> beforeTests, 
            List<TestInfo> afterTests)
        {
            List<Task> tasks = new List<Task>();

            foreach (var test in tests)
            {
                var testTask = new Task(() => RunBeforeAfterAndTest(test, beforeTests, afterTests));
                tasks.Add(testTask);
                testTask.Start();
            }

            foreach (var task in tasks)
            {
                task.Wait();
            }
        }


        /// <summary>
        /// Метод, подготавливащий основной тест к работе.
        /// При каждом запуске основного тестового метода производится запуск методов, 
        /// помеченных атрибутом Before. После запуска каждого основного тестового метода
        /// производится запуск методов, помеченных атрибутом After.
        /// </summary>
        /// <param name="test"></param>
        /// <param name="beforeTests"></param>
        /// <param name="afterTests"></param>
        private void RunBeforeAfterAndTest(TestInfo test, List<TestInfo> beforeTests,
            List<TestInfo> afterTests)
        {
            object obj = Activator.CreateInstance(test.TestType);
            test.Object = obj;

            foreach (var beforeTest in beforeTests)
            {
                beforeTest.Object = obj;
                var result = RunTest(beforeTest);
                if (!result.IsOk)
                {
                    this.isBeforeCompleted = false;
                }
                lock (lockObject)
                {
                    this.testResults.Add(RunTest(beforeTest));
                }
            }

            if (this.isBeforeCompleted)
            {
                lock (lockObject)
                {
                    this.testResults.Add(RunTest(test));
                }
            }

            foreach (var afterTest in afterTests)
            {
                afterTest.Object = obj;
                lock (lockObject)
                {
                    this.testResults.Add(RunTest(afterTest));
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
            var result = new TestResult(test.TestType.Name, test.TestMethod.Name);
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
                if (test.Attr.GetType() == typeof(BeforeClassAttribute)
                    || test.Attr.GetType() == typeof(AfterClassAttribute))
                {
                    test.TestMethod.Invoke(test.TestType, null);
                }
                else
                {
                    test.TestMethod.Invoke(test.Object, null);
                }
                
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
            foreach (var method in type.GetMethods())
            {
                foreach (var attr in Attribute.GetCustomAttributes(method))
                {
                    if (attr.GetType() == typeof(BeforeClassAttribute))
                    {
                        if (method.IsStatic)
                        {
                            var test = new TestInfo(method, type, attr);
                            beforeClassTestMethods.Add(test);
                        }
                        else
                        {
                            var result = new TestResult(type.Name, method.Name)
                            {
                                IsOk = false,
                                RealException = new Exception("BeforeClass Test wasn't static")
                            };
                            this.isBeforeClassCompleted = false;
                            lock(lockObject)
                            {
                                this.testResults.Add(result);
                            }
                        }
                    }

                    if (attr.GetType() == typeof(AfterClassAttribute))
                    {
                        if (method.IsStatic)
                        {
                            var test = new TestInfo(method, type, attr);
                            afterClassTestMethods.Add(test);
                        }
                        else
                        {
                            var result = new TestResult(type.Name, method.Name)
                            {
                                IsOk = false,
                                RealException = new Exception("After Class Test wasn't static")
                            };
                            lock(lockObject)
                            {
                                this.testResults.Add(result);
                            }
                        }
                    }

                    if (attr.GetType() == typeof(BeforeAttribute))
                    {
                        var test = new TestInfo(method, type, attr);
                        beforeTestMethods.Add(test);
                    }

                    if (attr.GetType() == typeof(AfterAttribute))
                    {
                        var test = new TestInfo(method, type, attr);
                        afterTestMethods.Add(test);
                    }

                    if (attr.GetType() == typeof(TestAttribute))
                    {
                        var test = new TestInfo(method, type, attr);
                        testMethods.Add(test);
                    }
                }

            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNUnit
{
    /// <summary>
    /// Класс, инкапсулирующий результаты выполнения тестов:
    /// статус теста (прошел или не прошел), время исполнения теста, объект исключения.
    /// Для обычных тестов хранится дополнительная информация - причина отключения,
    /// тип ожидаемого исключения.
    /// </summary>
    public class TestResult
    {
        public TestResult(string typeName, string testName)
        {
            TypeName = typeName;
            TestName = testName;
        }

        public string TypeName { get; set; }
        public string TestName { get; set; }
        public long Time { get; set; }
        public bool IsOk { get; set; }
        public string WhyIgnored { get; set; }
        public Type Expected { get; set; }
        public Exception RealException { get; set; }
    }
}

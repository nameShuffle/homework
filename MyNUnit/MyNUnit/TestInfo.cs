using System;
using System.Reflection;

namespace MyNUnit
{
    /// <summary>
    /// Данный класс инкапсулирует информацию о тестовом методе для удобства работы с ним.
    /// </summary>
    public class TestInfo
    {
        public TestInfo(MethodInfo method, Type type, Attribute attr)
        {
            this.TestMethod = method;
            this.TestType = type;
            this.Attr = attr;
        }

        public MethodInfo TestMethod { get; }
        public Type TestType { get; }
        public Attribute Attr { get; }
        public object Object { set;  get; }
    }
}

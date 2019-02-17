using System;

namespace MyNUnit
{
    /// <summary>
    /// Атрибут для методов, являющихся тестами.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAttribute : Attribute
    {
        public Type Expected { get; set; }
        public string Ignore { get; set; }
    }

    /// <summary>
    ///Атрибут для методов, которые должны запускаться перед запуском
    ///каждого теста.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeAttribute : Attribute
    {
    }

    /// <summary>
    /// Атрибут для методов, которые должны запускаться после запуска
    /// каждого теста.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterAttribute : Attribute
    {
    }


    /// <summary>
    /// Атрибут для методов, которые должны запускать перед запуском всех тестов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeClassAttribute : Attribute
    {
    }


    /// <summary>
    /// Атрибут для методов, которые должны запускать после запуска всех тестов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterClassAttribute : Attribute
    {
    }
}

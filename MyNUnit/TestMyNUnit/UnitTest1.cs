﻿using System;
using Xunit;
using System.Collections.Generic;
using MyNUnit;
using System.IO;

namespace TestMyNUnit
{
    public class UnitTest1
    {
        public string GetDirectory(string tail) 
            => new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName + tail;

        [Fact]
        public void IncorrectDirectoryTest()
        {
            string path = " ";
            List<TestResult> results = new List<TestResult>();
            UnitTesting testingSystem = new UnitTesting();
            results = testingSystem.StartUnitTesting(path);
            Assert.Null(results);
        }

        [Fact]
        public void CorrectDirectoryTest()
        {
            string path = GetDirectory("/TestApp/TestApp/bin/Debug");
            List<TestResult> results = new List<TestResult>();
            UnitTesting testingSystem = new UnitTesting();
            results = testingSystem.StartUnitTesting(path);
            Assert.NotNull(results);
        }

        [Fact]
        public void OkTest()
        {
            string path = GetDirectory("/TestApp/TestApp/bin/Debug");
            List<TestResult> results = new List<TestResult>();
            UnitTesting testingSystem = new UnitTesting();
            results = testingSystem.StartUnitTesting(path);
            foreach (var result in results)
            {
                if (result.TestName == "SubTest")
                {
                    Assert.True(result.IsOk);
                    break;
                }
            }
        }

        [Fact]
        public void NotTest()
        {
            string path = GetDirectory("/TestApp/TestApp/bin/Debug");
            List<TestResult> results = new List<TestResult>();
            UnitTesting testingSystem = new UnitTesting();
            results = testingSystem.StartUnitTesting(path);
            foreach (var result in results)
            {
                if (result.TestName == "ExceptionTest")
                {
                    Assert.False(result.IsOk);
                    break;
                }
            }
        }

        [Fact]
        public void ExceptionTest()
        {
            string path = GetDirectory("/TestApp/TestApp/bin/Debug");
            List<TestResult> results = new List<TestResult>();
            UnitTesting testingSystem = new UnitTesting();
            results = testingSystem.StartUnitTesting(path);
            foreach (var result in results)
            {
                if (result.TestName == "ExceptionTest")
                {
                    Assert.Equal(typeof(DivideByZeroException), result.RealException.GetType());
                    break;
                }
            }
        }

        [Fact]
        public void ExpectedExceptionTest()
        {
            string path = GetDirectory("/TestApp/TestApp/bin/Debug");
            List<TestResult> results = new List<TestResult>();
            UnitTesting testingSystem = new UnitTesting();
            results = testingSystem.StartUnitTesting(path);
            foreach (var result in results)
            {
                if (result.TestName == "TestMethod")
                {
                    Assert.Equal(result.Expected, result.RealException.GetType());
                    break;
                }
            }
        }

        [Fact]
        public void IgnoreTest()
        {
            string path = GetDirectory("/TestApp/TestApp/bin/Debug");
            List<TestResult> results = new List<TestResult>();
            UnitTesting testingSystem = new UnitTesting();
            results = testingSystem.StartUnitTesting(path);
            foreach (var result in results)
            {
                if (result.TestName == "IgnoringTest")
                {
                    Assert.False(result.IsOk);
                    break;
                }
            }
        }

        [Fact]
        public void TwoClassesTest()
        {
            string path = GetDirectory("/TestApp2/TestApp2/bin/Debug");
            List<TestResult> results = new List<TestResult>();
            UnitTesting testingSystem = new UnitTesting();
            results = testingSystem.StartUnitTesting(path);
            string firstClass = results[0].TypeName;
            Assert.Equal("TestClass1", firstClass);
            foreach (var result in results)
            {
                if (result.TypeName != firstClass)
                {
                    Assert.Equal("TestClass2", result.TypeName);
                }
            }
        }
    }
}

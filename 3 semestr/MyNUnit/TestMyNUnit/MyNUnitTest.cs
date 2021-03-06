﻿using System;
using Xunit;
using System.Collections.Generic;
using MyNUnit;
using System.IO;

namespace TestMyNUnit
{
    public class MyNUnitTest
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
            string path = GetDirectory("/TestApp/bin/Debug");
            UnitTesting testingSystem = new UnitTesting();
            var results = testingSystem.StartUnitTesting(path);
            Assert.NotNull(results);
        }

        [Fact]
        public void OkTest()
        {
            string path = GetDirectory("/TestApp/bin/Debug");
            UnitTesting testingSystem = new UnitTesting();
            var results = testingSystem.StartUnitTesting(path);
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
            string path = GetDirectory("/TestApp/bin/Debug");
            UnitTesting testingSystem = new UnitTesting();
            var results = testingSystem.StartUnitTesting(path);
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
            string path = GetDirectory("/TestApp/bin/Debug");
            UnitTesting testingSystem = new UnitTesting();
            var results = testingSystem.StartUnitTesting(path);
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
            string path = GetDirectory("/TestApp/bin/Debug");
            UnitTesting testingSystem = new UnitTesting();
            var results = testingSystem.StartUnitTesting(path);
            foreach (var result in results)
            {
                if (result.TestName == "TestMethod")
                {
                    Assert.True(result.IsOk);
                    break;
                }
            }
        }

        [Fact]
        public void IgnoreTest()
        {
            string path = GetDirectory("/TestApp/bin/Debug");
            UnitTesting testingSystem = new UnitTesting();
            var results = testingSystem.StartUnitTesting(path);
            foreach (var result in results)
            {
                if (result.TestName == "IgnoringTest")
                {
                    Assert.True(result.IsOk);
                    Assert.NotNull(result.WhyIgnored);
                    break;
                }
            }
        }

    }
}

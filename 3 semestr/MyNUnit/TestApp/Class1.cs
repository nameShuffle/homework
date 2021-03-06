﻿using System;
using MyNUnit;

namespace TestApp
{
    public class Class1
    {
        private int test = 0;

        [Test(Expected = typeof(DivideByZeroException))]
        public void TestMethod()
        {
            throw new DivideByZeroException("be careful!");
        }

        [Test(Ignore = "its stupid test...")]
        public int IgnoringTest()
        {
            return 21;
        }

        [Test]
        public int ExceptionTest()
        {
            int a = 5;
            return a / this.test;
        }

        [BeforeClass]
        public static string SubTest()
        {
            return "its beforeclass test";
        }

        [AfterClass]
        public static string AddTest()
        {
            return "its beforeclass test";
        }
    }
}

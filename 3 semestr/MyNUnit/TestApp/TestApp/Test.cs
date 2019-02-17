using System;
using MyNUnit;

namespace TestApp
{
    public class Test
    {
        private int test = 1;

        [Test(Expected = typeof(DivideByZeroException))]
        public void TestMethod()
        {
            throw new AggregateException("be careful!");
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
        public void SubTest()
        {
            test -= 1;
        }

        [AfterClass]
        public void AddTest()
        {
            test += 1;
        }
    }
    
}
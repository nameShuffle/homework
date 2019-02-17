using MyNUnit;

namespace TestApp2
{
    public class TestClass1
    {
        private int test = 0;

        [Test]
        public int NormalTest()
        {
            test += 1;
            return test;
        }

        [Before]
        public void SubTest()
        {
            test -= 1;
        }

        [After]
        public void AddTest()
        {
            test += 1;
        }
    }

    public class TestClass2
    {
        [BeforeClass]
        public string HelloTest()
        {
            return "hello!";
        }
    }
}

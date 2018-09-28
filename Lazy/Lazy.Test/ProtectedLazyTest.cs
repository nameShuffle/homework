using System;
using System.Linq;
using System.Threading;
using Xunit;

namespace Lazy.Test
{
    public class ProtectedLazyTest
    {
        [Fact]
        public void NullFunctionTest()
        {
            bool? nullFunction()
            {
                return null;
            }

            var protectedLazy = LazyFactory.CreateProtectedLazy(nullFunction);

            Assert.Equal(null, protectedLazy.Get);
        }

        [Fact]
        public void NormalGetTest()
        {
            int function()
            {
                return 21;
            }

            var protectedLazy = LazyFactory.CreateProtectedLazy(function);

            Assert.Equal(21, protectedLazy.Get);
        }

        [Fact]
        public void SecondCalculatingTest()
        {
            int randomFunction()
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 100);
                return randomNumber;
            }

            var protectedLazy = LazyFactory.CreateProtectedLazy(randomFunction);

            var firstResult = protectedLazy.Get;
            var secondResult = protectedLazy.Get;

            Assert.Equal(firstResult, secondResult);
        }

        /*[Fact]
        public void GetTestWithThreads()
        {
            var threads = new Thread[10];

            var results = new int[10, 2];

        }*/
    }
}

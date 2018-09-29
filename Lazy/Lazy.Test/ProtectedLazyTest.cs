using System;
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
        public void OrdinaryGetTest()
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

        [Fact]
        public void GetTestWithThreads()
        {
            int randomFunction()
            {
                Random random = new Random();
                int randomNumber = random.Next(0, 100);
                return randomNumber;
            }

            var protectedLazy = LazyFactory.CreateProtectedLazy(randomFunction);

            var threads = new Thread[10];
            var threadsResults = new int[10, 5];

            for (int i = 0; i < 10; i++)
            {
                int threadNumber = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < 5; j++)
                        threadsResults[threadNumber, j] = protectedLazy.Get;
                    Thread.Sleep(100);
                });
            }

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 5; j++)
                    Assert.Equal(threadsResults[0, 0], threadsResults[i, j]);
        }
    }
}

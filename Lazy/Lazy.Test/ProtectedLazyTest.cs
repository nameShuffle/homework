using System;
using System.Threading;
using Xunit;

namespace Lazy.Test
{
    public class ProtectedLazyTest
    {
        private Random random = new Random();

        [Fact]
        public void NullFunctionTest()
        {
            bool? nullFunction() => null;

            var protectedLazy = LazyFactory.CreateProtectedLazy(nullFunction);

            Assert.Null(protectedLazy.Get);
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
                int randomNumber = this.random.Next(0, 100);
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
                int randomNumber = this.random.Next(0, 100);
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

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            for (int i = 0; i < 10; i++)
                for (int j = 0; j < 5; j++)
                {
                    Assert.Equal(threadsResults[0, 0], threadsResults[i, j]);
                }
        }
    }
}

using System;
using System.Linq;
using Xunit;

namespace Lazy.Test
{
    public class SimpleLazyTest
    {
        [Fact]
        public void LazyNullTest()
        {
            bool? nullFunction()
            {
                return null;
            }

            var simpleLazy = LazyFactory.CreateSimpleLazy(nullFunction);

            Assert.Equal(null, simpleLazy.Get);
        }

        [Fact]
        public void LazyGetTest()
        {
            int function()
            {
                return 21;
            }

            var simpleLazy = LazyFactory.CreateSimpleLazy(function);

            Assert.Equal(21, simpleLazy.Get);
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

            var simpleLazy = LazyFactory.CreateSimpleLazy(randomFunction);

            var firstResult = simpleLazy.Get;
            var secondResult = simpleLazy.Get;

            Assert.Equal(firstResult, secondResult);
        }
    }
}
